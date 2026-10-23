using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace ProyectoSistemaDeControlDeCalificaciones
{
    public partial class frmControlCalificaciones : Form
    {
        private readonly string cadenaConexion = @"Server=(localdb)\MSSQLLocalDB;Database=ControlCalificaciones;Trusted_Connection=True;";

        public frmControlCalificaciones()
        {
            InitializeComponent();
            // Nota: Ya quitamos el evento automático de la tabla para que no cargue nada al solo hacer clic.
        }

        private void frmControlCalificaciones_Load(object sender, EventArgs e)
        {
            CargarDatosEnTabla();
        }

        private void CargarDatosEnTabla()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    string query = @"SELECT 
                                        e.IdEstudiante,
                                        e.Carnet,
                                        e.Nombre,
                                        e.Apellido,
                                        c.Nota,
                                        CASE
                                            WHEN c.Nota >= 6.00 THEN 'Aprobado'
                                            ELSE 'Reprobado'
                                        END AS Estado
                                     FROM Calificaciones c
                                     INNER JOIN Estudiantes e ON c.IdEstudiante = e.IdEstudiante
                                     ORDER BY e.Apellido, e.Nombre;";

                    SqlDataAdapter adaptador = new SqlDataAdapter(query, conexion);
                    DataTable dt = new DataTable();
                    adaptador.Fill(dt);
                    dgvEstudiantes.DataSource = dt;

                    if (dgvEstudiantes.Columns["IdEstudiante"] != null)
                    {
                        dgvEstudiantes.Columns["IdEstudiante"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message,
                                "Error de Conexión",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnCargarDatos_Click(object sender, EventArgs e)
        {
            CargarDatosEnTabla();
            LimpiarCampos();
        }

        // BOTÓN GUARDAR / ACTUALIZAR (El botón izquierdo principal)
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarnet.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtNota.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string notaTexto = txtNota.Text.Trim();
            if (!decimal.TryParse(notaTexto, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal notaValidada) &&
                !decimal.TryParse(notaTexto.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out notaValidada))
            {
                MessageBox.Show("El formato de la nota no es válido. Ingrese un número correcto.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SI EL BOTÓN DICE "Guardar": Inserta un estudiante nuevo
            if (btnGuardar.Text == "Guardar")
            {
                try
                {
                    using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                    {
                        conexion.Open();

                        string queryEstudiante = @"INSERT INTO Estudiantes (Carnet, Nombre, Apellido) 
                                                   VALUES (@Carnet, @Nombre, @Apellido);
                                                   SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdEstudiante = new SqlCommand(queryEstudiante, conexion);
                        cmdEstudiante.Parameters.AddWithValue("@Carnet", txtCarnet.Text.Trim());
                        cmdEstudiante.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        cmdEstudiante.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());

                        int idEstudiante = Convert.ToInt32(cmdEstudiante.ExecuteScalar());

                        string queryCalificacion = @"INSERT INTO Calificaciones (IdEstudiante, IdMateria, Nota) 
                                                     VALUES (@IdEstudiante, 1, @Nota);";

                        SqlCommand cmdCalificacion = new SqlCommand(queryCalificacion, conexion);
                        cmdCalificacion.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                        cmdCalificacion.Parameters.AddWithValue("@Nota", notaValidada);

                        cmdCalificacion.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Estudiante guardado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatosEnTabla();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // SI EL BOTÓN CAMBIÓ A "Actualizar": Aplica la modificación en la base de datos
            {
                if (dgvEstudiantes.CurrentRow == null || dgvEstudiantes.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Por favor, seleccione un estudiante de la tabla.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    int idEstudiante = Convert.ToInt32(dgvEstudiantes.CurrentRow.Cells["IdEstudiante"].Value);

                    using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                    {
                        conexion.Open();

                        string queryEstudiante = @"UPDATE Estudiantes 
                                                   SET Carnet = @Carnet, Nombre = @Nombre, Apellido = @Apellido 
                                                   WHERE IdEstudiante = @IdEstudiante;";

                        SqlCommand cmdEstudiante = new SqlCommand(queryEstudiante, conexion);
                        cmdEstudiante.Parameters.AddWithValue("@Carnet", txtCarnet.Text.Trim());
                        cmdEstudiante.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        cmdEstudiante.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());
                        cmdEstudiante.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                        cmdEstudiante.ExecuteNonQuery();

                        string queryCalificacion = @"UPDATE Calificaciones 
                                                     SET Nota = @Nota 
                                                     WHERE IdEstudiante = @IdEstudiante;";

                        SqlCommand cmdCalificacion = new SqlCommand(queryCalificacion, conexion);
                        cmdCalificacion.Parameters.AddWithValue("@Nota", notaValidada);
                        cmdCalificacion.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                        cmdCalificacion.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Registro actualizado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatosEnTabla();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // BOTÓN EDITAR: Aquí ocurre la magia. Seleccionas la fila en la tabla, haces clic en Editar, 
        // los datos suben a las cajas de texto y el botón Guardar se convierte en "Actualizar".
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.CurrentRow == null || dgvEstudiantes.CurrentRow.Index < 0)
            {
                MessageBox.Show("Por favor, seleccione un estudiante de la tabla para poder editarlo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvEstudiantes.CurrentRow;
            if (!fila.IsNewRow && fila.Cells["Carnet"].Value != null)
            {
                txtCarnet.Text = fila.Cells["Carnet"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
                txtNota.Text = fila.Cells["Nota"].Value.ToString();
                cmbEstado.Text = fila.Cells["Estado"].Value.ToString();

                // Cambiamos el texto del botón Guardar a "Actualizar"
                btnGuardar.Text = "Actualizar";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.CurrentRow == null || dgvEstudiantes.CurrentRow.Index < 0)
            {
                MessageBox.Show("Por favor, seleccione un estudiante de la tabla para eliminarlo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show("¿Está seguro de que desea eliminar este estudiante y su calificación?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int idEstudiante = Convert.ToInt32(dgvEstudiantes.CurrentRow.Cells["IdEstudiante"].Value);

                    using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                    {
                        conexion.Open();

                        string queryCalificacion = "DELETE FROM Calificaciones WHERE IdEstudiante = @IdEstudiante;";
                        SqlCommand cmdCal = new SqlCommand(queryCalificacion, conexion);
                        cmdCal.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                        cmdCal.ExecuteNonQuery();

                        string queryEstudiante = "DELETE FROM Estudiantes WHERE IdEstudiante = @IdEstudiante;";
                        SqlCommand cmdEst = new SqlCommand(queryEstudiante, conexion);
                        cmdEst.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                        cmdEst.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Registro eliminado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatosEnTabla();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dgvEstudiantes.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtCarnet.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtNota.Clear();
            cmbEstado.SelectedIndex = -1;
            btnGuardar.Text = "Guardar"; // Regresa el botón a su estado normal
            txtCarnet.Focus();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para procesar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double sumaNotas = 0;
            int totalEstudiantes = 0;
            double notaMayor = 0.0;
            double notaMenor = 10.0;
            int aprobados = 0;
            int reprobados = 0;

            for (int i = 0; i < dgvEstudiantes.Rows.Count; i++)
            {
                if (dgvEstudiantes.Rows[i].Cells["Nota"].Value != null)
                {
                    double notaActual = Convert.ToDouble(dgvEstudiantes.Rows[i].Cells["Nota"].Value);
                    sumaNotas += notaActual;
                    totalEstudiantes++;

                    if (notaActual > notaMayor) notaMayor = notaActual;
                    if (notaActual < notaMenor) notaMenor = notaActual;

                    if (notaActual >= 6.00)
                    {
                        aprobados++;
                    }
                    else
                    {
                        reprobados++;
                    }
                }
            }

            if (totalEstudiantes > 0)
            {
                double promedioGeneral = sumaNotas / totalEstudiantes;

                lblPromedio.Text = $"Promedio General: {promedioGeneral:F2}";
                lblNotaMayor.Text = $"Nota Más Alta: {notaMayor:F2}";
                lblNotaMenor.Text = $"Nota Más Baja: {notaMenor:F2}";
                lblAprobados.Text = $"Aprobados: {aprobados} | Reprobados: {reprobados}";
            }
        }
    }
}