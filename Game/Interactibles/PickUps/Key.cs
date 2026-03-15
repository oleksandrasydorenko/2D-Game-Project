using JailBreaker.Destructibles;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
 //event: enter door feuern, dann in in der szene die tür subscriben
namespace JailBreaker.Interactibles
{
    public class Key: PickUp
    {
        SpriteComponent renderer;

        public override void Construct()
        {
            base.Construct();

            Sprite keySprite = new Sprite("Game/Assets/Textures/key-Sheet.png", 1);

            renderer = new SpriteComponent(this, keySprite, Raylib_cs.Color.White); //game starts with leaver being off
            renderer.ZIndex = 1;

            Name = "Key";

			hitBox.Size = new Vector2(32, 32); //= new BoxCollider2D(this, width: leaverSprite.FrameWidth * renderer.SpriteScale, height: leaverSprite.FrameHeight * renderer.SpriteScale);

			interactPromptRenderer.TextureOffset = new Vector2(0, -16);

			pickUpSound.FilePath = "Game/Assets/Audio/PickUps/KeyCardSound.wav";
		}
        public override void Interact(GameObject other)
        {
            base.Interact(other);

            if (other.Name != "Player") return;
            LaniasPlayer player = other as LaniasPlayer;

            if (player == null) return;

            player.hasKey = true;
            Console.WriteLine($"{other.Name} ++++++++++++++++++picked up key");

			renderer.visible = false;

			PickUpItem();

		}
    }
}
