using System;
using System.Windows.Forms;

namespace ProyectoSistemaDeControlDeCalificaciones
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Esto arranca la aplicación abriendo el formulario de Login
            Application.Run(new frmLogin());
        }
    }
}

