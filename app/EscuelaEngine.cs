using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.util;

namespace CoreEscuela.app
{
    // sealed es sellada instancias sin que puedan heredar es decir que otra clase pueda usarla heredando atributos de ella
    public sealed class EscuelaEngine
    {
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academay", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();

        }

        #region Diccionarios
        public void ImprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dic,
            bool imprimirEval = false)
        {
            foreach (var objdic in dic)
            {
                Printer.Title($"{objdic.Key}");

                foreach (var val in objdic.Value)
                {
                    switch (objdic.Key)
                    {
                        case LlaveDiccionario.Evaluaciones:
                            if (imprimirEval)
                                Console.WriteLine(val);
                            break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + val);
                            break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine("Alumno: " + val.Nombre);
                            break;
                        case LlaveDiccionario.Curso:
                            var cursotmp = val as Curso;
                            if (cursotmp != null)
                            {
                                int count = ((Curso)val).Alumnos.Count;
                                Console.WriteLine("Curso: " + val.Nombre + " Cantidad Alumnos: " + count);
                            }
                            break;
                        default:
                            Console.WriteLine(val);
                            break;

                    }
                }
            }
        }
        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioOjetos()
        {

            var diccionario = new Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>>();

            diccionario.Add(LlaveDiccionario.Escuela, new[] { Escuela }); // con el enumerador ya no se puede hardcodear con string llaves de objetos
            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());

            var listTmpEv = new List<Evaluacion>();
            var listTmpAs = new List<Asignatura>();
            var listTmpAl = new List<Alumno>();


            foreach (var cur in Escuela.Cursos)
            {
                listTmpAs.AddRange(cur.Asignaturas);
                listTmpAl.AddRange(cur.Alumnos);

                foreach (var alum in cur.Alumnos)

                {
                    listTmpEv.AddRange(alum.Evaluacion);
                }

            }

            diccionario.Add(LlaveDiccionario.Evaluaciones,
                            listTmpEv.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Asignaturas,
                            listTmpAs.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Alumno,
                            listTmpAl.Cast<ObjetoEscuelaBase>());
            return diccionario;
        }
        #endregion

        #region Metodos de Carga
        private void CargarEvaluaciones()
        {
            var rnd = new Random();
            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluacion
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                Nota = MathF.Round(5 * (float)rnd.NextDouble(), 2),
                                Alumno = alumno
                            };
                            alumno.Evaluacion.Add(ev);
                        }
                    }
                }
            }

        }
        // sobre carga de metodos
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
                    out int conteoEvaluaciones,
                    bool traeCursos = true,
                    bool traeAsignaturas = true,
                    bool traeAlumnos = true,
                    bool traeEvaluaciones = true
                )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out int dummy, out dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
                   out int conteoEvaluaciones,
                   out int conteoAlumnos,
                   bool traeCursos = true,
                   bool traeAsignaturas = true,
                   bool traeAlumnos = true,
                   bool traeEvaluaciones = true
               )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoAlumnos, out int dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
                   out int conteoEvaluaciones,
                   out int conteoAlumnos,
                   out int conteoAsignaturas,
                   bool traeCursos = true,
                   bool traeAsignaturas = true,
                   bool traeAlumnos = true,
                   bool traeEvaluaciones = true
               )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoAlumnos, out conteoAsignaturas, out int dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoAlumnos,
            out int conteoAsignaturas,
            out int conteoCursos,
            bool traeCursos = true,
            bool traeAsignaturas = true,
            bool traeAlumnos = true,
            bool traeEvaluaciones = true
        )
        {
            conteoAlumnos = conteoAsignaturas = conteoEvaluaciones = 0;

            var listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);

            if (traeCursos)
                listaObj.AddRange(Escuela.Cursos);

            conteoCursos = Escuela.Cursos.Count;
            foreach (var curso in Escuela.Cursos)
            {
                conteoAsignaturas += curso.Asignaturas.Count;
                conteoAlumnos += curso.Alumnos.Count;
                if (traeAsignaturas)
                    listaObj.AddRange(curso.Asignaturas);

                if (traeAlumnos)
                    listaObj.AddRange(curso.Alumnos);

                if (traeEvaluaciones)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluacion);
                        conteoEvaluaciones += alumno.Evaluacion.Count;
                    }
                }
            }

            return listaObj.AsReadOnly();
        }

        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion
}