using JailBreaker.Destructibles;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Interactibles.WeaponPickUps
{
    public class PistolPickUp : PickUp
    {
        SpriteComponent renderer;
        public override void Construct()
        {
            base.Construct();

            Sprite weaponSprite = new Sprite("Game/Assets/Textures/Weapons/LaserPistol.png", 5);

            renderer = new SpriteComponent(this, weaponSprite, Raylib_cs.Color.White);

            Name = "PistolPickUp";

			hitBox.Size = new Vector2(32, 32); //= new BoxCollider2D(this, width: leaverSprite.FrameWidth * renderer.SpriteScale, height: leaverSprite.FrameHeight * renderer.SpriteScale);

			interactPromptRenderer.TextureOffset = new Vector2(0, -18);

			pickUpSound.FilePath = "Game/Assets/Audio/Weapons/PickUpWeapon.mp3";


		}
		public override void Interact(GameObject other)
        {
            base.Interact(other);

            if (other.Name != "Player") return;

            LaniasPlayer player = other as LaniasPlayer;

            if (player == null) return;
            
            player.weapons.EquipWeapon(WeaponType.Pistol,player);

            renderer.visible = false;
            PickUpItem();
        }
    }
}
