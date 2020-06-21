using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    /*En este archivo se colocarán las rutas de todas las ventanas emergentes para la edición y registro de cada CRUD.
     * 
     * Esta clase tiene como función poder llamar mediante Rooting (o enrutamiento) los Formularios asignados a una tabla
     * y a una función.
     */
    class Rutas
    {
        //Aquí se declaran todos los objetos (Formularios) que desean ser enrutados

        //CRUD GENERAL
        public static FrmCrud frmCrud;

        //ALUMNOS
        public static FrmAlumnoDatos frmAlumnoDatos;
        public static FrmAlumnoRegistro frmAlumnoRegistro;

        //ARTÍCULOS
        public static FrmArticuloDatos frmArticuloDatos;
        public static FrmArticuloRegistro frmArticuloRegistro;

        //ASIGNATURAS
        public static FrmAsignaturaDatos frmAsignaturaDatos;
        public static FrmAsignaturaRegistro frmAsignaturaRegistro;

        //CARRERAS
        public static FrmCarreraDatos frmCarreraDatos;
        public static FrmCarreraRegistro frmCarreraRegistro;

        //LABORATORIOS
        public static FrmLaboratorioDatos frmLaboratorioDatos;
        public static FrmLaboratorioRegistro frmLaboratorioRegistro;

        //MAESTROS
        public static FrmMaestroDatos frmMaestroDatos;
        public static FrmMaestroRegistro frmMaestroRegistro;

        //USUARIOS
        public static FrmUsuarioDatos frmUsuarioDatos;
        public static FrmUsuarioRegistro frmUsuarioRegistro;

        /*El método 'GetForm' es para obtener un formulario de los anteriormente enrutados especificando dos datos:
         * - tabla  : Nombre de la tabla que se usa para el crud
         * - accion : Función principal a la que se orienta un formulario
         */
        public static Form GetForm(string tabla, string accion)
        {
            //Con el Switch principal (tabla) vamos a diferenciar los diferentes CRUD por cada una de las tablas que podemos llamar mediante el enrutamiento
            switch (tabla)
            {
                case "Alumnos":
                    //Con el Switch interno (accion) podemos llamar el formulario ligado al CRUD con el que fue llamado en el anterior Switch, con su debido nombre asignado para su función
                    switch (accion)
                    {
                        case "Datos":
                            frmAlumnoDatos = new FrmAlumnoDatos();
                            return frmAlumnoDatos;
                        case "Registro":
                            frmAlumnoRegistro = new FrmAlumnoRegistro();
                            return frmAlumnoRegistro;
                    }
                    break;
                case "Articulos":
                    switch (accion)
                    {
                        case "Datos":
                            frmArticuloDatos = new FrmArticuloDatos();
                            return frmArticuloDatos;
                        case "Registro":
                            frmArticuloRegistro = new FrmArticuloRegistro();
                            return frmArticuloRegistro;
                    }
                    break;
                case "Asignaturas":
                    switch (accion)
                    {
                        case "Datos":
                            frmAsignaturaDatos = new FrmAsignaturaDatos();
                            return frmAsignaturaDatos;
                        case "Registro":
                            frmAsignaturaRegistro = new FrmAsignaturaRegistro();
                            return frmAsignaturaRegistro;
                    }
                    break;
                case "Carreras":
                    switch (accion)
                    {
                        case "Datos":
                            frmCarreraDatos = new FrmCarreraDatos();
                            return frmCarreraDatos;
                        case "Registro":
                            frmCarreraRegistro = new FrmCarreraRegistro();
                            return frmCarreraRegistro;
                    }
                    break;
                case "Laboratorios":
                    switch (accion)
                    {
                        case "Datos":
                            frmLaboratorioDatos = new FrmLaboratorioDatos();
                            return frmLaboratorioDatos;
                        case "Registro":
                            frmLaboratorioRegistro = new FrmLaboratorioRegistro();
                            return frmLaboratorioRegistro;
                    }
                    break;
                case "Maestros":
                    switch (accion)
                    {
                        case "Datos":
                            frmMaestroDatos = new FrmMaestroDatos();
                            return frmMaestroDatos;
                        case "Registro":
                            frmMaestroRegistro = new FrmMaestroRegistro();
                            return frmMaestroRegistro;
                    }
                    break;
                case "Usuarios":
                    switch (accion)
                    {
                        case "Datos":
                            frmUsuarioDatos = new FrmUsuarioDatos();
                            return frmUsuarioDatos;
                        case "Registro":
                            frmUsuarioRegistro = new FrmUsuarioRegistro();
                            return frmUsuarioRegistro;
                    }
                    break;

                default:
                    break;
            }
            frmCrud = new FrmCrud("Alumnos",Color.Orange);
            return frmCrud;
        }
    }
}
