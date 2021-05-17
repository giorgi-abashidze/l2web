using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace l2web.helpers
{
    public class HelperFunctions
    {
        public static string hCrypt(string password,string key)
        {
            string md5password = key;
            StringBuilder s = new StringBuilder();
            s.Append(CreateMD5Hash(password) + CreateMD5Hash(md5password));

            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (j >= md5password.Length)
                {
                    j = 0;
                }
                s[i] = (char)(s[i] ^ md5password[j]);
                j++;
            }
            return CreateMD5Hash(s.ToString());
        }

        public static string CreateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }



        public static string GenerateSSN()
        {
            Random r = new Random();

            int ssn1 = r.Next(100000, 999999);
            int ssn2 = r.Next(100000, 999999);

            return $"{ssn1}{ssn2}";
        }

        public static byte[] StrToByteArray(string value)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetBytes(value);
        }

        public static void Randomize<T>(T[] items)
        {
            Random rand = new Random();

            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }

        public static double GetDiffInMinutes(DateTime date1,DateTime date2)
        {
            TimeSpan ts = date2 - date1;

            return ts.TotalMinutes;
        }

    }

}

