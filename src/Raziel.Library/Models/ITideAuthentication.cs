namespace Raziel.Library.Models {
    public interface ITideAuthentication {
        TideResponse Login(AuthenticationModel model);

        TideResponse GetUserNodes(AuthenticationModel model);
    }
}