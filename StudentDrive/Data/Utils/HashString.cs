namespace Data.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    public class HashString
    {
        public static Guid GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);
            return new Guid(hash);
        }
    }
}