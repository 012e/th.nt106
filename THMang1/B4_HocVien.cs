using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THMang1
{
    [Serializable]
    public class HocVien
    {
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public float DiemToan { get; set; }
        public float DiemVan { get; set; }
        public float DTB { get; set; } // Điểm trung bình

        public HocVien() { }

        public HocVien(string mssv, string hoten, string dienthoai, float toan, float van)
        {
            MSSV = mssv;
            HoTen = hoten;
            DienThoai = dienthoai;
            DiemToan = toan;
            DiemVan = van;
            DTB = (toan + van) / 2; // Tính điểm trung bình
        }

        public override string ToString()
        {
            return $"{MSSV} - {HoTen} - {DienThoai} - Toán: {DiemToan} - Văn: {DiemVan} - DTB: {DTB}";
        }
    }
}
