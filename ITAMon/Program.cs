using System;
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
            while(!checkMenuInput(input, menuTexts.GetLength(0)))
            {
                Console.WriteLine($"Bitte einen der Menüpunkte auswählen! (1 - {menuTexts.GetLength(0) - 1})");
                input = Console.ReadLine();
            }

            //Run menu-command
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
        /// Checks wether an input is a valid input (accessing one of the menu points)
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>True if input is valid else false</returns>
        private static bool checkMenuInput(string input, int menuLenght)
        {
            int number;

            if (!int.TryParse(input, out number))
            {
                return false;
            }

            if (number >= menuLenght - 1)
            {
                return false;
            }

            return true;
        }
    }
}
