using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jaeger.Entities;
using MeerJager.Entities;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using SadConsole.Components;
using Microsoft.Xna.Framework.Input;
using MeerJager.Screens;
using MeerJager.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MeerJager
{
    class Program
    {
        internal static int Width = 150;
        internal static int Height = 50;
        internal static LogScreen CurrentLog;
        internal static Player Player;
        

        static void Main(string[] args)
        {
            
            SadConsole.Themes.Library.Default.ControlsConsoleTheme = new MyTheme();
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            Player = Player.GetPlayer;

            PhaseController.StartCombat();
        }



        
    }

    class MyTheme : SadConsole.Themes.ControlsConsoleTheme
    {
        public override void Draw(ControlsConsole console, CellSurface hostSurface)
        {
            this.FillStyle.Background = Color.Black;

            base.Draw(console, hostSurface);

        }
    }
}