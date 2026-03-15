using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;
using RocketEngine;
using System.Linq.Expressions;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System.Security.Cryptography.X509Certificates;

namespace JailBreaker.Ui
{
    /// <summary>
    /// PauseMenuPanel, allows to continue the game or switch to main menu
    /// </summary>
    public class MainMenuBackgroundPanel : UiElement
    {

        public override void Construct()
        {
            base.Construct();

            Sprite backgroundSprite = new Sprite("Game/Assets/Textures/Menu/MenuBackgroundTest.png");
            SpriteComponent background = new SpriteComponent(this, backgroundSprite, Color.DarkGray);


            int screenWidth = Raylib.GetScreenWidth();
            float factor = (float)screenWidth / backgroundSprite.FrameWidth;
            background.SpriteScale = factor;

        }
    }
}
