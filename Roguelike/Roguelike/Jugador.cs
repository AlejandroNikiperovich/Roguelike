using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    public class Jugador
    {
        // Mapa donde se mueve el jugador
        public Mapa mapa;

        // Enemigo del combate actual o anterior
        public Enemigo enemigo;

        // Coordenadas del jugador en el mapa
        public int x, y;

        // Direccion hacia la que mira el jugador
        public String direccion;

        // Contenido del log de combate
        public String mensaje;

        // Stats del jugador
        public int vida, maxVida, nivel, experiencia, siguienteNivel, evasion, proteccion;

        // Indica si el jugador esta en combate o no
        public bool enCombate;

        // Inventario de objetos unicos
        public Unico[] inventario = new Unico[9];

        // Mochila de pociones
        public List<Consumible> mochila = new List<Consumible>();

        public Jugador(Mapa mapa)
        {
            this.mapa = mapa;
            direccion = "Abajo";

            // Elegimos un punto de spawn en el mapa
            CambiarPosicion();

            maxVida = 20;
            vida = maxVida;

            // Evasion funciona del 1 al 100, cuanto mayor el numero mas facil es evadir un ataque
            // Proteccion se usa como un denominador, cuanto mas grande menos vida se pierde
            evasion = 50;
            proteccion = 1;

            nivel = 1;
            experiencia = 0;
            siguienteNivel = 10;

            enCombate = false;

            // Generamos el inventario inicial del jugador.
            for (var indice = 0; indice < inventario.Length; indice++)
            {
                // Cinco primeros slots son hechizos, el resto son equipamiento.
                if (indice < 5)
                {
                    inventario[indice] = new Hechizo(indice);

                    // El jugador empieza con los hechizos Rayos y Teletransportar.
                    if ((inventario[indice].id == NombreObjeto.rayos) || (inventario[indice].id == NombreObjeto.tp))
                    {
                        inventario[indice].conseguido = true;
                    }
                }
                else
                {
                    inventario[indice] = new Equipamiento(indice);
                }
            }

            // Cada objeto unico se encuentra en una ruina. Generamos las ruinas en el mapa.
            mapa.GenerarRuinas(inventario);
        }



        // Cambia la posicion del jugador en el mapa.
        public void CambiarPosicion()
        {
            var aleatorio = new Random();

            // Encontramos una casilla vacia y cambiamos las coordenadas del jugador a las de la celda.
            do
            {
                x = aleatorio.Next(mapa.anchura);
                y = aleatorio.Next(mapa.altura);
            } while (mapa.celdas[x, y].estado != EstadoCelda.vacio);
        }

        // Muestra informacion relevante del jugador a la derecha de la ventana del mapa
        public void MostrarMenu()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(mapa.anchuraVentana + 1, 0);
            Console.WriteLine("Nivel " + nivel);
            Console.SetCursorPosition(mapa.anchuraVentana + 25, 0);
            Console.WriteLine("(" + x + ", " + y + ")");

            Console.SetCursorPosition(mapa.anchuraVentana + 1, 1);
            Console.WriteLine(experiencia + "/" + siguienteNivel + " puntos de XP  ");

            Console.SetCursorPosition(mapa.anchuraVentana + 1, 2);
            Console.WriteLine(vida + "/" + maxVida + " puntos de vida  ");

            // Muestra el inventario y la mochila
            MostrarInventario(-1);
            MostrarMochila(-1);
        }
        
        // Dibuja en consola el inventario de objetos unicos
        public void MostrarInventario(int indiceInventario)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(mapa.anchuraVentana + 1, 4);
            Console.WriteLine("INVENTARIO");

            // Mostramos todos los objetos que hemos conseguido, uno debajo del otro
            for (int indice = 0; indice < inventario.Length; indice++)
            {
                if (inventario[indice].conseguido)
                {
                    Console.SetCursorPosition(mapa.anchuraVentana + 1, 5 + indice);

                    // Si el cursor esta en el objeto que vamos a dibujar, lo resaltamos
                    if (indice == indiceInventario)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(inventario[indice].nombre);
                        Console.BackgroundColor = ConsoleColor.Black;
                    } else
                    {
                        // Si el objeto no puede usarse, se escribe en gris
                        // Si el objeto es un hechizo, se muestran las cargas
                        if (inventario[indice].puedeUsarse)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine(inventario[indice].nombre);

                            if (inventario[indice] is Hechizo)
                            {
                                Console.SetCursorPosition(mapa.anchuraVentana + 16, 5 + indice);
                                inventario[indice].MostrarCargas();
                            }
                        } else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine(inventario[indice].nombre);

                            if (inventario[indice] is Hechizo)
                            {
                                Console.SetCursorPosition(mapa.anchuraVentana + 16, 5 + indice);
                                inventario[indice].MostrarCargas();
                            }
                        }
                    }
                }
            }

            // Mostramos la descripcion del objeto
            if (indiceInventario != -1)
            {
                BorrarDescripcion();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(0, mapa.alturaVentana + 1);
                Console.WriteLine(inventario[indiceInventario].descripcion);
            }
        }

        // Dibuja en consola la mochila de pociones
        public void MostrarMochila(int indiceMochila)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(mapa.anchuraVentana + 25, 4);
            Console.WriteLine("MOCHILA");

            // Mostramos todos los objetos que hemos conseguido, uno debajo del otro
            if (mochila.Count >= 1)
            {
                for (int indice = 0; indice < mochila.Count; indice++)
                {
                    Console.SetCursorPosition(mapa.anchuraVentana + 25, 5 + indice);

                    // Si el cursor esta en el objeto que vamos a dibujar, lo resaltamos
                    if (indice == indiceMochila)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(mochila[indice].nombre);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(mochila[indice].nombre);
                    }
                }
            }

            // Mostramos la descripcion del objeto
            if (indiceMochila != -1)
            {
                BorrarDescripcion();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(0, mapa.alturaVentana + 1);
                Console.WriteLine(mochila[indiceMochila].descripcion);
            }
        }

        // Contiene las acciones que el jugador puede realizar mientras no esta en combate
        // Depende de la ultima tecla pulsada
        public void ControlarJugador()
        {
            // Buffer
            while (Console.KeyAvailable == true)
            {
                Console.ReadKey(true);
            }

            // Leemos el input
            ConsoleKeyInfo tecla = Console.ReadKey(true);
            switch (tecla.Key)
            {
                // Moverse arriba
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (PuedeMoverse(x, y - 1))
                    {
                        y--;
                    }
                    direccion = "Arriba";
                    break;

                // Moverse a la izquierda
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    if (PuedeMoverse(x - 1, y))
                    {
                        x--;
                    }
                    direccion = "Izquierda";
                    break;

                // Moverse abajo
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (PuedeMoverse(x, y + 1))
                    {
                        y++;
                    }
                    direccion = "Abajo";
                    break;

                // Moverse a la derecha
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    if (PuedeMoverse(x + 1, y))
                    {
                        x++;
                    }
                    direccion = "Derecha";
                    break;

                // Con ESPACIO se abre el inventario de objetos unicos
                case ConsoleKey.Spacebar:
                    ControlarInventario();
                    break;

                // Con TAB se abre la mochila de pociones
                case ConsoleKey.Tab:
                    if (mochila.Count > 0)
                    {
                        ControlarMochila();
                    }
                    break;

                // Con ENTER interactuamos con el mapa
                // Depende de la casilla en la que estemos
                case ConsoleKey.Enter:
                    switch (mapa.celdas[x, y].estado)
                    {
                        // Cogemos una pocion si la mochila no esta llena
                        case EstadoCelda.pocionHP:
                        case EstadoCelda.pocionMP:
                        case EstadoCelda.pocionXP:
                            if (mochila.Count < inventario.Length)
                            {
                                CogerPocion();
                            }
                            break;

                        case EstadoCelda.ruina:
                            EntrarRuina();
                            break;

                        // Solo podemos combatir con el jefe final si tenemos la Llave pesada
                        case EstadoCelda.boss:
                            if (inventario[NombreObjeto.llave].conseguido)
                            {
                                EmpezarCombate("Jefe final");
                            } else
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.SetCursorPosition(0, mapa.alturaVentana + 1);
                                Console.WriteLine("Parece que necesitas una llave para acceder a esta ruina.");
                                Console.SetCursorPosition(0, mapa.alturaVentana + 2);
                                Console.WriteLine("Pulsa cualquier tecla para continuar...");
                                Console.ReadKey();
                                BorrarDescripcion();
                            }
                            break;
                    }
                    break;
            }
        }

        // Comprueba si el jugador puede moverse a la casilla
        // Devuelve true si es el caso
        public bool PuedeMoverse(int x, int y)
        {
            // Comprobamos que no nos salimos del mapa
            if (!PasadoBorde(x, y))
            {
                switch (mapa.celdas[x, y].estado)
                {
                    // Siempre podemos caminar sobre vacio, ruinas o pociones
                    case EstadoCelda.vacio:
                    case EstadoCelda.ruina:
                    case EstadoCelda.boss:
                    case EstadoCelda.pocionHP:
                    case EstadoCelda.pocionMP:
                    case EstadoCelda.pocionXP:
                        return true;

                    // No podemos caminar sobre agua, excepto si hemos conseguido el hechizo Levitar
                    case EstadoCelda.agua:
                        if (inventario[NombreObjeto.levitar].conseguido) {
                            return true;
                        } else
                        {
                            return false;
                        }

                    // No podemos caminar sobre bosque en ningun caso
                    case EstadoCelda.bosque:
                        return false;

                    // Siempre podemos caminar sobre trampas, pero perdemos un cuarto de nuestra vida maxima
                    case EstadoCelda.trampa:
                        PerderHP((maxVida / 4) / proteccion);
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Comprueba si la casilla esta fuera del mapa
        // Devuelve true si es el caso
        public bool PasadoBorde(int x, int y)
        {
            if (x < 0 || x > mapa.anchura - 1 || y < 0 || y > mapa.altura - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Reduce los puntos de vida del jugador
        public void PerderHP(int puntos)
        {
            vida -= puntos;

            // Si perdemos todos los puntos de vida, termina la partida
            if (vida <= 0)
            {
                Console.Clear();
                mapa.DibujarAscii("gameover.txt");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        // Controla las acciones que el jugador puede realizar mientras esta en el inventario
        // Depende de la ultima tecla pulsada
        public void ControlarInventario()
        {
            // Buffer
            while (Console.KeyAvailable == true)
            {
                Console.ReadKey(true);
            }

            // Cursor que muestra el indice del objeto con el que podemos interactuar
            int indiceInventario = 0;

            // Condicion para salir del inventario
            bool salirInventario = false;

            ConsoleKeyInfo teclaInventario;

            while (!salirInventario)
            {
                // Al abrir el inventario el puntero se coloca en el primer objeto
                MostrarInventario(indiceInventario);

                // Leemos el input
                teclaInventario = Console.ReadKey(true);
                switch (teclaInventario.Key)
                {
                    // Cambiamos al objeto encima del actual
                    // Cuando estamos arriba del todo, bajamos al ultimo
                    // Nos saltamos los objetos que no tengamos
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        do
                        {
                            if (indiceInventario > 0)
                            {
                                indiceInventario--;
                            }
                            else
                            {
                                indiceInventario = inventario.Length - 1;
                            }
                        } while (!inventario[indiceInventario].conseguido);
                        break;

                    // Igual que el anterior, pero vamos hacia abajo
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        do
                        {
                            if (indiceInventario < inventario.Length - 1)
                            {
                                indiceInventario++;
                            }
                            else
                            {
                                indiceInventario = 0;
                            }
                        } while (!inventario[indiceInventario].conseguido);
                        break;

                    // Si le damos a la derecha cambiamos a la mochila de pociones
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        // No funciona si la mochila esta vacia
                        if (mochila.Count >= 1)
                        {
                            // Refrescamos el inventario para que no se quede resaltado
                            indiceInventario = -1;
                            MostrarInventario(indiceInventario);
                            BorrarDescripcion();

                            ControlarMochila();
                            salirInventario = true;
                        }
                        break;

                    // Con ENTER podemos usar un hechizo, siempre que tenga cargas
                    case ConsoleKey.Enter:
                        if (inventario[indiceInventario].puedeUsarse && inventario[indiceInventario].TieneCargas())
                        {
                            // Reducimos las cargas del hechizo
                            inventario[indiceInventario].ReducirCargas();

                            UsarObjeto(inventario[indiceInventario]);
                            salirInventario = true;
                            BorrarDescripcion();
                        }
                        break;

                    // Con ESPACIO salimos del inventario y volvemos al mapa
                    case ConsoleKey.Spacebar:
                        salirInventario = true;
                        BorrarDescripcion();
                        break;
                }
            }
        }

        // Controla las acciones que el jugador puede realizar mientras esta en la mochila
        // Depende de la ultima tecla pulsada
        public void ControlarMochila()
        {
            // Buffer
            while (Console.KeyAvailable == true)
            {
                Console.ReadKey(true);
            }

            // Cursor que muestra el indice del objeto con el que podemos interactuar
            int indiceMochila = 0;

            // Condicion para salir de la mochila
            bool salirMochila = false;

            ConsoleKeyInfo teclaMochila;

            while (!salirMochila)
            {
                // Al abrir la mochila el puntero se coloca en el primer objeto
                MostrarMochila(indiceMochila);

                // Leemos el input
                teclaMochila = Console.ReadKey(true);
                switch (teclaMochila.Key)
                {
                    // Cambiamos al objeto encima del actual
                    // Cuando estamos arriba del todo, bajamos al ultimo
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (indiceMochila > 0)
                        {
                            indiceMochila--;
                        }
                        else
                        {
                            indiceMochila = mochila.Count - 1;
                        }
                        break;

                    // Igual que el anterior, pero vamos hacia abajo
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (indiceMochila < mochila.Count - 1)
                        {
                            indiceMochila++;
                        }
                        else
                        {
                            indiceMochila = 0;
                        }
                        break;

                    // Si le damos a la izquierda, cambiamos al inventario
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        // Refrescamos la mochila para que no se quede resaltada
                        BorrarDescripcion();
                        indiceMochila = -1;
                        MostrarMochila(indiceMochila);

                        ControlarInventario();
                        salirMochila = true;
                        break;

                    // Con ENTER usamos el objeto
                    case ConsoleKey.Enter:
                        UsarObjeto(mochila[indiceMochila]);

                        // Borreamos la lista de pociones
                        // No podemos usar un Console.WindowWidth como en mapa.DibujarLogCombate() porque nos borraria el mapa
                        for (int linea = 0; linea < mochila.Count; linea++)
                        {
                            Console.SetCursorPosition(mapa.anchuraVentana + 25, 5 + linea);
                            Console.Write("                     ");
                        }

                        // Eliminamos la pocion tras usarla
                        mochila.RemoveAt(indiceMochila);

                        salirMochila = true;
                        BorrarDescripcion();
                        break;

                    // Con TAB salimos de la mochila y volvemos al mapa
                    case ConsoleKey.Tab:
                        salirMochila = true;
                        BorrarDescripcion();
                        break;
                }
            }
        }

        // Borra la descripcion del objeto
        public void BorrarDescripcion()
        {
            Console.SetCursorPosition(0, mapa.alturaVentana + 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, mapa.alturaVentana + 2);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        // Usamos un objeto
        public void UsarObjeto(Objeto objeto)
        {
            Random aleatorio = new Random();
            int puntos;

            switch (objeto.id)
            {
                // Hechizo Rayos
                case NombreObjeto.rayos:
                    // Fuera de combate quema los bosque contiguos al jugador
                    if (!enCombate)
                    {
                        for (int columna = x - 1; columna <= x + 1; columna++)
                        {
                            for (int fila = y - 1; fila <= y + 1; fila++)
                            {
                                if (mapa.celdas[columna, fila].estado == EstadoCelda.bosque)
                                {
                                    mapa.celdas[columna, fila].estado = EstadoCelda.vacio;
                                }
                            }
                        }
                    }
                    // En combate reduce los puntos de vida del enemigo
                    else
                    {
                        mensaje = "Usas " + objeto.nombre + ".\n";

                        // Roll de evasion
                        if (enemigo.evasion < aleatorio.Next(100))
                        {
                            // Si tenemos el Anillo Canalizador, el enemigo pierde mas vida
                            if (inventario[NombreObjeto.anillo].conseguido)
                            {
                                puntos = (10 + (4 * nivel)) / enemigo.proteccion;
                                enemigo.vida -= puntos;
                            }
                            else
                            {
                                puntos = (10 + (2 * nivel)) / enemigo.proteccion;
                                enemigo.vida -= puntos;
                            }
                            mensaje += "El enemigo pierde " + puntos + " puntos de vida.";
                        }
                        else
                        {
                            mensaje += "El enemigo esquiva el ataque.";
                        }
                    }
                    break;

                // Hechizo Teleportar
                case NombreObjeto.tp:
                    // Cuenta las casillas desde la casilla del jugador
                    int pasos = 1;

                    // Indica si el jugador puede teletransportarse a una casilla
                    bool puedeTp = false;

                    // Si estamos mirando hacia una casilla de agua, atravesamos el lago
                    // Si no, el jugador se teletransporta a otro lugar del mapa aleatorio
                    switch (direccion)
                    {
                        case "Arriba":
                            if (!PasadoBorde(x, y - 1))
                            {
                                if (!mapa.EsAgua(x, y - 1))
                                {
                                    CambiarPosicion();
                                }
                                else
                                {
                                    do
                                    {
                                        if (PuedeMoverse(x, y - pasos) && mapa.celdas[x, y - pasos].estado != EstadoCelda.boss)
                                        {
                                            y -= pasos;
                                            puedeTp = true;
                                        }
                                        pasos++;
                                    } while (pasos <= 10 && !puedeTp);
                                }
                            }
                            break;

                        case "Izquierda":
                            if (!PasadoBorde(x - 1, y))
                            {
                                if (!mapa.EsAgua(x - 1, y))
                                {
                                    CambiarPosicion();
                                }
                                else
                                {
                                    do
                                    {
                                        if (PuedeMoverse(x - pasos, y) && mapa.celdas[x - pasos, y].estado != EstadoCelda.boss)
                                        {
                                            x -= pasos;
                                            puedeTp = true;
                                        }
                                        pasos++;
                                    } while (pasos <= 10 && !puedeTp);
                                }
                            }
                            break;

                        case "Abajo":
                            if (!PasadoBorde(x, y + 1))
                            {
                                if (!mapa.EsAgua(x, y + 1))
                                {
                                    CambiarPosicion();
                                }
                                else
                                {
                                    do
                                    {
                                        if (PuedeMoverse(x, y + pasos) && mapa.celdas[x, y + pasos].estado != EstadoCelda.boss)
                                        {
                                            y += pasos;
                                            puedeTp = true;
                                        }
                                        pasos++;
                                    } while (pasos <= 10 && !puedeTp);
                                }
                            }
                            break;

                        case "Derecha":
                            if (!PasadoBorde(x + 1, y))
                            {
                                if (!mapa.EsAgua(x + 1, y))
                                {
                                    CambiarPosicion();
                                }
                                else
                                {
                                    do
                                    {
                                        if (PuedeMoverse(x + pasos, y) && mapa.celdas[x + pasos, y].estado != EstadoCelda.boss)
                                        {
                                            x += pasos;
                                            puedeTp = true;
                                        }
                                        pasos++;
                                    } while (pasos <= 10 && !puedeTp);
                                }
                            }
                            break;
                    }
                    break;

                // Hechizo Levitar
                // Aumenta la evasion del jugador
                case NombreObjeto.levitar:
                    evasion += 25;

                    mensaje = "Usas " + objeto.nombre + ".\nEvasion aumentada.";
                    break;

                // Hechizo Temblor
                case NombreObjeto.temblor:
                    // Fuera de combate elimina las trampas contiguas al jugador
                    if (!enCombate)
                    {
                        for (int columna = x - 1; columna <= x + 1; columna++)
                        {
                            for (int fila = y - 1; fila <= y + 1; fila++)
                            {
                                if (mapa.celdas[columna, fila].estado == EstadoCelda.trampa)
                                {
                                    mapa.celdas[columna, fila].estado = EstadoCelda.vacio;
                                }
                            }
                        }
                    }
                    // En combate reduce la evasion del enemigo
                    else
                    {
                        enemigo.evasion -= 25;

                        mensaje = "Usas " + objeto.nombre + ".\nReduces la evasion del enemigo.";
                    }
                    break;

                // Hechizo Curacion
                // Restaura los puntos de vida perdidos del jugador
                case NombreObjeto.regenerarHP:
                    vida = maxVida;

                    mensaje = "Usas " + objeto.nombre + ".\nRestauras tus puntos de vida.";
                    break;

                // Pocion de vida
                // Restaura los puntos de vida perdidos del jugador
                case NombreObjeto.pocionHP:
                    vida = maxVida;

                    if (enCombate)
                    {
                        mensaje = "Usas una " + objeto.nombre + ".\nRestauras tus puntos de vida.";
                    }
                    break;

                // Pocion de energia
                // Restaura las cargas de todos los hechizos
                case NombreObjeto.pocionMP:
                    foreach (Unico unico in inventario)
                    {
                        if (unico.conseguido && unico is Hechizo)
                        {
                            unico.RestaurarCargas();
                        }
                    }

                    if (enCombate)
                    {
                        mensaje = "Usas una " + objeto.nombre + ".\nRecuperas todas las cargas de hechizos.";
                    }
                    break;

                // Pocion de experiencia
                // Sube al jugador de nivel y restaura sus puntos de vida perdidos
                // Cambia la experiencia a 0
                case NombreObjeto.pocionXP:
                    GanarExperiencia(siguienteNivel);
                    experiencia = 0;

                    if (enCombate)
                    {
                        mensaje = "Usas una " + objeto.nombre + ".\nSubes de nivel y te curas.";
                    }
                    break;
            }
        }

        // Aumenta la experiencia del jugador
        public void GanarExperiencia(int xp)
        {
            experiencia += xp;

            // Si llegamos al limite, subimos de nivel
            // Aumenta la vida maxima y el limite para el siguiente nivel
            // Restaura todos los puntos de vida perdidos
            if (experiencia >= siguienteNivel)
            {
                nivel++;
                maxVida += 5;
                vida = maxVida;
                experiencia -= siguienteNivel;
                siguienteNivel += 10;
            }
        }

        // Guarda una pocion del mapa en tu mochila
        // La pocion depende del valor de la celda en la que este el jugador
        // Elimina la pocion del mapa
        public void CogerPocion()
        {
            Consumible pocion;
            Random aleatorio = new Random();

            switch (mapa.celdas[x, y].estado)
            {
                case EstadoCelda.pocionHP:
                    pocion = new Consumible(9);
                    mochila.Add(pocion);
                    mapa.celdas[x, y].estado = EstadoCelda.vacio;
                    break;

                case EstadoCelda.pocionMP:
                    pocion = new Consumible(10);
                    mochila.Add(pocion);
                    mapa.celdas[x, y].estado = EstadoCelda.vacio;
                    break;

                case EstadoCelda.pocionXP:
                    pocion = new Consumible(11);
                    mochila.Add(pocion);
                    mapa.celdas[x, y].estado = EstadoCelda.vacio;
                    break;
            }

            // Tras conseguir la pocion, hay una probabilidad del 33% de que empiece un combate
            if (aleatorio.Next(100) < 33)
            {
                EmpezarCombate("Normal");
            }
        }

        // Empieza un combate con un mini-jefe
        // Si ganamos el combate, conseguimos un objeto unico y eliminamos la ruina del mapa
        public void EntrarRuina()
        {
            EmpezarCombate("Jefe");

            foreach (Unico unico in inventario)
            {
                if (unico.coordX == x && unico.coordY == y)
                {
                    unico.conseguido = true;
                }

                mapa.celdas[x, y].estado = EstadoCelda.vacio;
            }

            // Refrescamos los stats del jugador en caso de que hayamos conseguido un bufo pasivo
            ComprobarStats();
        }

        // Prepara y desarrolla un combate
        public void EmpezarCombate(String tipo)
        {
            Random aleatorio = new Random();

            // Elegimos un enemigo aleatoriamente del tipo indicado
            do
            {
                enemigo = new Enemigo(aleatorio.Next(7));
            } while (enemigo.tipo != tipo);

            // Ponemos al jugador en modo combate y cambiamos la disponibilidad de algunos hechizos
            enCombate = true;
            CambiarInventario();

            // Dibujamos el ASCII del enemigo
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            mapa.DibujarAscii(enemigo.ascii);

            // Empezamos el combate en si
            Turnos();

            // Dibujamos la pantalla de fin del combate
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            MostrarMenu();

            mapa.EscribirLogCombate(enemigo, "Has ganado.");
            mapa.DibujarLogCombate();

            Console.SetCursorPosition(2, mapa.alturaVentana + 9);
            Console.WriteLine("Pulsa cualquier tecla para continuar...");

            Console.ReadKey();
            Console.Clear();

            // Terminamos el combate, cambiamos la disponibilidad otra vez y restauramos los stats del jugador
            enCombate = false;
            CambiarInventario();
            ComprobarStats();

            // Ganamos experiencia tras el combate
            switch (tipo)
            {
                case "Normal":
                    GanarExperiencia(siguienteNivel / 4);
                    break;
                case "Jefe":
                    GanarExperiencia(siguienteNivel / 2);
                    break;
                default:
                    Console.Clear();
                    mapa.DibujarAscii("win.txt");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }
        }

        // Cambia la disponibilidad de algunos hechizos
        public void CambiarInventario()
        {
            foreach (Unico unico in inventario)
            {
                if (unico.id == NombreObjeto.tp)
                {
                    unico.puedeUsarse = !unico.puedeUsarse;
                }

                if (unico.id == NombreObjeto.levitar || unico.id == NombreObjeto.regenerarHP)
                {
                    unico.puedeUsarse = !unico.puedeUsarse;
                }
            }
        }

        // Fase de combate por turnos
        public void Turnos()
        {
            // Indica cual es el turno actual
            bool turnoJugador;

            Random aleatorio = new Random();
            mensaje = "Te ataca un " + enemigo.nombre + ".\n";

            // Si tenemos las Botas ligeras o ganamos el roll, tenemos el primer turno
            if (inventario[NombreObjeto.botas].conseguido || aleatorio.Next(2) == 0)
            {
                turnoJugador = true;
                mensaje += "Tienes la ventaja.";
            }
            else
            {
                turnoJugador = false;
                mensaje += "El enemigo tiene la ventaja.";
            }

            // Los turnos continuan hasta que el enemigo pierda todos sus puntos de vida
            while (enemigo.vida > 0)
            {
                // Refrescamos el menu y el log de combate
                Console.ForegroundColor = ConsoleColor.Magenta;
                MostrarMenu();
                mapa.EscribirLogCombate(enemigo, mensaje);
                mapa.DibujarLogCombate();

                if (turnoJugador)
                {
                    ControlarInventario();
                }
                else
                {
                    // Pausa para leer el log
                    Console.SetCursorPosition(2, mapa.alturaVentana + 9);
                    Console.WriteLine("Pulsa cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.SetCursorPosition(0, mapa.alturaVentana + 9);
                    Console.Write(new string(' ', Console.WindowWidth));

                    AccionEnemigo();
                }

                // Cambiamos el turno
                turnoJugador = !turnoJugador;
            }
        }

        // Turno del enemigo durante un combate
        // Devuelve un mensaje a turnos para mostrar en el log de combate
        // Todos los enemigos tienen dos ataques, excepto el jefe final que tiene 4
        public void AccionEnemigo()
        {
            Random aleatorio = new Random();

            // Valor de puntos que pierde el jugador al ser atacado
            int puntos;

            // Indica que ataque usara el enemigo
            int ataque;
            ataque = aleatorio.Next(4);

            switch (enemigo.id)
            {
                // Normal Lobo
                case NombreEnemigo.lobo:
                    // Mordisco: pierdes 1/4 de tu vida
                    if (ataque < 2)
                    {
                        mensaje = enemigo.nombre + " usa Mordisco.\n";

                        // Roll de evasion
                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 4) / proteccion;
                            PerderHP(puntos);

                            mensaje += "Pierdes " + puntos + " puntos de vida.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    // Rugido: reduce tu evasion
                    else
                    {
                        evasion -= 25;

                        mensaje = enemigo.nombre + " usa Rugido.\nEvasion reducida.";
                    }
                    break;

                // Normal Bison
                case NombreEnemigo.bison:
                    // Carga: pierdes 1/6 de tu vida
                    if (ataque < 2)
                    {
                        mensaje = enemigo.nombre + " usa Carga.\n";

                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 6) / proteccion;
                            PerderHP(puntos);

                            mensaje += "Pierdes " + puntos + " puntos de vida.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    // Refuerzo: aumenta la proteccion del enemigo
                    else
                    {
                        enemigo.proteccion = 2;

                        mensaje = enemigo.nombre + " usa Refuerzo.\nSu proteccion aumenta.";
                    }
                    break;

                // Normal Elefante
                case NombreEnemigo.elefante:
                    // Estampida: pierdes 1/4 de tu vida
                    if (ataque < 2)
                    {
                        mensaje = enemigo.nombre + " usa Estampida.\n";

                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 4) / proteccion;
                            PerderHP(puntos);

                            mensaje += "Pierdes " + puntos + " puntos de vida.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    // Pisoton: pierdes 1/6 de tu vida y reduce tu evasion
                    else
                    {
                        mensaje = enemigo.nombre + " usa Pisoton.\n";

                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 6) / proteccion;
                            PerderHP(puntos);
                            evasion -= 25;

                            mensaje += "Pierdes " + puntos + " puntos de vida y evasion.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    break;

                // Jefe Ciclope
                case NombreEnemigo.ciclope:
                    // Garrote: pierdes 1/6 de tu vida y reduce tu evasion
                    if (ataque < 2)
                    {
                        mensaje = enemigo.nombre + " usa Garrote.\n";

                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 6) / proteccion;
                            PerderHP(puntos);
                            evasion -= 25;

                            mensaje += "Pierdes " + puntos + " puntos de vida y evasion.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    // Refuerzo: aumenta la proteccion del enemigo
                    else
                    {
                        enemigo.proteccion = 3;

                        mensaje = enemigo.nombre + " usa Refuerzo.\nSu proteccion aumenta.";
                    }
                    break;

                // Jefe Dino
                case NombreEnemigo.dino:
                    // Golpe de Cola: pierdes 1/4 de tu vida
                    if (ataque < 2)
                    {
                        mensaje = enemigo.nombre + " usa Golpe de Cola.\n";

                        // Roll de evasion
                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 4) / proteccion;
                            PerderHP(puntos);

                            mensaje += "Pierdes " + puntos + " puntos de vida.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    // Agilidad: aumenta la evasion del enemigo
                    else
                    {
                        enemigo.evasion = 75;

                        mensaje = enemigo.nombre + " usa Agilidad.\nSu evasion aumenta.";
                    }
                    break;

                // Jefe Alien
                case NombreEnemigo.alien:
                    // Pistola Laser: pierdes 1/4 de tu vida
                    if (ataque < 2)
                    {
                        mensaje = enemigo.nombre + " usa Pistola Laser.\n";

                        // Roll de evasion
                        if (evasion < aleatorio.Next(100))
                        {
                            puntos = (maxVida / 4) / proteccion;
                            PerderHP(puntos);

                            mensaje += "Pierdes " + puntos + " puntos de vida.";
                        }
                        else
                        {
                            mensaje += "Esquivas el ataque.";
                        }
                    }
                    // Bloqueo Mental: elimina las cargas de uno de tus hechizos
                    else
                    {
                        int hechizo;

                        // Elige uno de los hechizos que has conseguido
                        do
                        {
                            hechizo = aleatorio.Next(5);
                        } while (!inventario[hechizo].conseguido);

                        // Elimina todas las cargas del hechizo
                        while (inventario[hechizo].TieneCargas())
                        {
                            inventario[hechizo].ReducirCargas();
                        }

                        mensaje = enemigo.nombre + " usa Bloqueo Mental.\n";
                        mensaje += "Cargas de " + inventario[hechizo].nombre + " perdidas.";
                    }
                    break;

                // Jefe final Dragon
                case NombreEnemigo.hidra:
                    switch (ataque)
                    {
                        // Bloqueo Mental
                        case 0:
                            int hechizo;

                            do
                            {
                                hechizo = aleatorio.Next(5);
                            } while (!inventario[hechizo].conseguido);

                            while (inventario[hechizo].TieneCargas())
                            {
                                inventario[hechizo].ReducirCargas();
                            }

                            mensaje = enemigo.nombre + " usa Bloqueo Mental.\n";
                            mensaje += "Cargas de " + inventario[hechizo].nombre + " perdidas.";
                            break;

                        // Agilidad
                        case 1:
                            enemigo.evasion = 75;

                            mensaje = enemigo.nombre + " usa Agilidad.\nSu evasion aumenta.";
                            break;

                        // Refuerzo
                        case 2:
                            mensaje = enemigo.nombre + " usa Refuerzo.\nSu proteccion aumenta.";
                            enemigo.proteccion = 3;
                            break;

                        // Llamarada: pierdes 1/3 de tu vida
                        case 3:
                            mensaje = enemigo.nombre + " usa Llamarada.\n";

                            // Roll de evasion
                            if (evasion < aleatorio.Next(100))
                            {
                                puntos = (maxVida / 3) / proteccion;
                                PerderHP(puntos);

                                mensaje += "Pierdes " + puntos + " puntos de vida.";
                            }
                            else
                            {
                                mensaje += "Esquivas el ataque.";
                            }
                            break;
                    }
                    break;
            }
        }

        // Restaura los stats del jugador tras un combate
        public void ComprobarStats()
        {
            if (inventario[NombreObjeto.botas].conseguido)
            {
                evasion = 65;
            }

            if (inventario[NombreObjeto.amuleto].conseguido)
            {
                proteccion = 2;
            }
        }
    }
}