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
    public partial class FrmNotificaciones : Form
    {
        public Form detalles;
        public DataSet ds;
        public int id;
        public FrmNotificaciones()
        {
            InitializeComponent();
        }

        private void FrmNotificaciones_Load(object sender, EventArgs e)
        {
            ds = Conexion.MySQL("SELECT prestamos.id ID, CONCAT(alumnos.nombre, ' ', alumnos.apellidop, ' ', alumnos.apellidom) AS Nombre FROM prestamos INNER JOIN alumnos ON prestamos.alumno = alumnos.id WHERE prestamos.fecha_fin < NOW() AND prestamos.status = 1 AND prestamos.fecha_fin != prestamos.created_at;");
            dataGridView1.DataSource = ds.Tables["tabla"];
            dataGridView1.ClearSelection();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                return;
            }
            detalles = Rutas.GetForm("Prestamos",id);
            detalles.Show();
        }

        private void FrmNotificaciones_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmMenu.frmPrincipal.NotificationsUpdate();
        }
    }
}
