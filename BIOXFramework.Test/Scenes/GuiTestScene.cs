using System;
using BIOXFramework.Scene;

namespace BIOXFramework.Test.Scenes
{
    public class GuiTestScene : BIOXScene
    {
        public GuiTestScene(GameTest game)
            : base(game)
        {
            game.Window.Title = "GUI Test Scene";
        }
    }
}