using System;
using System.Data;
using System.Collections.Generic;

namespace Do_An.DAL
{
    // Giả định class Database tồn tại trong namespace Do_An.DAL và có các hàm Execute/ExecuteScalar/ExecuteNonQuery
    public class HocVienDAL
    {
        private readonly Database db = new Database();

        // =========================================================================
        // 1️⃣ CÁC HÀM XỬ LÝ LỚP HỌC (Cho ComboBox Phụ thuộc và Lọc theo Trình độ)
        // =========================================================================

        /// <summary>
        /// Lấy danh sách Khóa học (cho ComboBox Khóa học đầu tiên)
        /// </summary>
        public DataTable LayDanhSachKhoaHoc()
        {
            string sql = "SELECT MaKH, TenKH FROM KhoaHoc ORDER BY TenKH";
            return db.Execute(sql);
        }

        /// <summary>
        /// Lấy danh sách Môn học theo Mã Khóa học
        /// </summary>
        public DataTable LayMonHocTheoKhoaHoc(int maKH)
        {
            string sql = "SELECT MaMH, TenMH FROM MonHoc WHERE MaKH = @MaKH ORDER BY TenMH";
            var parameters = new Dictionary<string, object> { { "@MaKH", maKH } };
            return db.Execute(sql, parameters);
        }

        /// <summary>
        /// Lấy danh sách Lớp học theo Mã Môn học (Lọc lớp CÒN CHỖ)
        /// </summary>
        public DataTable LayLopHocTheoMonHoc(int maMH)
        {
            // Logic tính Sĩ số hiện tại
            string sqlSiSo = @"
                SELECT MaLop, COUNT(MaHV) AS SiSoHienTai
                FROM DangKy
                GROUP BY MaLop";

            string sql = @"
                SELECT 
                    lh.MaLop, lh.TenLop, lh.ThoiGian, lh.SiSoToiDa, 
                    ISNULL(ss.SiSoHienTai, 0) AS SiSoHienTai,
                    (lh.SiSoToiDa - ISNULL(ss.SiSoHienTai, 0)) AS ChoTrong
                FROM LopHoc lh
                LEFT JOIN (" + sqlSiSo + @") ss ON lh.MaLop = ss.MaLop
                WHERE lh.MaMH = @MaMH 
                  AND (lh.SiSoToiDa - ISNULL(ss.SiSoHienTai, 0)) > 0
                  AND lh.TrangThai IN (N'Đang học', N'Chờ khai giảng')
                ORDER BY lh.TenLop";
            var parameters = new Dictionary<string, object> { { "@MaMH", maMH } };
            return db.Execute(sql, parameters);
        }

        /// <summary>
        /// Lấy danh sách các lớp CÒN CHỖ và KHỚP ĐÚNG với LopHoc.TrinhDo.
        /// </summary>
        public DataTable LayLopPhuHopVaConCho(string trinhDo)
        {
            string sqlSiSo = @"SELECT MaLop, COUNT(MaHV) AS SiSoHienTai FROM DangKy GROUP BY MaLop";

            string sqlLop = @"
                SELECT 
                    lh.MaLop, lh.TenLop, lh.ThoiGian, lh.SiSoToiDa, 
                    ISNULL(ss.SiSoHienTai, 0) AS SiSoHienTai,
                    (lh.SiSoToiDa - ISNULL(ss.SiSoHienTai, 0)) AS ChoTrong,
                    lh.TrinhDo, kh.TenKH
                FROM LopHoc lh
                INNER JOIN MonHoc mh ON lh.MaMH = mh.MaMH 
                INNER JOIN KhoaHoc kh ON mh.MaKH = kh.MaKH
                LEFT JOIN (" + sqlSiSo + @") ss ON lh.MaLop = ss.MaLop
                WHERE LOWER(LTRIM(RTRIM(ISNULL(lh.TrinhDo, '')))) = LOWER(LTRIM(RTRIM(ISNULL(@TrinhDo, ''))))
                  AND (lh.SiSoToiDa - ISNULL(ss.SiSoHienTai, 0)) > 0
                  AND lh.TrangThai IN (N'Đang học', N'Chờ khai giảng')
                ORDER BY lh.TenLop";

            var parameters = new Dictionary<string, object>
            {
                { "@TrinhDo", trinhDo ?? string.Empty }
            };

            return db.Execute(sqlLop, parameters);
        }

