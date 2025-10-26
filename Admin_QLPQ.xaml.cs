using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class Admin_QLPQ : UserControl
    {
        private readonly PhanQuyenBLL pqBLL = new PhanQuyenBLL();
        private DataTable dtPhanQuyen;

        public Admin_QLPQ()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dtPhanQuyen = pqBLL.GetAll();
            dgPhanQuyen.ItemsSource = dtPhanQuyen.DefaultView;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dtPhanQuyen == null) return;
            string keyword = txtSearch.Text.Trim().ToLower();

            DataView dv = dtPhanQuyen.DefaultView;
            if (string.IsNullOrEmpty(keyword))
                dv.RowFilter = string.Empty;
            else
                // Cột đúng trong bảng SQL là TenDN
                dv.RowFilter = $"TenDN LIKE '%{keyword.Replace("'", "''")}%'";

            dgPhanQuyen.ItemsSource = dv;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pqBLL.UpdatePhanQuyen(dtPhanQuyen);
                MessageBox.Show("Cập nhật phân quyền thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
