using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Roguelike
{
    public class Mapa
    {
        // Array de celdas que forman el mapa
        public Celda[,] celdas;

        // Altura y anchura, en numero de celdas, del mapa
        public int anchura, altura;

        // Altura y anchura, en consola, de la ventana del juego
        // Debe ser impar para evitar problemas al dibujar el mapa
        public int anchuraVentana, alturaVentana;

        public Mapa()
        {
            anchura = 200;
            altura = 200;
            celdas = new Celda[anchura, altura];
            anchuraVentana = 41;
            alturaVentana = 21;

            // Rellenamos el mapa con celdas vacias
            for (int x = 0; x < celdas.GetLength(0); x++)
            {
                for (int y = 0; y < celdas.GetLength(1); y++)
                {
                    celdas[x, y] = new Celda();
                    celdas[x, y].estado = EstadoCelda.vacio;
                }
            }
        }

        // Genera el mapa completo
        public void RellenarMapa()
        {
            GenerarAgua();
            GenerarBosque();
            GenerarTrampas();
            GenerarPociones();
            GenerarBoss();
        }

        // Cambia algunas celdas vacias con agua
        public void GenerarAgua()
        {
            Random aleatorio = new Random();
            int vecinos;

            // Cambiamos a agua todas las celdas en los bordes
            for (int x = 0; x < celdas.GetLength(0); x++)
            {
                for (int y = 0; y < celdas.GetLength(1); y++)
                {
                    if (y == 0 || y == celdas.GetLength(1) - 1 || x == 0 || x == celdas.GetLength(0) - 1)
                    {
                        celdas[x, y].estado = EstadoCelda.agua;
                    }
                }
            }

            // Recorremos todas las casillas que no esten en el borde
            // Cada casilla tiene un 40% de probabilidades de convertirse en agua
            for (int x = 1; x < celdas.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < celdas.GetLength(1) - 1; y++)
                {
                    if (aleatorio.Next(100) < 40)
                    {
                        celdas[x, y].estado = EstadoCelda.agua;
                    }
                }
            }

            // Vamos a modificar cada casilla segun sus casillas contiguas (vecinos)
            // Si no es agua pero tiene 5 vecinos o mas que son agua, se convierte en agua
            // Si es agua pero tiene 3 o menos vecinos que son agua, se convierte en vacio
            // Se realiza tres veces para minimizar las casillas sueltas
            for (int contador = 0; contador < 3; contador++) {
                for (int x = 1; x < celdas.GetLength(0) - 1; x++)
                {
                    for (int y = 1; y < celdas.GetLength(1) - 1; y++)
                    {
                        vecinos = CalcularVecinos(x, y);
                        if ((EsAgua(x, y) && vecinos >= 4) || (!EsAgua(x, y) && vecinos >= 5))
                         {
                            celdas[x, y].estado = EstadoCelda.agua;
                        }
                        else
                        {
                            celdas[x, y].estado = EstadoCelda.vacio;
                        }
                    }
                }
            }
        }

        // Calcula la cantidad de celdas con agua contiguas a una celda particular
        // El valor de agua es 1 y el de vacio es 0, asi que se suma el valor estado de cada casilla contigua
        public int CalcularVecinos(int x, int y)
        {
            int vecinos = 0;

            vecinos += celdas[x - 1, y - 1].estado + celdas[x, y - 1].estado + celdas[x + 1, y - 1].estado;
            vecinos += celdas[x - 1, y].estado + celdas[x + 1, y].estado;
            vecinos += celdas[x - 1, y + 1].estado + celdas[x, y + 1].estado + celdas[x + 1, y + 1].estado;

            return vecinos;
        }

        // Devuelve true si la casilla es agua
        public bool EsAgua(int x, int y)
        {
            if (celdas[x, y].estado == EstadoCelda.agua)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Cambia algunas celdas vacias a bosque
        public void GenerarBosque()
        {
            Random aleatorio = new Random();

            // Se recorren las casillas que no estan en el borde
            // Hay un 10% de probabilidades de que la celda se convierta en bosque
            // Solo se realiza con celdas vacias para no tener bosque en agua
            for (int x = 1; x < celdas.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < celdas.GetLength(1) - 1; y++)
                {
                    if (aleatorio.Next(100) < 10)
                    {
                        if (celdas[x, y].estado == EstadoCelda.vacio)
                        {
                            celdas[x, y].estado = EstadoCelda.bosque;
                        }
                    }
                }
            }
        }

        // Cambia algunas celdas vacias a trampas
        public void GenerarTrampas()
        {
            Random aleatorio = new Random();

            // Recorre las casillas que no estan en el borde
            // Hay un 5% de probabilidades de que la celda se convierta en una trampa
            // Solo se realiza en casillas vacias para no tener trampas en agua
            for (int x = 1; x < celdas.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < celdas.GetLength(1) - 1; y++)
                {
                    if (aleatorio.Next(100) < 5)
                    {
                        if (celdas[x, y].estado == EstadoCelda.vacio)
                        {
                            celdas[x, y].estado = EstadoCelda.trampa;
                        }
                    }
                }
            }
        }

        // Genera pociones en algunas casillas vacias
        public void GenerarPociones()
        {
            Random aleatorio = new Random();

            // Recorre las casillas que no estan en el borde
            // Hay un 2% de probabilidades de que se genere una pocion
            // Solo se realiza en casillas vacias para no tener pociones en agua
            for (int x = 1; x < celdas.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < celdas.GetLength(1) - 1; y++)
                {
                    if (aleatorio.Next(100) < 2)
                    {
                        // 49% seran pociones de vida
                        // 49% seran pociones de energia
                        // 2% seran pociones de experiencia
                        if (celdas[x, y].estado == EstadoCelda.vacio)
                        {
                            if (aleatorio.Next(100) < 48)
                            {
                                celdas[x, y].estado = EstadoCelda.pocionHP;
                            }
                            else if (aleatorio.Next(100) > 49)
                            {
                                celdas[x, y].estado = EstadoCelda.pocionMP;
                            }
                            else
                            {
                                celdas[x, y].estado = EstadoCelda.pocionXP;
                            }
                        }
                    }
                }
            }
        }

        // Genera la ruina con el jefe final
        public void GenerarBoss()
        {
            Random aleatorio = new Random();
            int bossX, bossY;

            // Encuentra unas coordenadas que no esten en el borde
            bossX = aleatorio.Next(1, anchura);
            bossY = aleatorio.Next(1, altura);

            // Rellena las celdas contiguas a esas coordenadas con agua
            for (int fila = bossX - 1; fila <= bossX + 1; fila++)
            {
                for (int columna = bossY - 1; columna <= bossY + 1; columna++)
                {
                    celdas[fila, columna].estado = EstadoCelda.agua;
                }
            }

            // En el centro se crea la casilla con la ruina
            celdas[bossX, bossY].estado = EstadoCelda.boss;
        }

        // Genera las ruinas con objetos unicos en el mapa
        public void GenerarRuinas(Unico[] inventario)
        {
            Random aleatorio = new Random();
            int x, y;

            // Genera una ruina por cada objeto unico con el que no empiece el jugador
            foreach (Unico unico in inventario)
            {
                if (!unico.conseguido)
                {
                    // Encontramos una casilla que este vacia
                    do
                    {
                        x = aleatorio.Next(1, anchura - 1);
                        y = aleatorio.Next(1, altura - 1);
                    } while (celdas[x, y].estado != EstadoCelda.vacio);

                    // Guardamos las coordenadas de la casilla
                    unico.coordX = x;
                    unico.coordY = y;

                    // Creamos la ruina en el mapa
                    celdas[x, y].estado = EstadoCelda.ruina;
                }
            }
        }

        // Dibuja en pantalla un ASCII desde un fichero
        public void DibujarAscii(String ascii)
        {
            // Creamos un StreamReader del archivo que contiene el ASCII
            StreamReader archivos;
            String rutaAscii = "../../../ascii/" + ascii;
            archivos = new StreamReader(rutaAscii);

            // Guardamos el contenido del fichero en un string y lo escribimos en consola
            String contenido = archivos.ReadToEnd();
            Console.WriteLine(contenido);
            archivos.Close();
        }

        // Dibuja el mapa en la ventana de la consola
        public void MostrarMapa(int jugadorX, int jugadorY)
        {
            int celdaX, celdaY;

            // Recorremos cada casilla de la ventana
            for (int x = 0; x < anchuraVentana; x++)
            {
                for (int y = 0; y < alturaVentana; y++)
                {
                    // Encontramos la casilla del mapa que corresponde a la casilla de la ventana
                    // Depende de la posicion del jugador en el mapa
                    Console.SetCursorPosition(x, y);
                    celdaX = jugadorX - anchuraVentana / 2 + x;
                    celdaY = jugadorY - alturaVentana / 2 + y;

                    // Comprobamos que la casilla que se va a mostrar no este fuera del mapa
                    // Si no lo esta se muestra sin mas
                    // Si lo esta se dibuja una nube blanca
                    if (celdaX >= 0 && celdaX < anchura && celdaY >= 0 && celdaY < altura)
                    {
                        celdas[celdaX, celdaY].MostrarCelda();
                    } else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("~");
                    }
                }
            }

            // Dibujamos al jugador en el centro de la ventana
            Console.SetCursorPosition(anchuraVentana / 2, alturaVentana / 2);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("@");
        }

        // Crea un archivo con una descripcion del ultimo turno del combate
        public void EscribirLogCombate(Enemigo enemigo, string mensaje)
        {
            StreamWriter salida;
            String log = "../../../ascii/log.txt";
            
            // Eliminamos el archivo si ya existe
            if (File.Exists(log))
            {
                File.Delete(log);
            }

            // Creamos el archivo
            var file = File.Create(log);
            file.Close();

            // Escribimos la descripcion del turno en el archivo
            salida = new StreamWriter(log);
            salida.WriteLine(enemigo.vida + "/" + enemigo.maxVida + " puntos de vida\n");
            salida.WriteLine(mensaje);
            salida.Flush();
            salida.Close();
        }

        // Dibuja en pantalla el log de combate
        public void DibujarLogCombate()
        {
            StreamReader archivos;
            String rutaLog = "../../../ascii/log.txt";

            // No vamos a escribir en x = 0 asi que tendremos que hacerlo linea por linea
            String linea;
            int lineas = 0;

            // Borramos el log
            for (int logY = 0; logY < 6; logY++)
            {
                Console.SetCursorPosition(0, alturaVentana + 4 + logY);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            // Dibujamos el marco
            for (int y = alturaVentana + 3; y <= alturaVentana + 8; y++)
            {
                for (int x = 0; x <= anchuraVentana; x++)
                {
                    Console.SetCursorPosition(x, y);

                    // Esquinas
                    if ((x == 0 && y == alturaVentana + 3) || (x == 0 && y == alturaVentana + 8) ||
                        (x == anchuraVentana && y == alturaVentana + 3) || (x == anchuraVentana &&
                        y == alturaVentana + 8))
                    {
                        Console.Write("*");
                    } 
                    // Lados izquierdo y derecho
                    else if (x == 0 || x == anchuraVentana)
                    {
                        Console.Write("|");
                    } 
                    // Lados superior e inferior
                    else if (y == alturaVentana + 3 || y == alturaVentana + 8)
                    {
                        Console.Write("-");
                    }
                }
            }

            // Leemos el archivo log
            archivos = new StreamReader(rutaLog);
            linea = archivos.ReadLine();

            // Guardamos una linea y la imprimimos
            // Repetimos hasta que no hayan mas lineas
            while (linea != null)
            {
                Console.SetCursorPosition(2, alturaVentana + 4 + lineas);
                Console.WriteLine(linea);

                linea = archivos.ReadLine();
                lineas++;
            }

            archivos.Close();
        }
    }
}