using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Do_An.BLL;

namespace Do_An
{
    public partial class GiaoDien_GV : Window
    {
        private LoaiNguoiDung vaiTro;
        public GiaoDien_GV()
        {
            InitializeComponent();
            MenuList.SelectedIndex = 0; // Khi mở cửa sổ, mặc định vào "Trang chủ"
        }
        public GiaoDien_GV(LoaiNguoiDung role)
        {
            InitializeComponent();
            this.vaiTro = role;
            // Ví dụ: hiển thị vai trò trên Title
            this.Title = "Giao diện " + vaiTro;
        }
        // Khi chọn menu bên trái
        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListBoxItem item)
            {
                string muc = item.Content.ToString();
                HienThiNoiDung(muc);
            }
        }

        // Hàm tạo card hiển thị
        private Border TaoCard(string tieuDe, UIElement noiDung)
        {
            return new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 10, 0, 10),
                Child = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = tieuDe,
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0,0,0,8)
                        },
                        noiDung
                    }
                }
            };
        }

        // Hiển thị nội dung chính
        private void HienThiNoiDung(string muc)
        {
            ContentPanel.Children.Clear();

            if (muc.Contains("Trang chủ"))
            {
                txtTitle.Text = "Trang chủ";

                ContentPanel.Children.Add(
                    TaoCard("📌 Thông báo", new TextBlock
                    {
                        Text = "- Khai giảng lớp IELTS ngày 5/10\n- Thi thử TOEIC ngày 20/10\n- Họp giảng viên chiều T7 lúc 15h",
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap
                    })
                );

                ContentPanel.Children.Add(
                    TaoCard("📅 Lịch dạy hôm nay", new ListBox
                    {
                        Items =
                        {
                            "Tiếng Anh giao tiếp (8h - 10h)",
                            "IELTS Reading (14h - 16h)",
                            "TOEIC Listening (18h - 20h)"
                        }
                    })
                );
            }
            else if (muc.Contains("Danh sách lớp"))
            {
                txtTitle.Text = "Danh sách lớp";

                ContentPanel.Children.Add(
                    TaoCard("📘 Các lớp giảng dạy", new ListBox
                    {
                        Items =
                        {
                            "Lớp A1 - 20 học viên",
                            "Lớp A2 - 18 học viên",
                            "Lớp B1 - 25 học viên",
                            "IELTS Intensive - 15 học viên",
                            "TOEIC 600+ - 22 học viên"
                        }
                    })
                );
            }
            else if (muc.Contains("Thông tin học viên"))
            {
                txtTitle.Text = "Thông tin học viên";

                var grid = new DataGrid
                {
                    AutoGenerateColumns = false,
                    IsReadOnly = true,
                    ItemsSource = new[]
                    {
                        new { Ten="Nguyễn Văn A", NamSinh=2005, Lop="IELTS" },
                        new { Ten="Trần Thị B", NamSinh=2004, Lop="TOEIC" },
                        new { Ten="Lê Văn C", NamSinh=2006, Lop="B1" }
                    }
                };

                grid.Columns.Add(new DataGridTextColumn { Header = "Tên học viên", Binding = new System.Windows.Data.Binding("Ten") });
                grid.Columns.Add(new DataGridTextColumn { Header = "Năm sinh", Binding = new System.Windows.Data.Binding("NamSinh") });
                grid.Columns.Add(new DataGridTextColumn { Header = "Lớp", Binding = new System.Windows.Data.Binding("Lop") });

                ContentPanel.Children.Add(TaoCard("👥 Danh sách học viên", grid));
            }
            else if (muc.Contains("Cập nhật điểm"))
            {
                txtTitle.Text = "Cập nhật điểm";

                var panel = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(0, 5, 0, 0) };
                panel.Children.Add(new TextBlock { Text = "👉 Nhập điểm theo kỹ năng:", FontWeight = FontWeights.Bold });

                panel.Children.Add(new TextBox { Text = "Listening", Margin = new Thickness(0, 5, 0, 0) });
                panel.Children.Add(new TextBox { Text = "Speaking", Margin = new Thickness(0, 5, 0, 0) });
                panel.Children.Add(new TextBox { Text = "Reading", Margin = new Thickness(0, 5, 0, 0) });
                panel.Children.Add(new TextBox { Text = "Writing", Margin = new Thickness(0, 5, 0, 0) });

                Button btnSave = new Button
                {
                    Content = "💾 Lưu điểm",
                    Background = Brushes.LightGreen,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                panel.Children.Add(btnSave);

                ContentPanel.Children.Add(TaoCard("✍️ Cập nhật điểm học viên", panel));
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?",
                                "Xác nhận",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
