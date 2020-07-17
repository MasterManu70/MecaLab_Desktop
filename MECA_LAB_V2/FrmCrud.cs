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

        Form btnRegistroForm;   //Objeto tipo 'Form' en el cual se le asignará el objeto obtenido con el método 'GetForm' de la clase de enrutado (Rutas).
        public FrmCrud(string tabla, Color color)
        {
            this.tabla = tabla;
            this.color = color;
            InitializeComponent();
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

            ds = Conexion.MySQL("SELECT COUNT(ID) FROM (" + Funciones.GetQuery(tabla, 0, cmbMostrar.SelectedIndex, txtBuscar.Text, inicio: inicio, fin: fin).Replace(';',' ') + ") as TablaCount;");

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
            string query = Funciones.GetQuery(tabla,0,cmbMostrar.SelectedIndex,like,pageLimit,0, inicio: inicio, fin: fin);
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = Conexion.MySQL(query).Tables["tabla"];
            dataGridView1.ClearSelection();
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            btnRegistroForm = Rutas.GetForm(tabla);
            btnRegistroForm.ShowDialog();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                btnRegistroForm = Rutas.GetForm(tabla, id);
                btnRegistroForm.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FrmCrud_Activated(object sender, EventArgs e)
        {
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Funciones.ReportPrint(tabla, dataGridView1);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            string query = Funciones.GetQuery(tabla, 0, status, like, pageLimit, (int.Parse(numericUpDown1.Value.ToString()) * pageLimit) - pageLimit);
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
    }
}
