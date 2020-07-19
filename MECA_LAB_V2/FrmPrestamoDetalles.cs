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
        string status;
        DataSet ds;
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

                ds = Conexion.MySQL("SELECT detalles.articulo ID, articulos.articulo Artículo, articulos.comentario Comentario FROM detalles INNER JOIN articulos ON articulos.id = detalles.articulo WHERE detalles.prestamo = " + id + ";");
                dataGridView1.DataSource = ds.Tables["tabla"];
                dataGridView1.ClearSelection();

                ds = Conexion.MySQL("SELECT articulos.id FROM detalles INNER JOIN articulos ON detalles.articulo = articulos.id WHERE detalles.prestamo = " + id + " AND detalles.status = 1;");

                if (ds.Tables["tabla"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
                    {
                        noEntregadoID.Add(int.Parse(ds.Tables["tabla"].Rows[i][0].ToString()));
                    }

                    foreach (int item in noEntregadoID)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (item == Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                                dataGridView1.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Red;
                            }
                        }
                    }
                }

                ds = Conexion.MySQL("SELECT status FROM prestamos WHERE id = " + id + ";");
                status = ds.Tables["tabla"].Rows[0][0].ToString();
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
                        Conexion.MySQL("UPDATE articulos SET disponible = 1 WHERE id = " + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()) + ";");
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                    }
                }
                else
                {
                    MessageBox.Show("El artículo seleccionado ya ha sido devuelto.");
                }
            }
        }
    }
}
