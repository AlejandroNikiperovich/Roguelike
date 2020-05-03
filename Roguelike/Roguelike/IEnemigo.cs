using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    public interface IEnemigo
    {
        int id { get; set; }
        int vida { get; set; }
        int maxVida { get; set; }
        int evasion { get; set; }
        int proteccion { get; set; }
        String ascii { get; set; }
        String nombre { get; set; }
        String tipo { get; set; }
        void AsignarStats(int cod);
    }
}