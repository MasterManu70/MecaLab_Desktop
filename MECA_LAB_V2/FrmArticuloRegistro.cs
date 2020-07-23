using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmArticuloRegistro : Form
    {
        int id;
        List<string> original = new List<string>();                 //Registro tomado de la base de datos.
        List<string> nuevo = new List<string>();                    //Registro con los nuevos valores que se comparará con los originales
        List<string> valores = new List<string>();                  //Registro que se actualizará/insertará en la base de datos.
        List<string> movimiento = new List<string>();               //Registro que se insertará en la tabla movimientos.
        string status;
        string descripcion;
        List<string> columnas = new List<string> { "Artículo", "Comentario"};

        DataSet ds;
        public FrmArticuloRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }

        private void FrmArticuloRegistro_Load(object sender, EventArgs e)
        {
            ds = Conexion.MySQL("SELECT DISTINCT articulo FROM articulos");

            for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
            {
                cmbArticulo.Items.Add(ds.Tables["tabla"].Rows[i][0].ToString());
            }

            if (id != 0)
            {
                ds = Conexion.MySQL("select * from articulos where id=" + id + ";");
                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnEliminar.Visible = true;
                btnActualizar.Text = "Actualizar";
                status = ds.Tables["tabla"].Rows[0]["status"].ToString();
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();

                original.Add(ds.Tables["tabla"].Rows[0][1].ToString());
                original.Add(ds.Tables["tabla"].Rows[0][2].ToString());

                cmbArticulo.Text = original[0];
                txtComentario.Text = original[1];
            }
        }

        //Variables Publicas y Privadas
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            List<string> valores = new List<string>();
            if (cmbArticulo.Text == "") { MessageBox.Show("Ingrese el nombre del articulo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); cmbArticulo.Focus(); return; }

            string disponible = "1";
            string status = "1";
            if (id != 0)
            {
                ds = Conexion.MySQL("SELECT disponible, status FROM articulos WHERE id = " + id + ";");
                disponible = ds.Tables["tabla"].Rows[0][0].ToString();
                status = ds.Tables["tabla"].Rows[0][1].ToString();
            }

            valores.Add(id.ToString());
            valores.Add("'" + cmbArticulo.Text + "'");
            valores.Add("'" + txtComentario.Text + "'");
            valores.Add(disponible);
            valores.Add("NOW()");
            valores.Add("NOW()");
            valores.Add(status);

            nuevo.Add(cmbArticulo.Text);
            nuevo.Add(txtComentario.Text);

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    if (Funciones.Insert("articulos", valores))
                    {
                        for (int i = 0; i < original.Count; i++)
                        {
                            if (original[i] != nuevo[i])
                            {
                                movimiento.Clear();
                                movimiento.Add("0");
                                movimiento.Add(FrmMenu.usuarioID.ToString());
                                movimiento.Add(id.ToString());
                                movimiento.Add("'Articulos'");
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
                    this.Close();
                }
            }
            else
            {
                if (Funciones.Insert("articulos", valores))
                {
                    ds = Conexion.MySQL("SELECT Last_Insert_ID();");
                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(ds.Tables["tabla"].Rows[0][0].ToString());
                    movimiento.Add("'Articulos'");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("'Agregó'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos", movimiento);
                }
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
            if (Funciones.StatusUpdate("articulos", btnEliminar.Text, id))
            {
                movimiento.Clear();
                movimiento.Add("0");
                movimiento.Add(FrmMenu.usuarioID.ToString());
                movimiento.Add(id.ToString());
                movimiento.Add("'Articulos'");
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
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        //Metodos
    }
}
