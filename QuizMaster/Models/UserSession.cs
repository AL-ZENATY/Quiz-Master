namespace QuizMaster
{
    public static class UserSession
    {
        public static int? UserId { get; private set; }
        public static string? Username { get; private set; }

        public static bool IsLoggedIn => UserId.HasValue;

        public static void Login(int userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        public static void Logout()
        {
            UserId = null;
            Username = null;
        }
    }
}