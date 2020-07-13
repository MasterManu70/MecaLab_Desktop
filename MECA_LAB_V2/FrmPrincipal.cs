using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmPrincipal : Form
    {
        bool devolver = false;
        Color devolucionColor = Color.DarkOrange;
        Color principalColor = Color.MediumSeaGreen;
        //IDs necesarios para llevar a cabo el préstamo
        int prestamoID = 0;
        int alumnoID = 0;
        string alumno = "";

        //Listas de llaves primarias correspondiente a cada registro
        public static List<int> maestros = new List<int>();
        public static List<int> asignaturas = new List<int>();
        public static List<int> laboratorios = new List<int>();
        public static List<int> devolucion = new List<int>();

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
            DataSet ds;

            if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtMatricula.Focus(); return; }
            if (dataGridView1.Rows.Count == 0) { MessageBox.Show("Agregue artículos a la lista", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtCodigo.Focus(); return; }
            if (cmbMaestro.Text == "") { MessageBox.Show("Seleccione el nombre del maestro", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbMaestro.Focus(); return; }
            if (cmbAsignatura.Text == "") { MessageBox.Show("Seleccione la asignatura", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbAsignatura.Focus(); return; }
            if (cmbLaboratorio.Text == "") { MessageBox.Show("Seleccione laboratorio", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbLaboratorio.Focus(); return; }

            if (!devolver)
            {
                List<string> valores = new List<string>();
                List<string> detalles = new List<string>();
                List<string> articulo = new List<string>();

                ds = Conexion.MySQL("SELECT status FROM usuarios WHERE id = " + FrmMenu.usuarioID + ";");

                if (ds.Tables["tabla"].Rows[0][0].ToString() == "False") { { MessageBox.Show("El usuario de la sesión se encuentra dado de baja en el sistema", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; } }

                var respuesta = MessageBox.Show("¿Desea realizar el siguiente prestamo?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    valores.Add("0");
                    valores.Add(alumnoID.ToString());
                    valores.Add(maestros[cmbMaestro.SelectedIndex].ToString());
                    valores.Add(laboratorios[cmbLaboratorio.SelectedIndex].ToString());
                    valores.Add(asignaturas[cmbAsignatura.SelectedIndex].ToString());
                    valores.Add(FrmMenu.usuarioID.ToString());
                    valores.Add("NOW()");
                    valores.Add("NOW()");
                    valores.Add("NOW()");
                    valores.Add("1");

                    Funciones.Insert("prestamos", valores);

                    ds = Conexion.MySQL("SELECT LAST_INSERT_ID();");

                    prestamoID = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        detalles.Clear();
                        detalles.Add("0");
                        detalles.Add(prestamoID.ToString());
                        detalles.Add(row.Cells[0].Value.ToString());
                        detalles.Add("NOW()");
                        detalles.Add("NOW()");
                        detalles.Add("1");

                        Funciones.Insert("detalles", detalles);

                        articulo.Clear();
                        articulo.Add(row.Cells[0].Value.ToString());
                        articulo.Add("'" + row.Cells[1].Value.ToString() + "'");
                        articulo.Add("'" + row.Cells[2].Value.ToString() + "'");
                        articulo.Add("0");
                        articulo.Add("NOW()");
                        articulo.Add("NOW()");
                        articulo.Add("1");

                        Funciones.Insert("articulos", articulo);
                    }

                    borrarContenido();
                }
            }
            else
            {
                string texto;
                int rowColorCount = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++) if (dataGridView1.Rows[i].DefaultCellStyle.ForeColor == Color.Red) rowColorCount++;

                if (rowColorCount == dataGridView1.Rows.Count)
                {
                    MessageBox.Show("No se ha devuelto ningún artículo.\nFavor de devolver al menos uno a la lista");
                    return;
                }
                else if (rowColorCount != 0)
                {
                    texto = "Hay artículos faltantes\nLos artículos faltantes serán registrados como no entregados\n¿Desea realizar la siguiente devolución?";
                }
                else
                {
                    texto = "¿Desea realizar la siguiente devolución?";
                }

                var respuesta = MessageBox.Show(texto, "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    Conexion.MySQL("UPDATE prestamos SET status = '0' WHERE prestamos.id = " + prestamoID + ";");

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.DefaultCellStyle.ForeColor == Color.Black)
                        {
                            Conexion.MySQL("UPDATE detalles SET status = '0' WHERE detalles.prestamo = " + prestamoID + " AND detalles.articulo = " + row.Cells[0].Value.ToString() + ";");
                        }
                    }
                    txtCodigo.Enabled = false;
                    txtMatricula.Enabled = true;
                    txtMatricula.Focus();
                    MessageBox.Show("La devolución se ha realizado correctamente.");
                    borrarContenido();
                }
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
            prestamoID = 0;
            alumnoID = 0;
            alumno = "";

            txtCodigo.Clear();
            txtMatricula.Clear();
            txtAlumno.Clear();
            txtArticulo.Clear();
            cmbAsignatura.Text = "";
            cmbLaboratorio.Text = "";
            cmbMaestro.Text = "";
            txtComentario.Clear();
            txtCodigo.Focus();

            int rows = dataGridView1.Rows.Count - 1;
            for (int i = rows; i >= 0; i--) dataGridView1.Rows.RemoveAt(i);

            Funciones.TableToCombo(cmbMaestro, maestros, "maestros");
            Funciones.TableToCombo(cmbAsignatura, asignaturas, "asignaturas");
            Funciones.TableToCombo(cmbLaboratorio, laboratorios, "laboratorios");
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

            if (txtCodigo.Text.Length == 4 && !devolver)
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
            else if (txtCodigo.Text.Length == 4 && devolver)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == codigo.ToString()) 
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                txtMatricula.Enabled = false;
                txtCodigo.Clear();
            }
        }

        private void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtMatricula.Text, out matricula))
            {
                txtMatricula.Clear();
                return;
            }

            DataSet ds;

            if (txtMatricula.Text.Length == 8)
            {
                ds = Conexion.MySQL("select id, concat(nombre,' ',apellidop,' ',apellidom, ' '),status from alumnos where matricula = " + matricula + ";");

                //Validar si el estudiante está dado de alta o de baja
                if (ds.Tables["tabla"].Rows[0][2].ToString() == "False")
                {
                    MessageBox.Show("El alumno se encuentra dado de baja del sistema.");
                    return;
                }

                alumnoID = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());
                txtAlumno.Text = alumno = ds.Tables["tabla"].Rows[0][1].ToString();

                if (devolver)
                {
                    ds = Conexion.MySQL(@"SELECT * FROM (SELECT prestamos.id ID, prestamos.alumno Alumno, 
                                        CONCAT(maestros.nombre,' ',maestros.apellidop,' ', maestros.apellidom) Maestro, 
                                        laboratorios.laboratorio Laboratorio, asignaturas.asignatura Asignaturas,
                                        usuarios.usuario Usuario,prestamos.fecha_fin Entrega,prestamos.created_at Creado,
                                        prestamos.updated_at Actualizado, prestamos.status status FROM prestamos 
                                        INNER JOIN alumnos ON prestamos.alumno = alumnos.id 
                                        INNER JOIN maestros ON prestamos.maestro = maestros.id 
                                        INNER JOIN laboratorios ON prestamos.laboratorio = laboratorios.id 
                                        INNER JOIN asignaturas ON prestamos.asignatura = asignaturas.id 
                                        INNER JOIN usuarios ON prestamos.usuario = usuarios.id) as Tabla WHERE Alumno = " + alumnoID + " and status = 1;");

                    if (ds.Tables["tabla"].Rows.Count == 0)
                    {
                        MessageBox.Show("El alumno no cuenta con préstamos activos.");
                        return;
                    }

                    prestamoID = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());
                    cmbMaestro.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                    cmbLaboratorio.Text = ds.Tables["tabla"].Rows[0][3].ToString();
                    cmbAsignatura.Text = ds.Tables["tabla"].Rows[0][4].ToString();

                    ds = Conexion.MySQL(@"SELECT
                                        detalles.articulo ID,
                                        articulos.articulo Artículo,
                                        articulos.comentario Comentario
                                        FROM
                                        detalles
                                        INNER JOIN
                                        articulos
                                        on
                                        detalles.articulo = articulos.id
                                        WHERE
                                        detalles.prestamo = " + prestamoID + "; ");

                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = ds.Tables["tabla"];

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }

                    dataGridView1.Enabled = false;
                    dataGridView1.ClearSelection();
                    txtCodigo.Enabled = true;
                    txtCodigo.Focus();

                    if (ds.Tables["tabla"].Rows.Count == 0) { txtCodigo.Clear(); return; }
                }
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

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            string texto = "";
            if (dataGridView1.Rows.Count > 0)
            {
                if (!devolver) texto = "¿Desea cambiar al modo devolución?, el préstamo actual se cancelará."; else texto = "¿Desea cambiar al modo préstamo?, la devolución actual se cancelará.";
                var respuesta = MessageBox.Show(texto, "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.No) return;
            }

            int rows = dataGridView1.Rows.Count - 1;

            for (int i = rows; i >= 0; i--) dataGridView1.Rows.RemoveAt(i);
            
            colorChange();
            borrarContenido();

            if (devolver) txtMatricula.Focus(); else txtCodigo.Focus();
        }

        public void colorChange()
        {
            devolver = !devolver;

            if (devolver)
            {
                panel1.BackColor = devolucionColor;
                btnDevolver.BackColor = devolucionColor;
                pictureBox1.BackColor = devolucionColor;
                button1.BackColor = devolucionColor;

                dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = devolucionColor;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = devolucionColor;
                dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = devolucionColor;

                cmbAsignatura.Enabled = false;
                cmbLaboratorio.Enabled = false;
                cmbMaestro.Enabled = false;
                txtCodigo.Enabled = false;

                btnDevolver.Text = "Prestar";
            }
            else
            {
                panel1.BackColor = principalColor;
                btnDevolver.BackColor = principalColor;
                pictureBox1.BackColor = principalColor;
                button1.BackColor = principalColor;

                dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = principalColor;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = principalColor;
                dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = principalColor;

                cmbAsignatura.Enabled = true;
                cmbLaboratorio.Enabled = true;
                cmbMaestro.Enabled = true;
                txtCodigo.Enabled = true;
                dataGridView1.Enabled = true;

                btnDevolver.Text = "Devolver";
            }
        }
    }
}
