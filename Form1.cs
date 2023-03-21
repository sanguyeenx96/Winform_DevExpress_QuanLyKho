using DevExpress.DataAccess.Native.Data;
using DevExpress.Office;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ExcelDataReader;
using QUANLYKHOHANG.DAO;
using QUANLYKHOHANG.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DataTable = System.Data.DataTable;

namespace QUANLYKHOHANG
{

    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
            navigationFrame.TransitionType = DevExpress.Utils.Animation.Transitions.Fade;

        }
        //TRANG CHỦ:********************************************************************************************************
        private void XtraForm1_Load(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = lichsupage;
            List<NhapDTO2> TableList = NhapDao.Instance.LoadTableList_dangdathang();
            gridControl_dangdathang.DataSource = TableList;
            this.gridView2.Columns[0].Visible = false;

            List<NhapDTO2> TableList2 = NhapDao.Instance.LoadTableList_hangdave();
            gridControl_hangdave.DataSource = TableList2;
            this.gridView3.Columns[0].Visible = false;
        }

        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            //navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
        }

        //TRANG KHO HÀNG:*****************************************************************************************************
        private void customersTileBarItem_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPage = khohangpage;
            List<NhapDTO> TableList = NhapDao.Instance.LoadTableList();
            gridControl1.DataSource = TableList;
            this.gridView1.Columns[0].Visible = false;
        }
   



        //TRANG ĐẶT HÀNG:*****************************************************************************************************
        private void tileBarItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            txtngay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            navigationFrame.SelectedPage = dathangpage;
            string basepath = Application.StartupPath;
            string txtpath = basepath + @"/setting.txt";
            if (File.Exists(txtpath))
            {
                using (StreamReader sr = new StreamReader(txtpath))
                {
                    while (sr.Peek() >= 0)
                    {
                        string ss = sr.ReadLine();
                        string[] txtsplit = ss.Split(';');
                        string sever = txtsplit[0].ToString();
                        string userid = txtsplit[1].ToString();
                        string password = txtsplit[2].ToString();
                        string database = txtsplit[3].ToString();
                        string connectstring = sever + ";" + userid + ";" + password + ";" + database;
                        using (SqlConnection sqlConnection = new SqlConnection(connectstring))
                        {
                            SqlCommand sqlCmd = new SqlCommand("SELECT distinct sopartno FROM khohang", sqlConnection);
                            sqlConnection.Open();
                            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                            while (sqlReader.Read())
                            {
                                combobox_partno.Properties.Items.Add(sqlReader["sopartno"].ToString());
                            }
                            sqlReader.Close();
                        }
                    }
                }
            }
        }

        private void combobox_partno_SelectedValueChanged(object sender, EventArgs e)
        {
            comboboxLink.Properties.Items.Clear();
            string sopartno = combobox_partno.Text;
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.khohang WHERE sopartno='" + sopartno + "'");
            {
                string mota = data.Rows[0][4].ToString();
                string linkmuahang1 = data.Rows[0][5].ToString();
                string linkmuahang2 = data.Rows[0][6].ToString();
                string linkmuahang3 = data.Rows[0][7].ToString();

                txtMota.Text = mota;
                if (linkmuahang1 != null && linkmuahang1 != "")
                {
                    comboboxLink.Properties.Items.Add(linkmuahang1);
                }
                if (linkmuahang2 != null && linkmuahang2 != "")
                {
                    comboboxLink.Properties.Items.Add(linkmuahang2);
                }
                if (linkmuahang3 != null && linkmuahang3 != "")
                {
                    comboboxLink.Properties.Items.Add(linkmuahang3);
                }
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var dlg = XtraMessageBox.Show($"Bạn có chắc chắn muốn đặt hàng?\n(Dữ liệu vừa nhập là chính xác?)", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                try
                {
                    string sopartno = combobox_partno.Text;
                    int soluong = ((int)numericUpDown1.Value);
                    string mota = txtMota.Text;
                    string linkmuahang = comboboxLink.Text;
                    string ngaydatmua = txtngay.Text;
                    string trangthai = "ĐANG ĐẶT HÀNG";
                    NhapDao.Instance.insert_dathang(sopartno, soluong, mota, linkmuahang, ngaydatmua, trangthai);
                    XtraMessageBox.Show($"Đã thêm dữ liệu đặt hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch
                {
                    XtraMessageBox.Show($"Có lỗi xảy ra...", "Không thành công!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string basepath = Application.StartupPath;
                string txtpath = basepath + @"/setting.txt";
                if (File.Exists(txtpath))
                {
                    using (StreamReader sr = new StreamReader(txtpath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string ss = sr.ReadLine();
                            string[] txtsplit = ss.Split(';');
                            string sever = txtsplit[0].ToString();
                            string userid = txtsplit[1].ToString();
                            string password = txtsplit[2].ToString();
                            string database = txtsplit[3].ToString();
                            string connectstring = sever + ";" + userid + ";" + password + ";" + database;
                            string StrConnection = connectstring;
                            SqlConnection conn = new SqlConnection(StrConnection);
                            OpenFileDialog OpenfileDialog = new OpenFileDialog();
                            //OpenfileDialog.Filter = "Excel 97-2003 Workbook (*.xls) | *.xls";
                            if (OpenfileDialog.ShowDialog() == DialogResult.OK)
                            {
                                txtDirectory.Text = OpenfileDialog.FileName;
                            }
                            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtDirectory.Text.Trim() + ";Extended Properties=Excel 8.0";
                            OleDbConnection oledbConn = new OleDbConnection(connectionString);
                            oledbConn.Open();
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            objAdapter1.Fill(ds);
                            DataTable Dt = ds.Tables[0];
                            int sodonhang = 0;
                            
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                DataRow row = Dt.Rows[i];
                                int columnCount = Dt.Columns.Count;
                                string[] columns = new string[columnCount];
                                for (int j = 0; j < columnCount; j++)
                                {
                                    columns[j] = row[j].ToString();
                                }

                                string query = "SELECT * FROM dbo.khohang WHERE sopartno='" + columns[0] + "'";
                                var result = DataProvider.Instance.ExecuteScalar(query);
                                if (result != null)
                                {
                                    //Có dữ liệu
                                    DataTable load = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.khohang WHERE sopartno='" + columns[0] + "'");
                                    //string load_solevel;
                                    //string load_soserialnumber;
                                    string load_mota;
                                    //string load_linkmuahang1;
                                    //string load_linkmuahang2;
                                    //string load_linkmuahang3;
                                    //load_solevel = load.Rows[0][1].ToString();
                                    //load_soserialnumber = load.Rows[0][2].ToString();
                                    load_mota = load.Rows[0][4].ToString();
                                    //load_linkmuahang1 = load.Rows[0][5].ToString();
                                    //load_linkmuahang2 = load.Rows[0][6].ToString();
                                    //load_linkmuahang3 = load.Rows[0][7].ToString();
                                    string ngaydatmua = DateTime.Now.ToString("dd/MM/yyyy");
                                    string trangthai = "ĐANG ĐẶT HÀNG";
                                    conn.Open();
                                    string sql = "INSERT INTO dathang(sopartno,soluong,mota,ngaydatmua,trangthai)";
                                    sql += "VALUES('" + columns[0] + "','" + columns[1] + "','" + load_mota + "','" + ngaydatmua + "',N'" + trangthai + "')";
                                    SqlCommand cmod = new SqlCommand(sql, conn);
                                    cmod.ExecuteNonQuery();
                                    conn.Close();
                                    sodonhang++;
                                }
                                else
                                {
                                    //Chưa có dữ liệu
                                    string mota = "";
                                    string ngaydatmua = DateTime.Now.ToString("dd/MM/yyyy");
                                    string trangthai = "ĐANG ĐẶT HÀNG";
                                    conn.Open();
                                    string sql = "INSERT INTO dathang(sopartno,soluong,mota,ngaydatmua,trangthai)";
                                    sql += "VALUES('" + columns[0] + "','" + columns[1] + "','" + mota + "','" + ngaydatmua + "',N'" + trangthai + "')";
                                    SqlCommand cmod = new SqlCommand(sql, conn);
                                    cmod.ExecuteNonQuery();
                                    conn.Close();
                                    sodonhang++;
                                }


                            }
                            XtraMessageBox.Show($"• Tổng: " + sodonhang + " dữ liệu.", "Đã thêm dữ liệu đặt hàng thành công!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show($"Có lỗi xảy ra...", "Không thành công!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //TRANG LỊCH SỬ:***********************************************************************************************************
        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPage = lichsupage;
            List<NhapDTO2> TableList = NhapDao.Instance.LoadTableList_dangdathang();
            gridControl_dangdathang.DataSource = TableList;
            this.gridView2.Columns[0].Visible = false;

            List<NhapDTO2> TableList2 = NhapDao.Instance.LoadTableList_hangdave();
            gridControl_hangdave.DataSource = TableList2;
            this.gridView3.Columns[0].Visible = false;
        }
        





        //TRANG THÊM:
        private void tileBarItem3_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPage = thempage;
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var dlg = XtraMessageBox.Show($"Bạn có chắc chắn muốn thêm dữ liệu?\n(Dữ liệu vừa nhập là chính xác?)", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                try
                {
                    string solevel = numericUpDown2.Text;
                    string soserialnumber = txt_serialnumber.Text;
                    string sopartno = txt_partno.Text;
                    string mota = txt_mota.Text;
                    string linkmuahang1 = link1.Text;
                    string linkmuahang2 = link2.Text;
                    string linkmuahang3 = link3.Text;
                    string query = "SELECT * FROM dbo.khohang WHERE sopartno='" + sopartno + "'";
                    var result = DataProvider.Instance.ExecuteScalar(query);
                    if (result != null)
                    {
                        //Trùng dữ liệu
                        NhapDao.Instance.capnhatdulieuthucong(solevel, soserialnumber, sopartno, mota, linkmuahang1, linkmuahang2, linkmuahang3);
                        XtraMessageBox.Show($"Đã cập nhật dữ liệu: " + sopartno + " thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //Chưa có dữ liệu
                        NhapDao.Instance.themdulieuthucong(solevel, soserialnumber, sopartno, mota, linkmuahang1, linkmuahang2, linkmuahang3);
                        XtraMessageBox.Show($"Đã thêm dữ liệu mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    XtraMessageBox.Show($"Có lỗi xảy ra...", "Không thành công!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
                
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            try
            {
                string basepath = Application.StartupPath;
                string txtpath = basepath + @"/setting.txt";
                if (File.Exists(txtpath))
                {
                    using (StreamReader sr = new StreamReader(txtpath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string ss = sr.ReadLine();
                            string[] txtsplit = ss.Split(';');
                            string sever = txtsplit[0].ToString();
                            string userid = txtsplit[1].ToString();
                            string password = txtsplit[2].ToString();
                            string database = txtsplit[3].ToString();
                            string connectstring = sever + ";" + userid + ";" + password + ";" + database;
                            string StrConnection = connectstring;
                            SqlConnection conn = new SqlConnection(StrConnection);
                            OpenFileDialog OpenfileDialog = new OpenFileDialog();
                            //OpenfileDialog.Filter = "Excel 97-2003 Workbook (*.xls) | *.xls";
                            if (OpenfileDialog.ShowDialog() == DialogResult.OK)
                            {
                                txtDirectory.Text = OpenfileDialog.FileName;
                            }
                            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtDirectory.Text.Trim() + ";Extended Properties=Excel 8.0";
                            OleDbConnection oledbConn = new OleDbConnection(connectionString);
                            oledbConn.Open();
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            objAdapter1.Fill(ds);
                            DataTable Dt = ds.Tables[0];
                            int themmoi = 0;
                            int capnhat = 0;
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                DataRow row = Dt.Rows[i];
                                int columnCount = Dt.Columns.Count;
                                string[] columns = new string[columnCount];
                                for (int j = 0; j < columnCount; j++)
                                {
                                    columns[j] = row[j].ToString();
                                }

                                string query = "SELECT * FROM dbo.khohang WHERE sopartno='" + columns[2] + "'";
                                var result = DataProvider.Instance.ExecuteScalar(query);
                                if (result != null)
                                {
                                    //Trùng dữ liệu
                                    conn.Open();
                                    string sql = "UPDATE khohang SET solevel ='"+ columns[0] + "',soserialnumber = '"+ columns[1] + "',mota = '"+ columns[3] + "',linkmuahang1 = '"+ columns[4] + "',linkmuahang2 = '"+ columns[5] + "',linkmuahang3 = '"+ columns[6] + "' ";
                                    sql += "WHERE sopartno='" + columns[2] + "'";
                                    //string sql = "INSERT INTO khohang(solevel,soserialnumber,sopartno,mota,linkmuahang1,linkmuahang2,linkmuahang3)";
                                    //sql += "VALUES('" + columns[0] + "','" + columns[1] + "','" + columns[2] + "',Convert(varchar(10),'" + columns[3] + "',103),'" + columns[4] + "')";
                                    //sql += "VALUES('" + columns[0] + "','" + columns[1] + "','" + columns[2] + "','" + columns[3] + "','" + columns[4] + "','" + columns[5] + "','" + columns[6] + "' )";
                                    SqlCommand cmod = new SqlCommand(sql, conn);
                                    cmod.ExecuteNonQuery();
                                    conn.Close();
                                    capnhat++;
                                }
                                else
                                {
                                    //Chưa có dữ liệu
                                    conn.Open();
                                    string sql = "INSERT INTO khohang(solevel,soserialnumber,sopartno,mota,linkmuahang1,linkmuahang2,linkmuahang3)";
                                    //sql += "VALUES('" + columns[0] + "','" + columns[1] + "','" + columns[2] + "',Convert(varchar(10),'" + columns[3] + "',103),'" + columns[4] + "')";
                                    sql += "VALUES('" + columns[0] + "','" + columns[1] + "','" + columns[2] + "','" + columns[3] + "','" + columns[4] + "','" + columns[5] + "','" + columns[6] + "' )";
                                    SqlCommand cmod = new SqlCommand(sql, conn);
                                    cmod.ExecuteNonQuery();
                                    conn.Close();
                                    themmoi++;
                                }


                            }
                            //MessageBox.Show("Đã nhập dữ liệu xong! Thêm mới: "+themmoi+" dữ liệu, Cập nhật: "+capnhat+" dữ liệu.");
                            XtraMessageBox.Show($"• Thêm mới: " +themmoi+ " dữ liệu.\n• Cập nhật: " + capnhat+" dữ liệu.", "Đã nhập dữ liệu thành công!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            
                        }
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show($"Có lỗi xảy ra...", "Không thành công!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void tileBarItem4_ItemClick(object sender, TileItemEventArgs e)
        {

        }

        private void tileBarItem5_ItemClick_1(object sender, TileItemEventArgs e)
        {

        }






















        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }

}