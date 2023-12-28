using BTL_ATBMTT_interface1.DES;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using Shamir_secret_v1;
using System.Data;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;


namespace BTL_ATBMTT_interface1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool check = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void clk_MaHoa(object sender, RoutedEventArgs e)
        {
            string plain_text = tb_input.Text;
            string key = tb_KeyMaHoa.Text;

            string cipher_text = DES_Function.Encrypt_DES(plain_text, key);
            tb_output.Text = cipher_text;
        }

        private void clk_GiaiMa(object sender, RoutedEventArgs e)
        {
            string cipher_text = tb_input.Text;
            string key = tb_KeyMaHoa.Text;

            string plaint_text = DES_Function.Decrypt_DES(cipher_text, key);
            tb_output.Text = plaint_text;
        }

        private void clk_TaoKhoa(object sender, RoutedEventArgs e)
        {
            try
            {
                string key_DES = tb_DesKey.Text;

                int n = Convert.ToInt32(tb_SoKhoaMuonTao.Text);
                int k = Convert.ToInt32(tb_SoKhoaToiThieu.Text);

                List<Tuple<int, BigInteger>> points = new List<Tuple<int, BigInteger>>();

                Share_Secret.secret_sharing(key_DES, n, k, ref points);

                int count = 0;

                var kq =
                    from p in points
                    select new
                    {
                        STT = count++,
                        k_x = p.Item1,
                        k_y = p.Item2,
                    };

                dtg_ShamirKey.ItemsSource = kq.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clk_GiaiKhoa(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Tuple<int, BigInteger>> points = new List<Tuple<int, BigInteger>>();
                foreach (var item in dtg_ShamirKey.Items)
                {
                    // Lấy các thuộc tính của DataGrid
                    Type t = item.GetType();
                    PropertyInfo[] temp = t.GetProperties();
                    
                    // Tạo phần tử để thêm vào Danh sách points
                    
                    int x = Convert.ToInt32(temp[1].GetValue(item).ToString());
                    //MessageBox.Show(x.ToString());
                    BigInteger y = BigInteger.Parse(temp[2].GetValue(item).ToString());

                    //MessageBox.Show("Ok");
                    var ele_point = new Tuple<int, BigInteger>(x, y);

                    points.Add(ele_point);
                }

                string keyDES_restore = Generate_Secret.restore(points);

                tb_DesKey.Text = keyDES_restore;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tb_SoKhoaToiThieu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_DesKey.Text != "") return;
            dtg_ShamirKey.Items.Clear();
            if (tb_SoKhoaToiThieu.Text == "") return;
            int k = Convert.ToInt32(tb_SoKhoaToiThieu.Text);

            for (int i = 0; i < k; i++)
            {
                var temp = new { STT = i + 1, X = "", Y = "" };
                dtg_ShamirKey.Items.Add(temp);
            }
        }

        private void dtg_ShamirKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dtg_ShamirKey.SelectedItem == null) return;
            Window_Nhap_Khoa w2 = new Window_Nhap_Khoa(this, dtg_ShamirKey.SelectedIndex);
            dtg_ShamirKey.SelectedItem = null;
            //w2.tb_tieude = "Nhập vào khoá con"
            w2.tb_KhoaX.Focus();
            w2.Show();
        }

        private void bt_Xoa_Click(object sender, RoutedEventArgs e)
        {
            tb_input.Text = "";
            tb_KeyMaHoa.Text = "";
            tb_output.Text = "";
            tb_input.Focus();
        }

        private void bt_XoaShamir_Click(object sender, RoutedEventArgs e)
        {
            
            dtg_ShamirKey.ItemsSource = null;
            dtg_ShamirKey.Items.Clear();
            tb_SoKhoaToiThieu.Text = "";
            tb_DesKey.Text = "";
            tb_SoKhoaMuonTao.Text = "";
            tb_DesKey.Focus();
        }

        private void w1_Loaded(object sender, RoutedEventArgs e)
        {
            CenterWindowOnScreen();
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;

            this.Left = (screenWidth - windowWidth) / 2;
            this.Top = (screenHeight - windowHeight) / 2;
        }

        private void bt_NhapTuFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word Files|*.doc;*.docx|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Đọc dữ liệu từ file Word và hiển thị lên txtCipherText
                string fileContent = ReadWordFile(filePath);
                tb_input.Text = fileContent;
            }
        }

        private string ReadWordFile(string filePath)
        {
            string text = "";

            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
                {
                    Body body = doc.MainDocumentPart.Document.Body;

                    foreach (Paragraph paragraph in body.Elements<Paragraph>())
                    {
                        foreach (Run run in paragraph.Elements<Run>())
                        {
                            foreach (Text t in run.Elements<Text>())
                            {
                                text += t.Text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý exception nếu có
                text = "Error reading the Word file: " + ex.Message;
            }

            return text;
        }

        private void bt_NhapTuFile2_Click(object sender, RoutedEventArgs e)
        {
            //tb_DesKey.Text = " ";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    int n = Convert.ToInt32(tb_SoKhoaToiThieu.Text);

                    string filePath = openFileDialog.FileName;

                    // Đọc dữ liệu từ file văn bản
                    string fileContent = ReadTxtFile(filePath);

                    // Tách dữ liệu thành các dòng
                    string[] lines = fileContent.Split('\n');

                    int index = 1;
                    dtg_ShamirKey.Items.Clear();
                    for (int i=0; i<n; i++)
                    {
                        string[] data = lines[i].Split('-');
                        
                        dtg_ShamirKey.Items.Add(new
                        {
                            STT = index,
                            k_x = data[0],
                            k_y = data[1],
                        });
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //tb_DesKey.Text = " ";
        }

        private string ReadTxtFile(string filePath)
        {
            try
            {
                // Đọc toàn bộ nội dung từ file và trả về chuỗi
                string fileContent = File.ReadAllText(filePath);

                return fileContent;
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi đọc file
                MessageBox.Show($"Có lỗi khi đọc file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void bt_KeyFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Đọc dữ liệu từ file văn bản và hiển thị lên tb_DesKey
                string fileContent = ReadTxtFile(filePath);
                tb_KeyMaHoa.Text = fileContent;
            }
        }

        private void bt_File_DES_key_Shamir(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Đọc dữ liệu từ file văn bản và hiển thị lên tb_DesKey
                string fileContent = ReadTxtFile(filePath);
                tb_DesKey.Text = fileContent;
            }
        }

        private void clk_Inkhoa(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Đọc dữ liệu từ file văn bản và hiển thị lên tb_DesKey
                string fileContent = "";
                foreach(var item in dtg_ShamirKey.Items)
                {
                    Type t = item.GetType();
                    PropertyInfo[] info = t.GetProperties();
                    fileContent += info[1].GetValue(item).ToString() + "-"
                        + info[2].GetValue(item).ToString() + "\n";
                }
                //MessageBox.Show(fileContent);
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.Write(fileContent);
                };
                return;
            }
        }
    }
}