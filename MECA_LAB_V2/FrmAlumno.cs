using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAlumno : Form
    {
        public FrmAlumno()
        {
            InitializeComponent();
        }
        //Variables publicas y privadas
        private FrmAlumnoRegistro frmAlumnoRegistro = new FrmAlumnoRegistro();
        private FrmAlumnoDatos frmAlumnoDatos = new FrmAlumnoDatos();
        //Formulario carga o cierra
        private void FrmAlumno_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoNaranja(dataGridView1);
            dataGridView1.Columns.Add("id","ID");
            dataGridView1.Columns.Add("matricula", "Matricula");
            dataGridView1.Columns.Add("nombre", "Nombre");
            dataGridView1.Columns.Add("apellidop", "Apellido Paterno");
            dataGridView1.Columns.Add("apellidom", "Apellido Materno");
            dataGridView1.Columns.Add("carrera", "Carrera");
            dataGridView1.Columns.Add("correo", "Correo");
            dataGridView1.Columns.Add("telefono", "Telefono");

            dataGridView1.Rows.Add("1", "17311041", "Jesus Francisco", "Ruiz", "Cruz","T.S.U Tecnologias de la Informacion","Jesus_Fco_981@hotmail.com","6624245958");
            dataGridView1.Rows.Add("2", "17311043", "Jose", "Padilla","Burgueño","T.S.U Mecatronica","Jose_Topo_981@hotmail.com", "6624245959");
            dataGridView1.Rows.Add("3", "17311044", "Manuel Adrian", "Garcia", "Franco", "T.S.U Tecnologias de la Informacion", "MasterManu@hotmail.com", "6624245960");
        }
        //Desarrollo
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Codigo Mysql
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmAlumnoDatos.txtId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            frmAlumnoDatos.txtMatricula.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frmAlumnoDatos.txtNombre.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            frmAlumnoDatos.txtPaterno.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            frmAlumnoDatos.txtMaterno.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            frmAlumnoDatos.cmbCarrera.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            frmAlumnoDatos.txtCorreo.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            frmAlumnoDatos.txtTelefono.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            frmAlumnoDatos.ShowDialog();
        }

        //Rutas
        private void btnAlumno_Click(object sender, EventArgs e)
        {
            frmAlumnoRegistro.ShowDialog();
        }
        //Formulario Minimzar, Maximizar, Cerrar y Diseño
        //Validaciones
        //Metodos
    }
}
