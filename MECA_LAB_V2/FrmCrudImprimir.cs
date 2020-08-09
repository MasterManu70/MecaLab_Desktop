using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmCrudImprimir : Form
    {
        string sql;
        string tabla;
        string inicio;
        string fin;
        bool periodo;
        string query;
        List<int> columnas = new List<int>();
        DataSet ds;
        public FrmCrudImprimir(string tabla, string sql, bool periodo)
        {
            this.sql = sql;
            this.tabla = tabla;
            this.periodo = periodo;
            InitializeComponent();
        }

        private void FrmCrudImprimir_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = periodo;
            groupBox2.Visible = periodo;

            dtgColumnas.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            webBrowser1.IsWebBrowserContextMenuEnabled = false;

            ReportView();
        }

        public void ReportView()
        {
            switch (tabla)
            {
                case "Alumnos":         query = "SELECT ID,Matrícula,Nombre,Paterno,Materno,Carrera,Correo,Teléfono,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Articulos":       query = "SELECT ID,Artículo,Comentario,IF(Disponible = 1,'Inventario','Prestado') Disponible,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Asignaturas":     query = "SELECT ID,Asignatura,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Carreras":        query = "SELECT ID,Carrera,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Laboratorios":    query = "SELECT ID,Asignatura,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Maestros":        query = "SELECT ID,Maestro,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Movimientos":     query = "SELECT ID,Usuario,Registro,Tabla,Campo,Nuevo,Viejo,Descripción,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Prestamos":       query = "SELECT ID,Alumno,Maestro,Laboratorio,Asignatura,Usuario,Entrega,Creado,Actualizado, IF(status = 1,'Activo','Terminado') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
                case "Usuarios":        query = "SELECT ID,Usuario,Creado,Actualizado, IF(status = 1,'Alta','Baja') status FROM (" + sql.Substring(0, sql.Length - 1) + ") Tabla2"; break;
            }

            if (chkPeriodo.Checked)
            {
                inicio = dtpInicio.Value.ToString().Substring(6, 4) + "-" + dtpInicio.Value.ToString().Substring(3, 2) + "-" + dtpInicio.Value.ToString().Substring(0, 2);
                fin = dtpFin.Value.ToString().Substring(6, 4) + "-" + dtpFin.Value.ToString().Substring(3, 2) + "-" + dtpFin.Value.ToString().Substring(0, 2);
                query += " WHERE Creado BETWEEN '" + inicio + " 00:00:00' AND '" + fin + " 23:59:59'";
            }

            query += ";";

            ds = Conexion.MySQL(query);

            columnas.Clear();
            for (int i = 0; i < ds.Tables["tabla"].Columns.Count; i++) columnas.Add(i);

            dtgColumnas.Rows.Clear();
            dtgColumnas.Rows.Add(ds.Tables["tabla"].Columns.Count);

            for (int i = 0; i < ds.Tables["tabla"].Columns.Count; i++)
            {
                dtgColumnas.Rows[i].Cells[0].Value = ds.Tables["tabla"].Columns[i].ToString();
                dtgColumnas.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }

            webBrowser1.DocumentText = dtgTohtml(ds);
        }

        public string dtgTohtml(DataSet ds)
        {
            string html = "";

            html += @"<!DOCTYPE html>
                            <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>";
            html += "<title>Sistema de Inventario y Préstamos MECALAB " + DateTime.Now.ToString("yyyy") + " Universidad Tecnologica de Hermosillo</title>";
            html += @"<style>
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
            </head>";

            html += @"<body>
    <div class='contenedor'>
        <div class='row'>
            <div id='formularioTabla'>
                <table>";
            html += "<tr><th colspan = '" + columnas.Count + "' style = 'font-size: 24PX;'> REPORTE DE " + tabla.ToUpper() + " </th> </tr> ";

            html += "<tr>";

            foreach (int columna in columnas)
            {
                html += "<th>" + ds.Tables["tabla"].Columns[columna].ToString() + "</th>";
            }

            html += "</tr>";

            for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
            {
                html += "<tr>";
                bool especialito = true;
                foreach (int columna in columnas)
                {
                    if (especialito)
                    {
                        html += "<td id='linea-izq'>" + ds.Tables["tabla"].Rows[i][columna].ToString() + " </td>";
                        especialito = !especialito;
                        continue;
                    }
                    html += "<td>" + ds.Tables["tabla"].Rows[i][columna].ToString() + " </td>";
                }
                html += "</tr>";
            }

            html += "<tr>";
            html += "<th colspan='" + columnas.Count + "' style='font-size: 12PX;'>Sistema de Inventario y Préstamos MECALAB " + DateTime.Now.ToString("yyyy") + "</th>";
            html += @"</tr>
                </table>
            </div>
        </div>
    </div>
    </div>
</body>

</html>";
            return html;
        }

        private void dtgColumnas_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Black)
                {
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
                    columnas.Remove(e.RowIndex);
                }
                else if (dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Red)
                {
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                    columnas.Add(e.RowIndex);
                }

                columnas.Sort();

                webBrowser1.DocumentText = dtgTohtml(ds);
            }
            catch (Exception)
            {
            }
        }

        private void chkPeriodo_CheckedChanged(object sender, EventArgs e)
        {
            ReportView();
        }

        private void dtpInicio_ValueChanged(object sender, EventArgs e)
        {
            dtpFin.MinDate = dtpInicio.Value;
            if (chkPeriodo.Checked)
            {
                ReportView();
            }
        }

        private void dtpFin_ValueChanged(object sender, EventArgs e)
        {
            if (chkPeriodo.Checked)
            {
                ReportView();
            }
        }

        private void btnVista_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                Curva.ElipseRadius = 0;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                Curva.ElipseRadius = 25;
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
