using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Hechizos
    public class Hechizo : Unico
    {
        public int cargasMax, cargasActuales;

        public Hechizo(int cod)
        {
            id = cod;
            coordX = 0;
            coordY = 0;
            conseguido = false;
            base.AsignarNombre(cod);
            AsignarCargas();
            cargasActuales = cargasMax;
        }

        // Cargas maximas de un hechizo segun su id
        public void AsignarCargas()
        {
            switch (id)
            {
                case NombreObjeto.rayos:
                    cargasMax = 10;
                    break;
                case NombreObjeto.tp:
                    cargasMax = 5;
                    break;
                case NombreObjeto.levitar:
                    cargasMax = 5;
                    break;
                case NombreObjeto.regenerarHP:
                    cargasMax = 5;
                    break;
                case NombreObjeto.temblor:
                    cargasMax = 5;
                    break;
            }
        }

        // Dibuja las cargas de un hechizo en consola
        public override void MostrarCargas()
        {
            Console.WriteLine(cargasActuales + "/" + cargasMax + " ");
        }

        // Devuelve true si el hechizo no se ha quedado sin cargas
        public override bool TieneCargas()
        {
            if (cargasActuales > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Reduce las cargas actuales
        public override void ReducirCargas()
        {
            cargasActuales--;
        }

        // Restaura las cargas de un hechizo a su maximo
        public override void RestaurarCargas()
        {
            cargasActuales = cargasMax;
        }
    }
}