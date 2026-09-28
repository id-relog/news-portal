namespace WebAppUI.Auth
{
    public static class AppRoles
    {
        public const string Analyst = "Analyst";
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string Moderator = "Moderator";
        public const string Correspondent = "Correspondent";
        public const string User = "User";
        public const string Guest = "Guest";

        public static readonly string[] All =
        [
            Analyst,
            Administrator,
            Manager,
            Moderator,
            Correspondent,
            User,
            Guest
        ];
    }
}

