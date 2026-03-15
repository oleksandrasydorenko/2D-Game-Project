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
    public class WeaponIndicator : UiElement
    {
        public SpriteComponent renderer;

        public WeaponIndicator(Sprite sprite) : base()
        {
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White);
            renderer.Name = "WeaponIndicator";
        }
    }
}
