using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHOHANG.DTO
{
    public class NhapDTO
    {
        public NhapDTO(int id, string solevel, string soserialnumber, string sopartno, string mota, string linkmuahang1, string linkmuahang2, string linkmuahang3)
        {
            this.Id = id;
            this.Solevel = solevel;
            this.Soserialnumber = soserialnumber;
            this.Sopartno = sopartno;
            this.Mota = mota;
            this.Linkmuahang1 = linkmuahang1;
            this.Linkmuahang2 = linkmuahang2;
            this.Linkmuahang3 = linkmuahang3;
        }
        public NhapDTO(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Solevel = row["solevel"].ToString();
            this.Soserialnumber = row["soserialnumber"].ToString();
            this.Sopartno = row["sopartno"].ToString();
            this.Mota = row["mota"].ToString();
            this.Linkmuahang1 = row["linkmuahang1"].ToString();
            this.Linkmuahang2 = row["linkmuahang2"].ToString();
            this.Linkmuahang3 = row["linkmuahang3"].ToString();
        }
        private int id;
        private string solevel;
        private string soserialnumber;
        private string sopartno;
        private string mota;
        private string linkmuahang1;
        private string linkmuahang2;
        private string linkmuahang3;

        public int Id { get => id; set => id = value; }
        public string Solevel { get => solevel; set => solevel = value; }
        public string Soserialnumber { get => soserialnumber; set => soserialnumber = value; }
        public string Sopartno { get => sopartno; set => sopartno = value; }
        public string Mota { get => mota; set => mota = value; }
        public string Linkmuahang1 { get => linkmuahang1; set => linkmuahang1 = value; }
        public string Linkmuahang2 { get => linkmuahang2; set => linkmuahang2 = value; }
        public string Linkmuahang3 { get => linkmuahang3; set => linkmuahang3 = value; }
    }
}
