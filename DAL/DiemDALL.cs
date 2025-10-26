using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Do_An.DAL
{
    // --------------------- XỬ LÝ ĐIỂM  ---------------------
    public class DiemDAL
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Lấy tất cả điểm (join với HocVien, LopHoc, MonHoc, KhoaHoc để hiển thị tên).
        /// </summary>
public DataTable LayTatCaDiem()
        {
            string sql = @"
                SELECT D.MaHV, HV.HoTen, LH.MaLop, LH.TenLop, MH.MaMH, MH.TenMH, KH.TenKH, 
                       D.DiemGK, D.DiemCK, D.DiemTB
                FROM Diem D
                INNER JOIN HocVien HV ON D.MaHV = HV.MaHV
                INNER JOIN LopHoc LH ON D.MaLop = LH.MaLop
                INNER JOIN MonHoc MH ON LH.MaMH = MH.MaMH
                INNER JOIN KhoaHoc KH ON MH.MaKH = KH.MaKH
                ORDER BY LH.TenLop, HV.HoTen";
            return db.Execute(sql, new Dictionary<string, object>());
        }
        /// <summary>
        /// Lấy danh sách lớp (dùng để load combobox)
        /// </summary>
        public DataTable LayDanhSachLopHoc()
        {
            string sql = "SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop";
            return db.Execute(sql, new Dictionary<string, object>());
        }

        /// <summary>
        /// Lấy danh sách môn học (dùng để load combobox)
        /// </summary>
        public DataTable LayDanhSachMonHoc()
        {
            string sql = "SELECT MaMH, TenMH FROM MonHoc ORDER BY TenMH";
            return db.Execute(sql, new Dictionary<string, object>());
        }
        public DataTable LayDanhSachKhoaHoc()
        {
            string sql = "SELECT MaKH, TenKH FROM KhoaHoc ORDER BY TenKH";
            return db.Execute(sql);
        }
        public DataTable LayMonHocTheoKhoaHoc(int maKhoaHoc)
        {
            // Tên cột trong CSDL là MaKH
            string sql = "SELECT MaMH, TenMH FROM MonHoc WHERE MaKH = @MaKhoaHoc ORDER BY TenMH";
            var parameters = new Dictionary<string, object>
            {
                { "@MaKhoaHoc", maKhoaHoc}
            };
            return db.Execute(sql, parameters);
        }

        /// <summary>
        /// Lấy điểm theo bộ lọc: maLop (nullable), maMH (nullable), keyword (mã HV hoặc tên HV).
        /// Nếu tham số null/empty thì sẽ không lọc theo tham số đó.
        /// </summary>
        /// <summary>
        /// Lấy điểm theo bộ lọc: maLop (nullable), maMH (nullable), maKH (nullable), keyword.
        /// </summary>
        public DataTable LayDiemTheoLoc(int? maLop, int? maMH, int? maKH, string keyword)
        {
            var sql = @"
                SELECT D.MaHV, HV.HoTen, LH.MaLop, LH.TenLop, MH.MaMH, MH.TenMH, KH.TenKH, -- Thêm TenKH
                       D.DiemGK, D.DiemCK, D.DiemTB
                FROM Diem D
                INNER JOIN HocVien HV ON D.MaHV = HV.MaHV
                INNER JOIN LopHoc LH ON D.MaLop = LH.MaLop
                INNER JOIN MonHoc MH ON LH.MaMH = MH.MaMH
                INNER JOIN KhoaHoc KH ON MH.MaKH = KH.MaKH -- Thêm JOIN Khóa học
                WHERE 1=1
            ";

            var parameters = new Dictionary<string, object>();

            if (maKH.HasValue)
            {
                sql += " AND KH.MaKH = @MaKH";
                parameters.Add("@MaKH", maKH.Value);
            }

            if (maMH.HasValue)
            {
                sql += " AND MH.MaMH = @MaMH";
                parameters.Add("@MaMH", maMH.Value);
            }

            if (maLop.HasValue)
            {
                sql += " AND LH.MaLop = @MaLop";
                parameters.Add("@MaLop", maLop.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (int.TryParse(keyword.Trim(), out int maHV))
                {
                    sql += " AND (D.MaHV = @SearchMaHV OR HV.HoTen LIKE @SearchName)";
                    parameters.Add("@SearchMaHV", maHV);
                    parameters.Add("@SearchName", $"%{keyword.Trim()}%");
                }
                else
                {
                    sql += " AND HV.HoTen LIKE @SearchName";
                    parameters.Add("@SearchName", $"%{keyword.Trim()}%");
                }
            }

            sql += " ORDER BY LH.TenLop, HV.HoTen";

            return db.Execute(sql, parameters);
        }
    }
}
