using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAsignaturas : Form
    {
        public FrmAsignaturas()
        {
            InitializeComponent();
        }
        //variables publicas y privadas
        FrmAsignaturaRegistro frmAsignaturaRegistro = new FrmAsignaturaRegistro();
        FrmAsignaturaDatos frmAsignaturaDatos = new FrmAsignaturaDatos();
        //Formulario carga o cierra
        private void FrmAsignaturas_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoRojo(dataGridView1);
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("nombre", "Nombre");

            dataGridView1.Rows.Add("1", "Fundamentos I");
            dataGridView1.Rows.Add("2", "Fundamentos II");
            dataGridView1.Rows.Add("3", "Fundamentos III");
        }
        //Desarrollo
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Codigo Mysql
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmAsignaturaDatos.txtId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            frmAsignaturaDatos.txtNombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frmAsignaturaDatos.ShowDialog();
        }
        //Rutas
        private void btnAsignatura_Click(object sender, EventArgs e)
        {
            frmAsignaturaRegistro.ShowDialog();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        //Metodos
    }
}
