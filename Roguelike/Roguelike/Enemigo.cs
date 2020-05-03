using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Enemigos del juego
    public class Enemigo : IEnemigo
    {
        public int id { get; set; }
        public int vida { get; set; }
        public int maxVida { get; set; }
        public int evasion { get; set; }
        public int proteccion { get; set; }
        public String ascii { get; set; }
        public String nombre { get; set; }
        public String tipo { get; set; }

        public Enemigo(int cod)
        {
            id = cod;
            AsignarStats(id);
        }

        // Asigna los stats segun el id del enemigo
        public void AsignarStats(int cod)
        {
            switch (cod)
            {
                case 0:
                    nombre = "Lobo";
                    maxVida = 15;
                    vida = maxVida;
                    ascii = "lobo.txt";
                    tipo = "Normal";
                    evasion = 50;
                    proteccion = 1;
                    break;
                case 1:
                    nombre = "Bison";
                    maxVida = 30;
                    vida = maxVida;
                    ascii = "bison.txt";
                    tipo = "Normal";
                    evasion = 25;
                    proteccion = 1;
                    break;
                case 2:
                    nombre = "Elefante";
                    maxVida = 40;
                    vida = maxVida;
                    ascii = "elefante.txt";
                    tipo = "Normal";
                    evasion = 10;
                    proteccion = 2;
                    break;
                case 3:
                    nombre = "Ciclope";
                    maxVida = 50;
                    vida = maxVida;
                    ascii = "ciclope.txt";
                    tipo = "Jefe";
                    evasion = 25;
                    proteccion = 2;
                    break;
                case 4:
                    nombre = "Dino";
                    maxVida = 30;
                    vida = maxVida;
                    ascii = "dino.txt";
                    tipo = "Jefe";
                    evasion = 50;
                    proteccion = 1;
                    break;
                case 5:
                    nombre = "Alien";
                    maxVida = 30;
                    vida = maxVida;
                    ascii = "alien.txt";
                    tipo = "Jefe";
                    evasion = 50;
                    proteccion = 1;
                    break;
                case 6:
                    nombre = "Dragon";
                    maxVida = 80;
                    vida = maxVida;
                    ascii = "hidra.txt";
                    tipo = "Jefe final";
                    evasion = 50;
                    proteccion = 2;
                    break;
            }
        }
    }
}