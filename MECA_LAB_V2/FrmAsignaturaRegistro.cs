using System;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAsignaturaRegistro : Form
    {
        int id;
        public FrmAsignaturaRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }

        private void FrmAsignaturaRegistro_Load(object sender, EventArgs e)
        {
            DataSet ds;
            if (id != 0)
            {
                ds = Conexion.MySQL("select * from asignaturas where id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                txtNombre.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                btnEliminar.Visible = true;
            }
        }

        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre de la asignatura", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtNombre.Focus(); return; }
            var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
 
                this.Close();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Funciones.StatusUpdate("asignaturas", btnEliminar.Text, id))
            {
                this.Close();
            }
        }
        //Rutas
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
        private void FrmAsignaturaDatos_MouseMove(object sender, MouseEventArgs e)
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
    }
}
