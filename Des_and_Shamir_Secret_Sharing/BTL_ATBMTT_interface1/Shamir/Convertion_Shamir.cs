using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shamir_secret_v1
{
    internal class Convertion_Shamir
    {
        // Hàm chuyển đồi hệ Hexa sang hệ 10
        public static BigInteger hexa_to_decimal(string hexa_string)
        {
            BigInteger temp = BigInteger.Parse(hexa_string, System.Globalization.NumberStyles.HexNumber);
            return temp;
        }
        // Hàm chuyển đổi hệ 10 sang hệ Hexa
        public static string decimal_to_hexa(BigInteger dec)
        {
            string temp = dec.ToString("X");
            while (temp[0] == '0')
            {
                temp = temp.Remove(0, 1);
            }
            return temp;
        }
    }
}
