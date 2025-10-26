using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;
using Do_An.DAL;
using System.Linq;
using System.Text.RegularExpressions; // Thêm Regex để loại bỏ ký tự không an toàn

namespace Do_An
{
    // Giả định bạn có một enum LoaiNguoiDung trong dự án
    public enum LoaiNguoiDung { Admin, GiaoVien, NhanVien, HocVien }

    public partial class GiaoDien_Admin : Window
    {
        private TaiKhoanBLL.LoaiNguoiDung userRole;

        public GiaoDien_Admin(TaiKhoanBLL.LoaiNguoiDung _userRole)
        {
            InitializeComponent();
            MenuList.SelectionChanged += MenuList_SelectionChanged;
            MenuList.SelectedIndex = 0; // Mặc định mở Trang chủ
            userRole = _userRole;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListBoxItem item)
            {
                // FIX LỖI: Loại bỏ các ký tự không phải chữ/số/khoảng trắng ở đầu bằng Regex
                // Lấy nội dung thô và chỉ giữ lại chữ cái và khoảng trắng.
                string rawSelectedContent = item.Content.ToString() ?? "";
                string selected = Regex.Replace(rawSelectedContent, @"[^a-zA-Z0-9\s\u00C0-\u00FF]", "").Trim();

                // Clear ContentPanel 
                if (ContentPanel == null) return;
                ContentPanel.Children.Clear();

                UserControl newControl = null;
                string title = "";

                // --- Xử lý chuyển đổi UserControl/Trang ---
                if (selected.Contains("Trang chủ"))
                {
                    title = "Trang Chủ";
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Chào mừng bạn đến với giao diện quản lý hệ thống!",
                        FontSize = 18,
                        Margin = new Thickness(20)
                    });
                }
                else if (selected.Contains("Quản lý tài khoản"))
                {
                    title = "Quản lý Tài khoản (Tất cả User)";
                    // newControl = new Admin_QLTK(); 
                    ContentPanel.Children.Add(new TextBlock { Text = "Form Quản lý tài khoản đang được xây dựng.", FontSize = 16, Margin = new Thickness(20) });
                }
                else if (selected.Contains("Quản lý lớp học"))
                {
                    title = "Quản lý Lớp học";
                    // newControl = new Admin_QLLH();
                    ContentPanel.Children.Add(new TextBlock { Text = "Form Quản lý lớp học đang được xây dựng.", FontSize = 16, Margin = new Thickness(20) });
                }
                else if (selected.Contains("Quản lý phân quyền"))
                {
                    title = "Quản lý Phân quyền";
                    // newControl = new Admin_QLPQ();
                    ContentPanel.Children.Add(new TextBlock { Text = "Trang quản lý phân quyền đang được xây dựng.", FontSize = 16, Margin = new Thickness(20) });
                }
                else if (selected.Contains("Bảo mật hệ thống"))
                {
                    title = "Bảo mật hệ thống";
                    ContentPanel.Children.Add(new TextBlock { Text = "Trang bảo mật hệ thống đang được xây dựng.", FontSize = 16, Margin = new Thickness(20) });
                }
                else if (selected.Contains("Xem báo cáo"))
                {
                    title = "Xem báo cáo tổng hợp";
                    // newControl = new NVQL_BC();
                    ContentPanel.Children.Add(new TextBlock { Text = "Trang xem báo cáo đang được phát triển.", FontSize = 16, Margin = new Thickness(20) });
                }

                // Cập nhật tiêu đề (FIX: Bây giờ txtTitle đã tồn tại trong XAML)
                

                // Nếu có Control mới, thêm vào ContentPanel
                if (newControl != null)
                {
                    ContentPanel.Children.Add(newControl);
                }
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            // Giả định GiaoDienDangNhap tồn tại và có thể show
            // GiaoDienDangNhap form = new GiaoDienDangNhap();
            this.Close();
            // form.Show();
        }
    }
}