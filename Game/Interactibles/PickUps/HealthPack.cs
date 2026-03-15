using JailBreaker.Destructibles;
using RocketEngine;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Interactibles
{
    public class HealthPack : PickUp
    {
        SpriteComponent renderer;

        protected int healthAmount = 20;
        public override void Construct()
        {
            base.Construct();

            Sprite healthPackSprite = new Sprite("Game/Assets/Textures/Healthpack-Sheet.png", 6);

            renderer = new SpriteComponent(this, healthPackSprite, Raylib_cs.Color.White); //game starts with leaver being off

            Name = "HealthPack";

			hitBox.Size = new Vector2(32, 32); //= new BoxCollider2D(this, width: leaverSprite.FrameWidth * renderer.SpriteScale, height: leaverSprite.FrameHeight * renderer.SpriteScale);

			interactPromptRenderer.TextureOffset = new Vector2(0, -18);

            pickUpSound.FilePath = "Game/Assets/Audio/PickUps/SodaSound.wav";
            

		}
        public override void Interact(GameObject other)
        {
            base.Interact(other);

            if (other.Name != "Player") return;

            ((IDestructable)other).IncreaseHealth(healthAmount);

			renderer.visible = false;

			PickUpItem();
        }
    }
}
