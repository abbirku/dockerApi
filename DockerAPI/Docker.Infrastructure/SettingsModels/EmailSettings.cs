namespace Docker.Infrastructure.SettingsModels
{
    public class EmailSettings
    {
        public static string SMTPEmail { get; private set; }
        public static string SMTPPassword { get; private set; }
        public static string SMTPPort { get; private set; }
        public static string SMTPHostname { get; private set; }

    }
}
