using Raylib_cs;
using RocketEngine;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Ui
{
    public class Crosshair : UiElement
    {
        public SpriteComponent renderer;
        private Sprite crosshairSprite;
        public override void Construct()
        {
            base.Construct();

            crosshairSprite = new Sprite("Game/Assets/Textures/Weapons/Crosshairv1.png", 1);
            renderer = new SpriteComponent(this, crosshairSprite, Raylib_cs.Color.White);
            //renderer.ZIndex = 2;
            renderer.SpriteScale = 2.0f;

            renderer.Name = "Crosshair";
        }

        
	}
}
