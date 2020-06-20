using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmLaboratorios : Form
    {
        public FrmLaboratorios()
        {
            InitializeComponent();
        }
        //variables publicas y privadas
        FrmLaboratorioRegistro frmLaboratorioRegistro = new FrmLaboratorioRegistro();
        FrmLaboratorioDatos frmLaboratorioDatos = new FrmLaboratorioDatos();
        //Formulario carga o cierra
        private void FrmLaboratorios_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoRojo(dataGridView1);
            dataGridView1.Columns.Add("id","ID");
            dataGridView1.Columns.Add("nombre", "Nombre");

            dataGridView1.Rows.Add("1","Laboratorio I");
            dataGridView1.Rows.Add("2", "Laboratorio II");
            dataGridView1.Rows.Add("3", "Laboratorio III");
        }
        //Desarrollo
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Codigo Mysql
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmLaboratorioDatos.txtId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            frmLaboratorioDatos.txtNombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frmLaboratorioDatos.ShowDialog();
        }
        //Rutas
        private void btnLaboratorio_Click(object sender, EventArgs e)
        {
            frmLaboratorioRegistro.ShowDialog();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        //Metodos
    }
}
