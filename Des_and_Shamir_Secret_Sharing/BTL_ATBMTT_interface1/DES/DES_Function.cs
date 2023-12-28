using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BTL_ATBMTT_interface1.DES
{
    internal class DES_Function
    {
        // Đầu vào gồm có plain text và key ban đầu (key[0])
        public static string Encrypt_DES(string plain_text, string base_key)
        {
            string cipher_text = "";

            // Hàm sinh khoá
            string[] key = new string[17];
            key[0] = base_key;
            Key_Generation.generate_key(ref key);

            // Nếu chuỗi plain_text có số lượng kí tự không chia hết cho 4
            // thì cần thêm kí tự 'space' để đủ chia hết cho 4
            int du = plain_text.Length % 4;
            if (du != 0)
            {
                for (int i = 0; i < 4 - du; i++)
                {
                    plain_text += ' ';
                }
            }

            for (int i = 0; i < plain_text.Length / 4; i++)
            {
                string khoi_ki_tu = plain_text.Substring(4 * i, 4); // Tách từng khối 8 kí tự để mã hoá
                string khoi_nhi_phan = Conversion_Function.char_to_binary(khoi_ki_tu); // Chuyển khối 8 kí tự sang khối nhị phân 64 bits
                //Console.WriteLine("Khoi nhi phan: "+khoi_nhi_phan);

                string khoi_IP_thuan = IP_Block.IP_Block_thuan(khoi_nhi_phan); // Khối kí tự nhị phân sau khi qua khối IP thuận
                //Console.WriteLine("Hoan vi IP thuan: " + khoi_IP_thuan);

                // Chia đôi khối 64 bits thành 2 khối L0, R0; mỗi khối 32 bits
                string[] L = new string[17];
                string[] R = new string[17];
                L[0] = khoi_IP_thuan.Substring(0, 32);
                R[0] = khoi_IP_thuan.Substring(32, 32);

                for (int j = 1; j <= 16; j++)
                {
                    L[j] = R[j - 1];
                    //Console.WriteLine(L[j]);

                    R[j] = xor(L[j - 1], Function_F.calculate_F(R[j - 1], key[j]));
                    //Console.WriteLine(R[j]);
                }
                //for(int j=1; j<=16; j++)
                //{
                //    Console.WriteLine("L["+j+"] = " + L[j]);
                //    Console.WriteLine("R[" + j + "] = " + R[j]);
                //}

                string khoi_IP_nguoc = R[16] + L[16];
                //Console.WriteLine("R+L:  " + R[16] + "  " + L[16]);
                khoi_IP_nguoc = IP_Block.IP_Block_nguoc(khoi_IP_nguoc);
                //Console.WriteLine("Khoi IP nguoc: " + khoi_IP_nguoc);

                //Console.WriteLine(Conversion_Function.binary_to_hexa(khoi_IP_nguoc));
                string khoi_ma_hoa = Conversion_Function.binary_to_char(khoi_IP_nguoc);

                cipher_text = cipher_text + khoi_ma_hoa;
            }
            return cipher_text;
        }
        public static string Decrypt_DES(string cipher_text, string base_key)
        {
            string plain_text = "";

            // Hàm sinh khoá
            string[] key = new string[17];
            key[0] = base_key;
            Key_Generation.generate_key(ref key);

            for (int i = 0; i < cipher_text.Length / 4; i++)
            {
                string khoi_ki_tu = cipher_text.Substring(4 * i, 4); // Tách từng khối 8 kí tự để mã hoá
                //Console.WriteLine("Khoi ki tu: " + khoi_ki_tu);

                string khoi_nhi_phan = Conversion_Function.char_to_binary(khoi_ki_tu); // Chuyển khối 8 kí tự sang khối nhị phân 64 bits
                //Console.WriteLine("Khoi nhi phan: " + khoi_nhi_phan);

                string khoi_IP_thuan = IP_Block.IP_Block_thuan(khoi_nhi_phan); // Khối kí tự nhị phân sau khi qua khối IP thuận

                //Console.WriteLine("Hoan vi IP thuan: " + khoi_IP_thuan);

                // Chia đôi khối 64 bits thành 2 khối L0, R0; mỗi khối 32 bits
                string[] L = new string[17];
                string[] R = new string[17];
                L[0] = khoi_IP_thuan.Substring(0, 32);
                R[0] = khoi_IP_thuan.Substring(32, 32);

                for (int j = 1; j <= 16; j++)
                {
                    L[j] = R[j - 1];
                    //Console.WriteLine(L[j]);

                    R[j] = xor(L[j - 1], Function_F.calculate_F(R[j - 1], key[17 - j]));
                    //Console.WriteLine(R[j]);
                }

                string khoi_IP_nguoc = R[16] + L[16];
                khoi_IP_nguoc = IP_Block.IP_Block_nguoc(khoi_IP_nguoc);
                //Console.WriteLine("Khoi IP nguoc: " + khoi_IP_nguoc);

                //Console.WriteLine(Conversion_Function.binary_to_hexa(khoi_IP_nguoc));
                string khoi_giai_ma = Conversion_Function.binary_to_char(khoi_IP_nguoc);

                plain_text = plain_text + khoi_giai_ma;
            }
            return plain_text;
        }
        public static string xor(string left, string f)
        {
            string temp = "";
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] == f[i]) temp += '0';
                else temp += '1';
            }
            return temp;
        }
    }
}
