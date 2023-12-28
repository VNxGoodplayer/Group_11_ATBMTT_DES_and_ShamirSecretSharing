using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_ATBMTT_interface1.DES
{
    // Lớp này có mục đích là chuyển đổi giữa hệ hexa và hệ nhị phân đối với khoá; chuyển đổi kí tự và hệ nhị phân
    internal class Conversion_Function
    {
        // Với mục đích sử dụng Tiếng việt trong bộ mã UNICODE UTF-8
        // nên các kí tự được biểu diễn bởi 2 bytes = 16 bits
        // Đầu vào của DES là 64 bits => Mỗi lần đầu vào hàm chỉ có 4 kí tự và trả ra chuỗi 64 kí tự bits
        public static string char_to_binary(string plain_text_string)
        {
            string temp = "";
            for (int i = 0; i < plain_text_string.Length; i++)
            {
                temp += Convert.ToString(plain_text_string[i], 2).PadLeft(16, '0');
            }
            return temp;
        }
        // Đầu vào là dãy kí tự bit dài 64 bits; trả về kí tự trong bộ mã UNICODE UTF-8 
        public static string binary_to_char(string plain_text_binary)
        {
            string temp = "";
            for (int i = 0; i < plain_text_binary.Length / 16; i++)
            {
                int a = Convert.ToInt32(plain_text_binary.Substring(16 * i, 16), 2);
                char k = (char)a;
                temp = temp + k;
            }

            return temp;
        }
        // Đầu vào là dãy nhị phân; đầu ra là dãy Hexa
        public static string binary_to_hexa(string binary_string)
        {
            string temp = "";
            for (int i = 0; i < binary_string.Length / 4; i++)
            {
                string binary4 = binary_string.Substring(i * 4, 4);
                int decimal_number = Convert.ToInt32(binary4, 2);
                temp += decimal_number.ToString("X");
            }
            return temp;
        }
        public static string hexa_to_binary(string hexa_string)
        {
            hexa_string.ToUpper();
            int decimal_number;
            string temp = "";
            for (int i = 0; i < hexa_string.Length; i++)
            {
                decimal_number = Convert.ToInt32(hexa_string[i].ToString(), 16);
                temp += Convert.ToString(decimal_number, 2).PadLeft(4, '0');
            }
            return temp;
        }
    }
}
