using System;
using System.Data;
using Do_An.DAL;
using static Do_An.DAL.Database; // giữ nguyên

namespace Do_An.BLL
{
    public class LopHocBLL
    {
        private readonly LopHocDAL lopHocDAL = new LopHocDAL();

        // ------------------- LOGIC CŨ -------------------
        public DataTable LayDanhSachLopHoc() => lopHocDAL.LayTatCaLopHoc();
        public DataTable LayDanhSachMonHoc() => lopHocDAL.LayDanhSachMonHoc();
        public DataTable LayDanhSachGiaoVien() => lopHocDAL.LayDanhSachGiaoVien();

        public DataTable LayDanhSachLopTheoGiangVien(int maGV)
        {
            if (maGV <= 0)
                throw new ArgumentException("Mã giảng viên không hợp lệ.");

            return lopHocDAL.LayLopTheoGiangVien(maGV);
        }

        public string CapNhatTrangThai(int maLop, string trangThai)
        {
            if (maLop <= 0)
                return "Mã lớp không hợp lệ.";
            if (string.IsNullOrWhiteSpace(trangThai))
                return "Trạng thái không được để trống.";

            try
            {
                int result = lopHocDAL.CapNhatTrangThaiLop(maLop, trangThai);
                return result > 0 ? "Cập nhật trạng thái thành công!" : "Không thể cập nhật trạng thái lớp.";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật trạng thái: " + ex.Message;
            }
        }

        public DataTable LayDanhSachLopHoc_Admin()
        {
            try
            {
                return lopHocDAL.LayThongTinLopHoc_DanhSach();
            }
            catch (MissingMethodException)
            {
                return lopHocDAL.LayTatCaLopHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tải danh sách lớp học (Admin): " + ex.Message, ex);
            }
        }

        public int XoaLopHoc_Admin(int maLop)
        {
            if (maLop <= 0)
                throw new ArgumentException("Mã lớp không hợp lệ.");

            try
            {
                return lopHocDAL.XoaLopHoc(maLop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa lớp học: " + ex.Message, ex);
            }
        }

        // ------------------- PHƯƠNG THỨC MỚI CHO MLH -------------------

        /// <summary>
        /// Tạo lớp học mới, xử lý nghiệp vụ, và gọi DAL
        /// Đã thêm tham số 'trinhDo' mới và đồng bộ với MLH
        /// </summary>
        public string TaoLopHoc(
                    string tenLop,
                    string trinhDo,
                    string phong,
                    string thoiGian,
                    int siSoToiDa,
                    string trangThai,
                    int maGV,
                    int maMH
                )
        {
            // BLL: Kiểm tra Validation cơ bản
            if (string.IsNullOrWhiteSpace(tenLop)) return "Tên lớp không được để trống.";
            if (string.IsNullOrWhiteSpace(trinhDo)) return "Vui lòng chọn Trình độ áp dụng.";
            if (maGV <= 0) return "Vui lòng chọn giáo viên phụ trách.";
            if (maMH <= 0) return "Vui lòng chọn môn học.";
            if (siSoToiDa <= 0) return "Sĩ số tối đa phải lớn hơn 0.";

            try
            {
                // Gọi DAL với 8 tham số đã được đồng bộ hóa
                int result = lopHocDAL.ThemLopHoc(
                                     tenLop,
                                     trinhDo,
                                     phong,
                                     thoiGian,
                                     siSoToiDa,
                                     trangThai,
                                     maGV,
                                     maMH
                                 );

                return result > 0 ? "Tạo lớp học thành công!" : "Không thể thêm lớp học.";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm lớp học: " + ex.Message;
            }
        }

        // Lấy danh sách khóa học (ComboBox Khóa học)
        public DataTable LayDanhSachKhoaHoc()
        {
            try
            {
                return lopHocDAL.LayDanhSachKhoaHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tải danh sách khóa học: " + ex.Message, ex);
            }
        }

        // Lấy danh sách môn học theo khóa học (ComboBox Môn học)
        public DataTable LayMonHocTheoKhoaHoc(int maKhoaHoc)
        {
            if (maKhoaHoc <= 0) return new DataTable();
            return lopHocDAL.LayMonHocTheoKhoaHoc(maKhoaHoc);
        }
    }
}
