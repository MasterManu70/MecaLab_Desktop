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
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = color;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = color;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = color;
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            string query = Funciones.GetQuery(tabla,0,cmbMostrar.SelectedIndex,txtBuscar.Text);
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = Conexion.MySQL(query).Tables["tabla"];
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
    }
}
