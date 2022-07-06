using System.Text;

namespace API_Alluring.Helper
{
    public class HandlePassword
    {
        private readonly MD5_hash _md5hash;

        public HandlePassword(MD5_hash md5hash)
        {
            _md5hash = md5hash;
        }

        public string hashPassword(string password)
        {
            return _md5hash.MD5Hash(password);
        }
    }
}
