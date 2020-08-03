using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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
        Panel pnl = new Panel();

        DataSet ds;
        public FrmArticuloRegistro(int id = 0)
        {
            this.id = id;
            pnl.Width = 400;
            pnl.Height = 100;
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
                ds = Conexion.MySQL("select * from articulos where id =" + id + ";");
                
                btnEliminar.Visible = true;

                if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False" && ds.Tables["tabla"].Rows[0]["disponible"].ToString() == "False")
                {
                    btnReparar.BackColor = Color.SeaGreen;
                    btnEliminar.Visible = false;
                }
                else if (ds.Tables["tabla"].Rows[0]["status"].ToString() == "False")
                {
                    btnEliminar.Text = "Habilitar";
                }
                btnReparar.Visible = true;

                btnActualizar.Text = "Actualizar";
                status = ds.Tables["tabla"].Rows[0]["status"].ToString();
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();

                original.Add(ds.Tables["tabla"].Rows[0][1].ToString());
                original.Add(ds.Tables["tabla"].Rows[0][2].ToString());

                cmbArticulo.Text = original[0];
                txtComentario.Text = original[1];

                btnImprimir.Visible = true;
                btnGuardar.Visible = true;
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
                var respuesta = MessageBox.Show("¿Está seguro de actualizar este registro?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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
            ds = Conexion.MySQL("SELECT * FROM articulos WHERE id = " + id + ";");

            if (ds.Tables["tabla"].Rows[0]["disponible"].ToString() == "False" && status == "True")
            {
                MessageBox.Show("El artículo se encuentra en un préstamo activo.\nPara ser dado de baja debe no estar en un préstamo activo.");
                return;
            }

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
                this.DialogResult = DialogResult.OK;
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

        private void button1_Click(object sender, EventArgs e)
        {
            BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
            Codigo.IncludeLabel = true;
            Codigo.AlternateLabel = original[0] + " - " + id.ToString("0000");
            pnl.BackgroundImage = Codigo.Encode(BarcodeLib.TYPE.CODE128, id.ToString("0000"), Color.Black, Color.Transparent, 400, 100);

            Image imgFinal = (Image)pnl.BackgroundImage.Clone();
            SaveFileDialog CajaDeDialogoGuardar = new SaveFileDialog();
            CajaDeDialogoGuardar.FileName = id.ToString("0000");
            CajaDeDialogoGuardar.AddExtension = true;
            CajaDeDialogoGuardar.Filter = "Image PNG (*.png)|*.png";
            CajaDeDialogoGuardar.ShowDialog();
            if (!string.IsNullOrEmpty(CajaDeDialogoGuardar.FileName))
            {
                imgFinal.Save(CajaDeDialogoGuardar.FileName, ImageFormat.Png);
            }
            imgFinal.Dispose();
        }

        private void btnReparar_Click(object sender, EventArgs e)
        {
            if (btnReparar.BackColor == Color.Crimson)
            {
                var respuesta = MessageBox.Show("¿Está seguro de mandar a reparación este artículo?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    ds = Conexion.MySQL("SELECT * FROM articulos WHERE id = " + id + ";");

                    if (ds.Tables["tabla"].Rows[0]["disponible"].ToString() == "False")
                    {
                        MessageBox.Show("El artículo se encuentra en un préstamo activo.\nPara ser marcado como mandado a reparación debe no estar en un préstamo activo.");
                        return;
                    }

                    Conexion.MySQL("UPDATE articulos SET disponible = 0, status = 0 WHERE id = " + id + ";");
                    btnEliminar.Visible = false;
                    btnReparar.BackColor = Color.SeaGreen;

                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(id.ToString());
                    movimiento.Add("'Articulos'");
                    movimiento.Add("'Reparación'");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("'Enviado'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos", movimiento);
                }
            }
            else
            {
                var respuesta = MessageBox.Show("¿Está seguro de marcar como reparado este artículo?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    Conexion.MySQL("UPDATE articulos SET disponible = 1, status = 1 WHERE id = " + id + ";");
                    btnEliminar.Visible = true;
                    btnReparar.BackColor = Color.Crimson;

                    movimiento.Clear();
                    movimiento.Add("0");
                    movimiento.Add(FrmMenu.usuarioID.ToString());
                    movimiento.Add(id.ToString());
                    movimiento.Add("'Articulos'");
                    movimiento.Add("'Reparación'");
                    movimiento.Add("NULL");
                    movimiento.Add("NULL");
                    movimiento.Add("'Recibido'");
                    movimiento.Add("NOW()");
                    movimiento.Add("NOW()");
                    movimiento.Add("1");
                    Funciones.Insert("movimientos", movimiento);
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
            Codigo.IncludeLabel = true;
            Codigo.AlternateLabel = original[0] + " - " + id.ToString("0000");
            pnl.BackgroundImage = Codigo.Encode(BarcodeLib.TYPE.CODE128, id.ToString("0000"), Color.Black, Color.Transparent, 400, 100);

            PrintDialog print = new PrintDialog();
            PrintPreviewDialog preview = new PrintPreviewDialog();
            PrintDocument pd = new PrintDocument();

            pd.PrintPage += Doc_PrintPage;

            preview.Document = pd;

            if (preview.ShowDialog() == DialogResult.OK)
            {
                print.Document = pd;
                if (print.ShowDialog() == DialogResult.OK)
                {
                    pd.Print();
                }
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pnl.BackgroundImage, 0, 0);
            pnl.Dispose();
        }
    }
}
