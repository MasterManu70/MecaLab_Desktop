using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmCarreras : Form
    {
        public FrmCarreras()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        FrmCarreraRegistro frmCarreraRegistro = new FrmCarreraRegistro();
        FrmCarreraDatos frmCarreraDatos = new FrmCarreraDatos();
        //Formulario carga o cierra
        private void FrmCarreras_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoRojo(dataGridView1);
            dataGridView1.Columns.Add("id","ID");
            dataGridView1.Columns.Add("nombre", "Nombre");

            dataGridView1.Rows.Add("1","T.S.U Mecatronica");
            dataGridView1.Rows.Add("2", "ING Mecatronica");
            dataGridView1.Rows.Add("3", "T.S.U Aeronautica");
            dataGridView1.Rows.Add("4", "ING Aeronautica");
        }
        //Desarrollo
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Codigo Mysql
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCarreraDatos.txtId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            frmCarreraDatos.txtNombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frmCarreraDatos.ShowDialog();
        }
        //Rutas
        private void btnCarrera_Click(object sender, EventArgs e)
        {
            frmCarreraRegistro.ShowDialog();
        }

        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        //Metodos
    }
}
