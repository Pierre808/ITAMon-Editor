using System;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace ITAMon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            createHeader("ITAMon Editor", 8);

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
            while(!checkIntInput(input, menuTexts.GetLength(0)))
            {
                Console.WriteLine($"Bitte einen der Menüpunkte auswählen! (1 - {menuTexts.GetLength(0)})");
                input = Console.ReadLine();
            }

            //Run menu-command
            if(input == "1")
            {
                Console.WriteLine("ITAMon bearbeiten:");
            }
            else if(input == "2")
            {
                Console.WriteLine("ITAMon erstellen:");
                Console.WriteLine();

                Console.WriteLine("Wo soll die ITAMon Datei gespeichert werden?");

                var path = Console.ReadLine();
                var success = FileManager.CheckPath(path);

                while (!success)
                {
                    Console.WriteLine("Die Datei konnte nicht erstellt werden. Bitte erneut den Pfad angeben");

                    path = Console.ReadLine();
                    success = FileManager.CheckPath(path);
                }

                Console.WriteLine("Wie soll das ITAMon heißen?");

                var name = Console.ReadLine();

                Console.WriteLine("Bitte gib den Dateipfad des Erscheinungsbildes des ITAMons an");

                var imagePath = Console.ReadLine();
                success = FileManager.CheckIfImage(imagePath);

                while(success == false)
                {
                    Console.WriteLine("Die angegebene Datei ist kein Bild. Bitte erneut den Pfad eingeben");

                    imagePath = Console.ReadLine();
                    success = FileManager.CheckIfImage(imagePath);
                }

                Console.WriteLine("Welchen der Typen soll das ITAMon haben?");
                var enumValues = Enum.GetNames(typeof(Typ));
                for (int i = 0; i < enumValues.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {enumValues[i]}");
                }

                var typInt = Console.ReadLine();

                while(!checkIntInput(typInt, enumValues.Length))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben");

                    typInt = Console.ReadLine();
                }

                var typ = enumValues[int.Parse(typInt) - 1];

                Itamon itamon = new Itamon();
                itamon.Name = name;
                itamon.ImagePath = imagePath;
                itamon.Typ = typ;

                Console.WriteLine("Datei wird erstellt...");
                success = itamon.SafeItamon(path);
                if (success)
                    Console.WriteLine("Datei erfolgreich erstellt.");
                else
                    Console.WriteLine("Es ist ein Fehler aufgetreten...");
            }
            else if(input == "3")
            {
                Console.WriteLine("Namen bearbeiten:");
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
            for(int i = 0; i < menuTexts.GetLength(0); i++)
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
        /// <returns>True if input is valid else false</returns>
        private static bool checkIntInput(string input, int max)
        {
            int number;

            if (!int.TryParse(input, out number))
            {
                return false;
            }

            if (number > max || number <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
