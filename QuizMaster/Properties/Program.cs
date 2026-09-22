namespace QuizMaster
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Create or open the QuizMaster database.
            QuizDatabase.Initialize();

            // Start the application as usual.
            Application.Run(new MainForm());
        }
    }
}