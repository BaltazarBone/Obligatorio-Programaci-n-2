using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio
{
    public abstract class Persona
    {
        public int ID { get; protected set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        protected Persona(string nombre, string apellido, string nombreUsuario,  string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(apellido)) throw new ArgumentException("El apellido no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(nombreUsuario)) throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(contrasenia)) throw new ArgumentException("La contraseña no puede estar vacía.");

            Nombre = nombre;
            Apellido = apellido;
            NombreUsuario = nombreUsuario;
            Contrasenia = contrasenia;
        }
        public override string ToString()
        {
            return $"ID: {ID} - Nombre y Apellido: {Nombre} {Apellido}";
        }
    }

    public class Paciente : Persona
    {
        private static int contadorID = 1;
        public string NumeroDocumento { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string ObraSocial { get; set; }
        public Paciente(string nombre, string apellido, string nombreUsuario, string contrasenia, string numeroDocumento, string telefono, DateTime fechaNacimiento, string email, string obraSocial) : base (nombre, apellido, nombreUsuario, contrasenia)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento)) throw new ArgumentException("El número de documento no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(telefono) || !telefono.All(char.IsDigit)) throw new ArgumentException("El teléfono solo puede contener números.");
            if (fechaNacimiento > DateTime.Today) throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new ArgumentException("Email inválido.");

            ID = contadorID++;
            NumeroDocumento = numeroDocumento;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Email = email;
            ObraSocial = obraSocial;
        }
        public override string ToString()
        {
            string obraSocialMostrar = string.IsNullOrWhiteSpace(ObraSocial) ? "Sin obra social" : ObraSocial;

            return $"{base.ToString()} (Email: {Email}, Telefono: {Telefono}, Numero de documento: {NumeroDocumento}, Fecha de nacimiento: {FechaNacimiento:dd/MM/yyyy}, Obra social: {obraSocialMostrar})";
        }
    }

    public class Medico : Persona
    {
        private static int contadorID = 1;
        public string Especialidad { get; set; }
        public string Matricula { get; set; }
        public List<string> DiasAtencion {  get; set; } = new List<string>();
        public List<TimeSpan> HorariosDisponibles { get; set; } = new List<TimeSpan>();
        public Medico(string nombre, string apellido, string nombreUsuario, string contrasenia, string especialidad, string matricula, List<string> diasAtencion, List<TimeSpan> horariosDisponibles) : base (nombre, apellido, nombreUsuario, contrasenia)
        {
            if (string.IsNullOrWhiteSpace(especialidad)) throw new ArgumentException("La especialidad no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(matricula)) throw new ArgumentException("La matrícula no puede estar vacía.");
            if (diasAtencion == null || diasAtencion.Count == 0) throw new ArgumentException("Debe ingresar al menos un día de atención.");
            if (horariosDisponibles == null || horariosDisponibles.Count == 0) throw new ArgumentException("Debe ingresar al menos un horario disponible.");

            ID = contadorID++;
            Especialidad = especialidad;
            Matricula = matricula;
            DiasAtencion = diasAtencion;
            HorariosDisponibles = horariosDisponibles;
        }
        public override string ToString()
        {
            string diasDisponibles = string.Join("/ ", DiasAtencion);
            string horariosDisponibles = string.Join("/ ", HorariosDisponibles.Select(h => h.ToString(@"hh\:mm")));

            return $"{base.ToString()} (Especialidad: {Especialidad}, Matrícula: {Matricula}, Días de atención: {diasDisponibles}, Horarios disponibles: {horariosDisponibles})";
        }
    }

    public class Administrativo : Persona
    {
        private static int contadorID = 1;
        public string NumeroDocumento { get; set; }
        public Administrativo(string nombre, string apellido, string nombreUsuario, string contrasenia, string numeroDocumento) : base (nombre, apellido, nombreUsuario, contrasenia)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento)) throw new ArgumentException("El número de documento no puede estar vacío.");

            ID = contadorID++;
            NumeroDocumento = numeroDocumento;
        }

        public override string ToString()
        {
            return $"{base.ToString()} (Número de documento: {NumeroDocumento})";
        }
    }
}
