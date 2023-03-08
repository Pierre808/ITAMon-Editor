using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAMon
{
    internal class Attack
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Accuracy { get; set; }
        public int Magazine { get; set; }

        public Attack(string name, int damage, int accuracy, int magazine)
        {
            Name = name;
            Damage = damage;
            Accuracy = accuracy;
            Magazine = magazine;
        }
    }
}