        public DataTable LayTatCaLopTheoTrinhDo(string trinhDo)
        {
            // Logic tính Sĩ số hiện tại
            string sqlSiSo = @"SELECT MaLop, COUNT(MaHV) AS SiSoHienTai FROM DangKy GROUP BY MaLop";

            string sql = @"
                SELECT 
                    lh.MaLop, lh.TenLop, lh.SiSoToiDa, 
                    ISNULL(ss.SiSoHienTai, 0) AS SiSoHienTai,
                    (lh.SiSoToiDa - ISNULL(ss.SiSoHienTai, 0)) AS ChoTrong,
                    lh.TrinhDo
                FROM LopHoc lh
                LEFT JOIN (" + sqlSiSo + @") ss ON lh.MaLop = ss.MaLop
                WHERE LOWER(LTRIM(RTRIM(ISNULL(lh.TrinhDo, '')))) = LOWER(LTRIM(RTRIM(ISNULL(@TrinhDo, ''))))
                  AND lh.TrangThai IN (N'Đang học', N'Chờ khai giảng')
                ORDER BY lh.TenLop";

            var parameters = new Dictionary<string, object>
            {
                { "@TrinhDo", trinhDo ?? string.Empty }
            };

            return db.Execute(sql, parameters);
        }


        // =========================================================================
        // 2️⃣ CÁC HÀM HỖ TRỢ NGHIỆP VỤ (Đăng ký và Sĩ số)
        // =========================================================================

        public bool DangKyHocVienVaoLop(int maHV, int maLop)
        {
            string sql = "INSERT INTO DangKy (MaHV, MaLop) VALUES (@MaHV, @MaLop)";
            var parameters = new Dictionary<string, object>
            {
                { "@MaHV", maHV },
                { "@MaLop", maLop }
            };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public int LaySiSoHienTaiCuaLop(int maLop)
        {
            string sql = "SELECT COUNT(MaHV) FROM DangKy WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object> { { "@MaLop", maLop } };
            object result = db.ExecuteScalar(sql, parameters);
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public int LaySiSoToiDaCuaLop(int maLop)
        {
            string sql = "SELECT SiSoToiDa FROM LopHoc WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object> { { "@MaLop", maLop } };
            object result = db.ExecuteScalar(sql, parameters);
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public int DemSoLuong()
        {
            string sql = "SELECT COUNT(*) FROM HocVien";
            object result = db.ExecuteScalar(sql);
            return Convert.ToInt32(result);
        }

        // ------------------------------------------------------------------
        // 3️⃣ CÁC HÀM QUẢN LÝ HỌC VIÊN CƠ BẢN (Giữ lại tính tương thích)
        // ------------------------------------------------------------------

        public DataTable LayDanhSachHocVien()
        {
            string sql = "SELECT MaHV, HoTen, NgaySinh, GioiTinh, DiaChi, SDT, Email, CCCD FROM HocVien ORDER BY MaHV DESC";
            return db.Execute(sql);
        }

        public DataTable LayHocVienTheoMa(int maHV)
        {
            string sql = "SELECT MaHV, HoTen, NgaySinh, GioiTinh, DiaChi, SDT, Email, CCCD FROM HocVien WHERE MaHV = @MaHV";
            var parameters = new Dictionary<string, object> { { "@MaHV", maHV } };
            return db.Execute(sql, parameters);
        }

        public bool ThemHocVien(string hoTen, DateTime ngaySinh, string gioiTinh, string diaChi, string sdt, string email, string cccd)
        {
            string sql = @"
                INSERT INTO HocVien (HoTen, NgaySinh, GioiTinh, DiaChi, SDT, Email, CCCD)
                VALUES (@HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SDT, @Email, @CCCD)";

            var parameters = new Dictionary<string, object>
            {
                { "@HoTen", hoTen }, { "@NgaySinh", ngaySinh }, { "@GioiTinh", gioiTinh },
                { "@DiaChi", diaChi }, { "@SDT", sdt }, { "@Email", email }, { "@CCCD", cccd }
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool CapNhatHocVien(int maHV, string hoTen, DateTime ngaySinh, string gioiTinh, string diaChi, string sdt, string email, string cccd)
        {
            string sql = @"
                UPDATE HocVien
                SET HoTen = @HoTen, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, 
                    DiaChi = @DiaChi, SDT = @SDT, Email = @Email, CCCD = @CCCD
                WHERE MaHV = @MaHV";

            var parameters = new Dictionary<string, object>
            {
                { "@MaHV", maHV }, { "@HoTen", hoTen }, { "@NgaySinh", ngaySinh },
                { "@GioiTinh", gioiTinh }, { "@DiaChi", diaChi }, { "@SDT", sdt },
                { "@Email", email }, { "@CCCD", cccd }
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool XoaHocVien(int maHV)
        {
            string sql = "DELETE FROM HocVien WHERE MaHV = @MaHV";
            var parameters = new Dictionary<string, object> { { "@MaHV", maHV } };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

    }
}