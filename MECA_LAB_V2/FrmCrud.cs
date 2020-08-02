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

        List<int> prestamosNoCompletos = new List<int>();

        Form btnRegistroForm;   //Objeto tipo 'Form' en el cual se le asignará el objeto obtenido con el método 'GetForm' de la clase de enrutado (Rutas).
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

            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(223, 223, 223);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = color;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = color;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 208, 220);
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
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
            //frmPrintConfig = new FrmPrintConfig(dataGridView1);
            //frmPrintConfig.ShowDialog();
            Funciones.ReportPrint(tabla, dataGridView1);
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
    }
}
