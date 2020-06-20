﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private FrmLogin FrmLogin = new FrmLogin();
        private FrmPrincipal frmPrincipal = new FrmPrincipal();
        private FrmProductos frmProductos = new FrmProductos();
        private FrmAlumno frmAlumno = new FrmAlumno();
        private FrmUsuarios frmUsuarios = new FrmUsuarios();
        private FrmMaestros frmMaestros = new FrmMaestros();
        private FrmCarreras frmCarreras = new FrmCarreras();
        private FrmLaboratorios frmLaboratorios = new FrmLaboratorios();
        private FrmAsignaturas frmAsignaturas = new FrmAsignaturas();

        private int xClick = 0, yClick = 0;
        //Formulario carga o cierra
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            this.Show();
            AbrirFormEnPanel(frmPrincipal);
            viejaPosicion();
            ocultarSubMenu();
            timer1.Enabled = true;
            FrmLogin.ShowDialog();
        }
        //Desarollo
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            pnlLado.Height = btnPrincipal.Height;
            pnlLado.Top = btnPrincipal.Top;
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmPrincipal);
            FrmLogin.ShowDialog();
            frmPrincipal.txtCodigo.Focus();
        }
        //Rutas
        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            pnlLado.Height = btnPrincipal.Height;
            pnlLado.Top = btnPrincipal.Top;
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmPrincipal);
            frmPrincipal.txtCodigo.Focus();
           

        }
        private void btnProductos_Click(object sender, EventArgs e)
        {
            pnlLado.Height = btnProductos.Height;
            pnlLado.Top = btnProductos.Top;
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmProductos);
        }
        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            pnlLado.Height = btnAlumnos.Height;
            pnlLado.Top = btnAlumnos.Top;
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmAlumno);
        }

        private void btnDatos_Click(object sender, EventArgs e)
        {
            pnlLado.Height = btnDatos.Height;
            pnlLado.Top = btnDatos.Top;
            mostrarSubMenu(pnlSubMenuDatos);
            alternarPosicion();
        }
        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            pnlLado.Height = btnPrestamos.Height;
            pnlLado.Top = btnPrestamos.Top;
            ocultarSubMenu();
            viejaPosicion();
        }
        private void btnConsultas_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            pnlLado.Height = btnConsultas.Height;
            pnlLado.Top = btnConsultas.Top;
        }
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            AbrirFormEnPanel(frmUsuarios);
        }
        private void btnAsignaturas_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            AbrirFormEnPanel(frmAsignaturas);
        }
        private void btnCarreras_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            AbrirFormEnPanel(frmCarreras);
        }
        private void btnLaboratorios_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            AbrirFormEnPanel(frmLaboratorios);
        }
        private void btnMaestros_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            AbrirFormEnPanel(frmMaestros);
        }
        //Formulario Maximiazar Minimizar, Cerrar y Diseño
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString();
        }
        //Metodo para abrir formularios dentro del panel contenedor
        private void AbrirFormEnPanel(Form Formhijo)
        {
            if (this.pnlContenedor.Controls.Count > 0)
                this.pnlContenedor.Controls.RemoveAt(0);
            Formhijo.TopLevel = false;
            Formhijo.Dock = DockStyle.Fill;
            this.pnlContenedor.Controls.Add(Formhijo);
            this.pnlContenedor.Tag = Formhijo;
            Formhijo.Show();
        }

        private void ocultarSubMenu() {
            pnlSubMenuDatos.Visible = false;
        }

        private void mostrarSubMenu(Panel subMenu) {
            if (pnlSubMenuDatos.Visible == false)
            {
                ocultarSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }
        private void viejaPosicion() {
            btnConsultas.Location = new Point(29, 539);
        }
        private void alternarPosicion()
        {
            if (btnConsultas.Location == new Point(29, 539))
            {
                btnConsultas.Location = new Point(29, 755);
            }
            else
            {
                btnConsultas.Location = new Point(29, 539);
            }
        }
    }
}
