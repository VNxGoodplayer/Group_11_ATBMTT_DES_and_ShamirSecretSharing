using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

// Khôi phục lại bí mật
namespace Shamir_secret_v1
{
    internal class Generate_Secret
    {
        // Đầu vào là các cặp khoá (x, y) tối thiểu k khoá
        // Đầu ra là bản bí mật được khôi phục hay chính là hệ số tự do
        public static string restore(List<Tuple<int, BigInteger>> points)
        {
            Fraction result = new Fraction(0, 1); // Lưu tổng các hệ số tự do của hàm Lagrange
            for (int i = 0; i < points.Count; i++)
            {
                Fraction l = new Fraction(points[i].Item2, 1); // Lưu các giá trị Lagrange nhỏ qua mỗi point
                for (int j = 0; j < points.Count; j++)
                {
                    if (j != i)
                    {
                        Fraction temp = new
                            Fraction(-points[j].Item1, points[i].Item1 - points[j].Item1); // Công thức tính L[i] tự do
                        l = l * temp;
                    }
                }
                result = result + l;
            }
            //return result.tuso;
            string s_restore_hexa = Convertion_Shamir.decimal_to_hexa(result.tuso);
            return s_restore_hexa;
        }
    }
}
