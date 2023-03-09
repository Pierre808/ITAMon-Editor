using System;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace ITAMon
{
    internal class Program
    {
        private static Itamon ActiveItamon;

        static void Main(string[] args)
        {
            createHeader("ITAMon Editor", 8);

            while(true)
                readMenu();
        }

        private static void readMenu()
        {
            string[,] menuTexts =
            {
                {"ITAMon bearbeiten", "Läd eine bereits existierende ITAMon Datei setzt das enthaltene ITAMon als aktuelles ITAMon" },
                {"ITAMon erstellen", "Erstellt eine ITAMon Datei und setzt diese als aktuelles ITAMon" },
                {"Namen des aktuellen ITAMons bearbeiten", "Bearbeitet den Namen des aktuellen ITAMons" },
                {"Bild des aktuellen ITAMons ändern", "Ersetzt das Bild des aktuellen ITAMons" },
                {"Änderungen speichern", "Speichert die vorgenommen Änderungen" },
                {"Help", "Ein ITAMon kann mithilfe dieses Editors bearbeitet/erstellt werden. " +
                    "Das zuletzt importierte/erstelle ITAMon stellt das aktuelle ITAMon dar und kann über die aufgelisteten Befehle bearbeitet werden." +
                    "Für mehr Infos zu den jeweiligen Befehlen: Help [Befehl Index] " }
            };
            createMenu(menuTexts);

            var input = Console.ReadLine();
            while (checkIntInput(input, menuTexts.GetLength(0)) != 1)
            {
                Console.WriteLine($"Bitte einen der Menüpunkte auswählen! (1 - {menuTexts.GetLength(0)})");
                input = Console.ReadLine();
            }

            //Run menu-command
            if (input == "1")
            {
                Console.WriteLine("ITAMon bearbeiten:");
                Console.WriteLine();

                Console.WriteLine("Wo ist die ITAMon Datein gespeichert?");

                var path = Console.ReadLine();

                while(FileManager.ReadITAFile(path).Equals(""))
                {
                    Console.WriteLine("Die Datei konnte nicht geladen werden." +
                        "\nIst es eine .ITA Datei? Bitte erneut den Pfad eingeben");

                    path = Console.ReadLine();
                }

                ActiveItamon = new Itamon();

                var success = ActiveItamon.LoadItamon(path);

                if(success != "")
                {
                    Console.WriteLine(success + " Vorgang abgebrochen");
                    return;
                }

                Console.WriteLine("ITAMon erfolgreich importiert. Du kannst es nun bearbeiten");
            }
            else if (input == "2")
            {
                Console.WriteLine("ITAMon erstellen:");
                Console.WriteLine();

                CreateItamon();
            }
            else if (input == "3")
            {
                Console.WriteLine("Namen bearbeiten:");
                Console.WriteLine();

                if (ActiveItamon == null)
                {
                    Console.WriteLine("Kein aktives ITAMon vorhanden. Bitte erst erstellen oder laden");

                    return;
                }

                SetName();
            }
            else if (input == "4")
            {
                Console.WriteLine("Bild bearbeiten:");
                Console.WriteLine();
            }
            else if (input == "5")
            {
                Console.WriteLine("Änderungen speichern:");
                Console.WriteLine();
            }
            else if (input == "6")
            {
                Console.WriteLine("Help:");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Creates a header
        /// </summary>
        /// <param name="content">Header text</param>
        /// <param name="spacing">Spacing between text and beginning/ending</param>
        private static void createHeader(string content, int spacing)
        {
            string header = "";


            for (int i = 0; i < 5; i++)
            {
                header += "|";

                if (i == 0 || i == 4)
                {
                    header += new String('-', content.Length + spacing * 2);
                    header += "|\n";
                    continue;
                }

                header += new String(' ', spacing);
                if (i == 2)
                {
                    header += content;
                }
                else
                {
                    header += new string(' ', content.Length);
                }
                header += new String(' ', spacing);

                header += "|\n";
            }

            Console.WriteLine(header);
        }

        private static void createMenu(string[,] menuTexts)
        {
            Console.WriteLine();

            for (int i = 0; i < menuTexts.GetLength(0); i++)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"] {menuTexts[i, 0]} \n");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Checks wether an input is a valid integer input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>1 if input is valid, 0 if not an int, -1 if too big and -2 if too small</returns>
        private static int checkIntInput(string input, int max)
        {
            int number;

            if (!int.TryParse(input, out number))
            {
                return 0;
            }

            if (number > max)
            {
                return -1;
            }
            if (number <= 0)
            {
                return -2;
            }

            return 1;
        }


        private static void CreateItamon()
        {
            ActiveItamon = new Itamon();

            Console.WriteLine("Wo soll die ITAMon Datei gespeichert werden?");

            var path = Console.ReadLine();
            var success = FileManager.CheckPath(path);

            while (!success)
            {
                Console.WriteLine("Die Datei konnte nicht erstellt werden. Bitte erneut den Pfad angeben");

                path = Console.ReadLine();
                success = FileManager.CheckPath(path);
            }

            SetName();

            SetImage();

            SetTyp();

            var attackAmount = 0;
            for (int i = 0; i < ActiveItamon.Attacks.Length; i++)
            {
                if (ActiveItamon.Attacks[i] != null)
                {
                    attackAmount++;
                }
            }
            Console.WriteLine($"Das ITAMon besizt {attackAmount}/4 Attacken");
            for (int i = 0; i < ActiveItamon.Attacks.Length; i++) 
            {
                Console.WriteLine();
                Console.WriteLine($"{i + 1}. Attacke: ");
                SetAttack(i);
            }

            Console.WriteLine();

            Console.WriteLine("Datei wird erstellt...");
            success = ActiveItamon.SafeItamon(path);
            if (success)
                Console.WriteLine("Datei erfolgreich gespeichert.");
            else
                Console.WriteLine("Es ist ein Fehler aufgetreten...");
        }



        private static void SetName()
        {
            Console.WriteLine("Wie soll das ITAMon heißen?");

            var name = Console.ReadLine();

            ActiveItamon.Name = name;
        }

        private static void SetImage()
        {
            Console.WriteLine("Bitte gib den Dateipfad des Erscheinungsbildes des ITAMons an");

            var imagePath = Console.ReadLine();
            var success = FileManager.CheckIfImage(imagePath);

            while (success == false)
            {
                Console.WriteLine("Die angegebene Datei ist kein Bild. Bitte erneut den Pfad eingeben");

                imagePath = Console.ReadLine();
                success = FileManager.CheckIfImage(imagePath);
            }

            ActiveItamon.ImagePath = imagePath;
        }

        private static void SetTyp()
        {
            Console.WriteLine("Welchen der Typen soll das ITAMon haben?");
            var enumValues = Enum.GetNames(typeof(Typ));
            for (int i = 0; i < enumValues.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {enumValues[i]}");
            }

            var typInt = Console.ReadLine();

            while (checkIntInput(typInt, enumValues.Length) != 1)
            {
                Console.WriteLine("Bitte eine gültige Zahl eingeben");

                typInt = Console.ReadLine();
            }

            var typ = enumValues[int.Parse(typInt) - 1];

            ActiveItamon.Typ = typ;
        }

        private static void SetAttack(int index)
        {
            var usedActionPoints = 0;
            foreach(var attack in ActiveItamon.Attacks)
            {
                if(attack == null)
                {
                    continue;
                }

                usedActionPoints += attack.Damage + attack.Accuracy + attack.Magazine;
            }

            var totalActionPoints = 100 * ActiveItamon.Attacks.Length + (6 * ActiveItamon.Level) - 6;
            var leftActionPoints = totalActionPoints - usedActionPoints;

            Console.WriteLine($"{leftActionPoints} Aktionspunkte übrig.");

            Console.WriteLine("Wie soll die Attacke heißen");
            var name = Console.ReadLine();


            Console.WriteLine("Wieviel Schaden soll die Attacke Machen?");
            var damage = Console.ReadLine();

            var result = checkIntInput(damage, leftActionPoints);

            while (result != 1)
            {
                if(result == 0)
                {
                    Console.WriteLine("Die Eingabe ist keine Zahl. Bitte erneut eingeben.");
                }
                else if(result == -1)
                {
                    Console.WriteLine("Nicht genügend Aktionspunkte vorhanden. Bitte erneut eingeben");
                }
                else if(result == -2)
                {
                    Console.WriteLine("Der Schaden muss größer als 0 sein. Bitte erneut eingeben");
                }

                damage = Console.ReadLine();

                result = checkIntInput(damage, leftActionPoints);
            }

            leftActionPoints -= int.Parse(damage);


            Console.WriteLine("Wie genau soll die Attacke sein?");
            var accuracy = Console.ReadLine();

            result = checkIntInput(accuracy, leftActionPoints);

            while (result != 1)
            {
                if (result == 0)
                {
                    Console.WriteLine("Die Eingabe ist keine Zahl. Bitte erneut eingeben.");
                }
                else if (result == -1)
                {
                    Console.WriteLine("Nicht genügend Aktionspunkte vorhanden. Bitte erneut eingeben");
                }
                else if (result == -2)
                {
                    Console.WriteLine("Die Genauigkeit muss größer als 0 sein. Bitte erneut eingeben");
                }

                accuracy = Console.ReadLine();

                result = checkIntInput(accuracy, leftActionPoints);
            }

            leftActionPoints -= int.Parse(accuracy);


            Console.WriteLine("Wieviel Magazin soll die Attacke haben?");
            var magazine = Console.ReadLine();

            result = checkIntInput(magazine, leftActionPoints);

            while (result != 1)
            {
                if (result == 0)
                {
                    Console.WriteLine("Die Eingabe ist keine Zahl. Bitte erneut eingeben.");
                }
                else if (result == -1)
                {
                    Console.WriteLine("Nicht genügend Aktionspunkte vorhanden. Bitte erneut eingeben");
                }
                else if (result == -2)
                {
                    Console.WriteLine("Das Magazin muss größer als 0 sein. Bitte erneut eingeben");
                }

                magazine = Console.ReadLine();

                result = checkIntInput(magazine, leftActionPoints);
            }

            leftActionPoints -= int.Parse(magazine);


            ActiveItamon.Attacks[index] = new Attack(name,int.Parse(damage) , int.Parse(accuracy), int.Parse(magazine));
            
        }
    }
}
