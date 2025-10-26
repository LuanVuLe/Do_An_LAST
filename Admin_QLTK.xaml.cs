using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class Admin_QLTK : UserControl
    {
        private readonly TaiKhoanBLL tkBLL = new TaiKhoanBLL();

        public Admin_QLTK()
        {
            InitializeComponent();
            LoadDSTaiKhoan();
        }

        // 🟢 Load danh sách tài khoản
        private void LoadDSTaiKhoan()
        {
            try
            {
                DataTable dt = tkBLL.LayTatCaTaiKhoan();
                dgTaiKhoan.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 🔍 Tìm kiếm theo Tên đăng nhập hoặc Họ tên
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            DataTable dt = tkBLL.TimKiemTaiKhoan(keyword);
            dgTaiKhoan.ItemsSource = dt.DefaultView;
        }

        // ➕ Thêm tài khoản (mở form thêm)
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            AdminQLTK_AddTK form = new AdminQLTK_AddTK();
            form.ShowDialog();
            LoadDSTaiKhoan();
        }

        // ✏ Sửa tài khoản (mở form sửa)
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaiKhoan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView row = dgTaiKhoan.SelectedItem as DataRowView;
            string tenDN = row["TenDN"].ToString();
            AdminQLTK_AddTK form = new AdminQLTK_AddTK(tenDN);
            form.ShowDialog();
            LoadDSTaiKhoan(); 
        }

        // 🗑 Xóa tài khoản
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaiKhoan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView row = dgTaiKhoan.SelectedItem as DataRowView;
            string tenDN = row["TenDN"].ToString();

            MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa tài khoản '{tenDN}' không?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool success = tkBLL.XoaTaiKhoan(tenDN);
                if (success)
                {
                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDSTaiKhoan();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 🔒 Khóa / Mở khóa tài khoản
        private void btnKhoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaiKhoan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần khóa/mở!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView row = dgTaiKhoan.SelectedItem as DataRowView;
            string tenDN = row["TenDN"].ToString();
            string trangThai = row["TrangThai"].ToString();

            string trangThaiMoi = (trangThai == "Hoạt động") ? "Bị khóa" : "Hoạt động";
            bool success = tkBLL.CapNhatTrangThai(tenDN, trangThaiMoi);

            if (success)
            {
                MessageBox.Show($"Đã chuyển trạng thái tài khoản '{tenDN}' sang '{trangThaiMoi}'.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDSTaiKhoan();
            }
            else
            {
                MessageBox.Show("Cập nhật trạng thái thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
