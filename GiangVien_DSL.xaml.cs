using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class GiangVien_DSL : Window
    {
        private readonly LopHocBLL lopHocBLL = new LopHocBLL();
        private readonly int maGV; // Mã giảng viên đang đăng nhập

        public GiangVien_DSL(int _maGV)
        {
            InitializeComponent();
            maGV = _maGV;
            LoadHocKy();
            LoadTuan();
        }

        private void LoadHocKy()
        {
            // Giả lập danh sách học kỳ
            cbHocKy.Items.Add("Học kỳ 1 - 2025");
            cbHocKy.Items.Add("Học kỳ 2 - 2025");
            cbHocKy.SelectedIndex = 0;
        }

        private void LoadTuan()
        {
            for (int i = 1; i <= 20; i++)
                cbTuan.Items.Add("Tuần " + i);
            cbTuan.SelectedIndex = 0;
        }

        private void btnTaiLich_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dsLop = lopHocBLL.LayDanhSachLopTheoGiangVien(maGV);
                HienThiThoiKhoaBieu(dsLop);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thời khóa biểu: " + ex.Message);
            }
        }

        private void HienThiThoiKhoaBieu(DataTable dsLop)
        {
            grdTKB.Children.Clear();

            // Tạo khung lưới trống (8 cột, 15 dòng)
            for (int i = 0; i < 8 * 15; i++)
            {
                Border border = new Border
                {
                    BorderBrush = System.Windows.Media.Brushes.LightGray,
                    BorderThickness = new Thickness(0.5),
                    Margin = new Thickness(0.5)
                };

                TextBlock txt = new TextBlock
                {
                    Text = "",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 13
                };

                border.Child = txt;
                grdTKB.Children.Add(border);
            }

            // Đổ dữ liệu lớp vào khung (giả lập vị trí cột dòng)
            foreach (DataRow row in dsLop.Rows)
            {
                string tenLop = row["TenLop"].ToString();
                string phong = row["Phong"].ToString();
                string thoiGian = row["ThoiGian"].ToString(); // VD: "Thứ 2-4-6 (18h-20h)"

                int thu = LayThuTuTuChuoi(thoiGian); // Chuyển "Thứ 2" → 1
                if (thu >= 1 && thu <= 7)
                {
                    int index = thu; // Dòng đầu tiên để trống
                    if (index < grdTKB.Children.Count)
                    {
                        var border = grdTKB.Children[index] as Border;
                        var txt = border.Child as TextBlock;
                        txt.Text = $"{tenLop}\nPhòng: {phong}\n{thoiGian}";
                    }
                }
            }
        }

        private int LayThuTuTuChuoi(string thoiGian)
        {
            if (thoiGian.Contains("2")) return 1;
            if (thoiGian.Contains("3")) return 2;
            if (thoiGian.Contains("4")) return 3;
            if (thoiGian.Contains("5")) return 4;
            if (thoiGian.Contains("6")) return 5;
            if (thoiGian.Contains("7")) return 6;
            if (thoiGian.Contains("CN") || thoiGian.Contains("Chủ nhật")) return 7;
            return -1;
        }
    }
}
