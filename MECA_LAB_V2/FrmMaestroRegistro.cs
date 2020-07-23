using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmMaestroRegistro : Form
    {
        int id = 0;

        List<string> original = new List<string>();     //Registro tomado de la base de datos.
        List<string> nuevo = new List<string>();        //Registro con los nuevos valores que se comparará con los originales
        List<string> valores = new List<string>();      //Registro que se actualizará/insertará en la base de datos.
        List<string> movimiento = new List<string>();   //Registro que se insertará en la tabla movimientos.

        string status;
        string descripcion;

        List<string> columnas = new List<string> {"Nombre", "Paterno", "Materno"};

        DataSet ds;
        public FrmMaestroRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }
        private void FrmMaestroRegistro_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                ds = Conexion.MySQL("select * from maestros where id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnEliminar.Visible = true;
                btnActualizar.Text = "Actualizar";
                status = ds.Tables["tabla"].Rows[0]["status"].ToString();
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();

                original.Add(ds.Tables["tabla"].Rows[0][1].ToString());
                original.Add(ds.Tables["tabla"].Rows[0][2].ToString());
                original.Add(ds.Tables["tabla"].Rows[0][3].ToString());

                txtNombre.Text = original[0];
                txtPaterno.Text = original[1];
                txtMaterno.Text = original[2];
            }
        }
        //Variables Publicas y Privadas
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
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

            nuevo.Add(txtNombre.Text);
            nuevo.Add(txtPaterno.Text);
            nuevo.Add(txtMaterno.Text);

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    if (Funciones.Insert("maestros", valores))
                    {
                        for (int i = 0; i < original.Count; i++)
                        {
                            if (original[i] != nuevo[i])
                            {
                                movimiento.Clear();
                                movimiento.Add("0");
                                movimiento.Add(FrmMenu.usuarioID.ToString());
                                movimiento.Add(id.ToString());
                                movimiento.Add("'Maestros'");
                                movimiento.Add("'" + columnas[i] + "'");
                                movimiento.Add("'" + nuevo[i] + "'");
                                movimiento.Add("'" + original[i] + "'");
                                movimiento.Add("'Modificó'");
                                movimiento.Add("NOW()");
                                movimiento.Add("NOW()");
                                movimiento.Add("1");
                                Funciones.Insert("movimientos", movimiento);
                            }
                        }
                    }
                    this.Close();
                }
            }
            else
            {
                if (Funciones.Insert("maestros", valores))
                {
                    ds = Conexion.MySQL("SELECT Last_Insert_ID();");
                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                    movimiento.Add("'Maestros'");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("'Agregó'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos", movimiento);
                }
                this.Close();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (status == "True")
            {
                descripcion = "Baja";
            }
            else
            {
                descripcion = "Alta";
            }
            if (Funciones.StatusUpdate("maestros", btnEliminar.Text, id))
            {
                movimiento.Clear();
                movimiento.Add("0");
                movimiento.Add(FrmMenu.usuarioID.ToString());
                movimiento.Add(id.ToString());
                movimiento.Add("'Maestros'");
                movimiento.Add("'status'");
                movimiento.Add("NULL");
                movimiento.Add("NULL");
                movimiento.Add("'" + descripcion + "'");
                movimiento.Add("NOW()");
                movimiento.Add("NOW()");
                movimiento.Add("1");
                Funciones.Insert("movimientos", movimiento);
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
