using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.util;
using CoreEscuela.app;
using static System.Console;
namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            // evento
            AppDomain.CurrentDomain.ProcessExit += AcciondelEvento;

            AppDomain.CurrentDomain.ProcessExit += (o, s) => Printer.Beep(100, 1000, 1);

            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.Title("Bienvenidos");
            var reporteador = new Reporteador(engine.GetDiccionarioOjetos());
            var evalList = reporteador.GetListaEvaluaciones();
            var asigList = reporteador.GetListaAsignaturas();
            var listEvaluaCAsignat = reporteador.GetDicdeEvalxAsignatura();
            var listPromedios = reporteador.GetpromedioAlumnosXAsignatura();

            Printer.Title("Reportes: ");
            var newEval = new Evaluacion();
            string nombre, notaString;


            Console.WriteLine("Escriba su nombre y ");
            Printer.PresioneEnter();
            nombre = Console.ReadLine();

            if (string.IsNullOrEmpty(nombre))
            {
                Printer.Title("El Valor del nombre no puede ser vacio");
                Console.WriteLine("Saliendo del programa");
            }
            else
            {
                try
                {
                    newEval.Nombre = nombre.ToLower();
                    WriteLine("El nombre de la evaluacion ha sido ingresado!");
                }
                catch
                {
                    Printer.Title("El Valor del nombre no puede ser vacio");
                    Console.WriteLine("Saliendo del programa");
                }
            }

            Console.WriteLine("Escriba la nota de la evaluacion ");
            Printer.PresioneEnter();
            notaString = Console.ReadLine();

            if (string.IsNullOrEmpty(notaString))
            {
                Printer.Title("El Valor de la nota no puede ser vacio");
                Console.WriteLine("Saliendo del programa");
            }
            else
            {
                try
                {
                    newEval.Nota = float.Parse(notaString);
                    if(newEval.Nota <0 || newEval.Nota > 5)
                    {
                        throw new ArgumentOutOfRangeException("La nota debe estar entre 0 y 5...");
                    }
                    WriteLine("La nota ha sido ingresada!");
                }
                catch (ArgumentOutOfRangeException arge)
                {
                    Printer.Title(arge.Message);
                    Console.WriteLine("Saliendo del programa");
                }
                catch(Exception)
                {

                    Printer.Title("El Valor de la nota no puede ser vacio");
                    Console.WriteLine("Saliendo del programa");
                }
                finally
                {
                    Printer.Title("Finalmente");
                    Printer.Beep(5000, 1000, 3);
                }
            }

            Dictionary<int, string> dicccionario = new Dictionary<int, string>();

            dicccionario.Add(10, "Juan");

            dicccionario.Add(11, "Yonathan");

            foreach (var keyValPair in dicccionario)
            {
                WriteLine($"Key: {keyValPair.Key} Valor: {keyValPair.Value}");
            }

            var dictmp = engine.GetDiccionarioOjetos();

            engine.ImprimirDiccionario(dictmp, true);
            /* Printer.Title("Acceso al diccionario");
            dicccionario[0] = "Pekerman";
            WriteLine(dicccionario[0]); // accediendo al valor por medio de la llave dentro del diccionario
            Printer.Title("Otro diccionario");

            var dic = new Dictionary<string, string>();

            dic["luna"] = "satelite de la tierra";
            WriteLine(dic["luna"]); */
            // dic.Add("luna", "personaje de tv");
            // WriteLine(dic["luna"]); // en este caso no sirve porque una cosa es remplazar el valor de la llave y otra adicionar una llave igual...
        }

        private static void AcciondelEvento(object sender, EventArgs e)
        {
            Printer.Title("Saliendo");
            // Printer.Beep(1000, 1000, 5);
            Printer.Title("Termino");
        }

        private static int Predicadomal(Curso curobj) // al ser una condicion segura con parametros booleanos, este predicate no funcionaria o daria error
        {
            return 301;
        }
        // este predicado se remplaza con el delegade y lamda que anexa la misma informacion pero de forma mas compacta en el codigo
        private static bool Predicado(Curso curobj) // para el caso de recorrido de listas se necesita una especie de parametrizador 
        {                                           // para recorreer la lista o coleccion y tomar los elementos o criterios a ejecutar, para el caso del ejemplo eliminarTodo lo que cumpla
            return curobj.Nombre == "301";          // el criterio 301 como string en este caso el nombre de cursos que cumpla con ese criterio
        }

        private static void ImprimirCursosEscuela(Escuela escuela)
        {
            // Clase estatica del util
            Printer.Title("Lista cursos de la escuela");
            // control de data para evitar crash inesperador... (array vacios, asignaciones a null...)
            if (escuela?.Cursos != null) // tambien podria hacerlo sin el if con != null, es decir diferente a nulo...
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre}, Id {curso.UniqueId}");
                }
            }
        }

        // ciclo while para iterar los arreglos...
        private static void ImprimirCursos(Curso[] ArrayCursos)
        {
            int contador = 0;
            while (contador < ArrayCursos.Length)
            {
                Console.WriteLine($"Nombre {ArrayCursos[contador].Nombre}, Id {ArrayCursos[contador].UniqueId}");
                contador++;
            }
        }
        private static void ImprimirCursosDoWhile(Curso[] ArrayCursos)
        {
            int contador = 0;
            do
            {
                WriteLine($"Nombre {ArrayCursos[contador].Nombre}, Id {ArrayCursos[contador].UniqueId}");
                contador++;
            } while (contador < ArrayCursos.Length);
        }
        private static void ImprimirCursosFor(Curso[] ArrayCursos)
        {
            for (int i = 0; i < ArrayCursos.Length; i++)
            {
                WriteLine($"Nombre {ArrayCursos[i].Nombre}, Id {ArrayCursos[i].UniqueId}");
            }
        }
        private static void ImprimirCursosForEach(Curso[] ArrayCursos)
        {
            foreach (var curso in ArrayCursos)
            {
                WriteLine($"Nombre {curso.Nombre}, Id {curso.UniqueId}");
            }
        }
    }
}
