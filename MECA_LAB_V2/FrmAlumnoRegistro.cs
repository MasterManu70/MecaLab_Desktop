using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAlumnoRegistro : Form
    {
        public FrmAlumnoRegistro()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Carga o cierra Fomulario
        private void FrmAlumnoRegistro_Load(object sender, EventArgs e)
        {
            cmbCarrera.SelectedIndex = 0;
        }
        //Desarrollo
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMatricula.Focus(); return; }
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
            if (txtApellidoP.Text == "") { MessageBox.Show("Ingrese el apellido paterno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellidoP.Focus(); return; }
            if (txtApellidoM.Text == "") { MessageBox.Show("Ingrese el apellido materno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellidoM.Focus(); return; }
            if (cmbCarrera.SelectedIndex == 0) { MessageBox.Show("Seleccione una carrera", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbCarrera.Focus(); return; }
            if (txtCorreo.Text == "") { MessageBox.Show("Ingrese el correo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCorreo.Focus(); return; }
            if (txtTelefono.Text == "") { MessageBox.Show("Ingrese el telefono", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTelefono.Focus(); return; }

            var respuesta = MessageBox.Show("¿Esta seguro de registrar al alumno?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();
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
        private void FrmAlumnoRegistro_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }
        //Validaciones
        private void txtMatricula_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumeros(e);
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        private void txtApellidoP_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        private void txtApellidoM_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumeros(e);
        }

        //Metodos
        void borrarContenido() {
            txtMatricula.Clear();
            txtNombre.Clear();
            txtApellidoP.Clear();
            txtApellidoM.Clear();
            cmbCarrera.SelectedIndex = 0;
            txtCorreo.Clear();
            txtTelefono.Clear();
        }
    }
}
