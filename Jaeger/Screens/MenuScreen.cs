using SadConsole;
using SadConsole.Input;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using MeerJager.Entities;
using System.Linq;

namespace MeerJager.Screens
{
    class MenuScreen : ContainerConsole
    {
        private Console MenuConsole { get; }
        private Console BorderConsole { get;  }
        internal List<MenuOption> Menu { get; set; } = new List<MenuOption>();


        public MenuScreen()
        {
            var ConsoleWidth = (int)((Global.RenderWidth / Global.FontDefault.Size.X) * 0.30);
            var ConsoleHeight = (int)((Global.RenderHeight / Global.FontDefault.Size.Y) * 1.0);

            Menu = new List<MenuOption>();
       
            MenuConsole = new Console(ConsoleWidth-2, ConsoleHeight-2);
            MenuConsole.Parent = this;
            MenuConsole.Position = new Point(MenuConsole.Position.X + 1, MenuConsole.Position.Y + 1);

            BorderConsole = new Console(ConsoleWidth, ConsoleHeight);
            BorderConsole.Position = new Point(MenuConsole.Position.X - 2, MenuConsole.Position.Y - 2);
            BorderConsole.DrawBox(new Rectangle(0, 0, BorderConsole.Width, BorderConsole.Height), new Cell(Color.White, Color.Navy, 0));
            BorderConsole.Parent = this;
            BorderConsole.IsVisible = true;
            MenuConsole.Children.Add(BorderConsole);

        }


        public void AddOption(MenuOption Option)
        {
            Menu.Add(Option);
        }

        public void ClearMenu()
        {
            Menu.Clear();
            RenderMenuOptions(); 
        }

        public void RenderMenuOptions()
        {
            MenuConsole.Clear();
            

            int StartLine = 1;

            foreach (var option in Menu)
            {

                string OptionText = string.Format("{0} [{1}]", option.Display, (option.Key.ToString().Length > 1 ? option.Key.ToString().TrimStart('D') : option.Key.ToString()));
                int Lines = (int)System.Math.Ceiling((double)OptionText.Length / (double)MenuConsole.Width);

                Color background = option.Selected ? option.Forground : option.Background;
                Color forground = option.Selected ? option.Background : option.Forground;
                
                MenuConsole.Print(0, StartLine, OptionText , forground, background);
                option.X = 0;
                option.Y = StartLine;

                StartLine += Lines+1;

            }
        }

        public override bool ProcessKeyboard(Keyboard info)
        {

            MenuOption CurrentlySelectedOption = new MenuOption()
            {
                Id = 0
            };

            if (Menu.Any(x => x.Selected))
            {
                CurrentlySelectedOption = Menu.Where(x => x.Selected).FirstOrDefault();
            }

            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up) || info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {


                if (Menu.Any(x => x.Id == CurrentlySelectedOption.Id - 1))
                {
                    Menu.Where(x => x.Id == CurrentlySelectedOption.Id - 1).FirstOrDefault().Selected = true;
                    CurrentlySelectedOption.Selected = false;
                }

            }

            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down) || info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {

                
                if (Menu.Any(x => x.Id == CurrentlySelectedOption.Id + 1))
                {
                    Menu.Where(x => x.Id == CurrentlySelectedOption.Id + 1).FirstOrDefault().Selected = true;
                    CurrentlySelectedOption.Selected = false;
                }

            }


            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter) && Menu.Any(x => x.Selected))
            {
                CurrentlySelectedOption.Action();
                return true;
            }

            foreach (var Item in Menu)
            {
                if (info.IsKeyPressed(Item.Key))
                {
                    Item.Action();
                    CurrentlySelectedOption.Selected = false;
                    Item.Selected = true;
                    return true;
                }
            }

            if (CurrentlySelectedOption != Menu.Where(x => x.Selected).FirstOrDefault())
            {
                RenderMenuOptions();
                return true;
            }
            return false;
        }
    }
}
