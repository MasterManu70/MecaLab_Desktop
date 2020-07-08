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
        public static FrmAlumnoRegistro frmAlumnoRegistro;

        //ARTÍCULOS
        public static FrmArticuloRegistro frmArticuloRegistro;

        //ASIGNATURAS
        public static FrmAsignaturaRegistro frmAsignaturaRegistro;

        //CARRERAS
        public static FrmCarreraRegistro frmCarreraRegistro;

        //LABORATORIOS
        public static FrmLaboratorioRegistro frmLaboratorioRegistro;

        //MAESTROS
        public static FrmMaestroRegistro frmMaestroRegistro;

        //USUARIOS
        public static FrmUsuarioRegistro frmUsuarioRegistro;

        /*El método 'GetForm' es para obtener un formulario de los anteriormente enrutados especificando con el dato:
         * - tabla  : Nombre de la tabla que se usa para el crud
         */
        public static Form GetForm(string tabla, int id = 0)
        {
            //Con el Switch principal (tabla) vamos a diferenciar los diferentes CRUD por cada una de las tablas que podemos llamar mediante el enrutamiento
            switch (tabla)
            {
                case "Alumnos":         return frmAlumnoRegistro = new FrmAlumnoRegistro(id);
                case "Articulos":       return frmArticuloRegistro = new FrmArticuloRegistro(id);
                case "Asignaturas":     return frmAsignaturaRegistro = new FrmAsignaturaRegistro(id);
                case "Carreras":        return frmCarreraRegistro = new FrmCarreraRegistro(id);
                case "Laboratorios":    return frmLaboratorioRegistro = new FrmLaboratorioRegistro(id);
                case "Maestros":        return frmMaestroRegistro = new FrmMaestroRegistro(id);
                case "Usuarios":        return frmUsuarioRegistro = new FrmUsuarioRegistro(id);
            }
            frmCrud = new FrmCrud("Alumnos",Color.Orange);
            return frmCrud;
        }
    }
}
