using MeerJager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaeger.Entities
{
    public static class UIScreen
    {
        public static List<string> DisplayLines { get; set; } = new List<string>();
        public static List<MenuOption> Menu { get; set; } = new List<MenuOption>();
        public static string MenuTitle { get; set; }
        private static int Height { get; set; }

        public static void RenderScreen()
        {
            Height = Console.WindowHeight;

            if (DisplayLines.Count > 0)
            {
                WriteLine();
                foreach (var line in DisplayLines)
                {
                    WriteLine(line);
                }
                WriteLine();
            }

            for (int i = 0; i < (Height - Menu.Count) + 2; i++)
            {
                WriteLine();
            }

            if (!string.IsNullOrEmpty(MenuTitle))
            {
                WriteLine();
                WriteLine();
                WriteLine(MenuTitle);
            }

            if (Menu.Count > 0)
            {
                WriteLine();
                foreach (var option in Menu.OrderBy(x => x.Id))
                {
                    Console.ForegroundColor = option.Forground;
                    Console.BackgroundColor = option.Background;
                    WriteLine(string.Format("{0}. {1} [{2}]", option.Id, option.Display, option.Key));
                    Console.ResetColor();
                }
            }

            Boolean isValidKey = false;
            do
            {
                Char key = Console.ReadKey().KeyChar;
                if (Menu.Any(x => char.ToLower(x.Key ?? ' ') == char.ToLower(key)))
                {
                    isValidKey = true;
                    Console.Clear();
                    DisplayLines.Clear();
                    Menu.Where(x => char.ToLower(x.Key ?? ' ') == char.ToLower(key)).FirstOrDefault().Action();
                    Menu.Clear();
                }
            } while (!isValidKey);
        }

        private static void WriteLine(string line)
        {
            Console.WriteLine(line);
            Height--;
        }
        private static void WriteLine()
        {
            Console.WriteLine();
            Height--;
        }

    }
}
