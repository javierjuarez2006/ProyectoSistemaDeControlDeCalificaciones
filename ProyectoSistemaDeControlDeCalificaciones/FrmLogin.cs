using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProyectoSistemaDeControlDeCalificaciones
{
    public partial class frmLogin : MetroFramework.Forms.MetroForm
    {
        // La misma cadena de conexión LocalDB que usas en tu otro formulario
        private readonly string cadenaConexion = @"Server=(localdb)\MSSQLLocalDB;Database=ControlCalificaciones;Trusted_Connection=True;";

        // Variables para controlar el límite de 3 intentos fallidos
        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Validar que no vayan vacíos los campos
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

                    // Consultamos el Rol del usuario en la base de datos
                    string query = "SELECT Rol FROM Usuarios WHERE Usuario = @Usuario AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                    object resultado = cmd.ExecuteScalar();

                    // Si encuentra el registro, el resultado no es nulo
                    if (resultado != null)
                    {
                        // Reiniciamos los intentos si entra con éxito
                        intentosFallidos = 0;

                        string rolUsuario = resultado.ToString(); // Obtiene 'Profesor' o 'Estudiante'

                        MessageBox.Show($"¡Bienvenido! Acceso concedido como: {rolUsuario}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Abrimos el formulario principal y le enviamos el rol detectado
                        frmControlCalificaciones formPrincipal = new frmControlCalificaciones(rolUsuario);
                        formPrincipal.Show();

                        // Ocultamos la ventana de login actual
                        this.Hide();
                    }
                    else
                    {
                        // Incrementamos el contador de intentos fallidos
                        intentosFallidos++;

                        if (intentosFallidos >= MAX_INTENTOS)
                        {
                            MessageBox.Show(
                                "Has superado el límite de 3 intentos permitidos.\nPor favor, comunícate con tu encargado para restablecer el acceso.",
                                "Acceso Bloqueado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );

                            // Cierra la aplicación por completo
                            Application.Exit();
                        }
                        else
                        {
                            int intentosRestantes = MAX_INTENTOS - intentosFallidos;
                            MessageBox.Show(
                                $"Usuario o contraseña incorrectos.\nIntentos restantes: {intentosRestantes}",
                                "Error de Autenticación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );

                            txtPassword.Clear();
                            txtUsuario.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}