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
    public class SwitchWeaponPrompt : UiElement
    {
        public SpriteComponent renderer;
        private Sprite switchWeaponSprite;

        public UiElement.AnchoringPosition CurrentAnchor { get; internal set; }

        public override void Construct()
        {
            base.Construct();

            switchWeaponSprite = new Sprite("Game/Assets/Textures/Prompts/SwitchWeapon.png", 1);
            renderer = new SpriteComponent(this, switchWeaponSprite, Raylib_cs.Color.White);
            renderer.Name = "SwitchWeapon";
            renderer.SpriteScale = 3.0f;
        }
    }
}
