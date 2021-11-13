using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jobs.Service.Common
{
    public static class Encryptor
    {
        /// <summary>
        /// This method for create SH1 hash from string.
        /// </summary>
        /// <param name="str">Steing for create hash</param>
        /// <returns>Return created SH1 hash string</returns>
        public static string SH1Hash(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            using SHA1 hash = SHA1.Create();
            return string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(b => b.ToString("x2")).ToArray());
        }

        /// <summary>
        /// This method for create MD5 hash from string.
        /// </summary>
        /// <param name="str">Steing for create hash</param>
        /// <returns>Return created MD5 hash string</returns>
        public static string MD5Hash(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            using MD5 hash = MD5.Create();
            return string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(b => b.ToString("x2")).ToArray());
        }

        /// <summary>
        /// This method for create Sha256 hash from string.
        /// </summary>
        public static string Sha256Hash(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            using SHA256 hash = SHA256.Create();
            return string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(b => b.ToString("x2")).ToArray());
        }

        public static string MD5Hash2(string text)
        {
            MD5 md5 = MD5.Create();
            //compute hash from the bytes of text  
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
