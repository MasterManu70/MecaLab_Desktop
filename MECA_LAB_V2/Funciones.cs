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
        //Método TableToCombo: Se trae consigo todos los registros de una tabla y a su vez sus llaves primarias.
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

        //Método SetStatus: Modifica el campo Status de un registo en la base de datos:
        // - tabla  : Nombre de la tabla en la base de datos
        // - id     : ID del registro
        // - status : valor del status que se desea colocar
        public static bool SetStatus(string tabla, int id, int status)
        {
            try
            {
                Conexion.MySQL("UPDATE " + tabla + " SET status = '" + status + "' WHERE " + tabla + ".id = " + id + ";");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error No. " + ex.ErrorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //Método StatusUpdate: Hace efectivo el dinamismo del botón Eliminar/Habilitar
        // - tabla  : Nombre de la tabla en la base de datos
        // - accion : Función que desempeñará el botón Eliminar/Habilitar introduciendo su propiedad '.Text'
        // - id     : ID del registro
        public static bool StatusUpdate(string tabla, string accion, int id )
        {
            var respuesta = MessageBox.Show("¿Esta seguro de " + accion + " este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                if (accion == "Eliminar")
                {
                    if (SetStatus(tabla, id, 0)) MessageBox.Show("El registro se ha dado de baja correctamente.","Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    if (SetStatus(tabla, id, 1)) MessageBox.Show("El registro se ha dado de alta correctamente.","Informacion",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
