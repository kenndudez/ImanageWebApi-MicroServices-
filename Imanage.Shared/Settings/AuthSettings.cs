namespace Imanage.Shared.Settings
{
    public class AuthSettings
    {
        public string SecretKey { get; set; }
        public string Authority { get; set; }
        public bool RequireHttps { get; set; }
    }
}