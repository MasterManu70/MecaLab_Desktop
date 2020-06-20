using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MECA_LAB_V2
{
    class Conexion
    {
        private static string host       = "localhost";
        private static string database   = "mecalab";
        private static string user       = "root";
        private static string password   = "";

        private static string cadconn = "Server=" + host + "; Database=" + database + "; Uid=" + user + "; Pwd=" + password + ";";

        public static DataSet MySQL(string sql)
        {
            MySqlConnection cnn;
            MySqlCommand cmd;
            MySqlDataAdapter da;
            DataSet ds;

            cnn = new MySqlConnection(cadconn);
            cmd = new MySqlCommand(sql, cnn);
            da = new MySqlDataAdapter(cmd);
            ds = new DataSet();

            try
            {
                da.Fill(ds, "tabla");
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message, "Error No. " + ex.ErrorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ds;
            }

            return ds;
        }
    }
}
