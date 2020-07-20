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
        public System.Action Action { get; set; }

        public MessageScreen(string Message, System.Action _Action, Color? MessageBackground, Color? MessageBorder, Color? TextBackground, Color? TextForground)
        {
            Action = _Action;

            if (!MessageBackground.HasValue)
            {
                MessageBackground = Color.Black;
            }
            if (!MessageBorder.HasValue)
            {
                MessageBorder = Color.Navy;
            }

            if (!TextBackground.HasValue)
            {
                TextBackground = Color.Transparent;
            }
            if (!TextForground.HasValue)
            {
                TextForground = Color.White;
            }

            string ContinueMessage = "[Press Space to Continue]";

            var ConsoleWidth = System.Math.Max(Message.Length, ContinueMessage.Length) + 4;
            var ConsoleHeight = 5;

            MessageConsole = new Console(ConsoleWidth, ConsoleHeight);
            MessageConsole.Parent = this;

            int ConsoleX = (int)((Global.RenderWidth / Global.FontDefault.Size.X) - ConsoleWidth) / 2;
            int ConsoleY = (int)((Global.RenderHeight / Global.FontDefault.Size.Y) - ConsoleHeight) / 2;
            this.Position = new Point(ConsoleX, ConsoleY);

            MessageConsole.Fill(MessageBackground, null, null);
            MessageConsole.DrawBox(new Rectangle(0, 0, ConsoleWidth, ConsoleHeight), new Cell(Color.White, MessageBorder.Value, 0));
            MessageConsole.Print(2, 2, Message, TextForground.Value, TextBackground.Value);
            MessageConsole.Print(2, ConsoleHeight-1, ContinueMessage);

            Global.CurrentScreen = this;
            Global.CurrentScreen.IsFocused = true;
        }


        public void RenderMessage()
        {

        }


        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space) || info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                Action();
                return true;
            }
            return false;
        }
    }
}
