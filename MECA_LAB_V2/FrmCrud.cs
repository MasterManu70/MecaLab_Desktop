using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    public partial class FrmCrud : Form
    {
        string tabla;
        Color color;
        public FrmCrud(string tabla, Color color)
        {
            this.tabla = tabla;
            this.color = color;
            InitializeComponent();
        }

        private void FrmCrud_Load(object sender, EventArgs e)
        {
            labelTitle.Text = tabla;

            panelTitle.BackColor = color;
            btnBuscar.BackColor = color;
            btnRegistro.BackColor = color;
        }
    }
}
