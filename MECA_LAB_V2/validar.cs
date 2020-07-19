using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MECA_LAB_V2
{
    class Validar
    {
        public static void SoloNumeros(KeyPressEventArgs pE)
        {
            if (char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else
            {
                pE.Handled = true;
            }
        }
        public static void SoloLetras(KeyPressEventArgs pE)
        {
            if (Char.IsLetter(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsSeparator(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else
            {
                pE.Handled = true;
            }
        }
        public static void SoloLetrasONumeros(KeyPressEventArgs pE)
        {
            if (Char.IsLetter(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsDigit(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsControl(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else if (Char.IsSeparator(pE.KeyChar))
            {
                pE.Handled = false;
            }
            else
            {
                pE.Handled = true;
            }
        }

        public static bool Validate(string texto, bool letras = false, bool numeros = false, string caracteres = "")
        {
            if (texto == "") return true;

            foreach (char c in texto)
            {
                if (caracteres.Contains(c.ToString()) && caracteres != "")
                {
                    continue;
                }
                if ((!char.IsNumber(c) && numeros) || (!Regex.IsMatch(c.ToString(), @"^[a-zA-Z]+$") && letras))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CorreoValidate(string correo)
        {
            int correoFirst = 0;
            int correoFirstCount = 0;
            int correoSecond = 0;

            if (!correo.Contains("@") || !correo.Contains(".")) return false;

            for (int i = 0; i < correo.Length; i++)
            {
                if (correo[i] == '@')
                {
                    correoFirst = i;
                    correoFirstCount++;
                }
                if (correo[i] == '.')
                {
                    correoSecond = i;
                }
            }

            if (correoFirstCount > 1) return false;

            if (correo[0] == '@' || correo[0] == '.' || correo[correo.Length - 1] == '@' || correo[correo.Length - 1] == '.' || correo[correoFirst] < correo[correoSecond - 1]) return false;

            return true;
        }
    }
}
