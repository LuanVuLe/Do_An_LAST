using System;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class NVQL_MLH : UserControl
    {
        private readonly LopHocBLL lopHocBLL = new LopHocBLL();

        public NVQL_MLH()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        /// <summary>
        /// Load dữ liệu vào các ComboBox khi khởi tạo
        /// </summary>
        private void LoadComboBoxData()
        {
            try
            {
                // --- Khóa học ---
                cbKhoaHoc.ItemsSource = lopHocBLL.LayDanhSachKhoaHoc().DefaultView;
                cbKhoaHoc.DisplayMemberPath = "TenKH";
                cbKhoaHoc.SelectedValuePath = "MaKh";
                cbKhoaHoc.SelectedIndex = -1;

                // --- Môn học ---
                cbMonHoc.ItemsSource = lopHocBLL.LayDanhSachMonHoc().DefaultView;
                cbMonHoc.DisplayMemberPath = "TenMH";
                cbMonHoc.SelectedValuePath = "MaMH";
                cbMonHoc.SelectedIndex = -1;

                // --- Giáo viên ---
                cbGiaoVien.ItemsSource = lopHocBLL.LayDanhSachGiaoVien().DefaultView;
                cbGiaoVien.DisplayMemberPath = "HoTen";
                cbGiaoVien.SelectedValuePath = "MaGV";
                cbGiaoVien.SelectedIndex = -1;

                // --- Trạng thái lớp ---
                cbTrangThai.SelectedIndex = 0; // Mặc định "Chờ khai giảng"

                // --- Trình độ lớp ---
                cbTrinhDoLop.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu khởi tạo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Khi chọn Khóa học, tự động lọc môn học
        /// </summary>
        private void cbKhoaHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbKhoaHoc.SelectedValue == null) return;

            try
            {
                int maKhoaHoc = Convert.ToInt32(cbKhoaHoc.SelectedValue);
                cbMonHoc.ItemsSource = lopHocBLL.LayMonHocTheoKhoaHoc(maKhoaHoc).DefaultView;
                cbMonHoc.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc môn học theo khóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Khi nhấn Tạo lớp
        /// </summary>
        private void btnTaoLop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tenLop = txtTenLop.Text.Trim();
                string phong = txtPhong.Text.Trim();
                string thoiGian = txtThoiGian.Text.Trim();
                string trangThai = cbTrangThai.SelectedItem is ComboBoxItem cti ? cti.Content.ToString() : "Chờ khai giảng";
                string trinhDo = cbTrinhDoLop.SelectedItem is ComboBoxItem ctd ? ctd.Content.ToString() : null;

                if (string.IsNullOrEmpty(tenLop) || cbMonHoc.SelectedValue == null || cbGiaoVien.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Tên lớp, Môn học và Giáo viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(trinhDo))
                {
                    MessageBox.Show("Vui lòng chọn Trình độ áp dụng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int siSoToiDa = 20;
                if (!int.TryParse(txtSiSoToiDa.Text.Trim(), out siSoToiDa) || siSoToiDa <= 0)
                    siSoToiDa = 20;

                int maGV = Convert.ToInt32(cbGiaoVien.SelectedValue);
                int maMH = Convert.ToInt32(cbMonHoc.SelectedValue);

                string message = lopHocBLL.TaoLopHoc(
                    tenLop,
                    trinhDo,
                    phong,
                    thoiGian,
                    siSoToiDa,
                    trangThai,
                    maGV,
                    maMH
                );

                MessageBox.Show(message, "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);
                btnLamMoi_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Làm mới form
        /// </summary>
        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            txtTenLop.Clear();
            txtPhong.Clear();
            txtThoiGian.Text = "VD: Thứ 2-4-6 (18h-20h)";
            txtSiSoToiDa.Text = "20";
            txtMaLop.Clear();

            cbKhoaHoc.SelectedIndex = -1;
            cbMonHoc.SelectedIndex = -1;
            cbGiaoVien.SelectedIndex = -1;
            cbTrinhDoLop.SelectedIndex = -1;
            cbTrangThai.SelectedIndex = 0;
        }

        /// <summary>
        /// Thoát form
        /// </summary>
        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        // Tránh bỏ trống nếu cần dùng
        private void cbMonHoc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
