namespace Raziel.Library.Models {
    public class AuthenticationModel {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PublicKey { get; set; }
        public string CvkPublic { get; set; }
        public string CvkPrivateFrag { get; set; }
        public string SiteUrl { get; set; }
        public string Ip { get; set; }
    }
}