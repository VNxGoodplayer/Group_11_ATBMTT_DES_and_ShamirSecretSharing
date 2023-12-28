using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_ATBMTT_interface1.DES
{
    internal class Key_Generation
    {
        // Hàm sinh khoá: Đầu vào là khoá đầu tiên 64 bits, đầu ra một mảng 16 khoá 48 bits
        public static void generate_key(ref string[] key)
        {
            string key_binary = Conversion_Function.hexa_to_binary(key[0]); // Chuyển đổi khoá từ hệ Hexa thành hệ Nhị phân
            Console.WriteLine(key_binary);
            string key_PC_1 = PC_1_Block(key_binary); // Khoá sau khi qua khối PC-1
            Console.WriteLine(key_PC_1);
            // Chia khoá 56 bits thành hai khối C0, D0; mỗi khối có 32 bits
            string[] C = new string[17]; // Mảng 16 chuỗi 28 bits Nửa trái khoá
            string[] D = new string[17]; // Mảng 16 chuỗi 28 bits Nửa phải khoá
            C[0] = key_PC_1.Substring(0, 28);
            D[0] = key_PC_1.Substring(28, 28);

            for (int i = 1; i <= 16; i++)
            {

                if (i == 1 || i == 2 || i == 9 || i == 16)
                {
                    // Tạo Nửa C, Nửa D với i = 1, 2, 9, 16 dịch trái 1 bit
                    C[i] = Left_Shift_i(C[i - 1], 1);
                    D[i] = Left_Shift_i(D[i - 1], 1);
                }
                else
                {
                    // Tạo Nửa C, Nửa D với i còn lại dịch trái 2 bit
                    C[i] = Left_Shift_i(C[i - 1], 2);
                    D[i] = Left_Shift_i(D[i - 1], 2);
                }
                key[i] = PC_2_Block(C[i], D[i]); // Chuỗi sau khi qua khối PC-2 là key[i]
            }
        }
        // Khối PC-1: Đầu vào là khoá có chuỗi 64 bits, đầu ra là chuỗi 56 bits
        public static string PC_1_Block(string key_start)
        {
            byte[] PC_1_block =
               { 57, 49, 41, 33, 25, 17, 9,
                1, 58, 50, 42, 34, 26, 18,
                10, 2, 59, 51, 43, 35, 27,
                19, 11, 3, 60, 52, 44, 36,
                63, 55, 47, 39, 31, 23, 15,
                7, 62, 54, 46, 38, 30, 22,
                14, 6, 61, 53, 45, 37, 29,
                21, 13, 5, 28, 20, 12, 4 };
            string temp = "";
            for (int i = 0; i < PC_1_block.Length; i++)
            {
                temp = temp + key_start[PC_1_block[i] - 1].ToString();
            }
            return temp;
        }
        // Khối dịch trái: Đầu vào chuỗi 28 bits, tham số i là số bit cần dịch; đầu ra là chuỗi 28 bits
        public static string Left_Shift_i(string binary_string, int i)
        {
            string temp = "";
            temp = binary_string.Substring(i);
            temp = temp + binary_string.Substring(0, i);
            return temp;
        }
        // Khối PC-2: Đầu vào là 2 chuỗi C[i] (28 bits), D[i] (28 bits), đầu ra là chuỗi 48 bits
        public static string PC_2_Block(string C, string D)
        {
            string CD = C + D;
            byte[] PC2block =
               { 14, 17, 11, 24, 1, 5,
                3, 28, 15, 6, 21, 10,
                23, 19, 12, 4, 26, 8,
                16, 7, 27, 20, 13, 2,
                41, 52, 31, 37, 47, 55,
                30, 40, 51, 45, 33, 48,
                44, 49, 39, 56, 34, 53,
                46, 42, 50, 36, 29, 32 };
            string temp = "";
            for (int i = 0; i < PC2block.Length; i++)
            {
                temp = temp + CD[PC2block[i] - 1];
            }
            return temp;
        }
    }
}
