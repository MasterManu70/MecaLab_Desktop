using System;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmUsuarioRegistro : Form
    {
        int id;
        public FrmUsuarioRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Carga o cierra Formulario
        private void FrmUsuarioRegistro_Load(object sender, EventArgs e)
        {
            DataSet ds;
            if (id != 0)
            {
                ds = Conexion.MySQL("select id,usuario,status from usuarios where id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnEliminar.Visible = true;
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                txtUsuario.Text = ds.Tables["tabla"].Rows[0][1].ToString();
            }
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

            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        private void btnCerrar_Click(object sender, EventArgs e)
        {
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
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Funciones.StatusUpdate("usuarios", btnEliminar.Text, id))
            {
                this.Close();
            }
        }

        //Metodos
    }
}
