using System.Drawing;

namespace MECA_LAB_V2
{
    class AlternarColorFilaDGV
    {
        public static void BlancoAzul(System.Windows.Forms.DataGridView dgv)
        {
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
        }
        public static void BlancoNaranja(System.Windows.Forms.DataGridView dgv)
        {
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 218, 174);
        }
        public static void BlancoRojo(System.Windows.Forms.DataGridView dgv)
        {
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 144, 144);
        }
        public static void BlancoVerde(System.Windows.Forms.DataGridView dgv)
        {
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(136, 233, 170);
        }
    }
}
