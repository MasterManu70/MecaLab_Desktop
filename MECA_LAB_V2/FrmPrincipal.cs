using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmPrincipal : Form
    {
        //IDs necesarios para llevar a cabo el préstamo
        int prestamoID = 0;
        int alumnoID = 0;
        string alumno = "";

        //Listas de llaves primarias correspondiente a cada registro
        public static List<int> maestros = new List<int>();
        public static List<int> asignaturas = new List<int>();
        public static List<int> laboratorios = new List<int>();

        //Lista de llaves primarias de cada artículo en la lista
        public static List<int> articulos = new List<int>();

        int codigo = 0;
        int matricula = 0;

        public FrmPrincipal()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        //Formulario carga o cierra
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            Funciones.TableToCombo(cmbMaestro, maestros, "maestros");
            Funciones.TableToCombo(cmbAsignatura, asignaturas, "asignaturas");
            Funciones.TableToCombo(cmbLaboratorio, laboratorios, "laboratorios");

            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue;

            dataGridView1.Columns.Add("ID","ID");
            dataGridView1.Columns.Add("Artículo", "Articulo");
            dataGridView1.Columns.Add("Comentario", "Comentario");
        }
        //Desarrollo
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> valores = new List<string>();
            if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtMatricula.Focus(); return; }
            if (cmbMaestro.Text == "") { MessageBox.Show("Seleccione el nombre del maestro", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbMaestro.Focus(); return; }
            if (cmbAsignatura.Text == "") { MessageBox.Show("Seleccione la asignatura", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbAsignatura.Focus(); return; }
            if (cmbLaboratorio.Text == "") { MessageBox.Show("Seleccione laboratorio", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbLaboratorio.Focus(); return; }
            if (dataGridView1.Rows.Count == 0) { MessageBox.Show("Agregue artículos a la lista", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtCodigo.Focus(); return; }
            var respuesta = MessageBox.Show("¿Desea realizar el siguiente prestamo?","Informacion",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes) {
                valores.Add("NULL");
                valores.Add(alumnoID.ToString());
                valores.Add(maestros[cmbMaestro.SelectedIndex].ToString());
                valores.Add(laboratorios[cmbLaboratorio.SelectedIndex].ToString());
                valores.Add(asignaturas[cmbAsignatura.SelectedIndex].ToString());
                valores.Add(FrmMenu.usuarioID.ToString());
                borrarContenido();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Esta seguro de cancelar siguiente prestamo?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (respuesta == DialogResult.Yes)
            {
                borrarContenido();
            }
        }
        //Rutas
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumeros(e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumeros(e);
        }
        //Metodos
        void borrarContenido() {
            txtCodigo.Clear();
            txtMatricula.Clear();
            txtArticulo.Clear();
            cmbAsignatura.SelectedIndex = 0;
            cmbLaboratorio.SelectedIndex = 0;
            cmbMaestro.SelectedIndex = 0;
            txtComentario.Clear();
            dataGridView1.Rows.Clear();
            txtCodigo.Focus();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            int rows = 0;
            DataSet ds;
            if (!Int32.TryParse(txtCodigo.Text, out codigo))
            {
                txtCodigo.Clear();
                return;
            }

            if (txtCodigo.Text.Length == 4)
            {
                rows = dataGridView1.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == Convert.ToString(codigo))
                    {
                        MessageBox.Show("El artículo ya existe en la lista.");
                        txtCodigo.Clear();
                        return;
                    }
                }

                ds = Conexion.MySQL("SELECT id,articulo,comentario,status FROM articulos WHERE id = " + codigo + ";");

                if (ds.Tables["tabla"].Rows.Count == 0) { txtCodigo.Clear(); return; }

                if (ds.Tables["tabla"].Rows[0][3].ToString() == "False")
                {
                    MessageBox.Show("El artículo se encuentra dado de baja del sistema.");
                    txtCodigo.Clear();
                    return;
                }

                dataGridView1.Rows.Add();
                int row = dataGridView1.RowCount - 1;
                dataGridView1.Rows[row].Cells[0].Value = ds.Tables["tabla"].Rows[0][0].ToString();
                dataGridView1.Rows[row].Cells[1].Value = ds.Tables["tabla"].Rows[0][1].ToString();
                txtArticulo.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                dataGridView1.Rows[row].Cells[2].Value = ds.Tables["tabla"].Rows[0][2].ToString();
                txtComentario.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                txtCodigo.Clear();
            }
        }

        private void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            DataSet ds;
            if (!Int32.TryParse(txtMatricula.Text, out matricula))
            {
                txtMatricula.Clear();
                return;
            }

            if (txtMatricula.Text.Length == 8)
            {
                ds = Conexion.MySQL("select id, concat(nombre,' ',apellidop,' ',apellidom, ' '),status from alumnos where matricula = " + matricula + ";");

                if (ds.Tables["tabla"].Rows[0][2].ToString() == "False")
                {
                    MessageBox.Show("El alumno se encuentra dado de baja del sistema.");
                    return;
                }

                //Validar si el estudiante está dado de alta o de baja
                alumnoID = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());
                txtAlumno.Text = alumno = ds.Tables["tabla"].Rows[0][1].ToString();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Está seguro de eliminar este artículo?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[e.RowIndex]);
            }
        }
    }
}
