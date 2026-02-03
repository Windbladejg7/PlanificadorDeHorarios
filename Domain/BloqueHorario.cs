namespace PlanificadorDeHorarios.Api.Domain
{
    public class BloqueHorario
    {
        public string Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public bool InterfiereCon(BloqueHorario bloque)
        {
            if (!MismoDia(bloque.Dia)) return false;
            if (HorasNoIncluidas(bloque)) return false;

            return true;
        }

        private bool MismoDia(string dia)
        {
            return this.Dia.Equals(dia);
        }

        private bool HorasNoIncluidas(BloqueHorario bloque)
        {
            return HoraInicio >= bloque.HoraFin || HoraFin <= bloque.HoraInicio;
        }

        public override string ToString()
        {
            return $"{Dia}, {HoraInicio} - {HoraFin}";
        }
    }
}
