using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmUsuarioRegistro : Form
    {
        public FrmUsuarioRegistro()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Carga o cierra Formulario
        private void FrmUsuarioRegistro_Load(object sender, EventArgs e)
        {
            cmbNivel.SelectedIndex = 0;
        }
        //Desarrollo
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "") { MessageBox.Show("Ingrese el usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtUsuario.Focus(); return; }
            if (txtContraseña.Text == "") { MessageBox.Show("Ingrese la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtContraseña.Focus(); return; }
            if (txtContraseñaC.Text == "") { MessageBox.Show("Ingrese la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtContraseñaC.Focus(); return; }
            if (!(txtContraseña.Text == txtContraseñaC.Text)) { MessageBox.Show("Las contraseñas no coinciden", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cmbNivel.SelectedIndex == 0) { MessageBox.Show("Seleccione el Nivel", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbNivel.Focus(); return; }

            var respuesta = MessageBox.Show("¿Esta seguro de registrar este usuario?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();

            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            borrarContenido();
            this.Close();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            borrarContenido();
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void FrmUsuarioRegistro_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }
        //Validaciones
        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
        private void txtContraseñan_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetrasONumeros(e);
        }
        //Metodos
        private void borrarContenido() {
            txtContraseña.Clear();
            txtContraseñaC.Clear();
            txtUsuario.Clear();
            txtUsuario.Focus();
            cmbNivel.SelectedIndex = 0;
        }
    }
}
