using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using System;

namespace Services
{
    public class KeyService
    {
        public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
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
        public static string Hash(string Text)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(Text)));
        }

        public static bool GetKey(string EMail, string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                //MessageService.Show("Активуйте свій продукт у налаштуваннях!");
                return false;
            }
            try
            {
                string[] stringTobyte = Key.Split('-');
                byte[] bytes = new byte[stringTobyte.Length];

                for (int i = 0; i < stringTobyte.Length; i++)
                {
                    string tmp = stringTobyte[i];
                    if (tmp[0] == '0') tmp = tmp.Substring(1, 2);

                    bytes[i] = Convert.ToByte(tmp);
                }

                var DecryptString = Encoding.UTF8.GetString(bytes);

                string[] DateEmail = DecryptString.Split('|');

                if (EMail != DateEmail[1])
                {
                    return false;
                }
                else if (!ValidateDate(DateEmail[0]))
                {
                    //MessageService.Show("У вас вийшов термін придатності користування програмою. Цей ключ не є дійсним!");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //MessageService.Show("Проблеми з активацією продукту!");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        static bool ValidateDate(string date)
        {
            string[] LastDate = date.Split('/');

            string Day = DateTime.Now.ToString("dd");
            string Month = DateTime.Now.ToString("MM");
            string Year = DateTime.Now.ToString("yy");

            int NowDays = GetCountOfDays(int.Parse(Year), int.Parse(Month), int.Parse(Day));
            int LastDays = GetCountOfDays(int.Parse(LastDate[2]), int.Parse(LastDate[1]), int.Parse(LastDate[0]));

            if ((NowDays - LastDays) <= 365) return true;

            return false;
        }

        public static string EncryptingText(string Text)
        {
            var bytes = Encoding.UTF8.GetBytes(Text);
            string EncryptString = bytes[0].ToString();
            string oneByte = "";

            for (int i = 1; i < bytes.Length; i++)
            {
                oneByte = bytes[i].ToString();

                if (oneByte.Length < 3) oneByte = "0" + oneByte;
                EncryptString += "-" + oneByte;
            }
            return EncryptString;
        }
        public static string DecryptingText(string Text)
        {
            string[] stringTobyte = Text.Split('-');

            if (stringTobyte.Length < 1) return "";
            byte[] bytes = new byte[stringTobyte.Length];

            for (int i = 0; i < stringTobyte.Length; i++)
            {
                string tmp = stringTobyte[i];
                if (tmp[0] == '0') tmp = tmp.Substring(1, 2);

                bytes[i] = Convert.ToByte(tmp);
            }

            var DecryptString = Encoding.UTF8.GetString(bytes);
            return DecryptString;
        }

        private static int GetCountOfDays(int year, int month, int day)
        {
            int iterator = year * 365;
            for (int i = 1; i <= month; i++) iterator += GetcountOfDaysInMonth(i);

            if (year % 4 != 0) iterator++;
            return iterator += day;
        }

        private static int GetcountOfDaysInMonth(int Month)
        {
            return 28 + (Month + Month / 8) % 2 + 2 % Month + 1 / Month * 2;
        }
    }
}