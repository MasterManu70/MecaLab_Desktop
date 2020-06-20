using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    /*En ese archivo se colocarán las rutas de todas las ventanas emergentes para la edición y registro de cada CRUD.
     * 
     * Esta clase tiene como función poder llamar mediante Rooting (o enrutamiento) los Formularios asignados a una tabla
     * y a una función.
     */
    class Rutas
    {
        //Aquí se declaran todos los objetos (Formularios) que desean ser enrutados
        public static FrmAlumnoDatos frmAlumnoDatos;
        public static FrmAlumnoRegistro frmAlumnoRegistro;
        public static FrmCrud frmCrud;

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
                default:
                    break;
            }
            frmCrud = new FrmCrud("Alumnos",Color.Orange);
            return frmCrud;
        }
    }
}
