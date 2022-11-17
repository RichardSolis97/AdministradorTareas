namespace Prueba2.Models
{
    public class Cl_Tareas
    {
        public int id { get; set; }
        public string Descripcion { get; set; }
        public int ColaboradorId { get; set; }
        public string Colaborador { get; set; }
        public string Estado { get; set; }
        public string Prioridad { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Notas { get; set; }
    }
}
