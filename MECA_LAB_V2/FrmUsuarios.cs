using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmUsuarios : Form
    {
        public FrmUsuarios()
        {
            InitializeComponent();
        }
        //variables publicas y privadas
        private FrmUsuarioRegistro frmUsuarioRegistro = new FrmUsuarioRegistro();
        //Formulario carga o cierra
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoRojo(dataGridView1);
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("usuario", "Usuario");
            dataGridView1.Columns.Add("nivel", "Nivel");

            dataGridView1.Rows.Add("1", "TheSincarS", "Propietario");
            dataGridView1.Rows.Add("2", "SincarS", "Administrador");
            dataGridView1.Rows.Add("3", "SincarSentrony", "Empleado");
        }
        //Desarrollo
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Codigo Mysql
        }
        //Rutas
        private void btnUsuario_Click(object sender, EventArgs e)
        {
            frmUsuarioRegistro.ShowDialog();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        //Metodos
    }
}
