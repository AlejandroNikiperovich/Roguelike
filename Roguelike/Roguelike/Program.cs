using System;

namespace Roguelike
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generamos un mapa
            Mapa mapa = new Mapa();
            mapa.RellenarMapa();

            // Creamos al jugador
            Jugador jugador = new Jugador(mapa);

            // Dibujamos la portada
            mapa.DibujarAscii("van.txt");
            Console.ReadKey();
            Console.Clear();

            // Mostramos las instrucciones
            Console.ForegroundColor = ConsoleColor.Magenta;
            mapa.DibujarAscii("instrucciones.txt");
            Console.ReadKey();
            Console.Clear();

            // Empezamos la partida
            while (true)
            {
                // Ocultamos el cursor
                Console.CursorVisible = false;

                // Dibujamos el mapa y el menu lateral
                mapa.MostrarMapa(jugador.x, jugador.y);
                jugador.MostrarMenu();

                // Permitimos controlar al jugador
                jugador.ControlarJugador();
            }
        }
    }
}
