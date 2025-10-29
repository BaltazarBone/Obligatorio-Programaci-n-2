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
            Console.WriteLine("===== Sistema de Gestión de la Clínica Vida Sana =====");
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
                Console.WriteLine("18. Ver comprobantes de pago.");
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

                        case 3:
                            Console.WriteLine("==== LISTADO DE PACIENTES POR ID ====");
                            ListarPacientesPorID(clinica);
                            break;

                        case 4:
                            Console.WriteLine("==== LISTADO DE PACIENTES POR ORDEN ALFABÉTICO ====");
                            ListarPacientesAlfabeticamente(clinica);
                            break;

                        case 5:
                            Console.WriteLine("==== LISTADO DE MÉDICOS POR ID ====");
                            ListarMedicosPorID(clinica);
                            break;

                        case 6:
                            Console.WriteLine("==== LISTADO DE MÉDICOS POR ESPECIALIDAD ====");
                            ListarMedicosPorEspecialidad(clinica);
                            break;

                        case 7:
                            Console.WriteLine("==== LISTADO DE MÉDICOS POR DÍA ====");
                            ListarMedicosPorDia(clinica);
                            break;

                        case 8:
                            Console.WriteLine("==== AGENDAR CONSULTA ====");

                            // Listar pacientes para que elijan
                            ListarPacientesPorID(clinica);
                            Console.Write("Ingrese el ID del paciente: ");
                            int idPaciente = int.Parse(Console.ReadLine());

                            // Listar médicos
                            ListarMedicosPorID(clinica);
                            Console.Write("Ingrese el ID del médico: ");
                            int idMedico = int.Parse(Console.ReadLine());

                            // Ingresar fecha y hora
                            DateTime fechaHora;
                            while (true)
                            {
                                Console.Write("Ingrese la fecha y hora (dd/MM/yyyy HH:mm): ");
                                if (DateTime.TryParse(Console.ReadLine(), out fechaHora))
                                {
                                    break;
                                }
                                Console.WriteLine("Formato inválido. Intente nuevamente.");
                            }

                            try
                            {
                                clinica.AgendarConsulta(idPaciente, idMedico, fechaHora);
                                Console.WriteLine("Consulta agendada con éxito.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;

                        case 9:
                            Console.WriteLine("==== CANCELAR CONSULTA ====");

                            // Mostrar consultas agendadas
                            var consultasAgendadas = clinica.consultas.Where(c => c.Estado == EstadoConsulta.Agendada).OrderBy(c => c.FechaHora).ToList();

                            if (consultasAgendadas.Count == 0)
                            {
                                Console.WriteLine("No hay consultas agendadas.");
                                break;
                            }

                            Console.WriteLine("Consultas agendadas:");
                            foreach (var consulta in consultasAgendadas)
                            {
                                Console.WriteLine(consulta.ToString());
                            }

                            Console.Write("Ingrese el ID de la consulta a cancelar: ");
                            if (int.TryParse(Console.ReadLine(), out int idConsulta))
                            {
                                try
                                {
                                    clinica.CancelarConsulta(idConsulta);
                                    Console.WriteLine("Consulta cancelada con éxito.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("ID inválido.");
                            }
                            break;

                        case 10: 
                            {
                                Console.WriteLine("==== REPROGRAMAR CONSULTA ====");

                                // Mostrar todas las consultas
                                if (clinica.consultas.Count == 0)
                                {
                                    Console.WriteLine("No hay consultas registradas.");
                                    break;
                                }

                                Console.WriteLine("Consultas existentes:");
                                foreach (var consulta in clinica.consultas)
                                {
                                    Console.WriteLine(consulta.ToString());
                                }

                                // Pedir ID de la consulta
                                int idConsultaReprogramar;
                                while (true)
                                {
                                    Console.Write("Ingrese el ID de la consulta a reprogramar: ");
                                    if (int.TryParse(Console.ReadLine(), out idConsultaReprogramar) && clinica.consultas.Any(c => c.ID == idConsultaReprogramar))
                                        break;
                                    Console.WriteLine("ID inválido. Intente nuevamente.");
                                }

                                // Pedir nueva fecha y hora
                                DateTime nuevaFechaHora;
                                while (true)
                                {
                                    Console.Write("Ingrese la nueva fecha y hora (dd/MM/yyyy HH:mm): ");
                                    if (DateTime.TryParse(Console.ReadLine(), out nuevaFechaHora))
                                        break;
                                    Console.WriteLine("Formato inválido. Use dd/MM/yyyy HH:mm");
                                }

                                // Intentar reprogramar
                                try
                                {
                                    clinica.ReprogramarConsulta(idConsultaReprogramar, nuevaFechaHora);
                                    Console.WriteLine("Consulta reprogramada con éxito.");

                                    // Mostrar la consulta actualizada
                                    var consultaActualizada = clinica.consultas.First(c => c.ID == idConsultaReprogramar);
                                    Console.WriteLine(consultaActualizada.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }

                                Console.WriteLine("Presione ENTER para continuar...");
                                Console.ReadLine();
                                break;
                            }
                        case 11: 
                            Console.WriteLine("==== LISTAR CONSULTAS POR PACIENTE ====");

                            // Primero listar pacientes para que elijan
                            ListarPacientesPorID(clinica);

                            Console.Write("Ingrese el ID del paciente: ");
                            if (int.TryParse(Console.ReadLine(), out int idPacienteSeleccionado))
                            {
                                var consultasDelPaciente = clinica.ObtenerConsultasPorPaciente(idPacienteSeleccionado);

                                if (consultasDelPaciente.Count == 0)
                                {
                                    Console.WriteLine("El paciente no tiene consultas registradas.");
                                }
                                else
                                {
                                    Console.WriteLine($"Consultas del paciente con ID {idPacienteSeleccionado}:");
                                    foreach (var consulta in consultasDelPaciente)
                                    {
                                        Console.WriteLine(consulta.ToString());
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("ID inválido.");
                            }
                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 12: 
                            Console.WriteLine("==== LISTAR CONSULTAS POR MÉDICO ====");

                            // Listar médicos para que elijan
                            ListarMedicosPorID(clinica);

                            Console.Write("Ingrese el ID del médico: ");
                            if (int.TryParse(Console.ReadLine(), out int idMedicoSeleccionado))
                            {
                                var consultasDelMedico = clinica.ObtenerConsultasPorMedico(idMedicoSeleccionado);

                                if (consultasDelMedico.Count == 0)
                                {
                                    Console.WriteLine("El médico no tiene consultas registradas.");
                                }
                                else
                                {
                                    Console.WriteLine($"\nConsultas del médico con ID {idMedicoSeleccionado}:");
                                    foreach (var consulta in consultasDelMedico)
                                    {
                                        Console.WriteLine(consulta.ToString());
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("ID inválido.");
                            }

                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 13:
                            Console.WriteLine("==== REGISTRAR PAGO ====");

                            // Mostrar consultas agendadas o realizadas
                            var consultasPagables = clinica.consultas.Where(c => c.Estado == EstadoConsulta.Agendada || c.Estado == EstadoConsulta.Realizada).OrderBy(c => c.FechaHora).ToList();

                            if (consultasPagables.Count == 0)
                            {
                                Console.WriteLine("No hay consultas disponibles para pagar.");
                                break;
                            }

                            Console.WriteLine("Consultas disponibles para pago:");
                            foreach (var consulta in consultasPagables)
                            {
                                Console.WriteLine(consulta.ToString());
                            }

                            // Pedir ID de la consulta a pagar
                            int idConsultaPago;
                            while (true)
                            {
                                Console.Write("Ingrese el ID de la consulta a pagar: ");
                                if (int.TryParse(Console.ReadLine(), out idConsultaPago) && consultasPagables.Any(c => c.ID == idConsultaPago))
                                    break;
                                Console.WriteLine("ID inválido. Intente nuevamente.");
                            }

                            // Pedir monto
                            decimal montoPago;
                            while (true)
                            {
                                Console.Write("Ingrese el monto a pagar: ");
                                if (decimal.TryParse(Console.ReadLine(), out montoPago) && montoPago > 0)
                                    break;
                                Console.WriteLine("Monto inválido. Debe ser un número mayor a 0.");
                            }

                            // Pedir método de pago
                            Console.WriteLine("Seleccione el método de pago:");
                            foreach (var metodo in Enum.GetValues(typeof(MetodoPago)))
                            {
                                Console.WriteLine($"{(int)metodo} - {metodo}");
                            }

                            MetodoPago metodoSeleccionado;
                            while (true)
                            {
                                Console.Write("Ingrese el número del método de pago: ");
                                if (int.TryParse(Console.ReadLine(), out int metodoInt) && Enum.IsDefined(typeof(MetodoPago), metodoInt))
                                {
                                    metodoSeleccionado = (MetodoPago)metodoInt;
                                    break;
                                }
                                Console.WriteLine("Método inválido. Intente nuevamente.");
                            }

                            try
                            {
                                clinica.RegistrarPago(idConsultaPago, montoPago, metodoSeleccionado);
                                Console.WriteLine("Pago registrado con éxito.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }

                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 14: 
                            Console.WriteLine("==== LISTAR PAGOS POR PACIENTE ====");

                            // Mostrar todos los pacientes
                            ListarPacientesPorID(clinica);

                            // Pedir ID del paciente
                            int idPacientePago;
                            while (true)
                            {
                                Console.Write("Ingrese el ID del paciente: ");
                                if (int.TryParse(Console.ReadLine(), out idPacientePago) && clinica.pacientes.Any(p => p.ID == idPacientePago))
                                    break;
                                Console.WriteLine("ID inválido. Intente nuevamente.");
                            }

                            // Obtener todas las consultas del paciente
                            var consultasPaciente = clinica.ObtenerConsultasPorPaciente(idPacientePago);

                            // Obtener pagos asociados a esas consultas
                            var pagosPaciente = clinica.pagos.Where(p => consultasPaciente.Any(c => c.ID == p.IdConsulta)).OrderBy(p => p.FechaPago).ToList();

                            if (pagosPaciente.Count == 0)
                            {
                                Console.WriteLine("El paciente no tiene pagos registrados.");
                            }
                            else
                            {
                                Console.WriteLine($"Pagos del paciente {clinica.pacientes.First(p => p.ID == idPacientePago).Nombre} {clinica.pacientes.First(p => p.ID == idPacientePago).Apellido}:");
                                foreach (var pago in pagosPaciente)
                                {
                                    // Mostramos también la consulta asociada
                                    var consulta = consultasPaciente.First(c => c.ID == pago.IdConsulta);
                                    Console.WriteLine($"Pago ID: {pago.ID}, Monto: {pago.Monto:C}, Método: {pago.Metodo}, Fecha: {pago.FechaPago}, Consulta: {consulta.FechaHora:dd/MM/yyyy HH:mm}");
                                }
                            }
                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 15:
                            Console.WriteLine("==== CONSULTAS MÁS FRECUENTES POR ESPECIALIDAD ====");
                            var rankingEspecialidades = clinica.ObtenerConsultasMasFrecuentesPorEspecialidad();

                            if (rankingEspecialidades.Count == 0)
                            {
                                Console.WriteLine("No hay consultas realizadas aún.");
                            }
                            else
                            {
                                Console.WriteLine("Especialidad - Cantidad de consultas");
                                foreach (var item in rankingEspecialidades)
                                {
                                    Console.WriteLine($"{item.Especialidad} - {item.Cantidad}");
                                }
                            }

                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 16: 
                            Console.WriteLine("==== RANKING DE MÉDICOS MÁS CONSULTADOS ====");

                            var rankingMedicos = clinica.ObtenerMedicosMasConsultados();

                            if (rankingMedicos.Count == 0)
                            {
                                Console.WriteLine("Aún no hay consultas realizadas.");
                            }
                            else
                            {
                                int posicion = 1;
                                foreach (var item in rankingMedicos)
                                {
                                    Console.WriteLine($"{posicion}. {item.Medico.Nombre} {item.Medico.Apellido} - Especialidad: {item.Medico.Especialidad}, Consultas realizadas: {item.Cantidad}");
                                    posicion++;
                                }
                            }

                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 17: // Cambiar contraseña
                            Console.WriteLine("==== CAMBIAR CONTRASEÑA ====");

                            Console.Write("Ingrese la contraseña actual: ");
                            string contraseniaActual = Console.ReadLine();

                            // Verifica que la contraseña ingresada coincida con la del administrativo en uso
                            if (!clinica.VerificarContrasenia(usuarioIngresado, contraseniaActual))
                            {
                                Console.WriteLine("Contraseña actual incorrecta.");
                                break;
                            }

                            Console.Write("Ingrese la nueva contraseña: ");
                            string nuevaContrasenia = Console.ReadLine();

                            Console.Write("Confirme la nueva contraseña: ");
                            string confirmacion = Console.ReadLine();

                            if (nuevaContrasenia != confirmacion)
                            {
                                Console.WriteLine("La confirmación no coincide con la nueva contraseña.");
                                break;
                            }

                            try
                            {
                                clinica.CambiarContrasenia(usuarioIngresado, nuevaContrasenia);
                                Console.WriteLine("Contraseña actualizada con éxito.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }

                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
                            break;

                        case 18:
                            Console.WriteLine("==== COMPROBANTES DE PAGO ====");

                            Console.Write("Ingrese el ID del pago a emitir: ");
                            int idPago = int.Parse(Console.ReadLine());
                            try
                            {
                                clinica.EmitirComprobante(idPago);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;


                        case 0:
                            Console.WriteLine("Saliendo del sistema. ¡Hasta luego!");
                            salir = true;
                            break;

                        default:
                            Console.WriteLine("Opción inválida. Intente nuevamente.");
                            Console.WriteLine("Presione ENTER para continuar...");
                            Console.ReadLine();
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

                if (antesGuion.Length != 7 || !antesGuion.All(char.IsDigit))
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
        static void ListarPacientesPorID(Clinica clinica)
        {
            var pacientesOrdenados = clinica.pacientes.OrderBy(p => p.ID).ToList();

            foreach (var paciente in pacientesOrdenados)
            {
                Console.WriteLine($"ID: {paciente.ID}, Nombre: {paciente.Nombre} {paciente.Apellido}, Fecha de nacimiento: {paciente.FechaNacimiento:dd/MM/yyyy}, Número de documento: {paciente.NumeroDocumento}, Telefono: {paciente.Telefono}, Email: {paciente.Email}, Obra social: {paciente.ObraSocial}");
            }
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();  // <--- pausa
        }
        static void ListarPacientesAlfabeticamente(Clinica clinica)
        {
            var pacientesOrdenados = clinica.pacientes.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToList();

            foreach (var paciente in pacientesOrdenados)
            {
                Console.WriteLine($"ID: {paciente.ID}, Nombre: {paciente.Nombre} {paciente.Apellido}, Fecha de nacimiento: {paciente.FechaNacimiento:dd/MM/yyyy}, Número de documento: {paciente.NumeroDocumento}, Telefono: {paciente.Telefono}, Email: {paciente.Email}, Obra social: {paciente.ObraSocial}");
            }

            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }
        static void ListarMedicosPorID(Clinica clinica)
        {
            var medicosOrdenados = clinica.medicos.OrderBy(m => m.ID).ToList();

            foreach (var medico in medicosOrdenados)
            {
                Console.WriteLine(medico.ToString());
            }

            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }
        static void ListarMedicosPorEspecialidad(Clinica clinica)
        {
            Console.Write("Ingrese la especialidad a buscar: ");
            string especialidadBuscada = Console.ReadLine()?.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n");

            var medicosFiltrados = clinica.medicos.Where(m => m.Especialidad.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n").Contains(especialidadBuscada)).OrderBy(m => m.ID).ToList();

            if (medicosFiltrados.Count == 0)
            {
                Console.WriteLine($"No se encontraron médicos con la especialidad '{especialidadBuscada}'.");
            }
            else
            {
                Console.WriteLine($"Médicos con especialidad '{especialidadBuscada}':");
                foreach (var medico in medicosFiltrados)
                {
                    Console.WriteLine(medico.ToString());
                }
            }
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }
        static void ListarMedicosPorDia(Clinica clinica)
        {
            Console.Write("Ingrese el día de atención: ");
            string diaBuscado = Console.ReadLine()?.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n");

            // Lista de días válidos normalizados
            string[] diasValidos = { "lunes", "martes", "miercoles", "jueves", "viernes", "sabado", "domingo" };

            if (!diasValidos.Contains(diaBuscado))
            {
                Console.WriteLine("Día inválido. Ingrese un día válido de la semana.");
                return;
            }

            var medicosFiltrados = clinica.medicos.Where(m => m.DiasAtencion.Select(d => d.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n")).Contains(diaBuscado)).OrderBy(m => m.ID).ToList();

            if (medicosFiltrados.Count == 0)
            {
                Console.WriteLine($"No se encontraron médicos que atiendan el día '{diaBuscado}'.");
            }
            else
            {
                Console.WriteLine($"Médicos que atienden el día '{diaBuscado}':");
                foreach (var medico in medicosFiltrados)
                {
                    Console.WriteLine(medico.ToString());
                }
            }
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}