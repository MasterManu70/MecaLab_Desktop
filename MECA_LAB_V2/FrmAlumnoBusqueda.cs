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
    public partial class FrmAlumnoBusqueda : Form
    {
        public DataSet ds;
        public FrmAlumnoBusqueda()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ds = Conexion.MySQL("SELECT Matrícula, Alumno FROM (SELECT matricula as Matrícula, CONCAT(nombre, ' ', apellidop, ' ', apellidom) AS Alumno, status FROM alumnos) AS Tabla WHERE status = 1 AND Alumno LIKE '%" + txtBuscar.Text + "%';");
            dataGridView1.DataSource = ds.Tables["tabla"];
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            FrmMenu.frmPrincipal.txtMatricula.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.Close();
        }
    }
}
