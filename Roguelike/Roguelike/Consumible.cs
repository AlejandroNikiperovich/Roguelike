using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Pociones
    public class Consumible : Objeto
    {
        public Consumible(int cod)
        {
            id = cod;
            base.AsignarNombre(cod);
        }
    }
}