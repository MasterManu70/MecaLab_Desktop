using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        public int xClick = 0, yClick = 0;
        //Formulario carga o cierra
        //Desarrollo
        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Ingrese el usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); textBox1.Focus(); return; }
            if (textBox2.Text == "") { MessageBox.Show("Ingrese la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); textBox2.Focus(); return; }
           
            this.Close();
            textBox1.Clear();
            textBox2.Clear();

            //Codigo conexion MYSQL
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Rutas
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void FrmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }
        //Validaciones
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
        //Metodos
    }
}
