using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        public static string GetQuery(string tabla, int id = 0, int status = 1, string like = "")
        {
            int result = 0;
            string query = "";
            tabla = tabla.ToLower();

            //Base de la consulta
            switch(tabla)
            {
                case "alumnos":         query = "SELECT * FROM (SELECT alumnos.id as ID, alumnos.matricula as Matrícula, alumnos.nombre as Nombre, alumnos.apellidop as Paterno, alumnos.apellidom as Materno, carreras.carrera as Carrera, alumnos.correo as Correo, alumnos.telefono as Teléfono, alumnos.created_at as Creado, alumnos.updated_at as Actualizado, alumnos.status as status FROM alumnos INNER JOIN carreras ON alumnos.carrera = carreras.id) as Tabla"; break;
                case "articulos":       query = "SELECT * FROM (SELECT id as ID, articulo as Artículo, comentario as Comentario, disponible as Disponible, created_at as Creado, updated_at as Actualizado, status FROM articulos) as Tabla"; break;
                case "asignaturas":     query = "SELECT * FROM (SELECT id as ID, asignatura as Asignatura, created_at as Creado, updated_at as Actualizado, status FROM asignaturas) as Tabla"; break;
                case "carreras":        query = "SELECT * FROM (SELECT id as ID, carrera as Carrera, created_at as Creado, updated_at as Actualizado, status FROM carreras) as Tabla";  break;
                case "laboratorios":    query = "SELECT * FROM (SELECT id as ID, laboratorio as Laboratorio, created_at as Creado, updated_at as Actualizado, status FROM laboratorios) as Tabla"; break;
                case "maestros":        query = "SELECT * FROM (SELECT id ID, CONCAT(nombre,' ', apellidop,' ', apellidom) Maestro, created_at Creado, updated_at Actualizado, status FROM maestros) as Tabla"; break;
                case "movimientos":     query = "SELECT * FROM (SELECT movimientos.id ID,usuarios.usuario Usuario,movimientos.id_registro Registro,movimientos.tabla Tabla,movimientos.campo Campo,movimientos.nuevo Nuevo,movimientos.viejo Viejo,movimientos.descripcion Descripción,movimientos.created_at Creado FROM `movimientos` INNER JOIN usuarios ON movimientos.usuario = usuarios.id) as Tabla"; break;
                case "prestamos":       query = @"SELECT * FROM (SELECT prestamos.id ID, CONCAT(alumnos.nombre,' ',alumnos.apellidop,' ', alumnos.apellidom) Alumno, CONCAT(maestros.nombre,' ',maestros.apellidop,' ', maestros.apellidom) Maestro, laboratorios.laboratorio Laboratorio,
                                                asignaturas.asignatura Asignaturas,usuarios.usuario Usuario,prestamos.fecha_fin Entrega,prestamos.created_at Creado,prestamos.updated_at Actualizado, prestamos.status status FROM prestamos
                                                INNER JOIN alumnos ON prestamos.alumno = alumnos.id INNER JOIN maestros ON prestamos.maestro = maestros.id INNER JOIN laboratorios ON prestamos.laboratorio = laboratorios.id
                                                INNER JOIN asignaturas ON prestamos.asignatura = asignaturas.id INNER JOIN usuarios ON prestamos.usuario = usuarios.id) as Tabla"; break;
                case "usuarios":        query = "SELECT * FROM (SELECT id as ID, usuario as Usuario, nivel as Nivel, created_at as Creado, updated_at as Actualizado, status from usuarios) as Tabla"; break;
            }

            //Validaciones avanzadas
            if (id != 0) 
            { 
                query += " WHERE ID = " + id;
                if (status == 1 || status == 0) query += " AND status = " + status;
            }

            if (id == 0 && (status == 1 || status == 0))
            {
                query += " WHERE status = " + status;
            }

            //Validación optimizada para búsqueda
            if (like != "")
            {
                if (id != 0 || status == 1 || status == 0) query += " AND "; else query += " WHERE ";

                switch (tabla)
                {
                    case "alumnos":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else if (int.TryParse(like, out result))                                query += " (Matrícula LIKE '%" + like + "%' OR Teléfono LIKE '%" + like + "%')";
                        else if (like.Contains("@"))                                            query += " Correo LIKE '%" + like + "%'";
                        else                                                                    query += " (Nombre LIKE '%" + like + "%' OR Paterno LIKE '%" + like + "%' OR Materno LIKE '%" + like + "%')";
                        break;
                    case "articulos":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else                                                                    query += " Artículo LIKE '%" + like + "%'";
                        break;
                    case "asignaturas":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else                                                                    query += " Asignatura LIKE '%" + like + "%'";
                        break;
                    case "carreras":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else                                                                    query += " Carrera LIKE '%" + like + "%'";
                        break;
                    case "laboratorios":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else                                                                    query += " Laboratorio LIKE '%" + like + "%'";
                        break;
                    case "maestros":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else                                                                    query += " Maestro LIKE '%" + like + "%'";
                        break;
                    case "usuarios":
                        if (int.TryParse(like, out result) && like.Length <= 4 && result > 0)   query += " ID = " + result + ";";
                        else                                                                    query += " Usuario LIKE '%" + like + "%'";
                        break;
                }
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

        public static void ReportPrint(string tabla, DataGridView dataGridView1)
        {
            string nombre = tabla.ToLower() + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".html";
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para imprimir.");
                return;
            }
            TextWriter writer = new StreamWriter(nombre);
            writer.WriteLine(@"<!DOCTYPE html>
                            <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Sistema de Inventario y Prestamos MECALAB 2020 Universidad Tecnologica de Hermosillo</title>
                            <style>
                            * {
                                font-family: century gothic;
                            }

                            .contenedor {
                                clear: both;
                                margin-left: auto;
                                margin-right: auto;
                                width: 100%;
                                height: 100%;
                                border-radius: 10px;
                                border: 1px solid whitesmoke;
                                background-color: white;

                            }

                            #formularioTabla {
                                width: 100%;
                                margin: 5px;
                                margin-left: auto;
                                margin-right: auto;
                            }

                            table {
                                width: 100%;
                                margin: 5px;
                                font-size: 16px;
                                margin-left: auto;
                                margin-right: auto;
                            }

                            th {
                                font-size: 12px;
                                border-bottom: 2px solid black;
                                text-align: center;
                                color: black;
                                padding: 10px;
                            }

                            td {
                                font-size: 12px;
                                border-right: 1px solid black;
                                border-bottom: 1px solid black;
                                text-align: justify;
                            }
                            #linea-izq{
                                border-left: 1px solid black;
                            }
                            </style>
                            </head>");

            writer.WriteLine(@"<body>
    <div class='contenedor'>
        <div class='row'>
            <div id='formularioTabla'>
                <table>");
            writer.WriteLine("<tr><th colspan = '" + dataGridView1.Columns.Count + "' style = 'font-size: 24PX;'> REPORTE DE " + tabla.ToUpper() + " </th> </tr> ");

            writer.WriteLine("<tr>");
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                writer.WriteLine("<th>" + column.HeaderText + "</th>");
            }
            writer.WriteLine("</tr>");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                writer.WriteLine("<tr>");
                bool especialito = true;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (especialito)
                    {
                        writer.WriteLine("<td id='linea-izq'>" + row.Cells[0].Value.ToString() + "</td>");
                        especialito = !especialito;
                        continue;
                    }
                    writer.WriteLine("<td>" + cell.Value + "</td>");
                }
                writer.WriteLine("</tr>");
            }


            writer.WriteLine("<tr>");
            writer.WriteLine("<th colspan='" + dataGridView1.Columns.Count + "' style='font-size: 12PX;'>Sistema de Inventario y Prestamos MECALAB 2020</th>");
            writer.WriteLine(@"</tr>
                </table>
            </div>
        </div>
    </div>
    </div>
</body>

</html>");
            writer.Close();

            MessageBox.Show("Se generó el reporte con el nombre: " + nombre);
        }
    }
}
