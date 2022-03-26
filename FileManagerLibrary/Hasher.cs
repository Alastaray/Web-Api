using System;
using System.Security.Cryptography;
using System.Text;

namespace FileManagerLibrary
{
    public static class Hasher
    {
        public static string CreateHashPath(string hashName)
        {
            const int countSubFolder = 2;
            const int countSymbolsNameFolder = 3;
            string path = string.Empty;
            for (int i = 0; i < countSubFolder; i++)
            {
                int startPos = countSymbolsNameFolder * i;
                path += hashName.Substring(startPos, countSymbolsNameFolder) + '/';
            }
            return path;
        }

        public static string CreateHashName(string url)
        {
            string hashName = CreateHashStr(url.Split('/')[^1]);
            string extension = '.' + url.Split('/')[^1].Split('.')[^1];
            return hashName + extension;
        }

        public static string CreateHashStr(string input, int? number = null, DateTime? time = null)
        {
            input += number ?? new Random().Next();
            input += time ?? DateTime.UtcNow;
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
