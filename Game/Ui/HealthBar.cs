using JailBreaker.Data;
using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;

namespace JailBreaker.Ui
{
    public class HealthBar : UiElement
    {
        public SpriteComponent spriteComponent;
        private Sprite healthbarSprite;
        public Sprite HealthbarSprite
        {
            get { return healthbarSprite; }
            set
            {
                healthbarSprite = value;
                spriteComponent.sprite = healthbarSprite;
            }
        }
        public HealthBar(Sprite sprite = null, float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.LeftTop, string name = "HealthBar") : base(x, y, anchor, name)
        {
            this.healthbarSprite = sprite;
            if (healthbarSprite == null)
            {
                this.healthbarSprite = new Sprite("Game/Assets/Textures/healthBar.png", 6);
            }
            this.spriteComponent = new SpriteComponent(this, healthbarSprite, new Raylib_cs.Color(0, 255, 255));
            this.spriteComponent.SpriteScale = 2f;
        }
    }
}
