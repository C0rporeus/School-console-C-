using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

namespace CoreEscuela.app
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc == null)
                throw new ArgumentNullException(nameof(dicObsEsc));
            _diccionario = dicObsEsc;
        }
        public IEnumerable<Evaluacion> GetListaEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluaciones,
                                         out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluacion>();
            }
            {
                return new List<Evaluacion>();
                // escritura log auditorias
            }
        }
        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(
                out var dummy
            );
        }
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluacion> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();
            return (from ev in listaEvaluaciones
                        // where ev.Nota >= 3.0f
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluacion>> GetDicdeEvalxAsignatura()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluacion>>();

            var listaAsig = GetListaAsignaturas(out var listaEvaluaciones);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEvaluaciones
                                where eval.Asignatura.Nombre == asig
                                select eval;

                dicRta.Add(asig, evalsAsig);
            }

            return dicRta;
        }
        public Dictionary<string, IEnumerable<object>> GetpromedioAlumnosXAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();

            var dicEvalxAsig = GetDicdeEvalxAsignatura();

            foreach (var asigConEval in dicEvalxAsig)
            {
                var promediosAlum = from eval in asigConEval.Value
                            group eval by new {
                                eval.Alumno.UniqueId,
                                eval.Alumno.Nombre
                                }
                            into grupoEvalAlumno
                            select new AlumnoPromedio
                            {
                                alumnoid = grupoEvalAlumno.Key.UniqueId,
                                alumnoNombre = grupoEvalAlumno.Key.Nombre,
                                promedio = grupoEvalAlumno.Average(evaluacion => evaluacion.Nota)
                            };

                            rta.Add(asigConEval.Key, promediosAlum);
            }
            return rta;
        }
    }
}