using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Do_An.DAL;
using Microsoft.Win32;

namespace Do_An
{
    public partial class NVQL_BC : UserControl
    {
        private readonly Database db = new Database();
        private DataTable currentTable;
        private readonly GiaoDien_NVQL NVQL;

        public NVQL_BC(GiaoDien_NVQL parent = null)
        {
            InitializeComponent();
            NVQL = parent;
            Loaded += NVQL_BC_Loaded;
        }

        private void NVQL_BC_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCombos();
            cbReportType.SelectedIndex = 0;
        }

        private void LoadCombos()
        {
            try
            {
                var dtLop = db.Execute("SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop");
                cbLop.ItemsSource = dtLop.DefaultView;
                cbLop.SelectedValuePath = "MaLop";
                cbLop.DisplayMemberPath = "TenLop";

                var dtMon = db.Execute("SELECT MaMH, TenMH FROM MonHoc ORDER BY TenMH");
                cbMon.ItemsSource = dtMon.DefaultView;
                cbMon.SelectedValuePath = "MaMH";
                cbMon.DisplayMemberPath = "TenMH";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu combobox:\n" + ex.Message);
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // giữ nguyên logic như cũ
            string reportType = (cbReportType.SelectedItem as ComboBoxItem)?.Content.ToString();
            int? maLop = cbLop.SelectedValue as int? ?? (cbLop.SelectedValue != null ? Convert.ToInt32(cbLop.SelectedValue) : (int?)null);
            int? maMH = cbMon.SelectedValue as int? ?? (cbMon.SelectedValue != null ? Convert.ToInt32(cbMon.SelectedValue) : (int?)null);
            string keyword = txtKeyword.Text?.Trim();

            try
            {
                // code tạo báo cáo giữ nguyên
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo báo cáo:\n" + ex.Message);
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            if (currentTable == null || currentTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.");
                return;
            }

            var dlg = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "report.csv"
            };

            if (dlg.ShowDialog() == true)
            {
                ExportDataTableToCsv(currentTable, dlg.FileName);
                MessageBox.Show("Xuất CSV thành công.");
            }
        }

        private void ExportDataTableToCsv(DataTable dt, string filePath)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1) sb.Append(',');
            }
            sb.AppendLine();

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(row[i]?.ToString()?.Replace(",", " "));
                    if (i < dt.Columns.Count - 1) sb.Append(',');
                }
                sb.AppendLine();
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private void dgReport_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void txtKeyword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtKeyword.Text == "Nhập từ khóa...")
            {
                txtKeyword.Text = "";
                txtKeyword.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void txtKeyword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                txtKeyword.Text = "Nhập từ khóa...";
                txtKeyword.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

    }
}
