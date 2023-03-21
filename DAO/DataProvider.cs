using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUANLYKHOHANG.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }
        private DataProvider() { }
        private static string getconnectionstring()
        {
            string basepath = Application.StartupPath;
            string txtpath = basepath + @"/setting.txt";
            string connectstring = "";
            try
            {
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
                            connectstring = sever + ";" + userid + ";" + password + ";" + database;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            return (connectstring);
        }


        public DataTable ExecuteQuery(string query, object[] paramater = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(getconnectionstring()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }
        public int ExcuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(getconnectionstring()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }
        public object ExecuteScalar(string query, object[] paramater = null)
        {
            object data = null;
            using (SqlConnection connection = new SqlConnection(getconnectionstring()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();
            }
            return data;
        }
    }
}
