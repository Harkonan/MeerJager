using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Screens
{
    class MessageScreen : ContainerConsole
    {
        public Console MessageConsole { get; }
        public string Message { get; set; }

        public MessageScreen()
        {
            var ConsoleWidth = (int)((Global.RenderWidth / Global.FontDefault.Size.X) * 1.0);
            var ConsoleHeight = (int)((Global.RenderHeight / Global.FontDefault.Size.Y) * 1.0);

            MessageConsole = new Console(ConsoleWidth, ConsoleHeight);
            MessageConsole.Parent = this;
        }


        public void RenderMessage()
        {
            MessageConsole.Print(1, 1, Message);
        }


        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Program.StartCombat();
                return true;
            }
            return false;
        }
    }
}
