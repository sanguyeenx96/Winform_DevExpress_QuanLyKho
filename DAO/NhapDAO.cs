using QUANLYKHOHANG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUANLYKHOHANG.DAO
{
    public class NhapDao
    {
        private static NhapDao instance;
        public static NhapDao Instance
        {
            get { if (instance == null) instance = new NhapDao(); return NhapDao.instance; }
            private set { NhapDao.instance = value; }
        }
        private NhapDao() { }
        public List<NhapDTO> LoadTableList()
        {
            List<NhapDTO> tablelist = new List<NhapDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.khohang");
            foreach (DataRow item in data.Rows)
            {
                NhapDTO nhap = new NhapDTO(item);
                tablelist.Add(nhap);
            }
            return tablelist;
        }
        public List<NhapDTO2> LoadTableList_dangdathang()
        {
            List<NhapDTO2> tablelist = new List<NhapDTO2>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.dathang where trangthai =N'ĐANG ĐẶT HÀNG'");
            foreach (DataRow item in data.Rows)
            {
                NhapDTO2 nhap = new NhapDTO2(item);
                tablelist.Add(nhap);
            }
            return tablelist;
        }
        public List<NhapDTO2> LoadTableList_hangdave()
        {
            List<NhapDTO2> tablelist = new List<NhapDTO2>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.dathang where trangthai =N'HÀNG ĐÃ VỀ'");
            foreach (DataRow item in data.Rows)
            {
                NhapDTO2 nhap = new NhapDTO2(item);
                tablelist.Add(nhap);
            }
            return tablelist;
        }

        public bool insert_dathang(string sopartno, int soluong, string mota, string linkmuahang, string ngaydatmua, string trangthai)
        {
            string query = string.Format("INSERT INTO [dbo].[dathang] (sopartno, soluong, mota, linkmuahang,ngaydatmua, trangthai) VALUES (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}')", sopartno, soluong, mota, linkmuahang, ngaydatmua, trangthai);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool themdulieuthucong(string solevel, string soserialnumber, string sopartno, string mota, string linkmuahang1, string linkmuahang2, string linkmuahang3)
        {
            string query = string.Format("INSERT INTO [dbo].[khohang] (solevel, soserialnumber, sopartno, mota,linkmuahang1, linkmuahang2, linkmuahang3) VALUES (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', N'{6}')",
                solevel, soserialnumber, sopartno, mota, linkmuahang1, linkmuahang2, linkmuahang3);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool capnhatdulieuthucong(string solevel, string soserialnumber, string sopartno, string mota, string linkmuahang1, string linkmuahang2, string linkmuahang3)
        {
            string query = string.Format("UPDATE [dbo].[khohang] set solevel=N'{0}', soserialnumber=N'{1}', mota=N'{3}',linkmuahang1=N'{4}', linkmuahang2=N'{5}', linkmuahang3=N'{6}' WHERE sopartno =N'{2}' ", solevel, soserialnumber, sopartno, mota, linkmuahang1, linkmuahang2, linkmuahang3);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }


    }
}