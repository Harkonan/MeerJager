using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Screens
{
    class LogScreen : ContainerConsole
    {
        private Console LogConsole { get; }
        private int CurrentLine { get; set; }

        public LogScreen()
        {
            var ConsoleWidth = (int)((Global.RenderWidth / Global.FontDefault.Size.X) * 0.7);
            var ConsoleHeight = (int)((Global.RenderHeight / Global.FontDefault.Size.Y) * 1.0);

            LogConsole= new Console(ConsoleWidth, ConsoleHeight);
            LogConsole.Parent = this;

            LogConsole.DrawBox(new Rectangle(0, 0, LogConsole.Width, LogConsole.Height), new Cell(Color.White, Color.DarkGray, 0));
            CurrentLine = 2;
        }

        public void WriteToLog(string message)
        {
            LogConsole.Print(2, CurrentLine, message);
            CurrentLine++;
        }

        public void ClearLog()
        {
            LogConsole.Clear();
            LogConsole.DrawBox(new Rectangle(0, 0, LogConsole.Width, LogConsole.Height), new Cell(Color.White, Color.DarkGray, 0));
            CurrentLine = 2;
        }

    }
}
