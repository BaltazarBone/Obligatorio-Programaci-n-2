using System;
using System.Linq;

namespace Obligatorio
{
    public class Program
    {
        static void Main(string[] args)
        {
            Clinica clinica = new Clinica();
            CargaDeDatosIniciales.Cargar(clinica);

            // ---------- LOGIN ADMINISTRATIVO ----------
            Console.WriteLine("=== Sistema de Gestión Clínica ===");
            Console.WriteLine("Login administrativo:");
            Console.Write("Usuario: ");
            string usuarioIngresado = Console.ReadLine();
            Console.Write("Contraseña: ");
            string contraseniaIngresada = Console.ReadLine();

            if (!clinica.Login(usuarioIngresado, contraseniaIngresada))
            {
                Console.WriteLine("Usuario o contraseña incorrectos. Fin del programa.");
                return;
            }

            Console.WriteLine("Login exitoso.");

            bool salir = false;
            
            while (!salir)
            {
                Console.WriteLine("===== BIENVENIDO AL SISTEMA DE LA CLINICA VIDA SANA =====");
                Console.WriteLine("1. Agregar un nuevo paciente.");
                Console.WriteLine("2. Agregar un nuevo médico.");
                Console.WriteLine("3. Listar pacientes por ID.");
                Console.WriteLine("4. Listar pacientes por orden alfabético.");
                Console.WriteLine("5. Listar médicos por ID.");
                Console.WriteLine("6. Listar médicos por especialidad.");
                Console.WriteLine("7. Listar médicos por día.");
                Console.WriteLine("8. Agendar consulta.");
                Console.WriteLine("9. Cancelar consulta.");
                Console.WriteLine("10. Reprogramar consulta.");
                Console.WriteLine("11. Listar consultas por paciente.");
                Console.WriteLine("12. Listar consultas por médico.");
                Console.WriteLine("13. Registrar pago.");
                Console.WriteLine("14. Listar pagos por paciente.");
                Console.WriteLine("15. Consultas más frecuentes por especialidad.");
                Console.WriteLine("16. Ranking de médicos más consultados.");
                Console.WriteLine("17. Cambiar contraseña.");
                Console.WriteLine("0. Salir.");
                Console.Write("Seleccione una opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            Console.WriteLine("==== ALTA DE PACIENTE ====");

                            Console.Write("Nombre: ");
                            string nombrePac = Console.ReadLine();

                            Console.Write("Apellido: ");
                            string apellidoPac = Console.ReadLine();

                            Console.Write("Correo: ");
                            string emailPac = PedirEmail();
                           
                            Console.Write("Contraseña: ");
                            string contraseniaPac = Console.ReadLine();

                            Console.Write("Número de documento ");
                            string numeroDocumentoPac = PedirNumeroDocumento();

                            Console.Write("Teléfono: ");
                            string telefono = PedirTelefono();

                            DateTime fechaNacimiento;
                            while (true)
                            {
                                Console.Write("Fecha de nacimiento (dd/MM/yyyy): ");
                                string entrada = Console.ReadLine();

                                if (!DateTime.TryParse(entrada, out fechaNacimiento))
                                {
                                    Console.WriteLine("Formato inválido. Use el formato dd/MM/yyyy.");
                                    continue; 
                                }

                                if (fechaNacimiento > DateTime.Today)
                                {
                                    Console.WriteLine("La fecha no puede ser futura.");
                                    continue; 
                                }

                                break;
                            }

                            Console.Write("Obra social (dejar vacío si no tiene): ");
                            string obraSocial = Console.ReadLine();

                            Paciente nuevoPaciente = new Paciente(nombrePac, apellidoPac, emailPac, contraseniaPac, numeroDocumentoPac, telefono, fechaNacimiento, emailPac, obraSocial);

                            try
                            {
                                clinica.AgregarPaciente(nuevoPaciente);
                                Console.WriteLine("Paciente agregado con éxito.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;

                        case 2: 
                            Console.WriteLine("==== ALTA DE MÉDICO ====");

                            Console.Write("Nombre: ");
                            string nombreMed = Console.ReadLine();

                            Console.Write("Apellido: ");
                            string apellidoMed = Console.ReadLine();

                            Console.Write("Correo: ");
                            string emailMed = PedirEmail();

                            Console.Write("Contraseña: ");
                            string contraseniaMed = Console.ReadLine();

                            Console.Write("Especialidad: ");
                            string especialidad = Console.ReadLine();

                            string matricula;
                            while (true)
                            {
                                Console.Write("Matrícula (ej: M000): ");
                                matricula = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(matricula))
                                {
                                    Console.WriteLine("La matrícula no puede estar vacía.");
                                    continue;
                                }
                                if (clinica.ObtenerTodosLosMedicos().Any(m => m.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase)))
                                {
                                    Console.WriteLine("Error: Ya existe un médico con esa matrícula. Ingrese otra.");
                                    continue;
                                }
                                break;
                            }

                            Console.Write("Días de atención: ");
                            List<string> diasAtencion = PedirDiasAtencion();

                            Console.Write("Horarios disponibles: ");
                            List<TimeSpan> horariosDisponibles = PedirHorariosDisponibles();

                            Medico nuevoMedico = new Medico(nombreMed, apellidoMed, emailMed, contraseniaMed, especialidad, matricula, diasAtencion, horariosDisponibles);

                            try
                            {
                                clinica.AgregarMedico(nuevoMedico);
                                Console.WriteLine("Médico agregado con éxito.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                    }
                }
            }
        }
        static string PedirEmail()
        {
            while (true)
            {
                string email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains("."))
                    return email;
                Console.WriteLine("Correo inválido. Debe contener '@' y un dominio.");
            }
        }

        static string PedirNumeroDocumento()
        {
            while (true)
            {
                Console.Write("(ej: 1234567-8): ");
                string documento = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(documento))
                {
                    Console.WriteLine("El número de documento no puede estar vacío.");
                    continue;
                }

                string[] partes = documento.Split('-');
                if (partes.Length != 2)
                {
                    Console.WriteLine("Formato inválido. Debe contener un guion, por ejemplo: 1234567-8");
                    continue;
                }

                string antesGuion = partes[0];
                string despuesGuion = partes[1];

                if (!(antesGuion.Length == 7) || !antesGuion.All(char.IsDigit))
                {
                    Console.WriteLine("Los números antes del guion deben ser 7 dígitos.");
                    continue;
                }

                if (despuesGuion.Length != 1 || !char.IsDigit(despuesGuion[0]))
                {
                    Console.WriteLine("El número después del guion debe ser un solo dígito.");
                    continue;
                }

                return documento;
            }
        }
        static string PedirTelefono()
        {
            while (true)
            {
                string telefono = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(telefono) && telefono.All(char.IsDigit))
                {
                    return telefono;
                }

                Console.WriteLine("Teléfono inválido. Solo se permiten números.");
            }
        }
        static string[] DiasValidos = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };
        static List<string> PedirDiasAtencion()
        {
            List<string> dias = new List<string>();
            Console.WriteLine("(Ingrese los días de atención, uno por uno. Escriba 'fin' para terminar:)");

            while (true)
            {
                string entrada = Console.ReadLine();

                if (entrada.Equals("fin", StringComparison.OrdinalIgnoreCase))
                {
                    if (dias.Count == 0)
                    {
                        Console.WriteLine("Debe ingresar al menos un día de atención.");
                        continue;
                    }
                    break;
                }

                if (DiasValidos.Contains(entrada, StringComparer.OrdinalIgnoreCase))
                {
                    if (!dias.Contains(entrada, StringComparer.OrdinalIgnoreCase))
                    {
                        dias.Add(entrada);
                    }
                    else
                    {
                        Console.WriteLine("Ese día ya fue agregado.");
                    }
                }
                else
                {
                    Console.WriteLine("Día inválido. Ingrese un día válido de la semana.");
                }
            }
            return dias;
        }
        static List<TimeSpan> PedirHorariosDisponibles()
        {
            List<TimeSpan> horarios = new List<TimeSpan>();
            Console.WriteLine("(Ingrese los horarios de atención en formato HH:mm, ej: 09:00). Escriba 'fin' para terminar:");

            while (true)
            {
                string entrada = Console.ReadLine();
                if (entrada.Equals("fin", StringComparison.OrdinalIgnoreCase))
                {
                    if (horarios.Count == 0)
                    {
                        Console.WriteLine("Debe ingresar al menos un horario.");
                        continue;
                    }
                    break;
                }

                if (TimeSpan.TryParse(entrada, out TimeSpan horario))
                {
                    // Validar que sean bloques de 30 minutos
                    if (horario.Minutes % 30 != 0)
                    {
                        Console.WriteLine("El horario debe ser en bloques de 30 minutos (ej: 09:00, 09:30, 10:00...).");
                        continue;
                    }

                    if (!horarios.Contains(horario))
                    {
                        horarios.Add(horario);
                    }
                    else
                    {
                        Console.WriteLine("Ese horario ya fue agregado.");
                    }
                }
                else
                {
                    Console.WriteLine("Formato de horario inválido. Use HH:mm.");
                }
            }
            return horarios;
        }
    }
}
