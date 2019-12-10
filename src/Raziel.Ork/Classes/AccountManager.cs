using Raziel.Library.Classes;
using Tide.Encryption.Ecc;
using Tide.Encryption.EcDSA;

namespace Raziel.Ork.Classes
{
    public interface IAccountManager
    {
        (EcScalar Pwd, EcDSAKey Key) GetAccount(string user);
    }

    public class AccountManager : IAccountManager
    {
        private readonly OrkRepo repo;

        public AccountManager(OrkRepo repo)
        {
            this.repo = repo;
        }

        public (EcScalar Pwd, EcDSAKey Key) GetAccount(string user)
        {
            var share = repo.GetShare(user.ConvertToUint64());
            return (new EcScalar(share.PasswordHash), EcDSAKey.FromPrivate(share.CvkFragment));
        }
    }
}