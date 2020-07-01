using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmPrincipal : Form
    {
        //Listas de llaves primarias correspondiente a cada registro
        public static List<int> maestros = new List<int>();
        public static List<int> asignaturas = new List<int>();
        public static List<int> laboratorios = new List<int>();

        int codigo = 0;
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


        }
        //Desarrollo
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text == "") { MessageBox.Show("Ingrese el codigo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtCodigo.Focus(); return; }
            if (txtMatricula.Text == "") { MessageBox.Show("Ingrese la matricula", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtMatricula.Focus(); return; }
            if (cmbMaestro.SelectedIndex == 0) { MessageBox.Show("Seleccione el nombre del maestro", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbMaestro.Focus(); return; }
            if (cmbAsignatura.SelectedIndex == 0) { MessageBox.Show("Seleccione la asignatura", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbAsignatura.Focus(); return; }
            if (cmbLaboratorio.SelectedIndex == 0) { MessageBox.Show("Seleccione laboratorio", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbLaboratorio.Focus(); return; }
            var respuesta = MessageBox.Show("¿Desea realizar el siguiente prestamo?","Informacion",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes) {
                //Codigo MYSQL
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
            txtProducto.Clear();
            cmbAsignatura.SelectedIndex = 0;
            cmbLaboratorio.SelectedIndex = 0;
            cmbMaestro.SelectedIndex = 0;
            txtComentario.Clear();
            dataGridView1.Rows.Clear();
            txtCodigo.Focus();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtCodigo.Text, out codigo))
            {
                txtCodigo.Clear();
                return;
            }
        }
    }
}
