namespace Raziel.Library.Models {
    public interface IAdminTideAuthentication {
        TideResponse CreateAccount(string publicKey, string username, bool seed);
        TideResponse AddFragment(AuthenticationModel model);
    }
}