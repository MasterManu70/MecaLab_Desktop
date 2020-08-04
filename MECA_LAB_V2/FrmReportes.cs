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
    public partial class FrmReportes : Form
    {
        string html;
        string query;
        string query_detalles;
        string inicio;
        string fin;
        DataSet ds;
        DataSet ds_detalles;
        DataGridView dtg = new DataGridView();
        List<int> columnas = new List<int>();
        public FrmReportes()
        {
            dtg.AllowUserToAddRows = false;
            dtg.RowHeadersVisible = false;
            InitializeComponent();
        }

        private void cmbReportes_TextChanged(object sender, EventArgs e)
        {
            ReportView();
            if (cmbReportes.Text == "Préstamos")
            {
                groupBox4.Visible = true;
                chkPrestamo.Checked = false;
            }
            else
            {
                groupBox4.Visible = false;
            }
        }

        private void ReportView()
        {
            switch (cmbReportes.SelectedIndex)
            {
                case 0:
                    query = "SELECT * FROM (SELECT movimientos.id_registro ID, articulos.articulo Artículo, movimientos.descripcion Descripción, movimientos.created_at Fecha FROM movimientos INNER JOIN articulos ON movimientos.id_registro = articulos.id WHERE movimientos.descripcion = 'Alta' OR movimientos.descripcion = 'Baja') Tabla";
                    break;
                case 1:
                    query = "SELECT * FROM (SELECT id ID, articulo Artículo, comentario Comentario, IF(disponible=1,'Inventario','Prestado') Disponibilidad, created_at Fecha,if (status=1,'Alta','Baja') as status FROM articulos) Tabla";
                    break;
                case 2:
                    query = @"SELECT * FROM (SELECT prestamos.id ID, CONCAT(alumnos.nombre,' ',alumnos.apellidop,' ', alumnos.apellidom) Alumno, CONCAT(maestros.nombre,' ',maestros.apellidop,' ', maestros.apellidom) Maestro, laboratorios.laboratorio Laboratorio,
                                                asignaturas.asignatura Asignatura,usuarios.usuario Usuario,prestamos.fecha_fin Entrega,prestamos.created_at Fecha,prestamos.updated_at Entregado, IF(prestamos.status = 1,'Activo','Terminado') status FROM prestamos
                                                INNER JOIN alumnos ON prestamos.alumno = alumnos.id INNER JOIN maestros ON prestamos.maestro = maestros.id INNER JOIN laboratorios ON prestamos.laboratorio = laboratorios.id
                                                INNER JOIN asignaturas ON prestamos.asignatura = asignaturas.id INNER JOIN usuarios ON prestamos.usuario = usuarios.id) as Tabla";
                    if (chkPrestamo.Checked)
                    {
                        query_detalles = "SELECT Artículos FROM (SELECT detalles.prestamo ID, GROUP_CONCAT(articulos.articulo,' - ',articulos.id) Artículos, detalles.created_at Fecha FROM detalles INNER JOIN articulos ON detalles.articulo = articulos.id GROUP BY ID) Tabla";
                    }
                    break;
            }

            if (chkPeriodo.Checked)
            {
                inicio = dtpInicio.Value.ToString().Substring(6, 4) + "-" + dtpInicio.Value.ToString().Substring(3, 2) + "-" + dtpInicio.Value.ToString().Substring(0, 2);
                fin = dtpFin.Value.ToString().Substring(6, 4) + "-" + dtpFin.Value.ToString().Substring(3, 2) + "-" + dtpFin.Value.ToString().Substring(0, 2);

                query += " WHERE Fecha BETWEEN '" + inicio + " 00:00:00' AND '" + fin + " 23:59:59'";
                if (cmbReportes.SelectedIndex == 2 && chkPrestamo.Checked)
                {
                    query_detalles += " WHERE Fecha BETWEEN '" + inicio + " 00:00:00' AND '" + fin + " 23:59:59'";
                }
            }

            query += ";";

            ds = Conexion.MySQL(query);
            if (cmbReportes.SelectedIndex == 2 && chkPrestamo.Checked)
            {
                ds_detalles = Conexion.MySQL(query_detalles);
            }

            columnas.Clear();
            for (int i = 0; i < ds.Tables["tabla"].Columns.Count; i++) columnas.Add(i);

            html = dtgTohtml(ds);

            webBrowser1.DocumentText = html;

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(ds.Tables["tabla"].Columns.Count);

            for (int i = 0; i < ds.Tables["tabla"].Columns.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = ds.Tables["tabla"].Columns[i].ToString();
                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }

            dataGridView1.ClearSelection();
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

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Black)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
                columnas.Remove(e.RowIndex);
            }
            else if (dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Red)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                columnas.Add(e.RowIndex);
            }

            columnas.Sort();

            html = dtgTohtml(ds);

            webBrowser1.DocumentText = html;
        }

        public string dtgTohtml(DataSet ds)
        {
            string html = "";

            html += @"<!DOCTYPE html>
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
                            </head>";

            html += @"<body>
    <div class='contenedor'>
        <div class='row'>
            <div id='formularioTabla'>
                <table>";
            html += "<tr><th colspan = '" + columnas.Count + "' style = 'font-size: 24PX;'> REPORTE DE " + cmbReportes.Items[cmbReportes.SelectedIndex].ToString().ToUpper() + " </th> </tr> ";

            html += "<tr>";

            foreach (int columna in columnas)
            {
                html += "<th>" + ds.Tables["tabla"].Columns[columna].ToString() + "</th>";
            }

            html += "</tr>";


            if (cmbReportes.SelectedIndex == 2 && chkPrestamo.Checked)
            {
                for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                {
                    html += "<tr>";
                    bool especialito = true;
                    foreach (int columna in columnas)
                    {
                        if (especialito)
                        {
                            html += "<td id='linea-izq'>" + ds.Tables["tabla"].Rows[i][columna].ToString() + "</td>";
                            especialito = !especialito;
                            continue;
                        }
                        html += "<td>" + ds.Tables["tabla"].Rows[i][columna].ToString() + "</td>";
                    }
                    html += "</tr>";
                    html += "<td id='linea-izq' colspan='" + columnas.Count + "'><strong style='color: red; ' > Lista de articulos: </strong><strong>" + ds_detalles.Tables["tabla"].Rows[i][0].ToString() + "</strong></td>";
                }
            }
            else
            {
                for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                {
                    html += "<tr>";
                    bool especialito = true;
                    foreach (int columna in columnas)
                    {
                        if (especialito)
                        {
                            html += "<td id='linea-izq'>" + ds.Tables["tabla"].Rows[i][columna].ToString() + "</td>";
                            especialito = !especialito;
                            continue;
                        }
                        html += "<td>" + ds.Tables["tabla"].Rows[i][columna].ToString() + "</td>";
                    }
                    html += "</tr>";
                }
            }

            html += "<tr>";
            html += "<th colspan='" + columnas.Count + "' style='font-size: 12PX;'>Sistema de Inventario y Prestamos MECALAB 2020</th>";
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

        private void chkPeriodo_CheckedChanged(object sender, EventArgs e)
        {
            ReportView();
        }

        private void chkPrestamo_CheckedChanged(object sender, EventArgs e)
        {
            ReportView();
        }
    }
}
