using System;
using System.Windows;
using Do_An.BLL;

namespace Do_An
{
    public partial class QuenMatKhau1 : Window
    {
        private readonly TaiKhoanBLL taiKhoanBLL;

        public QuenMatKhau1()
        {
            InitializeComponent();
            taiKhoanBLL = new TaiKhoanBLL();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string tenDN = txtUsername.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(tenDN))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lấy vai trò được chọn
            string expectedRole = rdHocSinh.IsChecked == true ? "Học viên"
                                 : rdGiaoVien.IsChecked == true ? "Giáo viên"
                                 : rdNhanVien.IsChecked == true ? "Nhân viên"
                                 : rdQuanLy.IsChecked == true ? "Quản lý"
                                 : "";

            if (string.IsNullOrEmpty(expectedRole))
            {
                MessageBox.Show("Vui lòng chọn loại tài khoản!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Kiểm tra tài khoản theo tên đăng nhập và vai trò
                TaiKhoanBLL.LoaiNguoiDung result = taiKhoanBLL.KiemTraDangNhap(tenDN, "", expectedRole);

                switch (result)
                {
                    case TaiKhoanBLL.LoaiNguoiDung.KhongHopLe:
                        MessageBox.Show("Tên đăng nhập không tồn tại trong hệ thống!",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.SaiVaiTro:
                        MessageBox.Show("Tên đăng nhập không khớp với vai trò đã chọn!",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case TaiKhoanBLL.LoaiNguoiDung.HocVien:
                    case TaiKhoanBLL.LoaiNguoiDung.GiaoVien:
                    case TaiKhoanBLL.LoaiNguoiDung.Admin: // Admin/Quản lý
                    case TaiKhoanBLL.LoaiNguoiDung.NhanVien: // nếu cần
                        // Nếu đúng, mở bước tiếp theo
                        QuenMatKhau2 step2 = new QuenMatKhau2(tenDN, expectedRole)
                        {
                            Owner = this
                        };
                        this.Hide();
                        step2.ShowDialog();
                        this.Show();
                        break;

                    default:
                        MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại sau.",
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi hệ thống: " + ex.Message,
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (this.Owner != null)
            {
                this.Owner.Show();
            }
            else
            {
                new GiaoDienDangNhap().Show();
            }
        }
    }
}
