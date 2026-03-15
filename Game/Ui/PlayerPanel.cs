using JailBreaker.Data;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Ui;
using JailBreaker.Player;
using JailBreaker.Weapons;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Settings;
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
    public class PlayerPanel : UiElement
    {
        private UiText playerHealth;
        private HealthBar health;
        private int currentDurability;
        public SpriteComponent HealthOverlay;
        public EnergyAmmo[] energyAmmos = new EnergyAmmo[0];
        public Crosshair crosshair;
        public SwitchWeaponPrompt switchWeaponPrompt;
        public WeaponIndicator weaponIndicator1;
        public WeaponIndicator weaponIndicator2;
        public override void Construct()
        {
            base.Construct();
            playerHealth = InstanceService.Instantiate(new UiText(Color.White, $"Health: 0", 20, 10, 10, AnchoringPosition.LeftTop));
            health = InstanceService.Instantiate(new HealthBar(x: 70, y: 70));
            crosshair = InstanceService.Instantiate(new Crosshair());
            crosshair.CurrentAnchor = AnchoringPosition.LeftTop;

            Sprite backgroundSprite = new Sprite("Game/Assets/Textures/BGPause.png");
            HealthOverlay = new SpriteComponent(this, backgroundSprite, new Color(200, 0, 0, 0));

            int screenWidth = DisplaySettings.WINDOW_WIDTH;
            float factor = (float)screenWidth / backgroundSprite.FrameWidth;
            HealthOverlay.SpriteScale = factor + 1;
            switchWeaponPrompt = InstanceService.Instantiate(new SwitchWeaponPrompt());
            switchWeaponPrompt.CurrentAnchor = AnchoringPosition.LeftTop;

            switchWeaponPrompt.SetPositionX(550);
            switchWeaponPrompt.SetPositionY(-300);
            weaponIndicator1 = new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/FistIcon.png"));
            weaponIndicator2 = new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/FistIcon.png"));

        }

        public override void Start()
        {
            weaponIndicator1.CurrentAnchor = AnchoringPosition.RightTop;
            weaponIndicator1.renderer.SpriteScale = 3.0f;
            weaponIndicator1.SetPositionX(-220);
            weaponIndicator1.SetPositionY(85);
            weaponIndicator2.CurrentAnchor = AnchoringPosition.RightTop;
            weaponIndicator2.SetPositionX(-60);
            weaponIndicator2.SetPositionY(85);
            weaponIndicator2.renderer.SpriteScale = 2.0f;
        }

        /// <summary>
        /// changes frames of healthbar sprite based on players remaining health
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="maxHealth"></param>
        public void UpdateHealthText(int amount, int maxHealth)
        {
            playerHealth.Text = $"Health: {amount}";


            if(amount < 20)
            {
                float alpha = amount / 20f;
				Console.WriteLine("ALPHAAAAAAAA" + alpha);
				alpha = 1 - alpha;
                alpha = Math.Clamp(alpha, 0, 1);
                HealthOverlay.colorTint = new Color(200, 0, 0, (int)(alpha * 150));
			}
            else
            {
				HealthOverlay.colorTint = new Color(200, 0, 0,0);
			}

            switch (amount)
            {
                case int h when h > maxHealth - maxHealth / 5:
                    health.spriteComponent.sprite.CurrentFrame = 0;
                    break;
                case int h when h <= maxHealth - maxHealth / 5 && h > maxHealth - 2 * (maxHealth / 5):
                    health.spriteComponent.sprite.CurrentFrame = 1;
                    break;
                case int h when h <= maxHealth - 2 * (maxHealth / 5) && h > maxHealth - 3 * (maxHealth / 5):
                    health.spriteComponent.sprite.CurrentFrame = 2;
                    break;
                case int h when h <= maxHealth - 3 * (maxHealth / 5) && h > maxHealth - 4 * (maxHealth / 5):
                    health.spriteComponent.sprite.CurrentFrame = 3;
                    break;
                case int h when h <= maxHealth - 4 * (maxHealth / 5) && h > maxHealth - 5 * (maxHealth / 5):
                    health.spriteComponent.sprite.CurrentFrame = 4;
                    break;
                case int h when h <= maxHealth - 5 * (maxHealth / 5) && h > 0:
                    health.spriteComponent.sprite.CurrentFrame = 5;
                    break;

                case int h when h <= 0:
                    health.spriteComponent.sprite.CurrentFrame = 5;
                    break;
            }
        }


        public void UpdateWeaponAmmo(WeaponBase weapon,WeaponType secondWeapon )
        {
            if (energyAmmos != null)
            {
                for (int i = 0; i < energyAmmos.Length; i++)
                {
                    if (energyAmmos[i] != null)
                    {
                        energyAmmos[i].PlayUsedAnimation();
                        energyAmmos[i] = null;
                    }
                }
            }
            energyAmmos = new EnergyAmmo[weapon.durability];
            currentDurability = weapon.durability;
            float distanceBetween = 5 / (1 + 0.1f * weapon.durability ) ;
            for (int i = 0; i < weapon.durability; i++)
            {
                energyAmmos[i] = InstanceService.Instantiate(new EnergyAmmo());
                energyAmmos[i].CurrentAnchor = AnchoringPosition.RightTop;
                energyAmmos[i].SetPositionX(-30 - i * (energyAmmos[i].renderer.sprite.FrameWidth + distanceBetween));
                energyAmmos[i].SetPositionY(40);
            }

            if (weaponIndicator1 != null)
            {
                InstanceService.Destroy(weaponIndicator1);
                weaponIndicator1 = null;
            }
            if (weaponIndicator2 != null)
            {
                InstanceService.Destroy(weaponIndicator2);
                weaponIndicator2 = null;
            }
            if(weapon.renderer == null)
            {
                weaponIndicator1 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/PlaceholderWeapon.png")));
            }
            else 
            { 
                weaponIndicator1 = InstanceService.Instantiate(new WeaponIndicator(weapon.renderer.sprite)); 
            }
            weaponIndicator1.CurrentAnchor = AnchoringPosition.RightTop;
            if (weapon.type == WeaponType.PlasmaSword)
                weaponIndicator1.renderer.TextureOffset = new Vector2(32, 0);
            else
            {
                weaponIndicator1.renderer.TextureOffset = new Vector2(0, 0);
            }
            weaponIndicator1.renderer.SpriteScale = 3.0f;
            weaponIndicator1.SetPositionX(-220);
            weaponIndicator1.SetPositionY(85);

            switch (secondWeapon)
            {
                case WeaponType.Fists:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/FistIcon.png")));
                    break;
                case WeaponType.Pistol:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/LaserPistol.png",5)));
                    break;
                case WeaponType.PlasmaLauncher:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/PlasmaLauncher.png",11)));
                    break;
                case WeaponType.MachineGun:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/LaserMachineGun.png",4)));
                    break;
                case WeaponType.Shotgun:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/LaserShotGun.png",6)));
                    break;
                case WeaponType.Sniper:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/LaserSniper.png",3)));
                    break;
                case WeaponType.PlasmaSword:
                    weaponIndicator2 = InstanceService.Instantiate(new WeaponIndicator(new Sprite("Game/Assets/Textures/Weapons/PlasmaSword.png",8)));
                    break;
            }
            weaponIndicator2.CurrentAnchor = AnchoringPosition.RightTop;
            weaponIndicator2.SetPositionX(-60);
            weaponIndicator2.SetPositionY(85);
            if (secondWeapon == WeaponType.PlasmaSword)
                weaponIndicator2.renderer.TextureOffset = new Vector2(32, 0);
            else
            {
                weaponIndicator2.renderer.TextureOffset = new Vector2(0, 0);
            }
                weaponIndicator2.renderer.SpriteScale = 2.0f;
        }
         
        public void WeaponWasShot(WeaponBase weapon)
        {
            currentDurability = weapon.durability;
            if (currentDurability < 0) return;
            if (energyAmmos.Length <= 0) return;
            if (energyAmmos[currentDurability] == null) return;

            energyAmmos[currentDurability].PlayUsedAnimation();
            energyAmmos[currentDurability] = null;
        }
    }
}
