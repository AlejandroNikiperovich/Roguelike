using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Casillas individuales del mapa
    public class Celda
    {
        // Tipo de terreno o pocion
        public int estado;

        public Celda()
        {
            estado = EstadoCelda.vacio;
        }

        // Dibuja el terreno segun el estado de la celda
        public void MostrarCelda()
        {
            switch (estado)
            {
                case EstadoCelda.vacio:
                    Console.Write(" ");
                    break;
                case EstadoCelda.bosque:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("A");
                    break;
                case EstadoCelda.agua:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("~");
                    break;
                case EstadoCelda.trampa:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("X");
                    break;
                case EstadoCelda.ruina:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("O");
                    break;
                case EstadoCelda.pocionHP:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("&");
                    break;
                case EstadoCelda.pocionMP:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("&");
                    break;
                case EstadoCelda.pocionXP:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("&");
                    break;
                case EstadoCelda.boss:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("O");
                    break;
            }
        }
    }
}