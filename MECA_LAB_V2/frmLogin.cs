using System;
using System.Data;
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


        //Formulario carga o cierra
        //Desarrollo
        private void btnlogin_Click(object sender, EventArgs e)
        {
            DataSet ds;
            if (textBox1.Text == "") { MessageBox.Show("Ingrese el usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); textBox1.Focus(); return; }
            if (textBox2.Text == "") { MessageBox.Show("Ingrese la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); textBox2.Focus(); return; }

            ds = Conexion.MySQL("SELECT id, nivel FROM usuarios WHERE usuario='" + textBox1.Text + "' AND password=md5('" + textBox2.Text + "') AND status=1;");

            int count = ds.Tables["tabla"].Rows.Count;
            if (count == 0) 
            { 
                MessageBox.Show("El usuario o la contraseña son incorrectos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); textBox2.Focus();
                FrmMenu.intentos++;
                if (FrmMenu.intentos == 3) Application.Exit();
                return;
            }

            FrmMenu.usuarioID = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());
            FrmMenu.usuario = textBox1.Text;
            FrmMenu.usuarioNivel = int.Parse(ds.Tables["tabla"].Rows[0][1].ToString());
            FrmMenu.frmPrincipal.NotificationsUpdate();

            FrmMenu.showed = true;

            this.Close();
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
        //Validaciones
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
    }
}
