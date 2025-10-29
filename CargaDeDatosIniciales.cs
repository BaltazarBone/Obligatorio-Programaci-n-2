using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio
{
    public static class CargaDeDatosIniciales
    {
        public static void Cargar(Clinica clinica)
        {
            //          ----- MÉDICOS -----
            Medico medico1 = new Medico("Francisco", "Arenas", "franarenas@clinicaVS.com", "1234", "Médico General", "M001", new List<string> { "Lunes", "Miércoles" }, new List<TimeSpan> { new TimeSpan(9, 0, 0), new TimeSpan(9, 30, 0) });

            Medico medico2 = new Medico("Megan", "Avila", "megan.avila@clinicaVS.com", "1234", "Neuróloga", "M002", new List<string> { "Martes", "Jueves" }, new List<TimeSpan> { new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0) });

            Medico medico3 = new Medico("Baltazar", "Boné", "baltazarbone@clinicaVS.com", "1234", "Dermatología", "M003", new List<string> { "Lunes", "Viernes" }, new List<TimeSpan> { new TimeSpan(11, 0, 0), new TimeSpan(11, 30, 0) });

            Medico medico4 = new Medico("Fausto", "Clara", "clarafausto@clinicaVS.com", "1234", "Cardiología", "M004", new List<string> { "Miércoles", "Jueves" }, new List<TimeSpan> { new TimeSpan(14, 0, 0), new TimeSpan(14, 30, 0) });

            Medico medico5 = new Medico("Carlos", "Rodriguez", "crodriguez@clinicaVS.com", "1234", "Odontología", "M005", new List<string> { "Martes", "Viernes" }, new List<TimeSpan> { new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) });

            clinica.AgregarMedico(medico1);
            clinica.AgregarMedico(medico2);
            clinica.AgregarMedico(medico3);
            clinica.AgregarMedico(medico4);
            clinica.AgregarMedico(medico5);

            //          ----- PACIENTES -----
            Paciente paciente1 = new Paciente("María", "Rodríguez", "mariarodriguez@gmail.com", "1234", "5634406-4", "099111111", new DateTime(1990, 5, 12), "mariarodriguez@gmail.com", "ORAMECO");
            Paciente paciente2 = new Paciente("Andres", "Klett", "klettandres@gmail.com", "1234", "2935406-4", "099222222", new DateTime(1985, 8, 23), "klettandres@gmail.com", "");
            Paciente paciente3 = new Paciente("Lucas", "García", "lucasgarcia@gmail.com", "1234", "5036406-1", "099333333", new DateTime(2000, 2, 15), "lucasgarcia@gmail.com", "ORAMECO");
            Paciente paciente4 = new Paciente("Diego", "Díaz", "diediaz@gmail.com", "1234", "5234306-9", "099444444", new DateTime(1978, 11, 5), "diediaz@gmail.com", "");
            Paciente paciente5 = new Paciente("Leopoldo", "Torres", "leotorres@gmail.com", "1234", "5133868-1", "099555555", new DateTime(1995, 7, 30), "leotorres@gmail.com", "BPS");
            Paciente paciente6 = new Paciente("Martin", "Bidart", "tinchobidart@gmail.com", "1234", "5333333-9", "099666666", new DateTime(1982, 3, 18), "tinchobidart@gmail.com", "BPS");
            Paciente paciente7 = new Paciente("Leo", "Messi", "goat@gmail.com", "1234", "3434343-4", "099777777", new DateTime(1998, 12, 9), "goat@gmail.com", "BPS");
            Paciente paciente8 = new Paciente("Neymar", "Junior", "elprincipe@gmail.com", "1234", "5130003-0", "099888888", new DateTime(1993, 6, 21), "elprincipe@gmail.com", "");
            Paciente paciente9 = new Paciente("Pancho", "Vega", "vegapancho@gmail.com", "1234", "3256383-9", "099999999", new DateTime(2001, 9, 2), "vegapancho@gmail.com", "ORAMECO");
            Paciente paciente10 = new Paciente("Martin", "Damonte", "deimontmartin@gmail.com", "1234", "5391247-7", "099101010", new DateTime(1988, 4, 10), "deimontmartin@gmail.com", "ORAMECO");

            clinica.AgregarPaciente(paciente1);
            clinica.AgregarPaciente(paciente2);
            clinica.AgregarPaciente(paciente3);
            clinica.AgregarPaciente(paciente4);
            clinica.AgregarPaciente(paciente5);
            clinica.AgregarPaciente(paciente6);
            clinica.AgregarPaciente(paciente7);
            clinica.AgregarPaciente(paciente8);
            clinica.AgregarPaciente(paciente9);
            clinica.AgregarPaciente(paciente10);

            //          ----- ADMINISTRATIVOS -----
            Administrativo administrativo1 = new Administrativo("Prueba", "Prueba", "prueba@clinicaVS.com", "1234", "4932136-2");
            Administrativo administrativo2 = new Administrativo("Marta", "Machín", "martamachin@clinicaVS.com", "1234", "2824308-1");

            clinica.AgregarAdministrativo(administrativo1);
            clinica.AgregarAdministrativo(administrativo2);

            //          ----- CONSULTAS -----
            clinica.AgendarConsulta(paciente1.ID, medico1.ID, DateTime.Today.AddDays(2).AddHours(9));
            clinica.AgendarConsulta(paciente2.ID, medico2.ID, DateTime.Today.AddDays(2).AddHours(10));
            clinica.AgendarConsulta(paciente3.ID, medico3.ID, DateTime.Today.AddDays(3).AddHours(11));
            clinica.AgendarConsulta(paciente4.ID, medico4.ID, DateTime.Today.AddDays(3).AddHours(14));
            clinica.AgendarConsulta(paciente5.ID, medico5.ID, DateTime.Today.AddDays(4).AddHours(15));

            //          ----- PAGOS -----
            clinica.RegistrarPago(1, 1500m, MetodoPago.Efectivo);
            clinica.RegistrarPago(3, 2000m, MetodoPago.Debito);
        }
    }
}
