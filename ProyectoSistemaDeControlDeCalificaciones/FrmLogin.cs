using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace ProyectoSistemaDeControlDeCalificaciones
{
    public partial class frmLogin : MetroFramework.Forms.MetroForm
    {
        private readonly string cadenaConexion =
    ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;

        // Variables para controlar el límite de 3 intentos fallidos
        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;

        public frmLogin()
        {
            InitializeComponent();
        }
        






        // 1. Este es el evento exclusivo de tu botón de Ingresar (el que valida usuario, contraseña y base de datos)
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, ingrese el usuario y la contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "SELECT Rol FROM Usuarios WHERE Usuario = @Usuario AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                    {
                        intentosFallidos = 0;
                        string rolUsuario = resultado.ToString();

                        MessageBox.Show($"¡Bienvenido! Acceso concedido como: {rolUsuario}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        frmControlCalificaciones formPrincipal = new frmControlCalificaciones(rolUsuario);
                        formPrincipal.Show();
                        this.Hide();
                    }
                    else
                    {
                        intentosFallidos++;

                        if (intentosFallidos >= 3)
                        {
                            MessageBox.Show("Has superado el límite de 3 intentos permitidos.", "Acceso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show($"Usuario o contraseña incorrectos. Intentos restantes: {3 - intentosFallidos}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Clear();
                            txtUsuario.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        // 2. Este es el evento exclusivo de tu botón "Ver Autores del Proyecto"
        private void btnVerAutores_Click(object sender, EventArgs e)
        {
            frmAutores formAutores = new frmAutores();
            formAutores.ShowDialog();
        }
    }
} 