﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmArticuloRegistro : Form
    {
        int id;
        public FrmArticuloRegistro(int id = 0)
        {
            this.id = id;
            InitializeComponent();
        }

        private void FrmArticuloRegistro_Load(object sender, EventArgs e)
        {
            DataSet ds;
            if (id != 0)
            {
                ds = Conexion.MySQL("select * from articulos where id=" + id + ";");
                btnEliminar.Visible = true;
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                txtNombre.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                txtComentario.Text = ds.Tables["tabla"].Rows[0][2].ToString();
            }
        }

        //Variables Publicas y Privadas
        private int xClick = 0, yClick = 0;
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "") { MessageBox.Show("Ingrese el nombre del articulo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtNombre.Focus(); return; }
            if (txtComentario.Text == "") { MessageBox.Show("Ingrese el comentario del articulo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); txtComentario.Focus(); return; }
            var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();
                this.Close();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Esta seguro de eliminar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (respuesta == DialogResult.Yes)
            {
                //Codigo Mysql
                borrarContenido();
                this.Close();
            }
        }
        //Rutas
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            borrarContenido();
            this.Close();
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            borrarContenido();
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void FrmProductoDatos_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }
        //Validaciones
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloLetras(e);
        }

        //Metodos
        private void borrarContenido()
        {
            txtNombre.Focus();
            txtNombre.Clear();
            txtComentario.Clear();
        }
    }
}