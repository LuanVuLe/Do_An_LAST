using System.Windows;
using System.Windows.Input;

namespace Do_An
{
    public partial class QuenMatKhau3 : Window
    {
        private string _tenDN;
        private string _otpCode;
        private string _maSo;

        public QuenMatKhau3(string otpCode, string maSo, string tenDN)
        {
            InitializeComponent();
            _otpCode = otpCode;
            _maSo = maSo;
            _tenDN = tenDN;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = otp1.Text + otp2.Text + otp3.Text + otp4.Text + otp5.Text + otp6.Text;

            if (enteredCode == _otpCode)
            {
                MessageBox.Show("Xác minh thành công! Vui lòng đặt lại mật khẩu.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                QuenMatKhau4 reset = new QuenMatKhau4(_tenDN);
                reset.Owner = this;
                this.Hide();
                reset.ShowDialog();
            }
            else
            {
                MessageBox.Show("Mã xác minh không đúng!",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       /* private void Back_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
            if (this.Owner != null) this.Owner.Show();
        }
       */
        private void txtResend_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Đã gửi lại mã xác nhận!");
        }
        private void Back_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            if (this.Owner != null)
                this.Owner.Show();
        }

    }
}
