using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmMenu : Form
    {
        //Variables globales
        public static string usuario = "";
        public static int usuarioID = 0;
        public static int usuarioNivel = 0;
        public static bool showed = false;
        public static int intentos = 0;
        public static List<string> actualizados = new List<string>();
        public static List<string> columnas = new List<string>(){"ID","Alumno","Maestro","Laboratorio","Asignatura","Usuario","Entrega","Creado","Actualizado","status","Artículo","Comentario","Disponible","Matrícula","Nombre","Paterno","Materno","Carrera","Correo","Teléfono", "Registro", "Tabla", "Campo","Nuevo","Viejo","Descripción"};

        public static DataSet ds;
        public FrmMenu()
        {
            InitializeComponent();
        }
        //Variables Publicas y Privadas
        private FrmLogin FrmLogin = new FrmLogin();
        public static FrmPrincipal frmPrincipal =  new FrmPrincipal();
        private FrmReportes frmReportes = new FrmReportes();

        //CRUDs
        private FrmCrud frmArticulos =          new FrmCrud("Articulos",Color.SteelBlue);
        private FrmCrud frmAlumnos =            new FrmCrud("Alumnos", Color.SteelBlue);
        private FrmCrud frmUsuarios =           new FrmCrud("Usuarios", Color.Crimson);
        private FrmCrud frmMaestros =           new FrmCrud("Maestros", Color.Crimson);
        private FrmCrud frmCarreras =           new FrmCrud("Carreras", Color.Crimson);
        private FrmCrud frmPrestamos =          new FrmCrud("Prestamos", Color.SteelBlue);
        private FrmCrud frmLaboratorios =       new FrmCrud("Laboratorios", Color.Crimson);
        private FrmCrud frmAsignaturas =        new FrmCrud("Asignaturas", Color.Crimson);
        private FrmCrud frmMovimientos =        new FrmCrud("Movimientos", Color.DarkOrange);

        //Formulario carga o cierra
        private void FrmMenu_Load(object sender, EventArgs e) { 
            //this.Show();
            AbrirFormEnPanel(frmPrincipal);
            viejaPosicion(btnConsultas, 31, 459);
            timer1.Enabled = true;
            FrmLogin.ShowDialog();

            //if (res == DialogResult.OK && usuarioNivel != 0)
            //{
            //}
            //else if (intentos < 3)
            //{
            //    FrmMenu_Load(sender, e);
            //}
            //else if (intentos == 3)
            //{
            //    Application.Exit();
            //}
        }
        //Desarollo
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            //panelPosicion(pnlLado,btnPrincipal,true);
            //ocultarSubMenu(pnlSubMenuDatos);
            //ocultarSubMenu(pnlSubMenuDatos2);
            //viejaPosicion(btnConsultas, 31, 459);
            //AbrirFormEnPanel(frmPrincipal);
            //btnDatos.Visible = false;
            //btnConsultas.Visible = false;
            //frmPrincipal.borrarContenido();
            //FrmLogin.ShowDialog();
            //frmPrincipal.txtCodigo.Focus();
            Application.Restart();
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

            if (!FrmPrincipal.devolver)
            {
                ds = Conexion.MySQL("SELECT COUNT(id) FROM maestros WHERE status = 1;");
                if (ds.Tables["tabla"].Rows[0][0].ToString() != frmPrincipal.cmbMaestro.Items.Count.ToString()) Funciones.TableToCombo(frmPrincipal.cmbMaestro, FrmPrincipal.maestros, "maestros");
                else
                {
                    ds = Conexion.MySQL("SELECT id FROM maestros WHERE status = 1;");
                    for (int i = 0; i < FrmPrincipal.maestros.Count; i++)
                    {
                        if (ds.Tables["tabla"].Rows[i][0].ToString() != FrmPrincipal.maestros[i].ToString())
                        {
                            Funciones.TableToCombo(frmPrincipal.cmbMaestro, FrmPrincipal.maestros, "maestros");
                            return;
                        }
                    }
                }

                ds = Conexion.MySQL("SELECT COUNT(id) FROM asignaturas WHERE status = 1;");
                if (ds.Tables["tabla"].Rows[0][0].ToString() != frmPrincipal.cmbAsignatura.Items.Count.ToString()) Funciones.TableToCombo(frmPrincipal.cmbAsignatura, FrmPrincipal.asignaturas, "asignaturas");
                else
                {
                    ds = Conexion.MySQL("SELECT id FROM asignaturas WHERE status = 1;");
                    for (int i = 0; i < FrmPrincipal.asignaturas.Count; i++)
                    {
                        if (ds.Tables["tabla"].Rows[i][0].ToString() != FrmPrincipal.asignaturas[i].ToString())
                        {
                            Funciones.TableToCombo(frmPrincipal.cmbAsignatura, FrmPrincipal.asignaturas, "asignaturas");
                            return;
                        }
                    }
                }

                ds = Conexion.MySQL("SELECT COUNT(id) FROM laboratorios WHERE status = 1;");
                if (ds.Tables["tabla"].Rows[0][0].ToString() != frmPrincipal.cmbLaboratorio.Items.Count.ToString()) Funciones.TableToCombo(frmPrincipal.cmbLaboratorio, FrmPrincipal.laboratorios, "laboratorios");
                else
                {
                    ds = Conexion.MySQL("SELECT id FROM laboratorios WHERE status = 1;");
                    for (int i = 0; i < FrmPrincipal.laboratorios.Count; i++)
                    {
                        if (ds.Tables["tabla"].Rows[i][0].ToString() != FrmPrincipal.laboratorios[i].ToString())
                        {
                            Funciones.TableToCombo(frmPrincipal.cmbLaboratorio, FrmPrincipal.laboratorios, "laboratorios");
                            return;
                        }
                    }
                }
            }
        }
        private void btnArticulos_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnArticulos,true);
            ocultarSubMenu(pnlSubMenuDatos);
            ocultarSubMenu(pnlSubMenuDatos2);
            viejaPosicion(btnConsultas, 31, 459);
            AbrirFormEnPanel(frmArticulos);
            frmArticulos.txtBuscar.Focus();
        }
        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            pnlLado2.Visible = false;
            panelPosicion(pnlLado,btnAlumnos,true);
            ocultarSubMenu(pnlSubMenuDatos);
            ocultarSubMenu(pnlSubMenuDatos2);
            viejaPosicion(btnConsultas, 31, 459);
            AbrirFormEnPanel(frmAlumnos);
            frmAlumnos.txtBuscar.Focus();
        }
        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado,btnPrestamos,true);
            ocultarSubMenu(pnlSubMenuDatos);
            viejaPosicion(btnConsultas,31, 459);
            AbrirFormEnPanel(frmPrestamos);
            frmPrestamos.RowsToRed();
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
            frmUsuarios.txtBuscar.Focus();
        }
        private void btnAsignaturas_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnAsignaturas,true);
            AbrirFormEnPanel(frmAsignaturas);
            frmAsignaturas.txtBuscar.Focus();
        }
        private void btnCarreras_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnCarreras,true);
            AbrirFormEnPanel(frmCarreras);
            frmCarreras.txtBuscar.Focus();
        }
        private void btnLaboratorios_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2,btnLaboratorios,true);
            AbrirFormEnPanel(frmLaboratorios);
            frmLaboratorios.txtBuscar.Focus();
        }
        private void btnMaestros_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado2, btnMaestros, true);
            AbrirFormEnPanel(frmMaestros);
            frmMaestros.txtBuscar.Focus();
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
            frmMovimientos.btnRegistro.Visible = false;
            frmMovimientos.txtBuscar.Focus();
        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            panelPosicion(pnlLado3, btnReportes, true);
            AbrirFormEnPanel(frmReportes);
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
                Curva.ElipseRadius = 0;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                Curva.ElipseRadius = 5;
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

                if (usuarioNivel == 1)
                {
                    btnDatos.Visible = true;
                    btnConsultas.Visible = true;
                }
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
