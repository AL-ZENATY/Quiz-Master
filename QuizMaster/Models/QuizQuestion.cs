
namespace QuizMaster
{
    public class QuizQuestion
    {
        public int Id { get; set; }

        public string Category { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;

        // Images are optional in our database.
        public string? ImagePath { get; set; }

        public string CorrectAnswer { get; set; } = string.Empty;
        public string WrongAnswer1 { get; set; } = string.Empty;
        public string WrongAnswer2 { get; set; } = string.Empty;
        public string WrongAnswer3 { get; set; } = string.Empty;
    }
}
