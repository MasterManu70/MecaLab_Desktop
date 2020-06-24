using System;
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
        private FrmPrincipal frmPrincipal =  new FrmPrincipal();

        //CRUDs
        private FrmCrud frmArticulos =       new FrmCrud("Articulos",Color.SteelBlue);
        private FrmCrud frmAlumnos =         new FrmCrud("Alumnos", Color.Orange);
        private FrmCrud frmUsuarios =        new FrmCrud("Usuarios", Color.Crimson);
        private FrmCrud frmMaestros =        new FrmCrud("Maestros", Color.Crimson);
        private FrmCrud frmCarreras =        new FrmCrud("Carreras", Color.Crimson);
        private FrmCrud frmLaboratorios =    new FrmCrud("Laboratorios", Color.Crimson);
        private FrmCrud frmAsignaturas =     new FrmCrud("Asignaturas", Color.Crimson);

        private int xClick = 0, yClick = 0;
        //Formulario carga o cierra
        private void FrmMenu_Load(object sender, EventArgs e) { 
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
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnPrincipal,true);
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmPrincipal);
            FrmLogin.ShowDialog();
            frmPrincipal.txtCodigo.Focus();
        }
        //Rutas
        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnPrincipal,true);
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmPrincipal);
            frmPrincipal.txtCodigo.Focus();

        }
        private void btnArticulos_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnArticulos,true);
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmArticulos);
            frmArticulos.textBox1.Focus();
        }
        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnAlumnos,true);
            ocultarSubMenu();
            viejaPosicion();
            AbrirFormEnPanel(frmAlumnos);
            frmAlumnos.textBox1.Focus();
        }

        private void btnDatos_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado, btnDatos,true);
            mostrarSubMenu(pnlSubMenuDatos);
            alternarPosicion();
        }
        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado,btnPrestamos,true);
            ocultarSubMenu();
            viejaPosicion();
            pnlLado2.Visible = false;
        }
        private void btnConsultas_Click(object sender, EventArgs e)
        {
            viejaPosicion();
            ocultarSubMenu();
            panelPosicion(pnlLado, btnConsultas, true);
            pnlLado2.Visible = false;
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
            {
                this.pnlContenedor.Controls.RemoveAt(0);
            }
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
            if (subMenu.Visible == false)
            {
                ocultarSubMenu();
                subMenu.Visible = true;
            }
        }
        private void viejaPosicion() {
            btnConsultas.Location = new Point(31, 459);
        }
        private void alternarPosicion()
        {
            if (btnConsultas.Location == new Point(31, 459))
            {
                btnConsultas.Location = new Point(31, 628);
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
