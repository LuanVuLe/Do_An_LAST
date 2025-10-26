using System;
using System.Data;
using Do_An.DAL;

namespace Do_An.BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();
        private readonly Database db = new Database();
        public enum LoaiNguoiDung
        {
            KhongHopLe = 0,
            SaiVaiTro = 1,
            HocVien = 2,
            GiaoVien = 3,
            NhanVien = 4,
            Admin = 5
        }

        public DataTable LayTatCaTaiKhoan() => taiKhoanDAL.LayTatCaTaiKhoan();

        public DataTable LayThongTinTaiKhoan(string tenDN)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");
            return taiKhoanDAL.LayThongTinTaiKhoan(tenDN);
        }

        public bool ThemTaiKhoan(string tenDN, string hoTen, string matKhau, string loaiNguoiDung)
        {
            if (string.IsNullOrWhiteSpace(tenDN) || string.IsNullOrWhiteSpace(matKhau))
                throw new ArgumentException("Tên đăng nhập và mật khẩu không được để trống.");

            if (taiKhoanDAL.KiemTraTonTai(tenDN))
                throw new Exception("Tên đăng nhập này đã tồn tại.");

            return taiKhoanDAL.ThemTaiKhoan(tenDN, matKhau, loaiNguoiDung);
        }

        public bool CapNhatTaiKhoan(string tenDN, string hoTen, string matKhauMoi, string loaiNguoiDung)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");
            return taiKhoanDAL.CapNhatTaiKhoan(tenDN, hoTen, matKhauMoi, loaiNguoiDung);
        }

        public bool XoaTaiKhoan(string tenDN)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");
            return taiKhoanDAL.XoaTaiKhoan(tenDN);
        }

        public bool KhoaTaiKhoan(string tenDN) => taiKhoanDAL.KhoaTaiKhoan(tenDN);
        public bool MoKhoaTaiKhoan(string tenDN) => taiKhoanDAL.MoKhoaTaiKhoan(tenDN);
        public bool CapNhatTrangThai(string tenDN, string trangThaiMoi) => taiKhoanDAL.CapNhatTrangThai(tenDN, trangThaiMoi);

        public DataTable TimKiemTaiKhoan(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return LayTatCaTaiKhoan();
            return taiKhoanDAL.TimKiemTaiKhoan(keyword);
        }

        public LoaiNguoiDung KiemTraDangNhap(string username, string password, string expectedRole)
        {
            var dt = taiKhoanDAL.LayThongTinTaiKhoan(username);
            if (dt.Rows.Count == 0) return LoaiNguoiDung.KhongHopLe;

            var row = dt.Rows[0];
            string matKhau = row["MatKhau"].ToString();
            string loaiND = row["LoaiNguoiDung"].ToString();

            if (matKhau != password) return LoaiNguoiDung.KhongHopLe;
            if (!string.Equals(loaiND, expectedRole, StringComparison.OrdinalIgnoreCase))
                return LoaiNguoiDung.SaiVaiTro;

            return loaiND switch
            {
                "Học viên" => LoaiNguoiDung.HocVien,
                "Giáo viên" => LoaiNguoiDung.GiaoVien,
                "Nhân viên" => LoaiNguoiDung.NhanVien,
                "Quản lý" => LoaiNguoiDung.Admin,
                _ => LoaiNguoiDung.KhongHopLe
            };
        }
        public int DemSoLuongNhanVien()
        {
            string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE LoaiNguoiDung = @Loai";
            var parameters = new Dictionary<string, object>
            {
                { "@Loai", "Nhân viên" }
            };

            object result = db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }
    }
}
