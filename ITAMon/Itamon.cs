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
        public string ImagePath { get; set; }
        public string Typ { get; set; }
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
        }

        public Itamon(string name, string imagePath, string typ, int level, double health, int experiencePoints, int actionPoints)
        {
            this.Name = name;
            this.ImagePath = imagePath;
            this.Typ = typ; 
            this.Level = level;
            this.Health = health;
            this.ExperiencePoints = experiencePoints;
            this.ActionPoints = actionPoints;

            //TODO: load attacks
        }

        public bool SafeItamon(string path)
        {
            string content = $"{Name}\n" +
                $"{ImagePath}\n" +
                $"{this.Typ};{Level};{Health};{ExperiencePoints}\n";

            //TODO: Attacks

            return FileManager.CreateITAFile(path, Name, content);
        }
    }
}
