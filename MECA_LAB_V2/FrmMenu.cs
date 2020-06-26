using System;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmMenu : Form
    {
        public static string usuario = "";
        public static int usuarioID = 0;
        public static int usuarioNivel = 0;
        public static bool showed = false;
        public FrmMenu()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private FrmLogin FrmLogin = new FrmLogin();
        private FrmPrincipal frmPrincipal =  new FrmPrincipal();

        //CRUDs
        private FrmCrud frmArticulos =       new FrmCrud("Articulos",Color.SteelBlue);
        private FrmCrud frmAlumnos =         new FrmCrud("Alumnos", Color.SteelBlue);
        private FrmCrud frmUsuarios =        new FrmCrud("Usuarios", Color.Crimson);
        private FrmCrud frmMaestros =        new FrmCrud("Maestros", Color.Crimson);
        private FrmCrud frmCarreras =        new FrmCrud("Carreras", Color.Crimson);
        private FrmCrud frmLaboratorios =    new FrmCrud("Laboratorios", Color.Crimson);
        private FrmCrud frmAsignaturas =     new FrmCrud("Asignaturas", Color.Crimson);
        private FrmCrud frmMovimientos =     new FrmCrud("Movimientos", Color.DarkOrange);

        //Formulario carga o cierra
        private void FrmMenu_Load(object sender, EventArgs e) { 
            this.Show();
            AbrirFormEnPanel(frmPrincipal);
            viejaPosicion(btnConsultas, 31, 459);
            timer1.Enabled = true;
            FrmLogin.ShowDialog();
        }
        //Desarollo
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado,btnPrincipal,true);
            ocultarSubMenu(pnlSubMenuDatos);
            ocultarSubMenu(pnlSubMenuDatos2);
            viejaPosicion(btnConsultas, 31, 459);
            AbrirFormEnPanel(frmPrincipal);
            FrmLogin.ShowDialog();
            frmPrincipal.txtCodigo.Focus();
        }
        //Rutas
        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnPrincipal,true);
            ocultarSubMenu(pnlSubMenuDatos);
            ocultarSubMenu(pnlSubMenuDatos2);
            viejaPosicion(btnConsultas, 31, 459);
            AbrirFormEnPanel(frmPrincipal);
            frmPrincipal.txtCodigo.Focus();

        }
        private void btnArticulos_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnArticulos,true);
            ocultarSubMenu(pnlSubMenuDatos);
            ocultarSubMenu(pnlSubMenuDatos2);
            viejaPosicion(btnConsultas, 31, 459);
            AbrirFormEnPanel(frmArticulos);
            frmArticulos.textBox1.Focus();
        }
        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnAlumnos,true);
            ocultarSubMenu(pnlSubMenuDatos);
            ocultarSubMenu(pnlSubMenuDatos2);
            viejaPosicion(btnConsultas, 31, 459);
            AbrirFormEnPanel(frmAlumnos);
            frmAlumnos.textBox1.Focus();
        }
        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado,btnPrestamos,true);
            ocultarSubMenu(pnlSubMenuDatos);
            viejaPosicion(btnConsultas,31, 459);
            pnlLado2.Visible = false;
        }
        private void btnDatos_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado, btnDatos, true);
            ocultarSubMenu(pnlSubMenuDatos2);
            mostrarSubMenu(pnlSubMenuDatos, 3, 459);
            viejaPosicion(btnConsultas, 31, 623);
            pnlLado3.Visible = false;
        }
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnUsuarios,true);
            AbrirFormEnPanel(frmUsuarios);
            frmUsuarios.textBox1.Focus();
        }
        private void btnAsignaturas_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnAsignaturas,true);
            AbrirFormEnPanel(frmAsignaturas);
            frmAsignaturas.textBox1.Focus();
        }
        private void btnCarreras_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnCarreras,true);
            AbrirFormEnPanel(frmCarreras);
            frmCarreras.textBox1.Focus();
        }
        private void btnLaboratorios_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnLaboratorios,true);
            AbrirFormEnPanel(frmLaboratorios);
            frmLaboratorios.textBox1.Focus();
        }
        private void btnMaestros_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2, btnMaestros, true);
            AbrirFormEnPanel(frmMaestros);
            frmMaestros.textBox1.Focus();
        }
        private void btnConsultas_Click(object sender, EventArgs e)
        {
            viejaPosicion(btnConsultas, 31, 459);
            ocultarSubMenu(pnlSubMenuDatos);
            mostrarSubMenu(pnlSubMenuDatos2, 3, 503);
            panelPosicion(pnlLado, btnConsultas, true);
            pnlLado2.Visible = false;
        }
        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado3,btnMovimientos,true);
            AbrirFormEnPanel(frmMovimientos);
            frmMovimientos.lblFiltrar.Visible = false;
            frmMovimientos.cmbFiltro.Visible = false;
            frmMovimientos.btnRegistro.Visible = false;
            frmMovimientos.textBox1.Focus();
        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado3, btnReportes, true);
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString();
        }
        //Metodo para abrir formularios dentro del panel contenedor
        private void AbrirFormEnPanel(Form Formhijo)
        {
            if (this.pnlContenedor.Controls.Count > 0)
            {
                this.pnlContenedor.Controls.RemoveAt(0);
            }
            Formhijo.TopLevel = false;
            Formhijo.Dock = DockStyle.Fill;
            this.pnlContenedor.Controls.Add(Formhijo);
            this.pnlContenedor.Tag = Formhijo;
            Formhijo.Show();
        }
        private void ocultarSubMenu(Panel pnl) {
            pnl.Visible = false;
        }

        private void mostrarSubMenu(Panel subMenu,int x, int y) {
            if (subMenu.Visible == false)
            {
                subMenu.Location = new Point(x,y);
                subMenu.Visible = true;
            }
        }
        private void viejaPosicion(Button btn, int x, int y) {
            btn.Location = new Point(x, y);
        }

        private void FrmMenu_Activated(object sender, EventArgs e)
        {
            if (showed)
            {
                label5.Visible = true;
                lblUsuario.Text = usuario;
                lblUsuario.Visible = true;

                label6.Visible = true;
                if (usuarioNivel == 1) lblNivel.Text = "Administrador"; else lblNivel.Text = "Usuario";
                lblNivel.Visible = true;
            }
        }
        private void panelPosicion(Panel pnl, Button btn, Boolean bol)
        {
            pnl.Height = btn.Height;
            pnl.Top = btn.Top;
            pnl.Visible = bol;
        }
    }
}
