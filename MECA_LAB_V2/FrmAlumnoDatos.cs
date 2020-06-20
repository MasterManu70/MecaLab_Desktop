using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAlumnoDatos : Form
    {
        public FrmAlumnoDatos()
        {
            InitializeComponent();
        }

        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMatricula.Focus(); return; }
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
            if (txtPaterno.Text == "") { MessageBox.Show("Ingrese el apellido paterno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPaterno.Focus(); return; }
            if (txtMaterno.Text == "") { MessageBox.Show("Ingrese el apellido materno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMaterno.Focus(); return; }
            if (cmbCarrera.SelectedIndex == 0) { MessageBox.Show("Seleccione una carrera", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbCarrera.Focus(); return; }
            if (txtCorreo.Text == "") { MessageBox.Show("Ingrese el correo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCorreo.Focus(); return; }
            if (txtTelefono.Text == "") { MessageBox.Show("Ingrese el telefono", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTelefono.Focus(); return; }

            var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();
                this.Close();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Esta seguro de eliminar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();
                this.Close();
            }
        }
        //Rutas
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
        private void FrmAlumnoDatos_MouseMove(object sender, MouseEventArgs e)
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
        private void txtPaterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }
        private void txtMaterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumeros(e);
        }
        //Metodos
        private void borrarContenido()
        {
            txtMatricula.Focus();
            txtMatricula.Clear();
            txtNombre.Clear();
            txtPaterno.Clear();
            txtMaterno.Clear();
            cmbCarrera.SelectedIndex = 0;
            txtCorreo.Clear();
            txtTelefono.Clear();
        }
    }
}
