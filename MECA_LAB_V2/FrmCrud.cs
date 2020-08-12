using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmCrud : Form
    {
        /* El formulario 'FrmCrud' está diseñado para poder adaptar sus propiedades dependiendo del CRUD que desea manejar.
         * Propiedades como:
         * - Color: Botón BackColor, Panel Backcolor, DataGridViewHeaders BackColor
         */

        string tabla;           //Nombre de la tabla que se encuentra en la base de datos.
        Color color;            //Objeto de tipo color el cual se asignará a las propiedades del formulario.
        //List<string> columnas;  //Lista de los nombres de cada columna en la tabla especificada en al variable 'tabla'.
        //public static bool consultar = false; //Variable que se usa para validar si el formulario requiere actualizarse.
        
        DataSet ds;
        string like;
        int status;
        int pageLimit = 1;
        int count;
        int residuo;
        int paginas;
        string inicio = "";
        string fin = "";
        string query = "";
        int id;
        bool columnsLoad = true;
        int operadores = 0;
        char[] separadores = new char[] { '=', '+', '-' };

        List<int> prestamosNoCompletos = new List<int>();
        List<string> parametros = new List<string>();
        List<string> valores = new List<string>();

        Form btnRegistroForm;               //Objeto tipo 'Form' en el cual se le asignará el objeto obtenido con el método 'GetForm' de la clase de enrutado (Rutas).
        FrmCrudImprimir frmCrudImprimir;
        public FrmCrud(string tabla, Color color)
        {
            this.tabla = tabla;
            this.color = color;
            InitializeComponent();
            if (tabla == "Prestamos")
            {
                btnRegistro.Visible = false;
            }
        }
        private void FrmCrud_Load(object sender, EventArgs e)
        {
            //DISEÑO
            labelTitle.Text = tabla;
            cmbMostrar.Text = "Alta";

            panelTitle.BackColor = color;
            btnBuscar.BackColor = color;
            btnRegistro.BackColor = color;
            btnImprimir.BackColor = color;
            btnColumnas.BackColor = color;

            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(223, 223, 223);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = color;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = color;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 208, 220);

            dtgColumnas.DefaultCellStyle.BackColor = Color.White;
            dtgColumnas.ColumnHeadersDefaultCellStyle.BackColor = color;
            dtgColumnas.ColumnHeadersDefaultCellStyle.SelectionBackColor = color;

            dtgColumnas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 208, 220);

            dtgColumnas.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text != "" && (txtBuscar.Text.Contains("=") || txtBuscar.Text.Contains("+") || txtBuscar.Text.Contains("-") || txtBuscar.Text.Contains(",")))
            {
                bool contiene = false;
                string columna = "";
                operadores = 0;
                parametros = txtBuscar.Text.Trim().Split(',').ToList<string>();

                foreach (string parametro in parametros)
                {
                    foreach (char operador in parametro)
                    {
                        if (operador == '+' || operador == '-' || operador == '=')
                        {
                            operadores++;
                        }
                    }
                }

                if (parametros.Count != operadores)
                {
                    lblError.Text = "¡Error! Cantidad no válida de operadores.";
                    lblError.Visible = true;
                    return;
                }

                foreach (string parametro in parametros)
                {
                    contiene = false;
                    valores = parametro.Split(separadores).ToList<string>();
                    columna = valores[0];
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        if (dataGridView1.Columns[i].Name == valores[0])
                        {
                            contiene = true;
                            break;
                        }
                    }

                    if (!contiene) break;
                }

                if (contiene)
                {
                    lblError.Text = "¡Error! La columna " + columna + " no existe.";
                    lblError.Visible = true;
                    return;
                }
            }
            lblError.Visible = false;
            status = cmbMostrar.SelectedIndex;
            if (txtBuscar.Text.ToLower() == "!nocompletos" && tabla == "Prestamos")
            {
                ds = Conexion.MySQL(@"SELECT Tabla.ID,Tabla.Alumno,Tabla.Maestro,Tabla.Laboratorio,Tabla.Asignatura,Tabla.Usuario,Tabla.Entrega,Tabla.Creado,Tabla.Actualizado FROM 
                                    (SELECT 
                                    prestamos.id ID, 
                                    CONCAT(alumnos.nombre,' ',alumnos.apellidop,' ', alumnos.apellidom) Alumno, 
                                    CONCAT(maestros.nombre,' ',maestros.apellidop,' ', maestros.apellidom) Maestro, 
                                    laboratorios.laboratorio Laboratorio,
                                    asignaturas.asignatura Asignatura,
                                    usuarios.usuario Usuario,
                                    prestamos.fecha_fin Entrega,
                                    prestamos.created_at Creado,
                                    prestamos.updated_at Actualizado, 
                                    prestamos.status
                                    FROM 
                                    prestamos
                                    INNER JOIN alumnos ON prestamos.alumno = alumnos.id 
                                    INNER JOIN maestros ON prestamos.maestro = maestros.id 
                                    INNER JOIN laboratorios ON prestamos.laboratorio = laboratorios.id
                                    INNER JOIN asignaturas ON prestamos.asignatura = asignaturas.id 
                                    INNER JOIN usuarios ON prestamos.usuario = usuarios.id) as Tabla
                                    INNER JOIN
                                    (
                                    SELECT 
                                    DISTINCT(prestamos.id), 
                                    prestamos.status AS statusPrestamos, 
                                    detalles.status AS statusDetalles
                                    FROM 
                                    prestamos 
                                    INNER JOIN 
                                    detalles 
                                    ON 
                                    detalles.prestamo = prestamos.id 
                                    WHERE prestamos.status = 0 AND detalles.status = 1   
                                    ) AS Tabla2
                                    ON
                                    Tabla.ID = Tabla2.ID");

                dataGridView1.DataSource = ds.Tables["tabla"];
                RowsToRed();
                dataGridView1.ClearSelection();
                return;
            }

            string prequery = "";
            if (FrmMenu.usuarioNivel != 1 && tabla != "Prestamos" && tabla != "Articulos" && tabla != "Alumnos")
            {
                MessageBox.Show("Solo un usuario con nivel de administrador puede agregar o alterar registros.");
                return;
            }

            pageLimit = int.Parse(numericUpDown2.Value.ToString());

            if (checkBoxFecha.Checked)
            {
                inicio = dateTimePickerInicio.Value.ToString().Substring(6, 4) + "-" + dateTimePickerInicio.Value.ToString().Substring(3, 2) + "-" + dateTimePickerInicio.Value.ToString().Substring(0, 2);
                fin = dateTimePickerFin.Value.ToString().Substring(6, 4) + "-" + dateTimePickerFin.Value.ToString().Substring(3, 2) + "-" + dateTimePickerFin.Value.ToString().Substring(0, 2);
            }
            else
            {
                inicio = "";
                fin = "";
            }

            prequery = Funciones.GetQuery(tabla, 0, cmbMostrar.SelectedIndex, txtBuscar.Text, inicio: inicio, fin: fin).Replace(';', ' ');
            if (prequery[0] == '¡')
            {
                lblError.Text = prequery;
                lblError.Visible = true;
                return;
            }

            ds = Conexion.MySQL("SELECT COUNT(ID) FROM (" + prequery + ") as TablaCount;");

            count = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());

            if (count > pageLimit)
            {
                residuo = count % pageLimit;

                paginas = (count - residuo) / pageLimit;

                if (residuo != 0) paginas++;

                lblPaginas.Text = Convert.ToString(paginas);
                numericUpDown1.Maximum = paginas;
                status = cmbMostrar.SelectedIndex;

                label3.Visible = true;
                label4.Visible = true;
                numericUpDown1.Visible = true;
                lblPaginas.Visible = true;
            }
            else
            {
                label3.Visible = false;
                label4.Visible = false;
                numericUpDown1.Visible = false;
                lblPaginas.Visible = false;
            }
            
            like = txtBuscar.Text;
            query = Funciones.GetQuery(tabla,0,cmbMostrar.SelectedIndex,like,pageLimit,0, inicio: inicio, fin: fin);
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = Conexion.MySQL(query).Tables["tabla"];
            lblError.Visible = false;
            dataGridView1.ClearSelection();

            if (tabla == "Prestamos")
            {
                RowsToRed();
            }

            if (columnsLoad)
            {
                dtgColumnas.Rows.Add(dataGridView1.Columns.Count);
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dtgColumnas.Rows[i].Cells[0].Value = dataGridView1.Columns[i].Name;
                    dtgColumnas.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                columnsLoad = false;
                dtgColumnas.ClearSelection();
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            if (FrmMenu.usuarioNivel != 1)
            {
                MessageBox.Show("Solo un usuario con nivel de administrador puede agregar o alterar registros.");
                return;
            }
            btnRegistroForm = Rutas.GetForm(tabla);
            btnRegistroForm.ShowDialog();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (FrmMenu.usuarioNivel != 1 && tabla != "Prestamos")
            {
                MessageBox.Show("Solo un usuario con nivel de administrador puede agregar o alterar registros.");
                return;
            }
            try
            {
                if (tabla != "Movimientos")
                {
                    id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    btnRegistroForm = Rutas.GetForm(tabla, id);
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "Detalles")
                {
                    id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    ds = Conexion.MySQL("SELECT prestamo FROM detalles WHERE id = " + id + ";");
                    id = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());
                    btnRegistroForm = Rutas.GetForm("Prestamos", id);
                }
                else
                {
                    id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    btnRegistroForm = Rutas.GetForm(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), id);
                }
                DialogResult res = btnRegistroForm.ShowDialog();
                if (res == DialogResult.OK)
                {
                    dataGridView1.DataSource = Conexion.MySQL(query).Tables["tabla"];
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void FrmCrud_Activated(object sender, EventArgs e)
        {
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Debe haber al menos un registro mostrado para poder imprimir.");
                return;
            }
            var respuesta = MessageBox.Show("¿Desea imprimir solo los registros mostrados?", "Informacion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                frmCrudImprimir = new FrmCrudImprimir(tabla, query, false);
            }
            else if (respuesta == DialogResult.No)
            {
                frmCrudImprimir = new FrmCrudImprimir(tabla, Funciones.GetQuery(tabla, 0, status, like), true);
            }
            else
            {
                return;
            }
            frmCrudImprimir.ShowDialog();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            query = Funciones.GetQuery(tabla, 0, status, like, pageLimit, (int.Parse(numericUpDown1.Value.ToString()) * pageLimit) - pageLimit);
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = Conexion.MySQL(query).Tables["tabla"];
        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                numericUpDown1_ValueChanged(sender, e);
            }
        }

        public void RowsToRed()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                ds = Conexion.MySQL("SELECT DISTINCT(prestamos.id), prestamos.status, detalles.status FROM prestamos INNER JOIN detalles ON detalles.prestamo = prestamos.id WHERE prestamos.status = 0 AND detalles.status = 1;");
                prestamosNoCompletos.Clear();

                for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                {
                    prestamosNoCompletos.Add(Convert.ToInt32(ds.Tables["tabla"].Rows[i][0].ToString()));
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (prestamosNoCompletos.Contains(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString())))
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        dataGridView1.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                }
            }
        }

        private void dateTimePickerInicio_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerFin.MinDate = dateTimePickerInicio.Value;
        }

        private void dateTimePickerInicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            dateTimePickerFin.MinDate = dateTimePickerInicio.Value;
        }


        bool ColumnAdjust = true;
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns.Count - 1 && ColumnAdjust)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    switch (dataGridView1.Columns[i].Name)
                    {
                        case "status":      dataGridView1.Columns[i].Width = TextRenderer.MeasureText(dataGridView1.Columns[i].Name, dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Disponible":  dataGridView1.Columns[i].Width = TextRenderer.MeasureText(dataGridView1.Columns[i].Name, dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "ID":          dataGridView1.Columns[i].Width = TextRenderer.MeasureText("0000", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Matrícula":   dataGridView1.Columns[i].Width = TextRenderer.MeasureText("00000000", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Teléfono":    dataGridView1.Columns[i].Width = TextRenderer.MeasureText("0000000000", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Entrega":     dataGridView1.Columns[i].Width = TextRenderer.MeasureText("00/00/0000", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Registro":    dataGridView1.Columns[i].Width = TextRenderer.MeasureText(dataGridView1.Columns[i].Name, dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Tabla":       dataGridView1.Columns[i].Width = TextRenderer.MeasureText("Laboratorios", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Descripción": dataGridView1.Columns[i].Width = TextRenderer.MeasureText("Reparación", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Usuario":     dataGridView1.Columns[i].Width = TextRenderer.MeasureText("adminadm", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Creado":      dataGridView1.Columns[i].Width = TextRenderer.MeasureText("00000000 00:00. pm.", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                        case "Actualizado": dataGridView1.Columns[i].Width = TextRenderer.MeasureText("00000000 00:00. pm.", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                    }
                }
                ColumnAdjust = false;
            }

            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "status" || this.dataGridView1.Columns[e.ColumnIndex].Name == "Disponible")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "True")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.SelectionBackColor = Color.DarkSeaGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.OrangeRed;
                    e.CellStyle.SelectionBackColor = Color.DarkSalmon;
                }
            }

            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "Entrega")
            {
                e.Value = e.Value.ToString().Substring(0, 10);
            }
        }

        private void btnColumnas_Click(object sender, EventArgs e)
        {
            pnlColumnas.Visible = !pnlColumnas.Visible;
        }

        private void dtgColumnas_MouseLeave(object sender, EventArgs e)
        {
            pnlColumnas.Visible = false;
            dtgColumnas.ClearSelection();
        }

        private void dtgColumnas_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Red)
                {
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                }
                else
                {
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    dtgColumnas.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
                }
                RowsHide();
            }
            catch (Exception)
            {
            }
        }

        private void RowsHide()
        {
            for (int i = 0; i < dtgColumnas.Rows.Count; i++)
            {
                if (dtgColumnas.Rows[i].DefaultCellStyle.ForeColor == Color.Red)
                {
                    dataGridView1.Columns[i].Visible = false;
                }
                else
                {
                    dataGridView1.Columns[i].Visible = true;
                }
            }
        }
    }
}
