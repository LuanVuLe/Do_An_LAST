using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;
using System.Windows.Data;
using System.Linq;

namespace Do_An
{
    public partial class NVQL_QLD : UserControl
    {
        private readonly DiemBLL diemBLL = new DiemBLL();

        public NVQL_QLD()
        {
            InitializeComponent();

            // ĐĂNG KÝ SỰ KIỆN Loaded để tự động load dữ liệu khi mở UserControl
            this.Loaded += NVQL_QLD_Loaded;
        }

        private void NVQL_QLD_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadKhoaHoc();
                LoadAllDiem();

                btnTimKiem.Click += BtnTimKiem_Click;
                btnLamMoi.Click += BtnLamMoi_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo giao diện: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadKhoaHoc()
        {
            var dtKhoaHoc = diemBLL.LayDanhSachKhoaHoc();
            cbKhoaHoc.ItemsSource = dtKhoaHoc.DefaultView;
            cbKhoaHoc.SelectedIndex = -1;

            // Reset các ComboBox phụ thuộc
            cbMonHoc.ItemsSource = null;
            cbLopHoc.ItemsSource = null;
        }

        private void LoadAllDiem()
        {
            txtMaHV.Text = string.Empty;
            txtPlaceholder.Visibility = Visibility.Visible;

            DataTable dt = diemBLL.LayTatCaDiem();
            dgDiem.ItemsSource = dt.DefaultView;
        }

        private void cbKhoaHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbMonHoc.ItemsSource = null;
            cbLopHoc.ItemsSource = null;

            if (cbKhoaHoc.SelectedValue == null) return;

            try
            {
                int maKH = Convert.ToInt32(cbKhoaHoc.SelectedValue);
                var dtMon = diemBLL.LayMonHocTheoKhoaHoc(maKH);

                if (dtMon != null && dtMon.Rows.Count > 0)
                {
                    cbMonHoc.ItemsSource = dtMon.DefaultView;
                    cbMonHoc.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbMonHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbLopHoc.ItemsSource = null;

            if (cbMonHoc.SelectedValue == null) return;

            try
            {
                // Nếu bạn có hàm LayLopHocTheoMaMonHoc thì gọi ở đây
                var dtLop = diemBLL.LayDanhSachLopHoc(); // tạm thời lấy tất cả
                cbLopHoc.ItemsSource = dtLop.DefaultView;
                cbLopHoc.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải lớp học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? maKH = cbKhoaHoc.SelectedValue != null ? (int?)Convert.ToInt32(cbKhoaHoc.SelectedValue) : null;
                int? maMH = cbMonHoc.SelectedValue != null ? (int?)Convert.ToInt32(cbMonHoc.SelectedValue) : null;
                int? maLop = cbLopHoc.SelectedValue != null ? (int?)Convert.ToInt32(cbLopHoc.SelectedValue) : null;

                string keyword = string.IsNullOrWhiteSpace(txtMaHV.Text) || txtPlaceholder.Visibility == Visibility.Visible
                    ? null
                    : txtMaHV.Text.Trim();

                DataTable dt = diemBLL.TimDiem(maLop, maMH, maKH, keyword);
                dgDiem.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            LoadAllDiem();
            LoadKhoaHoc();
        }

        private void txtMaHV_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtMaHV.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void cbLopHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}