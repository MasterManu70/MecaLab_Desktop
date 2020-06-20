using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmMaestros : Form
    {
        public FrmMaestros()
        {
            InitializeComponent();
        }
        //variables publicas y privadas
        FrmMaestroRegistro frmMaestroRegistro = new FrmMaestroRegistro();
        FrmMaestroDatos frmMaestroDatos = new FrmMaestroDatos();
        //Formulario carga o cierra
        private void FrmMaestros_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoRojo(dataGridView1);
            dataGridView1.Columns.Add("id","ID");
            dataGridView1.Columns.Add("nombre", "Nombre");
            dataGridView1.Columns.Add("apllidop", "Apellido Paterno");
            dataGridView1.Columns.Add("apllidom", "Apellido Materno");

            dataGridView1.Rows.Add("1","Jesus Francisco","Ruiz","Cruz");
            dataGridView1.Rows.Add("2", "Jose", "Padilla", "Burgueño");
            dataGridView1.Rows.Add("3", "Manuel Adrian", "Garcia", "Franco");
        }
        //Desarrollo
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Codigo Mysql
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmMaestroDatos.txtId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            frmMaestroDatos.txtNombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frmMaestroDatos.txtPaterno.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            frmMaestroDatos.txtMaterno.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            frmMaestroDatos.ShowDialog();
        }
        //Rutas
        private void btnMaestro_Click(object sender, EventArgs e)
        {
            frmMaestroRegistro.ShowDialog();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        //Metodos
    }
}
