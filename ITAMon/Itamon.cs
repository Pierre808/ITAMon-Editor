using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAMon
{
    internal class Itamon
    {
        public string Name { get; set; }
        public Bitmap Image { get; set; }
        public Typ Typ { get; set; }
        public int Level { get; set; }
        public double Health { get; set; }
        public int ExperiencePoints { get; set; }
        public int ActionPoints { get; set; }
        public Attack[] Attacks { get; set; }

        public Itamon() 
        {
            this.Level = 1;
            this.Health = 10;
            this.ExperiencePoints = 0;
            this.ActionPoints = 100;
            this.Attacks = new Attack[4];
        }

        public Itamon(string name, Bitmap image, Typ typ, int level, double health, int experiencePoints, int actionPoints)
        {
            this.Name = name;
            this.Image = image;
            this.Typ = typ; 
            this.Level = level;
            this.Health = health;
            this.ExperiencePoints = experiencePoints;
            this.ActionPoints = actionPoints;

            //TODO: load attacks
        }
    }
}
