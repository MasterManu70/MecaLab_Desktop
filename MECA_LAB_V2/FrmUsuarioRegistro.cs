using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmUsuarioRegistro : Form
    {
        int id;
        List<string> original = new List<string>();     //Registro tomado de la base de datos.
        List<string> nuevo = new List<string>();        //Registro con los nuevos valores que se comparará con los originales
        List<string> valores = new List<string>();      //Registro que se actualizará/insertará en la base de datos.
        List<string> movimiento = new List<string>();   //Registro que se insertará en la tabla movimientos.

        string status;
        string descripcion;

        List<string> columnas = new List<string> { "Usuario", "Nivel"};
        public FrmUsuarioRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        //Carga o cierra Formulario
        private void FrmUsuarioRegistro_Load(object sender, EventArgs e)
        {
            DataSet ds;
            if (id != 0)
            {
                ds = Conexion.MySQL("select id,usuario,nivel,status from usuarios where id=" + id + ";");
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

                txtUsuario.Text = original[0];
                cmbNivel.Text = original[1];
            }
        }
        //Desarrollo
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            DataSet ds;
            List<string> valores = new List<string>();

            if (txtUsuario.Text == "") { MessageBox.Show("Ingrese el usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtUsuario.Focus(); return; }
            if (txtContraseña.Text == "" && txtContraseñaC.Text != "") { MessageBox.Show("Ingrese la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtContraseña.Focus(); return; }
            if (txtContraseñaC.Text == "" && txtContraseña.Text != "") { MessageBox.Show("Ingrese la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtContraseñaC.Focus(); return; }
            if (!(txtContraseña.Text == txtContraseñaC.Text)) { MessageBox.Show("Las contraseñas no coinciden", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cmbNivel.Text == "") { MessageBox.Show("Seleccione el Nivel", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbNivel.Focus(); return; }
            if (txtContraseña.Text != txtContraseñaC.Text) { MessageBox.Show("Las contraseñas no coinciden", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtContraseña.Focus(); return; }

            string status = "1";
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT status FROM usuarios WHERE id = " + id + ";");
                status = ds.Tables["tabla"].Rows[0][0].ToString();
            }

            valores.Add(id.ToString());
            valores.Add("'" + txtUsuario.Text + "'");
            valores.Add("md5('" + txtContraseña.Text + "')");
            valores.Add("'" + cmbNivel.Text + "'");
            valores.Add("NOW()");
            valores.Add("NOW()");
            valores.Add(status);

            nuevo.Add(txtUsuario.Text);
            nuevo.Add(cmbNivel.Text);

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    if (Funciones.Insert("usuarios", valores))
                    {
                        for (int i = 0; i < original.Count; i++)
                        {
                            if (original[i] != nuevo[i])
                            {
                                movimiento.Clear();
                                movimiento.Add("0");
                                movimiento.Add(FrmMenu.usuarioID.ToString());
                                movimiento.Add(id.ToString());
                                movimiento.Add("'Usuarios'");
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

                        if (txtContraseña.Text == txtContraseñaC.Text)
                        {
                            movimiento.Clear();
                            movimiento.Add("0");
                            movimiento.Add(FrmMenu.usuarioID.ToString());
                            movimiento.Add(id.ToString());
                            movimiento.Add("'Usuarios'");
                            movimiento.Add("'Contraseña'");
                            movimiento.Add("NULL");
                            movimiento.Add("NULL");
                            movimiento.Add("'Modificó'");
                            movimiento.Add("NOW()");
                            movimiento.Add("NOW()");
                            movimiento.Add("1");
                            Funciones.Insert("movimientos", movimiento);
                        }
                    }
                    this.Close();
                }
            }
            else
            {
                if (Funciones.Insert("usuarios", valores))
                {
                    ds = Conexion.MySQL("SELECT Last_Insert_ID();");
                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                    movimiento.Add("'Usuarios'");
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
            if (status == "True")
            {
                descripcion = "Baja";
            }
            else
            {
                descripcion = "Alta";
            }
            if (Funciones.StatusUpdate("usuarios", btnEliminar.Text, id))
            {
                movimiento.Clear();
                movimiento.Add("0");
                movimiento.Add(FrmMenu.usuarioID.ToString());
                movimiento.Add(id.ToString());
                movimiento.Add("'Usuarios'");
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
    }
}
