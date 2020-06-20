using System;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmProductos : Form
    {
        public FrmProductos()
        {
            InitializeComponent();
        }
        //Variables publicas y privadas
        private FrmProductoRegistro frmProductoRegistro = new FrmProductoRegistro();
        private FrmProductoDatos frmProductoDatos = new FrmProductoDatos();
        //Formulario carga o cierra
        private void FrmProductos_Load(object sender, EventArgs e)
        {
            AlternarColorFilaDGV.BlancoAzul(dataGridView1);
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("nombre", "Nombre");
            dataGridView1.Columns.Add("comentario", "Comentario");
            dataGridView1.Columns.Add("disponible", "Disponible");
            dataGridView1.Columns.Add("status", "Status");

            dataGridView1.Rows.Add("1", "Piston", "Comentario", "Disponible", "Alta");
            dataGridView1.Rows.Add("2", "Piston", "Comentario", "No disponible", "Alta");
            dataGridView1.Rows.Add("3", "Piston", "Comentario", "Disponible", "Alta");
        }
        //Desarrollo
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmProductoDatos.txtId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            frmProductoDatos.txtNombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frmProductoDatos.txtComentario.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            frmProductoDatos.ShowDialog();
        }
        //Rutas
        private void btnProducto_Click(object sender, EventArgs e)
        {
            frmProductoRegistro.ShowDialog();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        //Validaciones
        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumeros(e);
        }
        //Metodos
    }
}
