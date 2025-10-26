using System;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class GiaoDien_NVQL : Window
    {
        private TaiKhoanBLL.LoaiNguoiDung userrole;

        public GiaoDien_NVQL(TaiKhoanBLL.LoaiNguoiDung _userrole)
        {
            InitializeComponent();
            userrole = _userrole;

            // Gắn sự kiện cho menu bên trái
            MenuList.SelectionChanged += MenuList_SelectionChanged;

            // Gắn sự kiện cho các nút trong dashboard
            GanSuKienChoButton();
        }

        // ================== GẮN SỰ KIỆN ==================
        private void GanSuKienChoButton()
        {
            foreach (var element in FindVisualChildren<Button>(this))
            {
                string content = element.Content?.ToString() ?? "";
                if (string.IsNullOrEmpty(content)) continue;

                // Bỏ qua nút đăng xuất
                if (content.Contains("Đăng xuất")) continue;

                element.Click += (s, e) =>
                {
                    XuLyChucNang(content);
                };
            }
        }

        // ================== XỬ LÝ MENU BÊN TRÁI ==================
        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListBoxItem item)
            {
                string muc = item.Content.ToString();
                XuLyChucNang(muc);
                MenuList.SelectedIndex = -1; // reset lại chọn menu
            }
        }

        // ================== HÀM XỬ LÝ CHỨC NĂNG ==================
        private void XuLyChucNang(string muc)
        {
            // Ẩn giao diện dashboard
            DashboardPanel.Visibility = Visibility.Collapsed;
            MainContent.Visibility = Visibility.Visible;

            if (muc.Contains("Trang chủ"))
            {
                // Hiện lại dashboard, ẩn content
                DashboardPanel.Visibility = Visibility.Visible;
                MainContent.Visibility = Visibility.Collapsed;
            }
            else if (muc.Contains("Đăng ký học viên"))
            {
                MainContent.Content = new NVQL_DKHV();
            }
            else if (muc.Contains("Mở lớp học"))
            {
                MainContent.Content = new NVQL_MLH();
            }
            else if (muc.Contains("Phân công giáo viên"))
            {
                MainContent.Content = new NVQL_PCGV();
            }
            else if (muc.Contains("Quản lý điểm"))
            {
                MainContent.Content = new NVQL_QLD();
            }
            else if (muc.Contains("Quản lý học phí"))
            {
                MainContent.Content = new NVQL_HP();
            }
            else if (muc.Contains("Báo cáo") || muc.Contains("Thống kê"))
            {
                MainContent.Content = new NVQL_BC();
            } 
        }

        // ================== NÚT ĐĂNG XUẤT ==================
        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                                "Xác nhận",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GiaoDienDangNhap login = new GiaoDienDangNhap();
                login.Show();
                this.Close();
            }
        }

        // ================== HÀM HỖ TRỢ TÌM NÚT ==================
        private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                    yield return t;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
