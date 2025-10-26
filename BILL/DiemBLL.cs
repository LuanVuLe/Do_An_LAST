using System;
using System.Data;
using Do_An.DAL;
using static Do_An.DAL.Database;
using System.Collections.Generic; 

namespace Do_An.BLL
{
    public class DiemBLL
    {
        // Khai báo DAL chính xác
        private readonly DiemDAL dal = new DiemDAL();

        // ------------------- LOGIC MỚI: LỌC PHỤ THUỘC -------------------

        /// <summary>
        /// Lấy danh sách Khóa học (cho ComboBox Khóa học)
        /// </summary>
        public DataTable LayDanhSachKhoaHoc()
        {
            return dal.LayDanhSachKhoaHoc();
        }

        /// <summary>
        /// Lấy danh sách Môn học theo Khóa học (cho ComboBox Môn học)
        /// </summary>
        public DataTable LayMonHocTheoKhoaHoc(int maKhoaHoc)
        {
            if (maKhoaHoc <= 0) return new DataTable();
            return dal.LayMonHocTheoKhoaHoc(maKhoaHoc);
        }
        
        // ------------------- LOGIC CHÍNH: LẤY VÀ LỌC ĐIỂM -------------------

        /// <summary>
        /// Lấy toàn bộ điểm (hiển thị ban đầu)
        /// </summary>
        public DataTable LayTatCaDiem()
        {
            return dal.LayTatCaDiem();
        }

        // FIX: Hàm LayDanhSachLopHoc (gọi DAL)
        /// <summary>
        /// Lấy danh sách lớp (dùng cho combobox)
        /// </summary>
        public DataTable LayDanhSachLopHoc()
        {
            return dal.LayDanhSachLopHoc();
        }

        // FIX: Hàm LayDanhSachMonHoc (gọi DAL)
        /// <summary>
        /// Lấy danh sách môn học (dùng cho combobox)
        /// </summary>
        public DataTable LayDanhSachMonHoc()
        {
            return dal.LayDanhSachMonHoc();
        }

        /// <summary>
        /// Lọc điểm theo điều kiện (Sử dụng 4 tham số mới)
        /// </summary>
        public DataTable TimDiem(int? maLop, int? maMH, int? maKH, string keyword)
        {
            // BLL chỉ đóng vai trò là tầng chuyển tiếp, gọi hàm DAL
            return dal.LayDiemTheoLoc(maLop, maMH, maKH, keyword);
        }
    }
}