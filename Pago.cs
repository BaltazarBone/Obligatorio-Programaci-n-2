using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio
{
    public enum MetodoPago
    {
        Efectivo,
        Debito,
        Credito
    }
    public class Pago
    {
        public static int contadorID = 1;
        public int ID { get; private set; }
        public int IdConsulta { get; private set; }
        public DateTime FechaPago { get; private set; }
        public decimal Monto { get; private set; }
        public MetodoPago Metodo { get; private set; }

        public Pago(int idConsulta, decimal monto, MetodoPago metodo, DateTime fechaPago)
        {
            if (monto <= 0) throw new ArgumentException("El monto del pago debe ser mayor a cero.");
            if (fechaPago > DateTime.Now) throw new ArgumentException("La fecha de pago no puede ser futura.");

            ID = contadorID++;
            IdConsulta = idConsulta;
            Monto = monto;
            Metodo = metodo;
            FechaPago = fechaPago;
        }
        public override string ToString()
        {
            return $"Pago {ID} - Consulta: {IdConsulta}, Fecha: {FechaPago:dd/MM/yyyy}, Monto: ${Monto:F2}, Método: {Metodo}";
        }
    }
}
