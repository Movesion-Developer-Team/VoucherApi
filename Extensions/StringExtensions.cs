using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public static class StringExtensions
    {
        private static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        /// <summary>
        /// Method is used to remove spaces in front of and in the end of the Login string
        /// </summary>
        public static string RemoveSpacesForLogin(string userName)
        {
            int nameLength = userName.Length;
            if (nameLength == 0)
            {
                return userName;
            }

            if (userName.Contains(" "))
            {

                char firstLetter = FirstLetterOrNot(userName);
                char lastLetter = FirstLetterOrNot(Reverse(userName));

                if (!Char.IsLetter(firstLetter))
                {
                    return userName;
                }

                int firstLetterIndex = userName.IndexOf(firstLetter);
                int lastLetterIndex = userName.LastIndexOf(lastLetter);

                while (firstLetterIndex > 0 || lastLetterIndex < nameLength-1)
                {
                    var firstSpaceIndex = userName.IndexOf(" ", StringComparison.Ordinal);
                    var lastSpaceIndex = userName.LastIndexOf(" ", StringComparison.Ordinal);
                    firstLetterIndex = userName.IndexOf(firstLetter);
                    lastLetterIndex = userName.LastIndexOf(lastLetter);

                    if (firstSpaceIndex < firstLetterIndex && firstSpaceIndex >= 0)
                    {
                        userName = userName.Remove(firstSpaceIndex, 1);
                    }

                    if (lastSpaceIndex > lastLetterIndex && lastLetterIndex >= 0)
                    {
                        userName = userName.Remove(lastSpaceIndex, 1);
                    }
                    nameLength = userName.Length;

                }

                return userName;

            }
            else
            {
                return userName;
            }

        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static char FirstLetterOrNot(string s)
        {
            char currentChar = s[0];
            foreach (var symbol in s)
            {
                if (Char.IsLetter(symbol))
                {
                    return symbol;
                }
            }

            return currentChar;
        }

        public static string RandomCodeGenerator(int size)
        {
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % Chars.Length;

                result.Append(Chars[idx]);
            }

            return result.ToString();
        }

    }
}