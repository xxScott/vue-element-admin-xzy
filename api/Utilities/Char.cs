using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Char
    {
        public static string GetRandomCode(int length)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                number = random.Next();code = (char)('0' + (char)(number % 10));
                checkCode += code.ToString();
            }
            return checkCode;
        }
    }
}
