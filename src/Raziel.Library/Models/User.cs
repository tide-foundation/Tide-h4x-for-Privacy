namespace Raziel.Library.Models {
    public class User : BaseModel {
        public new int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VendorPublicKey { get; set; }
        public string BitcoinPrivateKey { get; set; }
        public string Note { get; set; }
    }
}