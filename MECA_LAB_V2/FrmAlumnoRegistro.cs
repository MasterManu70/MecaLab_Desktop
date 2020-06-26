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
        List<int> llaves = new List<int>();
        List<string> registro = new List<string>();     //Registro tomado de la base de datos.
        List<string> valores = new List<string>();      //Registro que se actualizará/insertará en la base de datos.

        List<List<string>> movimientos = new List<List<string>>();
        List<string> movimiento = new List<string>();   //Registro que se insertará en la tabla movimientos.
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
            DataSet ds;
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT alumnos.id, alumnos.matricula, alumnos.nombre, alumnos.apellidop, alumnos.apellidom, carreras.carrera, alumnos.correo, alumnos.telefono,alumnos.created_at,alumnos.updated_at, alumnos.status as status FROM alumnos INNER JOIN carreras ON alumnos.carrera = carreras.id where alumnos.id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnEliminar.Visible = true;
                btnActualizar.Text = "Actualizar";
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                txtMatricula.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                txtNombre.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                txtPaterno.Text = ds.Tables["tabla"].Rows[0][3].ToString();
                txtMaterno.Text = ds.Tables["tabla"].Rows[0][4].ToString();
                cmbCarrera.Text = ds.Tables["tabla"].Rows[0][5].ToString();
                txtCorreo.Text = ds.Tables["tabla"].Rows[0][6].ToString();
                txtTelefono.Text = ds.Tables["tabla"].Rows[0][7].ToString();

                for (int i = 0; i < ds.Tables["tabla"].Columns.Count; i++)
                    registro.Add(ds.Tables["tabla"].Rows[0][i].ToString());
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DataSet ds;
            if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMatricula.Focus(); return; }
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
            if (txtPaterno.Text == "") { MessageBox.Show("Ingrese el apellido paterno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPaterno.Focus(); return; }
            if (txtMaterno.Text == "") { MessageBox.Show("Ingrese el apellido materno", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMaterno.Focus(); return; }
            if (cmbCarrera.Text == "") { MessageBox.Show("Seleccione una carrera", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbCarrera.Focus(); return; }
            if (txtCorreo.Text == "") { MessageBox.Show("Ingrese el correo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCorreo.Focus(); return; }
            if (txtTelefono.Text == "") { MessageBox.Show("Ingrese el telefono", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTelefono.Focus(); return; }

            valores.Add(id.ToString());
            valores.Add("'" + txtMatricula.Text + "'");
            valores.Add("'" + txtNombre.Text + "'");
            valores.Add("'" + txtPaterno.Text + "'");
            valores.Add("'" + txtMaterno.Text + "'");
            valores.Add("" + llaves[cmbCarrera.SelectedIndex] + "");
            valores.Add("'" + txtCorreo.Text + "'");
            valores.Add("'" + txtTelefono.Text + "'");
            valores.Add("NOW()");
            valores.Add("NOW()");
            valores.Add("1");

            if (id != 0)
            {
                List<string> columnas = Funciones.GetColumns("alumnos");
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    if(Funciones.Insert("alumnos", valores))
                    {
                        for (int i = 1; i < valores.Count; i++)
                        {
                            if (valores[i].Replace("'",String.Empty) != registro[i] && valores[i] != "NOW()" && (registro[i] != "True" || registro[i] != "False"))
                            {
                                List<string> movimiento = new List<string>();
                                movimiento.Add("0");
                                movimiento.Add(FrmMenu.usuarioID.ToString());
                                movimiento.Add(id.ToString());
                                movimiento.Add("'alumnos'");
                                movimiento.Add("'" + columnas[i].ToString() + "'");
                                movimiento.Add("'" + valores[i] + "'");
                                movimiento.Add("'" + registro[i] + "'");
                                movimiento.Add("'MODIFICACIÓN'");
                                movimiento.Add("NOW()");
                                movimiento.Add("NOW()");
                                movimiento.Add("1");
                                movimientos.Add(movimiento);
                            }
                        }

                        foreach (List<string> item in movimientos)
                        {
                            Funciones.Insert("movimientos",item);
                        }
                        this.Close();
                    }
                }
            }
            else
            {
                if(Funciones.Insert("alumnos", valores))
                {
                    ds = Conexion.MySQL("SELECT Last_Insert_ID();");
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                    movimiento.Add("'alumnos'");
                    movimiento.Add("' '");
                    movimiento.Add("' '");
                    movimiento.Add("' '");
                    movimiento.Add("'INSERCIÓN'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos",movimiento);
                    this.Close();
                }
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Funciones.StatusUpdate("alumnos", btnEliminar.Text, id))
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
    }
}
