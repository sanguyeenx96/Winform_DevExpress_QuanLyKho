using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHOHANG.DTO
{
    public class NhapDTO2
    {
        public NhapDTO2(int id, string sopartno, string soluong, string mota, string linkmuahang, string ngaydatmua, string ngayhangve, string trangthai)
        {
            this.Id = id;
            this.Sopartno = sopartno;
            this.Soluong = soluong;
            this.Mota = mota;
            this.Linkmuahang = linkmuahang;
            this.Ngaydatmua = ngaydatmua;
            this.Ngayhangve = ngayhangve;
            this.Trangthai = trangthai;
        }
        public NhapDTO2(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Sopartno = row["sopartno"].ToString();
            this.Soluong = row["soluong"].ToString();
            this.Mota = row["mota"].ToString();
            this.Linkmuahang = row["linkmuahang"].ToString();
            this.Ngaydatmua = row["ngaydatmua"].ToString();
            this.Ngayhangve = row["ngayhangve"].ToString();
            this.Trangthai = row["trangthai"].ToString();
        }
        private int id;
        private string sopartno;
        private string soluong;
        private string mota;
        private string linkmuahang;
        private string ngaydatmua;
        private string ngayhangve;
        private string trangthai;

        public int Id { get => id; set => id = value; }
        public string Sopartno { get => sopartno; set => sopartno = value; }
        public string Soluong { get => soluong; set => soluong = value; }
        public string Mota { get => mota; set => mota = value; }
        public string Linkmuahang { get => linkmuahang; set => linkmuahang = value; }
        public string Ngaydatmua { get => ngaydatmua; set => ngaydatmua = value; }
        public string Ngayhangve { get => ngayhangve; set => ngayhangve = value; }
        public string Trangthai { get => trangthai; set => trangthai = value; }
    }
}
