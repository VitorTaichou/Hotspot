using System;
using System.Linq;
using System.Text;

namespace Hotspot.Tools.Code
{
    public class GeneratorService : IGenerator
    {
        public string GenerateCourtesy(int n)
        {
            char[] s = EncodeInt32AsString(n, 3).ToCharArray();
            char[] r = RandomString(3).ToCharArray();

            string code = "";
            code += r[2];
            code += s[0];
            code += r[1];
            code += s[1];
            code += r[0];
            code += s[2];

            return code;
        }

        public string GenerateEightDigit(int n)
        {
            char[] s = EncodeInt32AsString(n, 4).ToCharArray();
            char[] r = RandomString(4).ToCharArray();

            string code = "";
            code += r[3];
            code += s[0];
            code += r[2];
            code += s[1];
            code += r[1];
            code += s[2];
            code += r[0];
            code += s[3];

            return code;
        }

        public string GenerateSevenDigit(int n)
        {
            char[] s = EncodeInt32AsString(n, 4).ToCharArray();
            char[] r = RandomString(3).ToCharArray();

            string code = "";
            code += s[0];
            code += r[2];
            code += s[1];
            code += r[1];
            code += s[2];
            code += r[0];
            code += s[3];

            return code;
        }

        public string GenerateNineDigit(int n)
        {
            char[] s = EncodeInt32AsString(n, 5).ToCharArray();
            char[] r = RandomString(4).ToCharArray();

            string code = "";
            code += s[0];
            code += r[3];
            code += s[1];
            code += r[2];
            code += s[2];
            code += r[1];
            code += s[3];
            code += r[0];
            code += s[4];

            return code;
        }

        public string GenerateTenDigit(int n)
        {
            int size = 5;
            Random random = new Random();

            char[] s = EncodeInt32AsString(n, size).ToCharArray();
            char[] r = RandomString(size).ToCharArray();

            string code = "";
            code += r[4];
            code += s[4];
            code += r[3];
            code += s[0];
            code += r[2];
            code += "-";
            code += s[3];
            code += r[1];
            code += s[1];
            code += r[0];
            code += s[2];

            return code;
        }

        public string GenerateTwelveDigit(int n)
        {
            int size = 5;
            Random random = new Random();

            char[] s = EncodeInt32AsString(n, size).ToCharArray();
            char[] r = RandomString(size).ToCharArray();

            string code = "";
            code += r[4];
            code += s[4];
            code += r[3];
            code += "-";
            code += s[0];
            code += r[2];
            code += s[3];
            code += r[1];
            code += "-";
            code += s[1];
            code += r[0];
            code += s[2];

            return code;
        }

        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static String EncodeInt32AsString(Int32 input, Int32 maxLength = 0)
        {
            // List of characters allowed in the target string 
            Char[] allowedList = new Char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z' };
            Int32 allowedSize = allowedList.Length;
            StringBuilder result = new StringBuilder(input.ToString().Length);

            Int32 moduloResult;
            while (input > 0)
            {
                moduloResult = input % allowedSize;
                input /= allowedSize;
                result.Insert(0, allowedList[moduloResult]);
            }

            if (maxLength > result.Length)
            {
                result.Insert(0, new String(allowedList[0], maxLength - result.Length));
            }

            if (maxLength > 0)
                return result.ToString().Substring(0, maxLength);
            else
                return result.ToString();
        }
    }
}
