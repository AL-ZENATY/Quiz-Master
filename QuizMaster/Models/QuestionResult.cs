
namespace QuizMaster
{
    public class QuestionResult
    {
        // ==========================================
        // ORIGINAL QUESTION
        // ==========================================

        // Keeps the original question from SQLite,
        // including its text, image and correct answer.
        public QuizQuestion Question { get; set; } = null!;


        // ==========================================
        // PLAYER'S ANSWER
        // ==========================================

        // Stores the answer selected by the player.
        // Null means no answer was selected.
        public string? UserAnswer { get; set; }


        // ==========================================
        // QUESTION STATUS
        // ==========================================

        // Possible values:
        // Correct, Wrong, Skipped, Timed out, Not reached.
        public string Status { get; set; } = "Not reached";
    }
}
