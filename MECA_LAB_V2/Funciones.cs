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
            cmb.Items.Clear();
            llaves.Clear();

            DataSet ds;

            try
            {
                ds = Conexion.MySQL(GetQuery(tabla));
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

        //Método GetColums: Obtener los nombres de cada columna de la tabla asignada en el constructor de la clase.
        public static List<string> GetColumns(string tabla)
        {
            List<string> Columns = new List<string>();
            DataSet ds = Conexion.MySQL("describe " + tabla + ";");

            try
            {
                for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                    Columns.Add(Convert.ToString(ds.Tables["tabla"].Rows[i][0]));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error de MySQL: " + e);
                throw;
            }

            return Columns;
        }

        //Método GetQuery: Devuelve la consulta correspondiente dependiendo del nombre de la tabla que se envíe al ser llamado el método.
        public static string GetQuery(string tabla, int id = 0, int status = 1)
        {
            string query = "";
            tabla = tabla.ToLower();
            switch(tabla)
            {
                case "alumnos":         query = "SELECT * FROM (SELECT alumnos.id as ID, alumnos.matricula as Matrícula, alumnos.nombre as Nombre, alumnos.apellidop as Paterno, alumnos.apellidom as Materno, carreras.carrera as Carrera, alumnos.correo as Correo, alumnos.telefono as Teléfono, alumnos.created_at as Creado, alumnos.updated_at as Actualizado, alumnos.status as status FROM alumnos INNER JOIN carreras ON alumnos.carrera = carreras.id) as Estudiante"; break;
                case "articulos":       query = "SELECT id as ID, articulo as Artículo, comentario as Comentario, disponible as Disponible, created_at as Creado, updated_at as Actualizado, status FROM articulos"; break;
                case "asignaturas":     query = "SELECT id as ID, asignatura as Asignatura, created_at as Creado, updated_at as Actualizado, status FROM asignaturas"; break;
                case "carreras":        query = "SELECT id as ID, carrera as Carrera, created_at as Creado, updated_at as Actualizado, status FROM carreras";  break;
                case "laboratorios":    query = "SELECT id as ID, laboratorio as Laboratorio, created_at as Creado, updated_at as Actualizado, status FROM laboratorios"; break;
                //No me sirvio nomas por el keys.space da error igual no tiene nada de malo poner ''   case "maestros":        query = "SELECT id as ID, concat(nombre," + Keys.Space + ",apellidop," + Keys.Space + ",apellidom) as Maestro,created_at as Creado, updated_at as Actualizado, status FROM maestros;"; break;
                case "maestros":        query = "SELECT id ID, CONCAT(nombre,' ', apellidop,' ', apellidom) Maestro, created_at Creado, updated_at Actualizado, status FROM maestros"; break;
                case "usuarios":        query = "SELECT id as ID, usuario as Usuario, nivel as Nivel, created_at as Creado, updated_at as Actualizado, status from usuarios"; break;
                case "movimientos":     query = "SELECT movimientos.id ID,usuarios.usuario Usuario,movimientos.id_registro Registro,movimientos.tabla Tabla,movimientos.campo Campo,movimientos.nuevo Nuevo,movimientos.viejo Viejo,movimientos.descripcion Descripción,movimientos.created_at Creado FROM `movimientos` INNER JOIN usuarios ON movimientos.usuario = usuarios.id"; break;
            }

            if (id != 0) 
            { 
                query += " WHERE ID = " + id;
                if (status == 1 || status == 0) query += " AND status = " + status;
            }

            if (id == 0)
            {
                if (status == 1 || status == 0) query += " WHERE status = " + status;
            }

            query += ";";

            return query;
        }

        //Método Insert: Inserta o actualiza los registros de una tabla dependiendo de qué tabla se le cargue al ser llamado.
        // - tabla      : Nombre de la tabla
        // - valores    : Lista de valores del registro que se desea agregar o modificar que se agregan a la lista en el orden que está en la tabla.
        //                Si se envía el ID correspondiente al registro se modificará, si se envía ID = 0 se creará un nuevo registro.
        public static bool Insert(string tabla, List<string> valores)
        {
            List<string> columnas;
            columnas = GetColumns(tabla);

            string query;

            if (valores[0] != "0")
            {
                query = "UPDATE " + tabla + " SET ";
                for (int i = 1; i < columnas.Count; i++)
                {
                    if (columnas[i].ToLower() == "created_at" || (columnas[i].ToLower() == "password" && valores[i].Length <= 7)) continue;

                    query += columnas[i] + "=" + valores[i];

                    if (i < columnas.Count - 1) query += ",";
                }

                query += " WHERE id=" + valores[0] + ";";
            }
            else
            {
                query = "INSERT INTO " + tabla + " VALUES(NULL,";
                for (int i = 1; i < columnas.Count; i++)
                {
                    query += valores[i];

                    if (i < columnas.Count - 1) query += ",";
                }
                query += ");";
            }

            try
            {
                Conexion.MySQL(query);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
