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
            this.Attacks = new Attack[4];
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
            //this.Attacks = new Attack[4];
        }

        public bool AddAttack(Attack att)
        {
            for(int i = 0; i < Attacks.Length; i++)
            {
                if(Attacks[i] == null)
                {
                    Attacks[i] = att;
                    return true;
                }
            }

            return false;
        }

        public bool SafeItamon(string path)
        {
            string content = $"{Name}\n" +
                $"{ImagePath}\n" +
                $"{this.Typ};{Level};{Health};{ExperiencePoints}\n";

            for(int i = 0; i < Attacks.Length; i++)
            {
                Attack attack = Attacks[i];
                content += $"{attack.Name};{attack.Damage};{attack.Accuracy};{attack.Magazine}\n";
            }

            return FileManager.CreateITAFile(path, Name, content);
        }

        public string LoadItamon(string path)
        {
            var content = FileManager.ReadITAFile(path);

            if(content == "")
            {
                return "Datei ist leer.";
            }

            string[] linesResult = content.Split("\n");
            string[] lines = new string[0];

            for (int i = 0; i < linesResult.Length; i++)
            {
                if (linesResult[i] != "" && linesResult[i] != " ")
                {
                    Array.Resize(ref lines, lines.Length + 1);
                    var result = linesResult[i];
                    if(result.EndsWith("\r"))
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    lines[i] = result;
                }
            }

            if(lines.Length != 7)
            {
                return "Datei ist fehlerhaft.";
            }

            this.Name = lines[0];
            this.ImagePath = lines[1];
            
            string[] third = lines[2].Split(";");
            if(third.Length != 4)
            {
                return "Datei ist fehlerhaft. Überprüfe die dritte Zeile.";
            }
            this.Typ = third[0];
            int level;
            var levelInt = int.TryParse(third[1], out level);
            if(levelInt == false)
            {
                return "Datei ist fehlerhaft. Überprüfe das Level in der dritten Zeile.";
            }
            this.Level = level;

            int health;
            var healthInt = int.TryParse(third[2], out health);
            if (healthInt == false)
            {
                return "Datei ist fehlerhaft. Überprüfe die Leben in der dritten Zeile.";
            }
            this.Health = health;

            int xp;
            var xpInt = int.TryParse(third[3], out xp);
            if (xpInt == false)
            {
                return "Datei ist fehlerhaft. Überprüfe die XP in der dritten Zeile.";
            }
            this.ExperiencePoints = xp;


            for(int i = 0; i < 4; i++)
            {
                string[] attack = lines[3 + i].Split(";");  
                if(attack.Length != 4)
                {
                    return $"Datei ist fehlerhaft. Überprüfe die {3 + i + 1}. Zeile.";
                }

                var attName = attack[0];
                int[] attStats = new int[3];
                for(int j = 0; j < 3; j++)
                {
                    int value;
                    if(!int.TryParse(attack[1 + j], out value))
                    {
                        return $"Datei ist fehlerhaft. Überprüfe die Werte in der {3 + i + 1}. Zeile.";
                    }
                    attStats[j] = value;
                }

                Attack att = new Attack(attName, attStats[0], attStats[1], attStats[2]);
                this.Attacks[i] = att;
            }

            return "";
        }
    }
}
