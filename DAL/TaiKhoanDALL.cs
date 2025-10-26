using System;
using System.Collections.Generic;
using System.Data;

namespace Do_An.DAL
{
    public class TaiKhoanDAL
    {
        private readonly Database db = new Database();

        // Lấy tất cả tài khoản
        public DataTable LayTatCaTaiKhoan()
        {
            string sql = @"SELECT TenDN, MatKhau, LoaiNguoiDung, MaHV, MaGV, MaNV, MaQL, TrangThai 
                           FROM TaiKhoan";
            return db.Execute(sql);
        }

        // Lấy thông tin chi tiết tài khoản
        public DataTable LayThongTinTaiKhoan(string tenDN)
        {
            string sql = @"
SELECT 
    TK.TenDN, 
    TK.MatKhau, 
    TK.LoaiNguoiDung, 
    TK.TrangThai,
    COALESCE(HV.HoTen, GV.HoTen, NV.HoTen, QL.HoTen, '') AS HoTen
FROM TaiKhoan TK
LEFT JOIN HocVien HV ON TK.MaHV = HV.MaHV
LEFT JOIN GiaoVien GV ON TK.MaGV = GV.MaGV
LEFT JOIN NhanVien NV ON TK.MaNV = NV.MaNV
LEFT JOIN QuanLy QL ON TK.MaQL = QL.MaQL
WHERE TK.TenDN = @TenDN";

            var parameters = new Dictionary<string, object>
            {
                {"@TenDN", tenDN}
            };

            return db.Execute(sql, parameters);
        }

        // Thêm tài khoản
        public bool ThemTaiKhoan(string tenDN, string matKhau, string loaiNguoiDung, int? maHV = null, int? maGV = null, int? maNV = null, int? maQL = null)
        {
            string sql = @"
INSERT INTO TaiKhoan (TenDN, MatKhau, LoaiNguoiDung, MaHV, MaGV, MaNV, MaQL, TrangThai)
VALUES (@TenDN, @MatKhau, @LoaiNguoiDung, @MaHV, @MaGV, @MaNV, @MaQL, 1)";

            var parameters = new Dictionary<string, object>
            {
                {"@TenDN", tenDN},
                {"@MatKhau", matKhau},
                {"@LoaiNguoiDung", loaiNguoiDung},
                {"@MaHV", (object)maHV ?? DBNull.Value},
                {"@MaGV", (object)maGV ?? DBNull.Value},
                {"@MaNV", (object)maNV ?? DBNull.Value},
                {"@MaQL", (object)maQL ?? DBNull.Value}
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // Cập nhật tài khoản
        public bool CapNhatTaiKhoan(string tenDN, string hoTen, string matKhauMoi, string loaiNguoiDung)
        {
            // 1. Cập nhật LoaiNguoiDung
            string sqlUpdateTaiKhoan = @"UPDATE TaiKhoan 
                                         SET LoaiNguoiDung = @LoaiNguoiDung
                                         WHERE TenDN = @TenDN";

            var parametersTaiKhoan = new Dictionary<string, object>
            {
                {"@TenDN", tenDN},
                {"@LoaiNguoiDung", loaiNguoiDung},
            };

            // 2. Cập nhật mật khẩu nếu có
            if (!string.IsNullOrEmpty(matKhauMoi))
            {
                string sqlUpdateMatKhau = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE TenDN = @TenDN";
                var parametersMatKhau = new Dictionary<string, object>
                {
                    {"@TenDN", tenDN},
                    {"@MatKhau", matKhauMoi}
                };
                db.ExecuteNonQuery(sqlUpdateMatKhau, parametersMatKhau);
            }

            // 3. Cập nhật HoTen trong bảng liên quan
            string sqlGetId = "SELECT MaHV, MaGV, MaNV, MaQL FROM TaiKhoan WHERE TenDN = @TenDN";
            var dt = db.Execute(sqlGetId, new Dictionary<string, object> { { "@TenDN", tenDN } });
            if (dt.Rows.Count > 0)
            {
                int? maHV = dt.Rows[0]["MaHV"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[0]["MaHV"]) : null;
                int? maGV = dt.Rows[0]["MaGV"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[0]["MaGV"]) : null;
                int? maNV = dt.Rows[0]["MaNV"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[0]["MaNV"]) : null;
                int? maQL = dt.Rows[0]["MaQL"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[0]["MaQL"]) : null;

                if (maHV.HasValue)
                    db.ExecuteNonQuery("UPDATE HocVien SET HoTen=@HoTen WHERE MaHV=@MaHV", new Dictionary<string, object> { { "@HoTen", hoTen }, { "@MaHV", maHV.Value } });
                else if (maGV.HasValue)
                    db.ExecuteNonQuery("UPDATE GiaoVien SET HoTen=@HoTen WHERE MaGV=@MaGV", new Dictionary<string, object> { { "@HoTen", hoTen }, { "@MaGV", maGV.Value } });
                else if (maNV.HasValue)
                    db.ExecuteNonQuery("UPDATE NhanVien SET HoTen=@HoTen WHERE MaNV=@MaNV", new Dictionary<string, object> { { "@HoTen", hoTen }, { "@MaNV", maNV.Value } });
                else if (maQL.HasValue)
                    db.ExecuteNonQuery("UPDATE QuanLy SET HoTen=@HoTen WHERE MaQL=@MaQL", new Dictionary<string, object> { { "@HoTen", hoTen }, { "@MaQL", maQL.Value } });
            }

            return db.ExecuteNonQuery(sqlUpdateTaiKhoan, parametersTaiKhoan) > 0;
        }

        public bool XoaTaiKhoan(string tenDN)
        {
            string sql = "DELETE FROM TaiKhoan WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object> { { "@TenDN", tenDN } };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool KhoaTaiKhoan(string tenDN) => CapNhatTrangThai(tenDN, "0");
        public bool MoKhoaTaiKhoan(string tenDN) => CapNhatTrangThai(tenDN, "1");

        public bool CapNhatTrangThai(string tenDN, string trangThaiMoi)
        {
            string sql = "UPDATE TaiKhoan SET TrangThai = @TrangThai WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object>
            {
                { "@TrangThai", trangThaiMoi },
                { "@TenDN", tenDN }
            };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public DataTable TimKiemTaiKhoan(string keyword)
        {
            string sql = @"SELECT TenDN, MatKhau, LoaiNguoiDung, MaHV, MaGV, MaNV, MaQL, TrangThai
                           FROM TaiKhoan
                           WHERE TenDN LIKE @Keyword OR LoaiNguoiDung LIKE @Keyword";
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", "%" + keyword + "%"}
            };
            return db.Execute(sql, parameters);
        }

        public bool KiemTraTonTai(string tenDN)
        {
            string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object> { { "@TenDN", tenDN } };
            var result = db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }
    }
}
