
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace QuizMaster
{
    public static class QuizDatabase
    {
        // ==========================================
        // DATABASE CONFIGURATION
        // ==========================================

        private static readonly string DatabasePath =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "QuizMaster",
                "quizmaster.db");

        private static readonly string ConnectionString =
            $"Data Source={DatabasePath}";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        // ==========================================
        // DATABASE INITIALIZATION
        // ==========================================

        public static void Initialize()
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(DatabasePath)!);

            using var connection = GetConnection();
            connection.Open();

            // Questions
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS Questions
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT NOT NULL,
                    QuestionText TEXT NOT NULL,
                    ImagePath TEXT,
                    CorrectAnswer TEXT NOT NULL,
                    WrongAnswer1 TEXT NOT NULL,
                    WrongAnswer2 TEXT NOT NULL,
                    WrongAnswer3 TEXT NOT NULL,
                    Difficulty TEXT NOT NULL DEFAULT 'Normal'
                        CHECK (Difficulty IN ('Normal', 'Tricky'))
                );");

            // Update older databases without losing questions.
            EnsureDifficultyColumn(connection);

            // Registered players
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS Users
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    IsAdmin INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
                );");

            // Every completed game
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS Games
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Category TEXT NOT NULL
                        CHECK (Category IN ('Flags', 'Cars', 'Mixed')),
                    Score REAL NOT NULL,
                    PlayedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,

                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );");

            // Personal records and game counts per category
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS UserScores
                (
                    UserId INTEGER NOT NULL,
                    Category TEXT NOT NULL
                        CHECK (Category IN ('Flags', 'Cars', 'Mixed')),
                    HighestScore REAL NOT NULL,
                    LowestScore REAL NOT NULL,
                    TotalGames INTEGER NOT NULL DEFAULT 0,

                    PRIMARY KEY (UserId, Category),
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );");

            // Rapid Fire personal completion records.
            // Only successfully completed runs are saved.
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS RapidFireScores
                (
                    UserId INTEGER PRIMARY KEY,
                    BestTime REAL NOT NULL,
                    WorstTime REAL NOT NULL,
                    TotalGames INTEGER NOT NULL DEFAULT 0,

                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );");
        }

        private static void ExecuteSql(
            SqliteConnection connection,
            string sql)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        private static void EnsureDifficultyColumn(
            SqliteConnection connection)
        {
            bool hasDifficulty = false;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info(Questions)";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (string.Equals(
                        reader.GetString(1),
                        "Difficulty",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        hasDifficulty = true;
                        break;
                    }
                }
            }

            if (!hasDifficulty)
            {
                ExecuteSql(connection, @"
                    ALTER TABLE Questions
                    ADD COLUMN Difficulty TEXT NOT NULL DEFAULT 'Normal'
                        CHECK (Difficulty IN ('Normal', 'Tricky'));");
            }
        }


        // ==========================================
        // SAVE QUIZ SCORE
        // ==========================================

        public static void SaveGameResult(
            int userId,
            string category,
            double score)
        {
            if (category != "Flags" &&
                category != "Cars" &&
                category != "Mixed")
            {
                throw new ArgumentException(
                    "Invalid quiz category.",
                    nameof(category));
            }

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO UserScores
                    (UserId, Category, HighestScore,
                     LowestScore, TotalGames)
                VALUES
                    ($userId, $category, $score, $score, 1)

                ON CONFLICT(UserId, Category)
                DO UPDATE SET
                    HighestScore = MAX(HighestScore, excluded.HighestScore),
                    LowestScore = MIN(LowestScore, excluded.LowestScore),
                    TotalGames = TotalGames + 1;";

            command.Parameters.AddWithValue("$userId", userId);
            command.Parameters.AddWithValue("$category", category);
            command.Parameters.AddWithValue("$score", score);

            command.ExecuteNonQuery();
        }

        // ==========================================
        // SAVE RAPID FIRE TIME
        // ==========================================

        public static void SaveRapidFireResult(
            int userId,
            double finalTimeSeconds)
        {
            if (!double.IsFinite(finalTimeSeconds) ||
                finalTimeSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(finalTimeSeconds));
            }

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO RapidFireScores
                    (UserId, BestTime, WorstTime, TotalGames)
                VALUES
                    ($userId, $time, $time, 1)

                ON CONFLICT(UserId)
                DO UPDATE SET
                    BestTime = MIN(BestTime, excluded.BestTime),
                    WorstTime = MAX(WorstTime, excluded.WorstTime),
                    TotalGames = TotalGames + 1;";

            command.Parameters.AddWithValue("$userId", userId);
            command.Parameters.AddWithValue(
                "$time", finalTimeSeconds);

            command.ExecuteNonQuery();
        }

        // ==========================================
        // LEADERBOARD - HIGHEST SCORES
        // ==========================================

        public static List<(string Username, double Score)>
            GetHighestScores(string category)
        {
            if (category != "Flags" &&
                category != "Cars" &&
                category != "Mixed")
            {
                throw new ArgumentException(
                    "Invalid quiz category.",
                    nameof(category));
            }

            List<(string Username, double Score)> scores = new();

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Users.Username, UserScores.HighestScore
                FROM UserScores
                INNER JOIN Users
                    ON Users.Id = UserScores.UserId
                WHERE UserScores.Category = $category
                ORDER BY UserScores.HighestScore DESC,
                         Users.Username ASC
                LIMIT 10;";

            command.Parameters.AddWithValue("$category", category);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                scores.Add((
                    reader.GetString(0),
                    reader.GetDouble(1)
                ));
            }

            return scores;
        }


        // ==========================================
        // LEADERBOARD RANKINGS
        // ==========================================

        public static List<(string Username, double Score)>
            GetLeaderboardScores(string category, bool hallOfFame)
        {
            if (category != "General" &&
                category != "Flags" &&
                category != "Cars" &&
                category != "Mixed")
            {
                throw new ArgumentException(
                    "Invalid leaderboard category.",
                    nameof(category));
            }

            List<(string Username, double Score)> rankings = new();

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            string scoreColumn = hallOfFame
                ? "HighestScore"
                : "LowestScore";

            string sortDirection = hallOfFame
                ? "DESC"
                : "ASC";

            // General combines the player's category records.
            if (category == "General")
            {
                command.CommandText = $@"
                    SELECT
                        Users.Username,
                        SUM(UserScores.{scoreColumn}) AS Score
                    FROM UserScores
                    INNER JOIN Users
                        ON Users.Id = UserScores.UserId
                    GROUP BY Users.Id, Users.Username
                    {(hallOfFame ? "" : "HAVING SUM(UserScores.TotalGames) >= 2")}
                    ORDER BY Score {sortDirection},
                             Users.Username ASC;";
            }
            else
            {
                command.CommandText = $@"
                    SELECT
                        Users.Username,
                        UserScores.{scoreColumn} AS Score
                    FROM UserScores
                    INNER JOIN Users
                        ON Users.Id = UserScores.UserId
                    WHERE UserScores.Category = $category
                    {(hallOfFame ? "" : "AND UserScores.TotalGames >= 2")}
                    ORDER BY Score {sortDirection},
                             Users.Username ASC;";

                command.Parameters.AddWithValue(
                    "$category", category);
            }

            using var reader = command.ExecuteReader();

            double? previousScore = null;
            List<string> tiedUsernames = new();

            void AddRanking()
            {
                if (previousScore.HasValue && rankings.Count < 10)
                {
                    rankings.Add((
                        string.Join(" ≡ ", tiedUsernames),
                        previousScore.Value));
                }
            }

            while (reader.Read())
            {
                string username = reader.GetString(0);
                double score = reader.GetDouble(1);

                if (previousScore.HasValue &&
                    score != previousScore.Value)
                {
                    AddRanking();
                    tiedUsernames.Clear();

                    if (rankings.Count >= 10)
                        break;
                }

                tiedUsernames.Add(username);
                previousScore = score;
            }

            AddRanking();

            return rankings;
        }

        // ==========================================
        // RAPID FIRE LEADERBOARD
        // ==========================================

        public static List<(string Username, double Time)>
            GetRapidFireLeaderboard(bool hallOfFame)
        {
            List<(string Username, double Time)> rankings = new();

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            string timeColumn = hallOfFame
                ? "BestTime"
                : "WorstTime";

            string sortDirection = hallOfFame
                ? "ASC"
                : "DESC";

            command.CommandText = $@"
                SELECT Users.Username,
                       RapidFireScores.{timeColumn}
                FROM RapidFireScores
                INNER JOIN Users
                    ON Users.Id = RapidFireScores.UserId
                ORDER BY RapidFireScores.{timeColumn} {sortDirection},
                         Users.Username ASC;";

            using var reader = command.ExecuteReader();

            double? previousTime = null;
            List<string> tiedUsernames = new();

            void AddRanking()
            {
                if (previousTime.HasValue && rankings.Count < 10)
                {
                    rankings.Add((
                        string.Join(" ≡ ", tiedUsernames),
                        previousTime.Value));
                }
            }

            while (reader.Read())
            {
                string username = reader.GetString(0);
                double time = reader.GetDouble(1);

                if (previousTime.HasValue &&
                    time != previousTime.Value)
                {
                    AddRanking();
                    tiedUsernames.Clear();

                    if (rankings.Count >= 10)
                        break;
                }

                tiedUsernames.Add(username);
                previousTime = time;
            }

            AddRanking();

            return rankings;
        }

        // ==========================================
        // RAPID FIRE TOP 10 POSITION
        // ==========================================

        public static int? GetRapidFireTop10Position(int userId)
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Username
                FROM Users
                WHERE Id = $userId;";

            command.Parameters.AddWithValue("$userId", userId);

            string? username = command.ExecuteScalar() as string;

            if (username == null)
                return null;

            var rankings = GetRapidFireLeaderboard(true);

            for (int i = 0; i < rankings.Count; i++)
            {
                string[] players = rankings[i].Username.Split(
                    " ≡ ",
                    StringSplitOptions.None);

                if (Array.Exists(players,
                    player => string.Equals(
                        player,
                        username,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    return i + 1;
                }
            }

            return null;
        }

        // ==========================================
        // PLAYER TOP 10 POSITION
        // ==========================================

        public static int? GetPlayerTop10Position(
            int userId,
            string category)
        {
            if (category != "Flags" &&
                category != "Cars" &&
                category != "Mixed")
            {
                throw new ArgumentException(
                    "Invalid leaderboard category.",
                    nameof(category));
            }

            using var connection = GetConnection();
            connection.Open();

            // Find the player's username.
            using var userCommand = connection.CreateCommand();

            userCommand.CommandText = @"
        SELECT Username
        FROM Users
        WHERE Id = $userId";

            userCommand.Parameters.AddWithValue("$userId", userId);

            string? username = userCommand.ExecuteScalar() as string;

            if (username == null)
                return null;

            // Use the same rankings as the Hall of Fame.
            var rankings = GetLeaderboardScores(category, true);

            for (int i = 0; i < rankings.Count; i++)
            {
                string[] tiedPlayers = rankings[i].Username.Split(
                    " ≡ ",
                    StringSplitOptions.None);

                if (Array.Exists(tiedPlayers,
                    player => string.Equals(
                        player,
                        username,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    return i + 1;
                }
            }

            return null;
        }

        // ==========================================
        // USER REGISTRATION
        // ==========================================

        public static string RegisterUser(
            string username,
            string email,
            string password)
        {
            using var connection = GetConnection();
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Username = $username COLLATE NOCASE";

                command.Parameters.AddWithValue("$username", username);

                if (Convert.ToInt64(command.ExecuteScalar()) > 0)
                    return "Username already taken!";
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Email = $email COLLATE NOCASE";

                command.Parameters.AddWithValue("$email", email);

                if (Convert.ToInt64(command.ExecuteScalar()) > 0)
                    return "Email already registered!";
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Users
                        (Username, Email, PasswordHash)
                    VALUES
                        ($username, $email, $passwordHash)";

                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$email", email);
                command.Parameters.AddWithValue(
                    "$passwordHash",
                    PasswordHelper.HashPassword(password));

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
                {
                    return "Username or email already registered!";
                }
            }

            return "Account created successfully!";
        }

        // ==========================================
        // USER LOGIN
        // ==========================================

        public static bool ValidateLogin(
            string usernameOrEmail,
            string password)
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT PasswordHash
                FROM Users
                WHERE Username = $login COLLATE NOCASE
                   OR Email = $login COLLATE NOCASE
                LIMIT 1";

            command.Parameters.AddWithValue(
                "$login",
                usernameOrEmail.Trim());

            object? result = command.ExecuteScalar();

            if (result is not string storedHash)
                return false;

            return PasswordHelper.VerifyPassword(
                password,
                storedHash);
        }

        // ==========================================
        // USER INFORMATION
        // ==========================================

        public static string? GetUsername(string usernameOrEmail)
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Username
                FROM Users
                WHERE Username = $login COLLATE NOCASE
                   OR Email = $login COLLATE NOCASE
                LIMIT 1";

            command.Parameters.AddWithValue(
                "$login",
                usernameOrEmail.Trim());

            return command.ExecuteScalar() as string;
        }

        public static int? GetUserId(string usernameOrEmail)
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id
                FROM Users
                WHERE Username = $login COLLATE NOCASE
                   OR Email = $login COLLATE NOCASE
                LIMIT 1";

            command.Parameters.AddWithValue(
                "$login",
                usernameOrEmail.Trim());

            object? result = command.ExecuteScalar();

            return result == null
                ? null
                : Convert.ToInt32(result);
        }

        // ==========================================
        // QUIZ QUESTIONS
        // ==========================================

        public static List<QuizQuestion> GetQuestions(string category)
        {
            if (string.Equals(
                category,
                "Flags",
                StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    category,
                    "Cars",
                    StringComparison.OrdinalIgnoreCase))
            {
                return GetRandomQuizQuestions(category);
            }

            List<QuizQuestion> questions = new List<QuizQuestion>();

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, Category, QuestionText, ImagePath,
                       CorrectAnswer, WrongAnswer1,
                       WrongAnswer2, WrongAnswer3
                FROM Questions
                WHERE Category = $category COLLATE NOCASE
                ORDER BY RANDOM()";

            command.Parameters.AddWithValue("$category", category);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                questions.Add(ReadQuestion(reader));
            }

            return questions;
        }

        // ==========================================
        // FLAGS AND CARS - 15 NORMAL + 5 TRICKY
        // ==========================================

        private static List<QuizQuestion> GetRandomQuizQuestions(string category)
        {
            List<QuizQuestion> questions = new List<QuizQuestion>();

            using var connection = GetConnection();
            connection.Open();

            // Verify that enough questions exist.
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        SUM(CASE WHEN Difficulty = 'Normal' THEN 1 ELSE 0 END),
                        SUM(CASE WHEN Difficulty = 'Tricky' THEN 1 ELSE 0 END)
                    FROM Questions
                    WHERE Category = $category COLLATE NOCASE";

                command.Parameters.AddWithValue("$category", category);

                using var reader = command.ExecuteReader();
                reader.Read();

                int normalCount = reader.IsDBNull(0)
                    ? 0
                    : reader.GetInt32(0);

                int trickyCount = reader.IsDBNull(1)
                    ? 0
                    : reader.GetInt32(1);

                if (normalCount < 15 || trickyCount < 5)
                {
                    throw new InvalidOperationException(
                         $"{category} Quiz needs at least 15 Normal questions " +
                        "and 5 Tricky questions in the database. " +
                        $"Currently: {normalCount} Normal and {trickyCount} Tricky.");
                }
            }

            // Select both difficulty groups and shuffle them.
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT *
                    FROM
                    (
                        SELECT Id, Category, QuestionText, ImagePath,
                               CorrectAnswer, WrongAnswer1,
                               WrongAnswer2, WrongAnswer3
                        FROM
                        (
                            SELECT Id, Category, QuestionText, ImagePath,
                                   CorrectAnswer, WrongAnswer1,
                                   WrongAnswer2, WrongAnswer3
                            FROM Questions
                            WHERE Category = $category COLLATE NOCASE
                              AND Difficulty = 'Normal'
                            ORDER BY RANDOM()
                            LIMIT 15
                        )

                        UNION ALL

                        SELECT Id, Category, QuestionText, ImagePath,
                               CorrectAnswer, WrongAnswer1,
                               WrongAnswer2, WrongAnswer3
                        FROM
                        (
                            SELECT Id, Category, QuestionText, ImagePath,
                                   CorrectAnswer, WrongAnswer1,
                                   WrongAnswer2, WrongAnswer3
                            FROM Questions
                            WHERE Category = $category COLLATE NOCASE
                              AND Difficulty = 'Tricky'
                            ORDER BY RANDOM()
                            LIMIT 5
                        )
                    )
                    ORDER BY RANDOM()";

                command.Parameters.AddWithValue("$category", category);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    questions.Add(ReadQuestion(reader));
                }
            }

            return questions;
        }

        private static QuizQuestion ReadQuestion(
            SqliteDataReader reader)
        {
            return new QuizQuestion
            {
                Id = reader.GetInt32(0),
                Category = reader.GetString(1),
                QuestionText = reader.GetString(2),

                ImagePath = reader.IsDBNull(3)
                    ? null
                    : reader.GetString(3),

                CorrectAnswer = reader.GetString(4),
                WrongAnswer1 = reader.GetString(5),
                WrongAnswer2 = reader.GetString(6),
                WrongAnswer3 = reader.GetString(7)
            };
        }

        // ==========================================
        // ADMIN - ADD QUESTION
        // ==========================================

        public static void AddQuestion(
            QuizQuestion question,
            string difficulty)
        {
            ValidateDifficulty(difficulty);

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Questions
                    (Category, QuestionText, ImagePath,
                     CorrectAnswer, WrongAnswer1,
                     WrongAnswer2, WrongAnswer3, Difficulty)
                VALUES
                    ($category, $text, $image,
                     $correct, $wrong1, $wrong2,
                     $wrong3, $difficulty)";

            AddQuestionParameters(command, question);
            command.Parameters.AddWithValue("$difficulty", difficulty);

            command.ExecuteNonQuery();
        }

        // ==========================================
        // ADMIN - EDIT QUESTION
        // ==========================================

        public static void UpdateQuestion(
            QuizQuestion question,
            string difficulty)
        {
            ValidateDifficulty(difficulty);

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Questions
                SET Category = $category,
                    QuestionText = $text,
                    ImagePath = $image,
                    CorrectAnswer = $correct,
                    WrongAnswer1 = $wrong1,
                    WrongAnswer2 = $wrong2,
                    WrongAnswer3 = $wrong3,
                    Difficulty = $difficulty
                WHERE Id = $id";

            AddQuestionParameters(command, question);

            command.Parameters.AddWithValue("$difficulty", difficulty);
            command.Parameters.AddWithValue("$id", question.Id);

            command.ExecuteNonQuery();
        }

        // ==========================================
        // ADMIN - DELETE QUESTION
        // ==========================================

        public static void DeleteQuestion(int questionId)
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText =
                "DELETE FROM Questions WHERE Id = $id";

            command.Parameters.AddWithValue("$id", questionId);
            command.ExecuteNonQuery();
        }

        // ==========================================
        // ADMIN - CHANGE DIFFICULTY
        // ==========================================

        public static void SetQuestionDifficulty(
            int questionId,
            string difficulty)
        {
            ValidateDifficulty(difficulty);

            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Questions
                SET Difficulty = $difficulty
                WHERE Id = $id";

            command.Parameters.AddWithValue("$difficulty", difficulty);
            command.Parameters.AddWithValue("$id", questionId);

            command.ExecuteNonQuery();
        }

        // ==========================================
        // ADMIN HELPERS
        // ==========================================

        private static void ValidateDifficulty(string difficulty)
        {
            if (difficulty != "Normal" && difficulty != "Tricky")
            {
                throw new ArgumentException(
                    "Difficulty must be 'Normal' or 'Tricky'.",
                    nameof(difficulty));
            }
        }

        private static void AddQuestionParameters(
            SqliteCommand command,
            QuizQuestion question)
        {
            command.Parameters.AddWithValue("$category", question.Category);
            command.Parameters.AddWithValue("$text", question.QuestionText);

            command.Parameters.AddWithValue(
                "$image",
                (object?)question.ImagePath ?? DBNull.Value);

            command.Parameters.AddWithValue("$correct", question.CorrectAnswer);
            command.Parameters.AddWithValue("$wrong1", question.WrongAnswer1);
            command.Parameters.AddWithValue("$wrong2", question.WrongAnswer2);
            command.Parameters.AddWithValue("$wrong3", question.WrongAnswer3);
        }
    }
}
