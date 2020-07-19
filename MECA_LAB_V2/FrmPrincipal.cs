using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmPrincipal : Form
    {
        DataSet ds;
        bool devolver = false;
        Color devolucionColor = Color.DarkOrange;
        Color principalColor = Color.MediumSeaGreen;
        //IDs necesarios para llevar a cabo el préstamo
        int prestamoID = 0;
        int alumnoID = 0;
        string alumno = "";
        string fecha;

        FrmAlumnoBusqueda frmAlumnoBusqueda;
        FrmNotificaciones frmNotificaciones;

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
                    if (DateTime.Now.ToString().Substring(0, 10) == dateTimePickerFin.Value.ToString().Substring(0, 10))
                    {
                        fecha = "NOW()";
                    }
                    else
                    {
                        fecha = "'" + dateTimePickerFin.Value.ToString().Substring(6, 4) + "-" + dateTimePickerFin.Value.ToString().Substring(3, 2) + "-" + dateTimePickerFin.Value.ToString().Substring(0, 2) + "'";
                    }

                    valores.Add("0");
                    valores.Add(alumnoID.ToString());
                    valores.Add(maestros[cmbMaestro.SelectedIndex].ToString());
                    valores.Add(laboratorios[cmbLaboratorio.SelectedIndex].ToString());
                    valores.Add(asignaturas[cmbAsignatura.SelectedIndex].ToString());
                    valores.Add(FrmMenu.usuarioID.ToString());
                    valores.Add(fecha);
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
                            Conexion.MySQL("UPDATE articulos SET disponible = '1' WHERE articulos.id = " + row.Cells[0].Value.ToString() + ";");
                        }
                    }
                    txtCodigo.Enabled = false;
                    txtMatricula.Enabled = true;
                    txtMatricula.Focus();
                    MessageBox.Show("La devolución se ha realizado correctamente.");
                    borrarContenido();
                }
            }
            NotificationsUpdate();
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
        public void borrarContenido() {
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
            txtUsuario.Clear();
            dateTimePickerFin.ResetText();
            lblRegistro.Visible = false;
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

                ds = Conexion.MySQL("SELECT id,articulo,comentario,disponible, status FROM articulos WHERE id = " + codigo + ";");

                if (ds.Tables["tabla"].Rows.Count == 0) { txtCodigo.Clear(); return; }

                if (ds.Tables["tabla"].Rows[0][3].ToString() == "False")
                {
                    MessageBox.Show("El artículo no se encuentra disponible o está registrado en algún préstamo no devuelto.");
                    txtCodigo.Clear();
                    return;
                }

                if (ds.Tables["tabla"].Rows[0][4].ToString() == "False")
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
                ds = Conexion.MySQL("select id, concat(nombre,' ',apellidop,' ',apellidom),status from alumnos where matricula = " + matricula + ";");

                if (ds.Tables["tabla"].Rows.Count == 0 && !devolver)
                {
                    lblRegistro.Text = "X";
                    lblRegistro.ForeColor = Color.Red;
                    lblRegistro.Visible = true;
                    return;
                }

                //Validar si el estudiante está dado de alta o de baja
                if (ds.Tables["tabla"].Rows[0][2].ToString() == "False")
                {
                    MessageBox.Show("El alumno se encuentra dado de baja del sistema.");
                    return;
                }

                alumnoID = int.Parse(ds.Tables["tabla"].Rows[0][0].ToString());
                txtAlumno.Text = alumno = ds.Tables["tabla"].Rows[0][1].ToString();

                if (!devolver)
                {
                    lblRegistro.Text = "✓";
                    lblRegistro.ForeColor = Color.Green;
                    lblRegistro.Visible = true;
                }

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
                    txtUsuario.Text = ds.Tables["tabla"].Rows[0][5].ToString();
                    dateTimePickerFin.Value = Convert.ToDateTime(ds.Tables["tabla"].Rows[0][6].ToString());

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

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }

                    for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = ds.Tables["tabla"].Rows[i][0].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = ds.Tables["tabla"].Rows[i][1].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = ds.Tables["tabla"].Rows[i][2].ToString();
                    }

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
                dateTimePickerFin.Enabled = false;

                lblRegistro.Visible = false;

                dateTimePickerFin.MinDate = DateTime.Parse("01/01/2020");

                btnDevolver.Text = "Prestar";
                txtUsuario.Clear();
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
                dateTimePickerFin.Enabled = true;

                lblRegistro.Visible = false;

                dateTimePickerFin.MinDate = DateTime.Now;

                btnDevolver.Text = "Devolver";
                txtUsuario.Clear();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtArticulo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtComentario.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void lblRegistro_Click(object sender, EventArgs e)
        {
            if (lblRegistro.Text == "X")
            {
                FrmAlumnoRegistro frm = new FrmAlumnoRegistro();
                frm.txtMatricula.Text = txtMatricula.Text;
                DialogResult res = frm.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ds = Conexion.MySQL("SELECT LAST_INSERT_ID();");
                    alumnoID = Convert.ToInt32(ds.Tables["tabla"].Rows[0][0].ToString());
                    ds = Conexion.MySQL("SELECT CONCAT(nombre,' ',apellidop,' ',apellidom),status FROM alumnos WHERE id = " + alumnoID + ";");
                    alumno = ds.Tables["tabla"].Rows[0][0].ToString();
                    txtAlumno.Text = alumno;
                    lblRegistro.Text = "✓";
                    lblRegistro.ForeColor = Color.Green;
                    lblRegistro.Visible = true;
                    MessageBox.Show("El alumno ha sido registrado con éxito.");
                }
            }
        }

        private void picBuscar_Click(object sender, EventArgs e)
        {
            frmAlumnoBusqueda = new FrmAlumnoBusqueda();
            frmAlumnoBusqueda.ShowDialog();
        }

        private void dateTimePickerFin_MouseEnter(object sender, EventArgs e)
        {
            dateTimePickerFin.MinDate = DateTime.Now;
        }

        private void picNotificacion_Click(object sender, EventArgs e)
        {
            frmNotificaciones = new FrmNotificaciones();
            frmNotificaciones.ShowDialog();
        }

        public void NotificationsUpdate()
        {
            ds = Conexion.MySQL("SELECT COUNT(ID) FROM (SELECT prestamos.id ID FROM prestamos INNER JOIN alumnos ON prestamos.alumno = alumnos.id WHERE prestamos.fecha_fin < NOW() AND prestamos.status = 1 AND prestamos.fecha_fin != prestamos.created_at) as Tabla;");
            if (ds.Tables["tabla"].Rows[0][0].ToString() != "0")
            {
                picNotificacion.Image = Properties.Resources.campana_ciruclo_roojo;
            }
            else
            {
                picNotificacion.Image = Properties.Resources.campana;
            }
        }
    }
}
