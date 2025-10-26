using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Do_An.DAL
{
    public class LopHocDAL
    {
        private readonly Database db = new Database();

        // --------------------- LOGIC CŨ ---------------------

        public DataTable LayTatCaLopHoc()
        {
            string sql = "SELECT * FROM LopHoc";
            return db.Execute(sql);
        }

        public int ThemLopHoc(string tenLop, string trinhDo, string phong, string thoiGian, int siSoToiDa,
                              string trangThai, int maGV, int maMH)
        {
            string sql = @"INSERT INTO LopHoc (TenLop, TrinhDo, Phong, ThoiGian, SiSoToiDa, TrangThai, MaGV, MaMH)
                           VALUES (@TenLop, @TrinhDo, @Phong, @ThoiGian, @SiSoToiDa, @TrangThai, @MaGV, @MaMH)";
            var parameters = new Dictionary<string, object>
            {
                {"@TenLop", tenLop},
                {"@TrinhDo", trinhDo},
                {"@Phong", phong},
                {"@ThoiGian", thoiGian},
                {"@SiSoToiDa", siSoToiDa},
                {"@TrangThai", trangThai},
                {"@MaGV", maGV},
                {"@MaMH", maMH}
            };

            return db.ExecuteNonQuery(sql, parameters);
        }

        public DataTable LayDanhSachMonHoc()
        {
            string sql = "SELECT MaMH, TenMH FROM MonHoc";
            return db.Execute(sql);
        }

        public DataTable LayDanhSachGiaoVien()
        {
            string sql = "SELECT MaGV, HoTen FROM GiaoVien";
            return db.Execute(sql);
        }

        public DataTable LayLopTheoGiangVien(int maGV)
        {
            string sql = @"SELECT MaLop, TenLop, Phong, ThoiGian, SiSoToiDa, TrangThai, TrinhDo
                           FROM LopHoc WHERE MaGV = @MaGV";
            var parameters = new Dictionary<string, object>
            {
                {"@MaGV", maGV}
            };
            return db.Execute(sql, parameters);
        }

        public int CapNhatTrangThaiLop(int maLop, string trangThai)
        {
            string sql = @"UPDATE LopHoc SET TrangThai = @TrangThai WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@TrangThai", trangThai},
                {"@MaLop", maLop}
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        public DataTable LayThongTinLopHoc_DanhSach()
        {
            string sql = @"
                SELECT l.MaLop, l.TenLop, l.TrinhDo, g.HoTen AS GiaoVien, 
                       COUNT(dk.MaHV) AS SiSo
                FROM LopHoc l
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN DangKy dk ON l.MaLop = dk.MaLop
                GROUP BY l.MaLop, l.TenLop, l.TrinhDo, g.HoTen";
            return db.Execute(sql);
        }

        public DataTable TimKiemLopHoc(string tuKhoa)
        {
            string sql = @"
                SELECT l.MaLop, l.TenLop, l.TrinhDo, g.HoTen AS GiaoVien, 
                       COUNT(dk.MaHV) AS SiSo
                FROM LopHoc l
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN DangKy dk ON l.MaLop = dk.MaLop
                WHERE l.TenLop LIKE @TuKhoa OR g.HoTen LIKE @TuKhoa
                GROUP BY l.MaLop, l.TenLop, l.TrinhDo, g.HoTen";
            var parameters = new Dictionary<string, object>
            {
                {"@TuKhoa", "%" + tuKhoa + "%"}
            };
            return db.Execute(sql, parameters);
        }

        public int XoaLopHoc(int maLop)
        {
            string sql = "DELETE FROM LopHoc WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@MaLop", maLop}
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        public int SuaLopHoc(int maLop, string tenLop, string trinhDo, int maGV)
        {
            string sql = "UPDATE LopHoc SET TenLop = @TenLop, TrinhDo = @TrinhDo, MaGV = @MaGV WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@TenLop", tenLop},
                {"@TrinhDo", trinhDo},
                {"@MaGV", maGV},
                {"@MaLop", maLop}
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        public DataTable LayThongTinLopHocChiTiet(string maLop)
        {
            string sql = @"
                SELECT l.*, g.HoTen AS GiaoVien, m.TenMH AS MonHoc
                FROM LopHoc l
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN MonHoc m ON l.MaMH = m.MaMH
                WHERE l.MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@MaLop", maLop}
            };
            return db.Execute(sql, parameters);
        }

        public int DemSoHocVien(int maLop)
        {
            string sql = "SELECT COUNT(*) FROM DangKy WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@MaLop", maLop}
            };
            object result = db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        // --------------------- PHƯƠNG THỨC MỚI ĐỒNG BỘ VỚI MLH ---------------------

        // Lấy danh sách khóa học (cho ComboBox Khóa học)
        public DataTable LayDanhSachKhoaHoc()
        {
            string sql = "SELECT MaKH, TenKH FROM KhoaHoc";
            return db.Execute(sql);
        }

        // Lấy danh sách môn học theo khóa học (cho ComboBox Môn học)
        public DataTable LayMonHocTheoKhoaHoc(int maKhoaHoc)
        {
            // Tên cột trong CSDL là MaKH
            string sql = "SELECT MaMH, TenMH FROM MonHoc WHERE MaKH = @MaKhoaHoc ORDER BY TenMH";
            var parameters = new Dictionary<string, object>
            {
                // Tên tham số @MaKhoaHoc được sử dụng trong hàm này
                {"@MaKhoaHoc", maKhoaHoc}
            };
            return db.Execute(sql, parameters);
        }
    }
}
