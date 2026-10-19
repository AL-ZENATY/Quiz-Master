
using System;
using System.Collections.Generic;

namespace QuizMaster
{
    public class QuizEngine
    {
        // ==========================================
        // QUIZ DATA
        // ==========================================

        public List<QuizQuestion> Questions { get; }
        public List<QuestionResult> Results { get; } = new();

        // ==========================================
        // QUESTION TRACKING
        // ==========================================

        public int CurrentQuestionIndex { get; private set; }

        public int CorrectAnswers { get; private set; }
        public int WrongAnswers { get; private set; }
        public int UnansweredQuestions { get; private set; }

        public bool SkipUsed { get; private set; }

        public bool HasQuestions => Questions.Count > 0;

        public bool HasMoreQuestions =>
            CurrentQuestionIndex < Questions.Count;

        public QuizQuestion CurrentQuestion =>
            Questions[CurrentQuestionIndex];

        // ==========================================
        // GOLDEN QUESTION
        // ==========================================

        public int GoldenQuestionIndex { get; }

        public bool IsGoldenQuestion =>
            HasMoreQuestions &&
            GoldenQuestion.IsGolden(
                CurrentQuestionIndex,
                GoldenQuestionIndex);

        // ==========================================
        // SCORE TRACKING
        // ==========================================

        public decimal Score { get; private set; }
        public decimal SpeedBonus { get; private set; }
        public decimal UnreachedPenalty { get; private set; }

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public QuizEngine(
            List<QuizQuestion> questions,
            bool isSpeedQuiz = false)
        {
            Questions = questions;

            // Select one golden question for this game.
            GoldenQuestionIndex = GoldenQuestion.SelectIndex(
                Questions.Count,
                isSpeedQuiz);

            // Prepare a result for every question.
            foreach (QuizQuestion question in Questions)
            {
                Results.Add(new QuestionResult
                {
                    Question = question,
                    UserAnswer = null,
                    Status = "Not reached"
                });
            }
        }

        // ==========================================
        // ANSWER SHUFFLING
        // ==========================================

        public string[] ShuffleAnswers()
        {
            string[] answers =
            {
                CurrentQuestion.CorrectAnswer,
                CurrentQuestion.WrongAnswer1,
                CurrentQuestion.WrongAnswer2,
                CurrentQuestion.WrongAnswer3
            };

            Random.Shared.Shuffle(answers);

            return answers;
        }

        // ==========================================
        // SUBMIT ANSWER
        // ==========================================

        public bool SubmitAnswer(string answer)
        {
            QuestionResult result = Results[CurrentQuestionIndex];

            result.UserAnswer = answer;

            bool correct =
                answer == CurrentQuestion.CorrectAnswer;

            if (correct)
            {
                result.Status = "Correct";
                CorrectAnswers++;

                Score += IsGoldenQuestion
                    ? GoldenQuestion.GetCorrectPoints()
                    : 5m;
            }
            else
            {
                result.Status = "Wrong";
                WrongAnswers++;

                Score += IsGoldenQuestion
                    ? GoldenQuestion.GetWrongPenalty()
                    : -2m;
            }

            return correct;
        }

        // ==========================================
        // QUESTION TIMEOUT
        // ==========================================

        public void TimeoutQuestion()
        {
            UnansweredQuestions++;

            Score += IsGoldenQuestion
                ? GoldenQuestion.GetTimeoutPenalty()
                : -1m;

            Results[CurrentQuestionIndex].Status = "Timed out";
            Results[CurrentQuestionIndex].UserAnswer = null;
        }

        // ==========================================
        // GAME TIMEOUT
        // ==========================================

        public void GameTimedOut(bool currentQuestionUnanswered)
        {
            int unreached = Math.Max(
                0,
                Questions.Count - CurrentQuestionIndex - 1);

            UnreachedPenalty = unreached * -0.5m;
            Score += UnreachedPenalty;

            if (currentQuestionUnanswered)
            {
                UnansweredQuestions++;

                Score += IsGoldenQuestion
                    ? GoldenQuestion.GetTimeoutPenalty()
                    : -1m;

                Results[CurrentQuestionIndex].Status = "Timed out";
                Results[CurrentQuestionIndex].UserAnswer = null;
            }
        }

        // ==========================================
        // SKIP QUESTION
        // ==========================================

        public bool SkipQuestion()
        {
            if (SkipUsed)
                return false;

            SkipUsed = true;
            UnansweredQuestions++;

            Results[CurrentQuestionIndex].Status = "Skipped";
            Results[CurrentQuestionIndex].UserAnswer = null;

            return true;
        }

        // ==========================================
        // ADVANCE TO NEXT QUESTION
        // ==========================================

        public void AdvanceQuestion()
        {
            CurrentQuestionIndex++;
        }

        // ==========================================
        // SPEED BONUS
        // ==========================================

        public void ApplySpeedBonus(double secondsRemaining)
        {
            int fullSeconds = (int)Math.Floor(
                Math.Max(0, secondsRemaining));

            SpeedBonus = fullSeconds * 0.1m;
            Score += SpeedBonus;
        }
    }
}
