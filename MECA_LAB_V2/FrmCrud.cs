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
        List<string> columnas;  //Lista de los nombres de cada columna en la tabla especificada en al variable 'tabla'.

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

            panelTitle.BackColor = color;
            btnBuscar.BackColor = color;
            btnRegistro.BackColor = color;

            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = color;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = color;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = color;
            
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string query = "select * from " + tabla + ";";

            if (columnas == null) AddColumns(dataGridView1, columnas = GetColumns(tabla));
            dataGridView1.Rows.Clear();
            AddRows(dataGridView1, Conexion.MySQL(query));
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            btnRegistroForm = Rutas.GetForm(tabla);
            btnRegistroForm.ShowDialog();
        }

        //Método GetColums: Obtener los nombres de cada columna de la tabla asignada en el constructor de la clase.
        public List<string> GetColumns(string tabla)
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

        //Método AddColums: Agregar los nombres de columnas contenidas en una lista de tipo 'string' al DataGridView cargado en el método.
        public void AddColumns(DataGridView DataGrid, List<string> Columns)
        {
            foreach (string Column in Columns) DataGrid.Columns.Add(Column, Column);
        }

        //Método AddRows: Rellenar las filas de DataGridView cargado en el método con un DataSet.
        public void AddRows(DataGridView DataGrid, DataSet ds)
        {
            //For con variable X: Encargado de recorrer cada fila en el DataSet 'ds' con la variable X como índice de la fila
            for (int x = 0; x < ds.Tables["tabla"].Rows.Count; x++)
            {
                DataGrid.Rows.Add();

                //For con variable X: Encargado de recorrer cada columna en el DataSet 'ds' con la variable X (del For con variable X)
                //como índice para la fila a recorrer y la variable Y como índice de columna.
                for (int y = 0; y < ds.Tables["tabla"].Columns.Count; y++)
                    DataGrid.Rows[x].Cells[y].Value = ds.Tables["tabla"].Rows[x][y];
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            btnRegistroForm = Rutas.GetForm(tabla,id);
            btnRegistroForm.ShowDialog();
        }
    }
}
