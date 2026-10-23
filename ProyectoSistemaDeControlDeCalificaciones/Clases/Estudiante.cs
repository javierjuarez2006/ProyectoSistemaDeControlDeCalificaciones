using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSistemaDeControlDeCalificaciones
{
    public class Estudiante
    {
        // Propiedades que coinciden con la tabla "Estudiantes" de la Base de Datos
        public int IdEstudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Carnet { get; set; }

        // Constructor vacío
        public Estudiante()
        {
        }

        // Constructor con parámetros para instanciar objetos fácilmente si los necesitas luego
        public Estudiante(int idEstudiante, string nombre, string apellido, string carnet)
        {
            IdEstudiante = idEstudiante;
            Nombre = nombre;
            Apellido = apellido;
            Carnet = carnet;
        }
    }
}