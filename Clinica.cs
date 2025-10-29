using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio
{
    public class Clinica
    {
        public List<Paciente> pacientes = new List<Paciente>();
        public List<Medico> medicos = new List<Medico>();
        public List<Administrativo> administrativos = new List<Administrativo>();
        public List<Consulta> consultas = new List<Consulta>();
        public List<Pago> pagos = new List<Pago>();

        //          ----- PACIENTES -----
        public void AgregarPaciente(Paciente nuevoPaciente)
        {
            if (pacientes.Any(x => x.NumeroDocumento == nuevoPaciente.NumeroDocumento)) throw new ArgumentException("Ya existe un paciente con ese documento.");
            pacientes.Add(nuevoPaciente);
        }
        public List<Paciente> ObtenerPacientes() => pacientes.OrderBy(p => p.Apellido).ToList();

        //          ----- MÉDICOS -----
        public void AgregarMedico(Medico nuevoMedico)
        {
            if (medicos.Any(x => x.Matricula == nuevoMedico.Matricula)) throw new ArgumentException("Ya existe un médico con esa matrícula.");
            medicos.Add(nuevoMedico);
        }
        public List<Medico> ObtenerMedicosPorEspecialidad(string especialidad)
        {
            return medicos.Where(medicoEnLista => medicoEnLista.Especialidad.Equals(especialidad, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public List<Medico> ObtenerMedicosPorDia(string dia)
        {
            return medicos.Where(medicoEnLista => medicoEnLista.DiasAtencion.Contains(dia, StringComparer.OrdinalIgnoreCase)).ToList();
        }
        public List<Medico> ObtenerTodosLosMedicos()
        {
            return new List<Medico>(medicos); // medicos es la lista interna de la clínica
        }


        //          ----- ADMINISTRATIVOS -----
        public void AgregarAdministrativo(Administrativo nuevoAdministrativo)
        {
            if (administrativos.Any(x => x.NumeroDocumento == nuevoAdministrativo.NumeroDocumento)) throw new ArgumentException("Ya existe un administrativo con ese documento.");
            administrativos.Add(nuevoAdministrativo);
        }
        public bool Login(string nombreUsuario, string contrasenia)
        {
            return administrativos.Any(administrativoEnLista => administrativoEnLista.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase) && administrativoEnLista.Contrasenia == contrasenia);
        }

        //          ----- CONSULTAS -----
        public void AgendarConsulta(int idPaciente, int idMedico, DateTime fechaHora)
        {
            // Verifica que el paciente no tenga otra consulta con el mismo medico el mismo dia //
            if (consultas.Any(consulta => consulta.IdPaciente == idPaciente && consulta.IdMedico == idMedico && consulta.FechaHora.Date == fechaHora.Date && consulta.Estado == EstadoConsulta.Agendada)) throw new ArgumentException("El paciente ya tiene una consulta agendada con este médico el mismo día.");

            // Verifica que el medico no tenga otro turno en el mismo horario //
            if (consultas.Any(consulta => consulta.IdMedico == idMedico && consulta.FechaHora == fechaHora && consulta.Estado == EstadoConsulta.Agendada)) throw new ArgumentException("El médico ya tiene un turno en ese horario.");

            // Chequear maximo de 6 turnos por dia para el medico //
            int turnosDia = consultas.Count(consulta => consulta.IdMedico == idMedico && consulta.FechaHora.Date == fechaHora.Date && consulta.Estado == EstadoConsulta.Agendada);
            if (turnosDia >= 6) throw new InvalidOperationException("El médico ya alcanzó el máximo de 6 turnos en ese día.");

            //Crea la consulta //
            Consulta nuevaConsulta = new Consulta(idPaciente, idMedico, fechaHora);
            consultas.Add(nuevaConsulta);
        }
        public void CancelarConsulta(int idConsulta)
        {
            Consulta consultaEncontrada = consultas.FirstOrDefault(consulta => consulta.ID == idConsulta);
            if (consultaEncontrada == null) throw new ArgumentException("No se encontró la consulta.");
            consultaEncontrada.Cancelar();
        }

        public void ReprogramarConsulta(int idConsulta, DateTime nuevaFechaHora)
        {
            Consulta consultaAReprogramar = consultas.FirstOrDefault(consulta => consulta.ID == idConsulta);
            if (consultaAReprogramar == null) throw new ArgumentException("No se encontró la consulta.");

            // Verifica que el paciente no tenga otra consulta con el mismo medico el mismo dia //
            if (consultas.Any(c => c.IdPaciente == consultaAReprogramar.IdPaciente && c.IdMedico == consultaAReprogramar.IdMedico && c.FechaHora.Date == nuevaFechaHora.Date && c.Estado == EstadoConsulta.Agendada && c.ID != idConsulta)) throw new ArgumentException("El paciente ya tiene una consulta agendada con este médico el mismo día.");

            // Verifica que el medico no tenga otro turno en el mismo horario //
            if (consultas.Any(c => c.IdMedico == consultaAReprogramar.IdMedico && c.FechaHora == nuevaFechaHora && c.Estado == EstadoConsulta.Agendada && c.ID != idConsulta)) throw new ArgumentException("El médico ya tiene un turno en ese horario.");

            // Chequear maximo de 6 turnos por dia para el médico //
            int turnosDia = consultas.Count(c => c.IdMedico == consultaAReprogramar.IdMedico && c.FechaHora.Date == nuevaFechaHora.Date && c.Estado == EstadoConsulta.Agendada && c.ID != idConsulta);
            if (turnosDia >= 6) throw new InvalidOperationException("El médico ya alcanzó el máximo de 6 turnos en ese día.");

            consultaAReprogramar.Reprogramar(nuevaFechaHora);
        }
        public List<Consulta> ObtenerConsultasPorPaciente(int idPaciente)
        {
            return consultas.Where(consulta => consulta.IdPaciente == idPaciente).OrderBy(consulta => consulta.FechaHora).ToList();
        }
        public List<Consulta> ObtenerConsultasPorMedico(int idMedico)
        {
            return consultas.Where(consulta => consulta.IdMedico == idMedico).OrderBy(consulta => consulta.FechaHora).ToList();
        }

        //          ----- PAGOS -----
        public void RegistrarPago(int idConsulta, decimal monto, MetodoPago metodo, bool emitirComprobante = true)
        {
            Consulta consultaSeleccionada = consultas.FirstOrDefault(c => c.ID == idConsulta);
            if (consultaSeleccionada == null) throw new ArgumentException("No se encontró la consulta.");
            if (consultaSeleccionada.Estado != EstadoConsulta.Agendada && consultaSeleccionada.Estado != EstadoConsulta.Realizada)
                throw new InvalidOperationException("Solo se puede pagar una consulta agendada o realizada.");

            Pago nuevoPago = new Pago(idConsulta, monto, metodo, DateTime.Now);
            pagos.Add(nuevoPago);

            if (emitirComprobante) // Solo imprime si el usuario lo pidió
                EmitirComprobante(nuevoPago.ID);
        }

        public void EmitirComprobante(int idPago)
        {
            Pago pagoSeleccionado = pagos.FirstOrDefault(p => p.ID == idPago);
            if (pagoSeleccionado == null) throw new ArgumentException("No se encontró el pago.");

            Consulta consultaSeleccionada = consultas.FirstOrDefault(c => c.ID == pagoSeleccionado.IdConsulta);
            if (consultaSeleccionada == null) throw new ArgumentException("No se encontró la consulta asociada al pago.");

            Paciente paciente = pacientes.FirstOrDefault(p => p.ID == consultaSeleccionada.IdPaciente);
            string nombrePaciente = paciente != null ? $"{paciente.Nombre} {paciente.Apellido}" : "Desconocido";
            int idPaciente = consultaSeleccionada != null ? consultaSeleccionada.IdPaciente : 0;

            Console.WriteLine("----- COMPROBANTE DE PAGO -----");
            Console.WriteLine($"ID Pago: {pagoSeleccionado.ID}");
            Console.WriteLine($"Paciente: {nombrePaciente} - ID: {idPaciente}");
            Console.WriteLine($"ID Consulta: {pagoSeleccionado.IdConsulta}");
            Console.WriteLine($"Fecha de pago: {pagoSeleccionado.FechaPago:dd/MM/yyyy}");
            Console.WriteLine($"Monto: ${pagoSeleccionado.Monto:F2}");
            Console.WriteLine($"Método: {pagoSeleccionado.Metodo}");
            Console.WriteLine("-------------------------------");
        }

        public List<Pago> ObtenerPagosPorPaciente(int idPaciente)
        {
            return pagos.Where(pago => consultas.Any(consulta => consulta.ID == pago.IdConsulta && consulta.IdPaciente == idPaciente)).OrderBy(pago => pago.FechaPago).ToList();
        }

        //          ----- MÉTODOS DE RANKING -----

        // Consultas mas frecuentes por especialidad //
        public List<(string Especialidad, int Cantidad)> ObtenerConsultasMasFrecuentesPorEspecialidad()
        {
            var consultasRealizadas = consultas.Where(consulta => consulta.Estado == EstadoConsulta.Realizada);

            var rankingPorEspecialidad = consultasRealizadas.GroupBy(consulta => 
                {
                var medicoAsociado = medicos.FirstOrDefault(medico => medico.ID == consulta.IdMedico);
                return medicoAsociado != null ? medicoAsociado.Especialidad : "Desconocida";
                }).Select(grupo => (Especialidad: grupo.Key, Cantidad: grupo.Count())).OrderByDescending(resultado => resultado.Cantidad).ToList();
            
            return rankingPorEspecialidad;
        }

        // Ranking de medicos mas consultados //
        public List<(Medico Medico, int Cantidad)> ObtenerMedicosMasConsultados()
        {
            var consultasRealizadas = consultas.Where(c => c.Estado == EstadoConsulta.Realizada);

            var rankingPorMedico = consultasRealizadas.GroupBy(c => c.IdMedico).Select(grupo =>
                {
                    var medicoAsociado = medicos.FirstOrDefault(m => m.ID == grupo.Key);
                    return medicoAsociado != null ? (Medico: medicoAsociado, Cantidad: grupo.Count()) : default;
                }).Where(r => r.Medico != null).OrderByDescending(r => r.Cantidad).ToList();

            return rankingPorMedico;
        }

        // Cambio de contraseña del usuario administrativo //

        // Verifica la contraseña
        public bool VerificarContrasenia(string usuario, string contrasenia)
        {
            var admin = administrativos.FirstOrDefault(a => a.NombreUsuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
            return admin != null && admin.Contrasenia == contrasenia;
        }

        // Cambiar contraseña
        public void CambiarContrasenia(string usuario, string nuevaContrasenia)
        {
            var admin = administrativos.FirstOrDefault(a => a.NombreUsuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
            if (admin == null) throw new ArgumentException("Administrativo no encontrado.");
            admin.Contrasenia = nuevaContrasenia;
        }


    }
}
