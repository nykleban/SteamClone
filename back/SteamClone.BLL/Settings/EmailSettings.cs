namespace SteamClone.BLL.Settings
{
    public class EmailSettings
    {
        public string FromEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string ConfirmEmailUrl { get; set; } = string.Empty;
    }
}
