using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmAlumnoRegistro : Form
    {
        int id;
        List<int> llaves = new List<int>();             //Llaves primaria de los registros de la tabla carreras
        List<string> original = new List<string>();     //Registro tomado de la base de datos.
        List<string> nuevo = new List<string>();        //Registro con los nuevos valores que se comparará con los originales
        List<string> valores = new List<string>();      //Registro que se actualizará/insertará en la base de datos.
        List<string> movimiento = new List<string>();   //Registro que se insertará en la tabla movimientos.

        string status;
        string descripcion;

        List<string> columnas = new List<string> { "Matrícula", "Nombre", "Paterno", "Materno", "Carrera", "Correo", "Teléfono" };

        DataSet ds;

        public FrmAlumnoRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
            Funciones.TableToCombo(cmbCarrera, llaves, "Carreras");
        }

        //Variables Publicas y Privadas
        //Formulario Carga o Cierra
        //Desarrollo
        private void FrmAlumnoRegistro_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT alumnos.id, alumnos.matricula, alumnos.nombre, alumnos.apellidop, alumnos.apellidom, carreras.carrera, alumnos.correo, alumnos.telefono,alumnos.created_at,alumnos.updated_at, alumnos.status as status FROM alumnos INNER JOIN carreras ON alumnos.carrera = carreras.id where alumnos.id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnEliminar.Visible = true;
                btnActualizar.Text = "Actualizar";
                status = ds.Tables["tabla"].Rows[0]["status"].ToString();
                //txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                //txtMatricula.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                //txtNombre.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                //txtPaterno.Text = ds.Tables["tabla"].Rows[0][3].ToString();
                //txtMaterno.Text = ds.Tables["tabla"].Rows[0][4].ToString();
                //cmbCarrera.Text = ds.Tables["tabla"].Rows[0][5].ToString();
                //txtCorreo.Text = ds.Tables["tabla"].Rows[0][6].ToString();
                //txtTelefono.Text = ds.Tables["tabla"].Rows[0][7].ToString();

                original.Add(ds.Tables["tabla"].Rows[0][1].ToString());     //Matrícula
                original.Add(ds.Tables["tabla"].Rows[0][2].ToString());     //Nombre
                original.Add(ds.Tables["tabla"].Rows[0][3].ToString());     //Paterno
                original.Add(ds.Tables["tabla"].Rows[0][4].ToString());     //Materno
                original.Add(ds.Tables["tabla"].Rows[0][5].ToString());     //Carrera (texto)
                original.Add(ds.Tables["tabla"].Rows[0][6].ToString());     //Correo
                original.Add(ds.Tables["tabla"].Rows[0][7].ToString());     //Teléfono

                txtMatricula.Text = original[0];
                txtNombre.Text = original[1];
                txtPaterno.Text = original[2];
                txtMaterno.Text = original[3];
                cmbCarrera.Text = original[4];
                txtCorreo.Text = original[5];
                txtTelefono.Text = original[6];
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            //if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMatricula.Focus(); return; }
            //if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
            //if (txtPaterno.Text == "") { MessageBox.Show("Ingrese el apellido paterno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPaterno.Focus(); return; }
            //if (txtMaterno.Text == "") { MessageBox.Show("Ingrese el apellido materno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMaterno.Focus(); return; }
            if (cmbCarrera.Text == "") { MessageBox.Show("Seleccione una carrera", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbCarrera.Focus(); return; }
            //if (txtCorreo.Text == "") { MessageBox.Show("Ingrese el correo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCorreo.Focus(); return; }
            //if (txtTelefono.Text == "") { MessageBox.Show("Ingrese el telefono", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTelefono.Focus(); return; }

            if (!DatosCompletos())
            {
                MessageBox.Show("Debe de llenar los datos con el formato correcto.");
                return;
            }

            string status = "1";
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT status FROM alumnos WHERE id = " + id + ";");
                status = ds.Tables["tabla"].Rows[0][0].ToString();
            }

            valores.Add(id.ToString());
            valores.Add("'" + txtMatricula.Text + "'");
            valores.Add("'" + txtNombre.Text + "'");
            valores.Add("'" + txtPaterno.Text + "'");
            valores.Add("'" + txtMaterno.Text + "'");
            valores.Add(llaves[cmbCarrera.SelectedIndex].ToString());
            valores.Add("'" + txtCorreo.Text + "'");
            valores.Add("'" + txtTelefono.Text + "'");
            valores.Add("NOW()");
            valores.Add("NOW()");
            valores.Add(status);

            nuevo.Add(txtMatricula.Text);
            nuevo.Add(txtNombre.Text);
            nuevo.Add(txtPaterno.Text);
            nuevo.Add(txtMaterno.Text);
            nuevo.Add(cmbCarrera.Text);
            nuevo.Add(txtCorreo.Text);
            nuevo.Add(txtTelefono.Text);

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    if (Funciones.Insert("alumnos", valores))
                    {
                        for (int i = 0; i < original.Count; i++)
                        {
                            if (original[i] != nuevo[i])
                            {
                                movimiento.Clear();
                                movimiento.Add("0");
                                movimiento.Add(FrmMenu.usuarioID.ToString());
                                movimiento.Add(id.ToString());
                                movimiento.Add("'Alumnos'");
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
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                if (Funciones.Insert("alumnos", valores))
                {
                    ds = Conexion.MySQL("SELECT Last_Insert_ID();");
                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                    movimiento.Add("'Alumnos'");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("'Agregó'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos", movimiento);
                }
                this.DialogResult = DialogResult.OK;
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
            if (Funciones.StatusUpdate("alumnos", btnEliminar.Text, id))
            {
                movimiento.Clear();
                movimiento.Add("0");
                movimiento.Add(FrmMenu.usuarioID.ToString());
                movimiento.Add(id.ToString());
                movimiento.Add("'Alumnos'");
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

        private void FrmAlumnoRegistro_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            if (!Validar.Validate(txtMatricula.Text, numeros:true))
            {
                lblErrorMatricula.Text = "Solo números";
                lblErrorMatricula.Visible = true;
            }
            else
            {
                lblErrorMatricula.Visible = false;
            }

            if (Validar.Validate(txtMatricula.Text, numeros: true) && txtMatricula.Text.Length == 8)
            {
                ds = Conexion.MySQL("SELECT id FROM alumnos WHERE matricula = " + txtMatricula.Text + " AND id != " + id + ";");
                if (ds.Tables["tabla"].Rows.Count > 0)
                {
                    lblErrorMatricula.Text = "Matrícula en uso";
                    lblErrorMatricula.Visible = true;
                }
                else
                {
                    lblErrorMatricula.Visible = false;
                }
            }
        }

        private void txtMatricula_Leave(object sender, EventArgs e)
        {
            if (txtMatricula.Text.Length < 8 || txtMatricula.Text.Length > 8)
            {
                lblErrorMatricula.Text = "8 caracteres";
                lblErrorMatricula.Visible = true;
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (Validar.Validate(txtNombre.Text, letras: true,caracteres: " "))
            {
                lblErrorNombre.Visible = false;
            }
            else
            {
                lblErrorNombre.Text = "Solo letras";
                lblErrorNombre.Visible = true;
            }
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if (txtNombre.Text.Length == 0 )
            {
                lblErrorNombre.Text = "Requerido";
                lblErrorNombre.Visible = true;
            }

            if (txtNombre.Text.Length > 60)
            {
                lblErrorNombre.Text = "60 caracteres";
                lblErrorNombre.Visible = true;
            }
        }

        private void txtPaterno_TextChanged(object sender, EventArgs e)
        {
            if (!Validar.Validate(txtPaterno.Text, letras: true))
            {
                lblErrorPaterno.Text = "Solo letras";
                lblErrorPaterno.Visible = true;
            }
            else
            {
                lblErrorPaterno.Visible = false;
            }
        }

        private void txtPaterno_Leave(object sender, EventArgs e)
        {
            if (txtPaterno.Text.Length == 0)
            {
                lblErrorPaterno.Text = "Requerido";
                lblErrorPaterno.Visible = true;
            }

            if (txtPaterno.Text.Length > 60)
            {
                lblErrorPaterno.Text = "60 caracteres";
                lblErrorPaterno.Visible = true;
            }
        }

        private void txtMaterno_TextChanged(object sender, EventArgs e)
        {
            if (!Validar.Validate(txtMaterno.Text, letras: true))
            {
                lblErrorMaterno.Text = "Solo letras";
                lblErrorMaterno.Visible = true;
            }
            else
            {
                lblErrorMaterno.Visible = false;
            }
        }

        private void txtMaterno_Leave(object sender, EventArgs e)
        {
            if (txtMaterno.Text.Length == 0)
            {
                lblErrorMaterno.Text = "Requerido";
                lblErrorMaterno.Visible = true;
            }

            if (txtMaterno.Text.Length > 60)
            {
                lblErrorMaterno.Text = "60 caracteres";
                lblErrorMaterno.Visible = true;
            }
        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {
            //if (!Validar.Validate(txtCorreo.Text, letras: true,numeros:true," _-@.!¡¿?/") && !Validar.CorreoValidate(txtCorreo.Text))
            //{
            //    lblErrorCorreo.Text = "Formato no válido";
            //    lblErrorCorreo.Visible = true;
            //}
            //else
            //{
            lblErrorCorreo.Visible = false;
            //}
        }

        private void txtCorreo_Leave(object sender, EventArgs e)
        {
            if (txtMaterno.Text.Length > 255)
            {
                lblErrorMaterno.Text = "255 caracteres";
                lblErrorMaterno.Visible = true;
            }
        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            if (!Validar.Validate(txtTelefono.Text, numeros: true))
            {
                lblErrorTelefono.Text = "Solo números";
                lblErrorTelefono.Visible = true;
            }
            else
            {
                lblErrorTelefono.Visible = false;
            }

            if (Validar.Validate(txtTelefono.Text, numeros: true) && txtTelefono.Text.Length == 10)
            {
                ds = Conexion.MySQL("SELECT id FROM alumnos WHERE telefono = " + txtTelefono.Text + " AND id != " + id + ";");
                if (ds.Tables["tabla"].Rows.Count > 0)
                {
                    lblErrorTelefono.Text = "Teléfono en uso";
                    lblErrorTelefono.Visible = true;
                }
                else
                {
                    lblErrorTelefono.Visible = false;
                }
            }
        }

        private void txtTelefono_Leave(object sender, EventArgs e)
        {
            if (txtTelefono.Text.Length < 10 || txtTelefono.Text.Length > 10)
            {
                lblErrorTelefono.Text = "10 caracteres";
                lblErrorTelefono.Visible = true;
            }
        }

        public bool DatosCompletos()
        {
            //Matrícula
            if (!Validar.Validate(txtMatricula.Text, numeros: true)) return false;
            if (txtMatricula.Text.Length < 8 || txtMatricula.Text.Length > 8) return false;
            ds = Conexion.MySQL("SELECT id FROM alumnos WHERE matricula = " + txtMatricula.Text + " AND id != " + id + ";");
            if (ds.Tables["tabla"].Rows.Count > 0) return false;

            //Nombre
            if (!Validar.Validate(txtNombre.Text, letras: true, caracteres: " ")) return false;
            if (txtNombre.Text.Length == 0) return false;
            if (txtNombre.Text.Length > 60) return false;

            //Paterno
            if (!Validar.Validate(txtPaterno.Text, letras: true)) return false;
            if (txtPaterno.Text.Length == 0) return false;
            if (txtPaterno.Text.Length > 60) return false;

            //Materno
            if (!Validar.Validate(txtMaterno.Text, letras: true)) return false;
            if (txtMaterno.Text.Length == 0) return false;
            if (txtMaterno.Text.Length > 60) return false;

            //Correo
            //if (!Validar.Validate(txtCorreo.Text, letras: true, numeros: true, " _-@.!¡¿?/") && !Validar.CorreoValidate(txtCorreo.Text) && txtCorreo.Text != "") return false;
            //if (txtCorreo.Text.Length > 255) return false;

            //Telefono
            if (!Validar.Validate(txtTelefono.Text, numeros: true)) return false;
            if (txtTelefono.Text.Length < 10 || txtTelefono.Text.Length > 10) return false;
            ds = Conexion.MySQL("SELECT id FROM alumnos WHERE telefono = " + txtTelefono.Text + " AND id != " + id + ";");
            if (ds.Tables["tabla"].Rows.Count > 0) return false;

            return true;
        }
    }
}
