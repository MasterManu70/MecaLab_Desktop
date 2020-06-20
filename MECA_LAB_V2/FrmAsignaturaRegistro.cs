using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAsignaturaRegistro : Form
    {
        public FrmAsignaturaRegistro()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre de la asignatura", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtNombre.Focus(); return; }
            var respuesta = MessageBox.Show("¿Esta seguro de registrar esta asignatura?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();

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
        private void FrmAsignaturaRegistro_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }
        //Validaciones
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }
        //Metodos
        private void borrarContenido()
        {
            txtNombre.Focus();
            txtNombre.Clear();
        }

    }
}
