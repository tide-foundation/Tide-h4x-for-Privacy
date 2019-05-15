namespace Raziel.Library.Models {
    public interface IVendorService {
        AuthenticationRequest GenerateToken(AuthenticationRequest request);
        User GetDetails(AuthenticationRequest request);
        bool Save(AuthenticationRequest request);
    }
}