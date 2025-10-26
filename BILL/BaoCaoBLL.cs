using System;
using System.Data;
using Do_An.DAL;
using static Do_An.DAL.Database;

namespace Do_An.BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL dal = new ReportDAL();

        public DataTable GetClasses() => dal.LayDanhSachLop();

        public DataTable GetSubjects() => dal.LayDanhSachMonHoc();

        public DataTable GetTeachers() => dal.LayDanhSachGiaoVien();

        // Report: students by class/subject
        public DataTable ReportStudents(int? maLop, int? maMH)
        {
            return dal.LayHocVienTheoLopOrMon(maLop, maMH);
        }

        // Report: scores
        public DataTable ReportScores(int? maLop, int? maMH, string keyword)
        {
            return dal.LayDiemTheoLoc(maLop, maMH, keyword);
        }

        // Report: fees
        public DataTable ReportFees(DateTime? fromDate, DateTime? toDate, int? maLop)
        {
            return dal.LayBaoCaoHocPhi(fromDate, toDate, maLop);
        }

        // Report: schedule
        public DataTable ReportSchedule(int? maGV, int? maLop, DateTime? fromDate, DateTime? toDate)
        {
            return dal.LayLichGiangDay(maGV, maLop, fromDate, toDate);
        }
    }
}
