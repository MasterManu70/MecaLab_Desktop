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
        string tabla;
        Color color;
        List<string> columnas;

        Form btnRegistroForm;
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
            btnRegistroForm = Rutas.GetForm(tabla, "Registro");
            btnRegistroForm.Show();
        }

        public List<string> GetColumns(string tabla)
        {
            List<string> Columns = new List<string>();
            DataSet ds = Conexion.MySQL("describe " + tabla + ";");

            for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                Columns.Add(Convert.ToString(ds.Tables["tabla"].Rows[i][0]));

            return Columns;
        }

        public void AddColumns(DataGridView DataGrid, List<String> Columns)
        {
            foreach (string Column in Columns) DataGrid.Columns.Add(Column, Column);
        }

        public void AddRows(DataGridView DataGrid, DataSet ds)
        {
            for (int x = 0; x < ds.Tables["tabla"].Rows.Count; x++)
            {
                DataGrid.Rows.Add();
                for (int y = 0; y < ds.Tables["tabla"].Columns.Count; y++)
                    DataGrid.Rows[x].Cells[y].Value = ds.Tables["tabla"].Rows[x][y];
            }
        }
    }
}
