using System;
using System.Windows;
using Do_An.BLL;
using System.Data;
using System.Linq; // Cần thiết cho Enumerable.Select

namespace Do_An
{
    public partial class AdminQLLH_AddLH : Window
    {
        private readonly LopHocBLL lopHocBLL = new LopHocBLL();

        public AdminQLLH_AddLH()
        {
            InitializeComponent();
            LoadComboData();
        }

        private void LoadComboData()
        {
            try
            {
                // Giả định: Bạn đã có các ComboBox (cbGiaoVien, cbMonHoc) trong XAML
                // Nếu vẫn dùng TextBox, ta giữ nguyên logic Tooltip cũ
                DataTable dsGV = lopHocBLL.LayDanhSachGiaoVien();
                if (dsGV.Rows.Count > 0)
                {
                    // Hiển thị gợi ý tên GV trong tooltip (tạm thời)
                    string dsTenGV = string.Join(", ", dsGV.AsEnumerable().Select(r => r["HoTen"].ToString()));
                    // Sửa lỗi tham chiếu đến txtGiaoVien nếu nó không phải là ComboBox
                    // Giả định txtGiaoVien là một TextBox như trong code gốc của bạn
                    //txtGiaoVien.ToolTip = "Danh sách GV: " + dsTenGV; 
                    // Nếu là ComboBox, hãy gán ItemsSource ở đây
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách giáo viên: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. Thu thập và kiểm tra dữ liệu ---
            string tenLop = txtTenLop.Text.Trim();
            string giaoVien = txtGiaoVien.Text.Trim(); // Giữ lại nếu là TextBox
            string siSoText = txtSoHocVien.Text.Trim();

            // THAM SỐ MỚI VÀ CŨ
            // Giả định txtTrinhDoLop là TextBox cho Trình độ
            string trinhDo = "Beginner"; // Dùng giá trị mặc định tạm thời nếu chưa có ComboBox/TextBox Trình độ
            // string trinhDo = txtTrinhDoLop.Text.Trim(); // Nếu bạn đã thêm TextBox/ComboBox này

            string phong = "P101";
            string thoiGian = DateTime.Now.ToString("dd/MM/yyyy HH:mm"); // Format thời gian
            string trangThai = "Chờ khai giảng"; // Giả định

            // Kiếm tra cơ bản
            if (string.IsNullOrWhiteSpace(tenLop))
            {
                MessageBox.Show("Vui lòng nhập tên lớp học.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTenLop.Focus();
                return;
            }

            if (!int.TryParse(siSoText, out int siSo) || siSo <= 0)
            {
                MessageBox.Show("Sĩ số học viên phải là số nguyên dương.", "Sai định dạng", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSoHocVien.Focus();
                return;
            }

            // Lấy mã GV/MH. Trong môi trường thực, cần tra cứu từ DB qua BLL dựa trên tên nhập vào.
            int maGV = 1; // Tạm thời
            int maMH = 1; // Tạm thời

            // --- 2. Gọi BLL với 8 tham số mới ---
            try
            {
                string kq = lopHocBLL.TaoLopHoc(
                    tenLop,     // 1. string tenLop
                    trinhDo,    // 2. string trinhDo (MỚI)
                    phong,      // 3. string phong
                    thoiGian,   // 4. string thoiGian
                    siSo,       // 5. int siSoToiDa
                    trangThai,  // 6. string trangThai
                    maGV,       // 7. int maGV
                    maMH        // 8. int maMH
                );

                if (kq.Contains("thành công"))
                {
                    MessageBox.Show(kq, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(kq, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
