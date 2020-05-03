using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Items del juego
    public abstract class Objeto
    {
        public int id;
        public String descripcion, nombre;

        // Indica si el objeto puede usarse en el contexto actual
        public bool puedeUsarse; 

        // Asigna las propiedades a cada objeto segun su id.
        public void AsignarNombre(int cod)
        {
            switch (cod)
            {
                case NombreObjeto.pocionHP:
                    nombre = "Pocion de vida";
                    descripcion = "Restaura tus puntos de vida perdidos.";
                    puedeUsarse = true;
                    break;

                case NombreObjeto.pocionMP:
                    nombre = "Pocion de energia";
                    descripcion = "Restaura las cargas de todos los hechizos.";
                    puedeUsarse = true;
                    break;

                case NombreObjeto.pocionXP:
                    nombre = "Pocion de experiencia";
                    descripcion = "Subes de nivel y restauras tus puntos de vida perdidos.";
                    puedeUsarse = true;
                    break;

                case NombreObjeto.rayos:
                    nombre = "Rayos";
                    descripcion = "Daña a un enemigo con una descarga electrica. En el mapa, quema arboles a tu"
                        + " alrededor.";
                    puedeUsarse = true;
                    break;

                case NombreObjeto.tp:
                    nombre = "Teletransporte";
                    descripcion = "Cambia tu posicion a una casilla aleatoria. Si estas mirando hacia una casilla de"
                        + " agua, puedes atravesar un charco si mide menos de 10 casillas.";
                    puedeUsarse = true;
                    break;

                case NombreObjeto.levitar:
                    nombre = "Levitar";
                    descripcion = "Aumenta tu evasion durante el resto del combate. Tras adquirirlo, puedes levitar "
                        + "sobre casillas de agua.";
                    puedeUsarse = false;
                    break;

                case NombreObjeto.regenerarHP:
                    nombre = "Curacion";
                    descripcion = "Restaura tus puntos de salud perdidos.";
                    puedeUsarse = false;
                    break;

                case NombreObjeto.temblor:
                    nombre = "Temblor";
                    descripcion = "Disminuye la evasion de un enemigo. En el mapa, elimina trampas a tu alrededor.";
                    puedeUsarse = true;
                    break;

                case NombreObjeto.llave:
                    nombre = "Llave pesada";
                    descripcion = "Abre las puertas a las ruinas de un enemigo poderoso.";
                    puedeUsarse = false;
                    break;

                case NombreObjeto.botas:
                    nombre = "Botas ligeras";
                    descripcion = "Aumenta tu evasion. Siempre tienes la ventaja al empezar el combate.";
                    puedeUsarse = false;
                    break;

                case NombreObjeto.anillo:
                    nombre = "Anillo canalizador";
                    descripcion = "Tus hechizos son mas potentes.";
                    puedeUsarse = false;
                    break;

                case NombreObjeto.amuleto:
                    nombre = "Amuleto protector";
                    descripcion = "Recibes menos daño de cualquier fuente.";
                    puedeUsarse = false;
                    break;
            }
        }

        // Placeholders para metodos de Hechizos
        public virtual bool TieneCargas()
        {
            return true;
        }

        public virtual void ReducirCargas() { }

        public virtual void MostrarCargas() { }

        public virtual void RestaurarCargas() { }
    }
}