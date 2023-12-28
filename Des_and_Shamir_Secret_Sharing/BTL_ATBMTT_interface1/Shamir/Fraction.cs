using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

// Định nghĩa lớp phân số, xử lý tối giản phép toán cộng, nhân phân số
namespace Shamir_secret_v1
{
    internal class Fraction
    {
        public BigInteger tuso {  get; set; }
        public BigInteger mauso {  get; set; }
        public Fraction(BigInteger tuso, BigInteger mauso)
        {
            this.tuso = tuso;
            this.mauso = mauso;
        }

        // Phuong thuc toi gian phan
        // Hàm trả về Ước số chung lớn nhất
        public BigInteger fgcd(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        public void reduce_fraction(ref Fraction other)
        {
            BigInteger gcd = fgcd(other.tuso, other.mauso);
            other.tuso = other.tuso / gcd;
            other.mauso = other.mauso / gcd;
        }

        // Định nghĩa toán tử +
        public static Fraction operator +(Fraction a, Fraction b)
        {
            Fraction temp = new Fraction(0, 0);
            temp.tuso = a.tuso * b.mauso + b.tuso * a.mauso;
            temp.mauso = a.mauso * b.mauso;
            temp.reduce_fraction(ref temp);
            return temp;
        }
        // Định nghĩa toán tử *
        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction temp = new Fraction(0, 0);
            temp.tuso = a.tuso * b.tuso;
            temp.mauso = a.mauso * b.mauso;
            temp.reduce_fraction(ref temp);
            return temp;
        }
    }
}
