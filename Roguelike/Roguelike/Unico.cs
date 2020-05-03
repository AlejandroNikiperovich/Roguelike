using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    // Items unicos
    public abstract class Unico : Objeto
    {
        public int coordX, coordY;
        public bool conseguido;
    }
}