using System;
using System.Data.SqlClient;
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

            // 1. Creamos la BD, tablas y usuarios por defecto al arrancar el programa
            InicializarBaseDeDatosGlobal();

            // 2. Abrimos el Login
            Application.Run(new frmLogin());
        }

        private static void InicializarBaseDeDatosGlobal()
        {
            try
            {
                string conexionMaster = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";
                string cadenaConexion = @"Server=(localdb)\MSSQLLocalDB;Database=ControlCalificaciones;Trusted_Connection=True;";

                using (SqlConnection conexion = new SqlConnection(conexionMaster))
                {
                    conexion.Open();
                    string queryCheck = "SELECT database_id FROM sys.databases WHERE Name = 'ControlCalificaciones'";
                    SqlCommand cmdCheck = new SqlCommand(queryCheck, conexion);
                    var existe = cmdCheck.ExecuteScalar();

                    if (existe == null)
                    {
                        string queryCreate = "CREATE DATABASE ControlCalificaciones;";
                        SqlCommand cmdCreate = new SqlCommand(queryCreate, conexion);
                        cmdCreate.ExecuteNonQuery();
                    }
                }

                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string queryTablas = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Estudiantes' and xtype='U')
                        CREATE TABLE Estudiantes (
                            IdEstudiante INT IDENTITY(1,1) PRIMARY KEY,
                            Carnet VARCHAR(20) UNIQUE NOT NULL,
                            Nombre VARCHAR(50) NOT NULL,
                            Apellido VARCHAR(50) NOT NULL
                        );

                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Materias' and xtype='U')
                        CREATE TABLE Materias (
                            IdMateria INT IDENTITY(1,1) PRIMARY KEY,
                            NombreMateria VARCHAR(100) NOT NULL
                        );

                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Calificaciones' and xtype='U')
                        CREATE TABLE Calificaciones (
                            IdCalificacion INT IDENTITY(1,1) PRIMARY KEY,
                            IdEstudiante INT NOT NULL,
                            IdMateria INT NOT NULL,
                            Nota DECIMAL(5,2) NOT NULL
                        );

                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' and xtype='U')
                        CREATE TABLE Usuarios (
                            IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
                            Usuario VARCHAR(50) UNIQUE NOT NULL,
                            Password VARCHAR(50) NOT NULL,
                            Rol VARCHAR(20) NOT NULL
                        );

                        IF NOT EXISTS (SELECT * FROM Usuarios WHERE Usuario = 'admin')
                        INSERT INTO Usuarios (Usuario, Password, Rol) VALUES ('admin', '1234', 'Profesor');

                        IF NOT EXISTS (SELECT * FROM Usuarios WHERE Usuario = 'alumno')
                        INSERT INTO Usuarios (Usuario, Password, Rol) VALUES ('alumno', '1234', 'Alumno');";

                    SqlCommand cmdTablas = new SqlCommand(queryTablas, conexion);
                    cmdTablas.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar la BD global: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}