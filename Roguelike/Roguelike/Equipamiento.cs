using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Equipamiento que otorga bufos pasivos
    public class Equipamiento : Unico
    {
        public Equipamiento(int cod)
        {
            id = cod;
            coordX = 0;
            coordY = 0;
            conseguido = false;
            base.AsignarNombre(cod);
        }
    }
}