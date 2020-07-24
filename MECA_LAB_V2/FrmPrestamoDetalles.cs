using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmPrestamoDetalles : Form
    {
        int id;
        List<int> noEntregadoID = new List<int>();
        List<string> movimiento = new List<string>();
        List<string> detalles = new List<string>();
        string status;
        DataSet ds;
        int codigo;
        int rows = 0;
        public FrmPrestamoDetalles(int id = 0)
        {
            this.id = id;
            InitializeComponent();
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

        private void FrmPrestamoDetalles_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(223, 223, 223);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.SteelBlue;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 208, 220);

            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Artículo", "Articulo");
            dataGridView1.Columns.Add("Comentario", "Comentario");

            if (id != 0)
            {
                ds = Conexion.MySQL(Funciones.GetQuery("Prestamos",id,2));

                txtAlumno.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                txtMaestro.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                txtLaboratorio.Text = ds.Tables["tabla"].Rows[0][3].ToString();
                txtAsignatura.Text = ds.Tables["tabla"].Rows[0][4].ToString();
                txtUsuario.Text = ds.Tables["tabla"].Rows[0][5].ToString();
                txtEntrega.Text = ds.Tables["tabla"].Rows[0][6].ToString().Substring(0,10);
                txtCreado.Text = ds.Tables["tabla"].Rows[0][7].ToString();
                txtActualizado.Text = ds.Tables["tabla"].Rows[0][8].ToString();

                ds = Conexion.MySQL("SELECT detalles.articulo ID, articulos.articulo Artículo, articulos.comentario Comentario, detalles.status status FROM detalles INNER JOIN articulos ON articulos.id = detalles.articulo WHERE detalles.prestamo = " + id + ";");
                //dataGridView1.DataSource = ds.Tables["tabla"];

                for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = ds.Tables["tabla"].Rows[i][0].ToString();
                    dataGridView1.Rows[i].Cells[1].Value = ds.Tables["tabla"].Rows[i][1].ToString();
                    dataGridView1.Rows[i].Cells[2].Value = ds.Tables["tabla"].Rows[i][2].ToString();

                    if (ds.Tables["tabla"].Rows[i][3].ToString() == "True") { dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red; dataGridView1.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red; }
                }

                dataGridView1.ClearSelection();

                ds = Conexion.MySQL("SELECT articulos.id FROM detalles INNER JOIN articulos ON detalles.articulo = articulos.id WHERE detalles.prestamo = " + id + " AND detalles.status = 1;");

                if (ds.Tables["tabla"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                    {
                        noEntregadoID.Add(int.Parse(ds.Tables["tabla"].Rows[i][0].ToString()));
                    }
                }

                ds = Conexion.MySQL("SELECT status FROM prestamos WHERE id = " + id + ";");
                status = ds.Tables["tabla"].Rows[0][0].ToString();
                
                if (status == "True")
                {
                    txtCodigo.Enabled = true;
                    btnActualizar.Visible = true;
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (status == "False")
            {
                if (noEntregadoID.Contains(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString())))
                {
                    var respuesta = MessageBox.Show("¿Desea marcar como devuelto este artículo?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (respuesta == DialogResult.Yes)
                    {
                        Conexion.MySQL("UPDATE detalles SET status = 0 WHERE articulo = " + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()) + " AND prestamo = " + id + ";");

                        ds = Conexion.MySQL("SELECT id FROM detalles WHERE articulo = " + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()) + " AND prestamo = " + id + ";");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                        movimiento.Add("'Detalles'");
                        movimiento.Add("'Artículo'");
                        movimiento.Add("'" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                        movimiento.Add("NULL");
                        movimiento.Add("'Devolvió'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);

                        Conexion.MySQL("UPDATE articulos SET disponible = 1 WHERE id = " + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()) + ";");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        movimiento.Add("'Articulos'");
                        movimiento.Add("'Disponible'");
                        movimiento.Add("'1'");
                        movimiento.Add("'0'");
                        movimiento.Add("'Modificó'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);

                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                    }
                }
                else
                {
                    MessageBox.Show("El artículo seleccionado ya ha sido devuelto.");
                }
            }
            else
            {
                if (dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Black)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red; 
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
                }
                else if (dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.Red)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                }
                else if (dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor == Color.LimeGreen)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtCodigo.Text, out codigo))
            {
                txtCodigo.Clear();
                return;
            }

            lblError.Visible = false;

            if (txtCodigo.Text.Length == 4)
            {
                rows = dataGridView1.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == Convert.ToString(codigo))
                    {
                        lblError.Text = "El artículo ya existe en la lista.";
                        lblError.Visible = true;
                        txtCodigo.Focus();
                        txtCodigo.SelectAll();
                        return;
                    }
                }

                ds = Conexion.MySQL("SELECT id,articulo,comentario,disponible, status FROM articulos WHERE id = " + codigo + ";");

                if (ds.Tables["tabla"].Rows.Count == 0)
                {
                    lblError.Text = "El artículo no se está registrado.";
                    lblError.Visible = true;
                    txtCodigo.Focus();
                    txtCodigo.SelectAll();
                    return;
                }

                if (ds.Tables["tabla"].Rows[0][3].ToString() == "False")
                {
                    lblError.Text = "El artículo no se encuentra disponible.";
                    lblError.Visible = true;
                    txtCodigo.Focus();
                    txtCodigo.SelectAll();
                    return;
                }

                if (ds.Tables["tabla"].Rows[0][4].ToString() == "False")
                {
                    lblError.Text = "El artículo se encuentra dado de baja del sistema.";
                    lblError.Visible = true;
                    txtCodigo.Focus();
                    txtCodigo.SelectAll();
                    return;
                }

                dataGridView1.Rows.Add();
                int row = dataGridView1.RowCount - 1;
                dataGridView1.Rows[row].Cells[0].Value = ds.Tables["tabla"].Rows[0][0].ToString();
                dataGridView1.Rows[row].Cells[1].Value = ds.Tables["tabla"].Rows[0][1].ToString();
                dataGridView1.Rows[row].Cells[2].Value = ds.Tables["tabla"].Rows[0][2].ToString();
                dataGridView1.Rows[row].DefaultCellStyle.ForeColor = Color.LimeGreen;
                dataGridView1.Rows[row].DefaultCellStyle.SelectionForeColor = Color.LimeGreen;
                txtCodigo.Clear();
                txtCodigo.Focus();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el préstamo?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.DefaultCellStyle.ForeColor == Color.LimeGreen)
                    {
                        detalles.Clear();
                        detalles.Add("0");
                        detalles.Add(id.ToString());
                        detalles.Add(row.Cells[0].Value.ToString());
                        detalles.Add("NOW()");
                        detalles.Add("NOW()");
                        detalles.Add("1");

                        Funciones.Insert("detalles", detalles);

                        ds = Conexion.MySQL("SELECT LAST_INSERT_ID();");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                        movimiento.Add("'Detalles'");
                        movimiento.Add("'Artículo'");
                        movimiento.Add("'" + row.Cells[1].Value.ToString() + "'");
                        movimiento.Add("NULL");
                        movimiento.Add("'Prestó'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);
                    }

                    if (row.DefaultCellStyle.ForeColor == Color.Black && noEntregadoID.Contains(int.Parse(row.Cells[0].Value.ToString())))
                    {
                        Conexion.MySQL("UPDATE detalles SET status = 0, updated_at = NOW() WHERE articulo = " + row.Cells[0].Value.ToString() + " AND prestamo = " + id + ";");

                        ds = Conexion.MySQL("SELECT id FROM detalles WHERE articulo = " + row.Cells[0].Value.ToString() + " AND prestamo = " + id + ";");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                        movimiento.Add("'Detalles'");
                        movimiento.Add("'Artículo'");
                        movimiento.Add("'" + row.Cells[1].Value.ToString() + "'");
                        movimiento.Add("NULL");
                        movimiento.Add("'Devolvió'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);

                        Conexion.MySQL("UPDATE articulos SET disponible = 1, updated_at = NOW() WHERE id = " + row.Cells[0].Value.ToString() + ";");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(row.Cells[0].Value.ToString());
                        movimiento.Add("'Articulos'");
                        movimiento.Add("'Disponible'");
                        movimiento.Add("'1'");
                        movimiento.Add("'0'");
                        movimiento.Add("'Modificó'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);
                    }

                    if (row.DefaultCellStyle.ForeColor == Color.Red && !noEntregadoID.Contains(int.Parse(row.Cells[0].Value.ToString())))
                    {
                        Conexion.MySQL("UPDATE detalles SET status = 1, updated_at = NOW() WHERE articulo = " + row.Cells[0].Value.ToString() + " AND prestamo = " + id + ";");

                        ds = Conexion.MySQL("SELECT id FROM detalles WHERE articulo = " + row.Cells[0].Value.ToString() + " AND prestamo = " + id + ";");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                        movimiento.Add("'Detalles'");
                        movimiento.Add("'Artículo'");
                        movimiento.Add("'" + row.Cells[1].Value.ToString() + "'");
                        movimiento.Add("NULL");
                        movimiento.Add("'Prestó'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);

                        Conexion.MySQL("UPDATE articulos SET disponible = 0, updated_at = NOW() WHERE id = " + row.Cells[0].Value.ToString() + ";");

                        movimiento.Clear();
                        movimiento.Add("0");
                        movimiento.Add(FrmMenu.usuarioID.ToString());
                        movimiento.Add(row.Cells[0].Value.ToString());
                        movimiento.Add("'Articulos'");
                        movimiento.Add("'Disponible'");
                        movimiento.Add("'0'");
                        movimiento.Add("'1'");
                        movimiento.Add("'Modificó'");
                        movimiento.Add("NOW()");
                        movimiento.Add("NOW()");
                        movimiento.Add("1");
                        Funciones.Insert("movimientos", movimiento);
                    }
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].DefaultCellStyle.ForeColor == Color.LimeGreen)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        dataGridView1.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                }

                noEntregadoID.Clear();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.DefaultCellStyle.ForeColor == Color.Red)
                    {
                        noEntregadoID.Add(int.Parse(row.Cells[0].Value.ToString()));
                    }
                }

                if (noEntregadoID.Count == 0)
                {
                    Conexion.MySQL("UPDATE prestamos SET status = 0, updated_at = NOW() WHERE id = " + id + ";");

                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(id.ToString());
                    movimiento.Add("'Prestamos'");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("'Devolvió'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos", movimiento);

                    txtCodigo.Enabled = false;
                    btnActualizar.Visible = false;
                }
            }
        }
    }
}
