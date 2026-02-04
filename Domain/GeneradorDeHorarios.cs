namespace PlanificadorDeHorarios.Api.Domain
{
    public class GeneradorDeHorarios
    {
        List<Horario> horariosGenerados;

        public GeneradorDeHorarios()
        {
            this.horariosGenerados = new();
        }

        public List<Horario> generar(List<Materia> materias)
        {
            backtrack(0, materias, new(), new());
            return horariosGenerados;
        }

        public void backtrack(int index, List<Materia> materias, List<BloqueHorario> bloquesOcupados, Dictionary<String, Aula> seleccion)
        {
            if(index == materias.Count)
            {
                horariosGenerados.Add(new(seleccion));
                return;
            }

            Materia materia = materias[index];

            foreach (var aula in materia.OpcionesDeAula)
            {
                if (!HayConflicto(aula, bloquesOcupados))
                {
                    seleccion.Add(materia.Nombre, aula);
                    foreach(var bloque in aula.Bloques)
                    {
                        bloquesOcupados.Add(bloque);
                    }

                    backtrack(index + 1, materias, bloquesOcupados, seleccion);
                    seleccion.Remove(materia.Nombre);
                    foreach(var bloque in aula.Bloques)
                    {
                        bloquesOcupados.RemoveAt(bloquesOcupados.Count - 1);
                    }
                }
            }
        }

        private bool HayConflicto(Aula aula, List<BloqueHorario> bloques)
        {
            foreach(var bloque1 in aula.Bloques)
            {
                foreach(var bloque2 in bloques)
                {
                    if (bloque1.InterfiereCon(bloque2)) return true;
                }
            }
            return false;
        }
    }
}
