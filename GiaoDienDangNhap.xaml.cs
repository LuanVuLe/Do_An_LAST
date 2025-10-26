using System;
using System.Windows;
using Do_An.BLL;
using DoAn;

namespace Do_An
{
    public partial class GiaoDienDangNhap : Window
    {
        private readonly TaiKhoanBLL taiKhoanBLL;

        public GiaoDienDangNhap()
        {
            InitializeComponent();
            taiKhoanBLL = new TaiKhoanBLL();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string expectedRole = rbStudent.IsChecked == true ? "Học viên"
                                 : rbTeacher.IsChecked == true ? "Giáo viên"
                                 : rbStaff.IsChecked == true ? "Nhân viên"
                                 : rbAdmin.IsChecked == true ? "Quản lý"
                                 : "";

            if (string.IsNullOrEmpty(expectedRole))
            {
                MessageBox.Show("Vui lòng chọn vai trò đăng nhập!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                TaiKhoanBLL.LoaiNguoiDung role = taiKhoanBLL.KiemTraDangNhap(username, password, expectedRole);

                switch (role)
                {
                    case TaiKhoanBLL.LoaiNguoiDung.HocVien:
                        MessageBox.Show("Đăng nhập thành công với vai trò Học viên!");
                        GiaoDien_HocVien hvWindow = new GiaoDien_HocVien(role);
                        hvWindow.Owner = this;
                        this.Hide();
                        hvWindow.ShowDialog();
                        this.Show();
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.GiaoVien:
                        MessageBox.Show("Đăng nhập thành công với vai trò Giáo viên!");
                        GiaoDien_GV gvWindow = new GiaoDien_GV();
                        gvWindow.Owner = this;
                        this.Hide();
                        gvWindow.ShowDialog();
                        this.Show();
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.NhanVien:
                        MessageBox.Show("Đăng nhập thành công với vai trò Nhân viên!");
                        GiaoDien_NVQL nvWindow = new GiaoDien_NVQL(role);
                        nvWindow.Owner = this;
                        this.Hide();
                        nvWindow.ShowDialog();
                        this.Show();
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.Admin:
                        MessageBox.Show("Đăng nhập thành công với vai trò Quản lý!");
                        GiaoDien_Admin adWindow = new GiaoDien_Admin(role);
                        adWindow.Owner = this;
                        this.Hide();
                        adWindow.ShowDialog();
                        this.Show();
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.SaiVaiTro:
                        MessageBox.Show($"Bạn đã chọn sai vai trò. Tài khoản này không phải {expectedRole}!",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.KhongHopLe:
                    default:
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi hệ thống: " + ex.Message,
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ForgotPassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Ẩn form hiện tại và gọi form QuenMatKhau1
            // QuenMatKhau1 forgot = new QuenMatKhau1();
            // forgot.Owner = this;
            this.Hide();
            // forgot.ShowDialog();
            this.Show();
        }
    }
}
