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
                txtId.Text = ds.Tables["tabla"].Rows[0][0].ToString();
                cmbArticulo.Text = ds.Tables["tabla"].Rows[0][1].ToString();
                txtComentario.Text = ds.Tables["tabla"].Rows[0][2].ToString();
            }
        }

        //Variables Publicas y Privadas
        //Formulario Carga o Cierra
        //Desarrollo
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DataSet ds;
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

            if (id != 0)
            {
                var respuesta = MessageBox.Show("¿Esta seguro de actualizar este registro?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (respuesta == DialogResult.Yes)
                {
                    Funciones.Insert("articulos", valores);
                    this.Close();
                }
            }
            else
            {
                Funciones.Insert("articulos", valores);
                this.Close();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Funciones.StatusUpdate("articulos", btnEliminar.Text, id))
            {
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
