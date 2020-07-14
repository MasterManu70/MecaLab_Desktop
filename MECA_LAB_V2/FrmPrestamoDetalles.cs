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
                DataSet ds;

                ds = Conexion.MySQL(Funciones.GetQuery("Prestamos",id,2));

                txtAlumno.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                txtMaestro.Text = ds.Tables["tabla"].Rows[0][2].ToString();
                txtLaboratorio.Text = ds.Tables["tabla"].Rows[0][3].ToString();
                txtAsignatura.Text = ds.Tables["tabla"].Rows[0][4].ToString();
                txtUsuario.Text = ds.Tables["tabla"].Rows[0][5].ToString();
                txtEntrega.Text = ds.Tables["tabla"].Rows[0][6].ToString();
                txtCreado.Text = ds.Tables["tabla"].Rows[0][7].ToString();
                txtActualizado.Text = ds.Tables["tabla"].Rows[0][8].ToString();

                ds = Conexion.MySQL("SELECT detalles.articulo ID, articulos.articulo Artículo, articulos.comentario Comentario FROM detalles INNER JOIN articulos ON articulos.id = detalles.articulo WHERE detalles.prestamo = " + id + ";");
                dataGridView1.DataSource = ds.Tables["tabla"];
            }
        }
    }
}
