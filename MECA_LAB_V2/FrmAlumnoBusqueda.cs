using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if (Validar.Validate(txtBuscar.Text, letras: true)) lblError.Visible = false; else { lblError.Visible = true; lblError.Text = "¡Error! Solo letras en el campo de texto."; return; }
            ds = Conexion.MySQL("SELECT Matrícula, Alumno FROM (SELECT matricula as Matrícula, CONCAT(nombre, ' ', apellidop, ' ', apellidom) AS Alumno, status FROM alumnos) AS Tabla WHERE status = 1 AND Alumno LIKE '%" + txtBuscar.Text + "%';");
            dataGridView1.DataSource = ds.Tables["tabla"];

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                switch (dataGridView1.Columns[i].Name)
                {
                    case "Matrícula": dataGridView1.Columns[i].Width = TextRenderer.MeasureText("000000000", dataGridView1.Columns[i].DefaultCellStyle.Font).Width; break;
                }
            }

            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ds = Conexion.MySQL("SELECT id FROM alumnos WHERE matricula = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + ";");
                ds = Conexion.MySQL("SELECT id FROM prestamos WHERE alumno = " + ds.Tables["tabla"].Rows[0][0].ToString() + " AND status = 1;");
                if (ds.Tables["tabla"].Rows.Count > 0)
                {
                    MessageBox.Show("El alumno ya cuenta con un préstamo activo.");
                    return;
                }
                FrmMenu.frmPrincipal.txtMatricula.Clear();
                FrmMenu.frmPrincipal.txtMatricula.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            catch (Exception)
            {
                return;
            }
            this.Close();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (Validar.Validate(txtBuscar.Text,letras: true)) lblError.Visible = false; else { lblError.Visible = true; lblError.Text = "¡Error! Solo letras en el campo de texto."; }
        }
    }
}
