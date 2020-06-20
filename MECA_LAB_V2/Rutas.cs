using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    class Rutas
    {
        public static FrmAlumnoDatos frmAlumnoDatos;
        public static FrmAlumnoRegistro frmAlumnoRegistro;
        public static FrmCrud frmCrud;
        public static Form GetForm(string tabla, string accion)
        {
            switch (tabla)
            {
                case "Alumnos":
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
