using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmMaestroRegistro : Form
    {
        int id = 0;
        public FrmMaestroRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }
        private void FrmMaestroRegistro_Load(object sender, EventArgs e)
        {
            DataSet ds;
            if (id != 0)
            {
                ds = Conexion.MySQL("select * from maestros where id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnEliminar.Visible = true;
                btnActualizar.Text = "Actualizar";
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                txtNombre.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                txtPaterno.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                txtMaterno.Text = ds.Tables["tabla"].Rows[0][3].ToString();
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
            if (txtPaterno.Text == "") { MessageBox.Show("Ingrese el apellido paterno del maestro", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtPaterno.Focus(); return; }
            if (txtMaterno.Text == "") { MessageBox.Show("Ingrese el apellido materno del maestro", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtMaterno.Focus(); return; }

            string status = "1";
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT status FROM articulos WHERE id = " + id + ";");
                status = ds.Tables["tabla"].Rows[0][0].ToString();
            }

            valores.Add(id.ToString());
            valores.Add("'"+txtNombre.Text+"'");
            valores.Add("'"+txtPaterno.Text+"'");
            valores.Add("'"+txtMaterno.Text+"'");
            valores.Add("NOW()");
            valores.Add("NOW()");
            valores.Add(status);

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    Funciones.Insert("maestros",valores);
                    this.Close();
                }
            }
            else
            {
                Funciones.Insert("maestros", valores);
                this.Close();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Funciones.StatusUpdate("maestros", btnEliminar.Text, id))
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

        private void txtPaterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        private void txtMaterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        //Metodos
    }
}
