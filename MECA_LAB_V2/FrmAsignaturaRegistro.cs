using System;
using System.Collections.Generic;
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
                btnEliminar.Visible = true;
                btnActualizar.Text = "Actualizar";
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                txtNombre.Text = ds.Tables["tabla"].Rows[0][1].ToString();
            }
        }

        //Variables Publicas y Privadas
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DataSet ds;
            List<string> valores = new List<string>();
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre de la asignatura", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtNombre.Focus(); return; }

            string status = "1";
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT status FROM asignaturas WHERE id = " + id + ";");
                status = ds.Tables["tabla"].Rows[0][0].ToString();
            }

            valores.Add(id.ToString());
            valores.Add("'" + txtNombre.Text + "'");
            valores.Add("NOW()");
            valores.Add("NOW()");
            valores.Add(status);

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    Funciones.Insert("asignaturas", valores);
                    this.Close();
                }
            }
            else
            {
                Funciones.Insert("asignaturas", valores);
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
        //Validaciones
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        //Metodos
    }
}
