using System;
using System.Collections.Generic;
using static System.Console;
namespace CoreEscuela.util {
    // clase estatica no se puede cambiar en este caso son propias de acuerdo a la necesidad
    public static class Printer
    {
        public static void DrawLine(int tam = 10) {
            WriteLine("".PadLeft(tam, '='));
            
        }
        public static void PresioneEnter() {
            WriteLine("Presione Enter para continuar....");
            
        }
        public static void Title(string titulo) {
            var tamanio = titulo.Length + 4;
            DrawLine(tamanio);
            WriteLine($"| {titulo} |");
            DrawLine(tamanio);
        }
        public static void Beep (int hz = 2000, int tiempo = 500, int repeticion = 1) {
            while (repeticion-- > 0 ) {
                Console.Beep(hz, tiempo);
            }
        }
    }
}