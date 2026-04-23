using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace UHFAPP.Entity
{
    internal class AntennaPowerInfo
    {
        public AntennaPowerInfo(int ant, int power)
        {
            this.ant = ant;
            this.power = power;
        }
        private int ant;
        private int power;

        public int Ant { get => ant; set => ant = value; }
        public int Power { get => power; set => power = value; }
    }
}
