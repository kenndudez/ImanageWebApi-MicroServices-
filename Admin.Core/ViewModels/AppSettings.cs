namespace Auth.Core.ViewModels
{
    public class AppSettings
    {
        public int LockoutMinutes { get; set; }
        public int MinimumPasswordLength { get; set; }
        public int MaxLockoutAttempt { get; set; }
    }
}