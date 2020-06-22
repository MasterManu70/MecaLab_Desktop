using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    //Clase Funciones: Aquí se alojan todas las funciones globales del sistema.
    class Funciones
    {
        public static void TableToCombo(ComboBox cmb, List<int> llaves, string tabla)
        {
            DataSet ds;

            try
            {
                ds = Conexion.MySQL("select distinct * from " + tabla + ";");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error No. " + ex.ErrorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            if (ds == null) return;

            for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
            {
                llaves.Add(int.Parse(ds.Tables["tabla"].Rows[i][0].ToString()));
                cmb.Items.Add(ds.Tables["tabla"].Rows[i][1].ToString());
            }
        }
    }
}
