using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

// Chia khoá DES thành n khoá và ngưỡng là k
namespace Shamir_secret_v1
{
    internal class Share_Secret
    {
        // Sử dụng tính hàm Y = poly[0] + x*poly[1] + x^2*poly[2] + ...
        // Đầu vào gồm x và mảng chứa các hệ số
        // Đầu ra là giá trị của Y
        public static BigInteger caculate_Y(int x, BigInteger[] poly)
        {
            BigInteger y = BigInteger.Parse("0");
            BigInteger temp = 1; // Tính các giá trị mũ x
            foreach (var ele in poly)
            {
                y = y + temp * ele;
                temp = temp * x;
            }
            return y;
        }
        // Chia bản rõ thành n phần với ngưỡng k
        // Đầu vào gồm bản rõ (key_DES) ở dạng chuỗi;
        // số lượng phần (n); ngưỡng (k); k<=n ; danh sách chứa cặp khoá
        // Trả về được danh sách cặp khoá
        public static void secret_sharing(string key_DES, int n, int k, ref List<Tuple<int, BigInteger>> points)
        {
            // Đầu vào là khoá DES có 16 kí tự
            key_DES = '0' + key_DES; // Thêm 0 vào đầu để bỏ qua việc chuyển đổi có tính bù 2, nếu bit đầu là 1 thì khi chuyển sẽ là số âm

            // Chuyển khoá DES từ hệ 16 sang hệ 10 làm đầu vào cho thuật toán Shamir
            BigInteger s = Convertion_Shamir.hexa_to_decimal(key_DES);

            // **Xây dựng đa thức f bậc k-1**

            // Lưu trữ các hệ số đa thức f có k hệ số;
            // có kiểu BigInteger do có poly[0] chứa bản rõ
            BigInteger[] poly = new BigInteger[k]; 

            poly[0] = s; // Hệ số tự do chứa bản rõ

            // Sinh ngẫu nhiên k-1 hệ số còn lại và khác 0
            Random r = new Random(); // Khởi tạo đối tượng ngẫu nhiên
            for(int i=1; i<k; i++)
            {
                int p = 0;
                while(p == 0)
                {
                    p = r.Next(); // p được tạo ngẫu nhiên giới hạn trong kiểu int
                }
                poly[i] = p;
            }

            // Sinh n điểm ngẫu nhiên để chia sẻ
            // Giá trị x là ngẫu nhiên và kiểm tra không trùng
            for (int i = 0; i < n; i++)
            {
                int x = 0;
                while (x == 0 || check_trung(x, points)==true) // Kiểm tra x khác 0 và x không trùng trước đó
                {
                    x = r.Next(); // Sinh ngẫu nhiên x
                }

                BigInteger y = caculate_Y(x, poly);

                // Tạo điểm chia sẻ
                var temp = new Tuple<int, BigInteger>(x, y);
                points.Add(temp);
            }
        }
        // Kiểm tra khoá trùng; nếu trùng trả về 1
        public static bool check_trung(int x, List<Tuple<int, BigInteger>> points)
        {
            foreach (var ele in points)
            {
                if (ele.Item1 == x) return true;
            }
            return false;
        }
    }
}
