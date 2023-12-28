using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BTL_ATBMTT_interface1
{
    /// <summary>
    /// Interaction logic for Window_Nhap_Khoa.xaml
    /// </summary>
    public partial class Window_Nhap_Khoa : Window
    {
        public int index {  get; set; }
        private BTL_ATBMTT_interface1.MainWindow w1;
        public Window_Nhap_Khoa(BTL_ATBMTT_interface1.MainWindow w1, int index)
        {
            this.w1 = w1;
            this.index = index;
            InitializeComponent();
        }
        private void clk_ok(object sender, RoutedEventArgs e)
        {
            //var temp = new object<int, string, string>(index, tb_KhoaX.Text, tb_KhoaY.Text);
            w1.dtg_ShamirKey.Items.RemoveAt(index);
            w1.dtg_ShamirKey.Items.Add(new
            {
                STT = index+1,
                k_x = tb_KhoaX.Text.ToString(),
                k_y = tb_KhoaY.Text.ToString(),
            });
            this.Close();
        }
    }
}
