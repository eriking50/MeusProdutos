using System;
using System.Security.Cryptography;
using System.Text;

namespace MyProducts
{
    /// <summary>
    /// Classe que cuida da conversão da senha em hash
    /// </summary>
    public class PasswordHashing
    {
        public string GetHash(string password)
        {
            byte[] hashValue;
            UnicodeEncoding ue = new UnicodeEncoding();
            //Transforma o password em um formato byte
            byte[] passwordBytes = ue.GetBytes(password);
            SHA256 shHash = SHA256.Create();
            //Converte o formato byte do password em um formato byte de Sha256
            hashValue = shHash.ComputeHash(passwordBytes);

            return Convert.ToBase64String(hashValue);
        }
    }
}