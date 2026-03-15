using JailBreaker.Weapons;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Classes.Weapons
{

    public enum WeaponType
    {
        Fists, 
        PlasmaLauncher,
        Pistol,
        MachineGun,
        Shotgun,
        Sniper,
        PlasmaSword
    }
    public class WeaponManager
    {
        public WeaponBase currentWeapon;
        private int currentWeaponDurability;
        public WeaponType secondaryWeapon;
        public int secondaryWeaponDurability;
        public Action attackedWithFist;
        public Action<WeaponBase,WeaponType> onWeaponEquipped;
        public Action<WeaponBase> onWeaponShot;
        public bool shouldFlip;
        public WeaponManager()
        {
            currentWeapon = InstanceService.Instantiate(new Fists());
            currentWeaponDurability = 0;
            secondaryWeapon = WeaponType.Fists;
            secondaryWeaponDurability = 0;
        }

        public void SwitchWeapons()
        {
            bool shouldFlip = currentWeapon.renderer.FlipSpriteHorizontaly;
            WeaponType temp = currentWeapon.type;
            InstanceService.Destroy(currentWeapon);
            switch (secondaryWeapon) 
            { 
                case WeaponType.Pistol:
                    currentWeapon = InstanceService.Instantiate(new Pistol());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
                case WeaponType.PlasmaLauncher:
                    currentWeapon = InstanceService.Instantiate(new PlasmaLauncher());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
                case WeaponType.Fists:
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
                case WeaponType.MachineGun:
                    currentWeapon = InstanceService.Instantiate(new MachineGun());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
                case WeaponType.Shotgun:
                    currentWeapon = InstanceService.Instantiate(new ShotGun());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
                case WeaponType.Sniper:
                    currentWeapon = InstanceService.Instantiate(new Sniper());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
                case WeaponType.PlasmaSword:
                    currentWeapon = InstanceService.Instantiate(new PlasmaSword());
                    currentWeapon.durability = secondaryWeaponDurability;
                    break;
            } 
            int tempDurability = currentWeaponDurability;
            currentWeaponDurability = secondaryWeaponDurability;
            secondaryWeaponDurability = tempDurability;
            secondaryWeapon = temp;
            onWeaponEquipped?.Invoke(currentWeapon,secondaryWeapon);
            if(shouldFlip)
            {
                currentWeapon.weaponPos = currentWeapon.weaponPos * new Vector2(1, -1);
                currentWeapon.renderer.FlipSpriteHorizontaly = true;
            }
        }

        public void ThrowAwayCurrentWeapon(GameObject origin)
        {
            switch (currentWeapon.type)
            {
                case WeaponType.Pistol:
                    currentWeapon.Throw(origin);
                    InstanceService.Destroy(currentWeapon);
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeaponDurability = 0;
                    break;
                case WeaponType.PlasmaLauncher:
                    currentWeapon.Throw(origin);
                    InstanceService.Destroy(currentWeapon);
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeaponDurability = 0;
                    break;
                case WeaponType.Fists:
                    // do nothing
                    break;
                case WeaponType.MachineGun:
                    currentWeapon.Throw(origin);
                    InstanceService.Destroy(currentWeapon);
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeaponDurability = 0;
                    break;
                case WeaponType.Shotgun:
                    currentWeapon.Throw(origin);
                    InstanceService.Destroy(currentWeapon);
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeaponDurability = 0;
                    break;
                case WeaponType.Sniper:
                    currentWeapon.Throw(origin);
                    InstanceService.Destroy(currentWeapon);
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeaponDurability = 0;
                    break;
                case WeaponType.PlasmaSword:
                    currentWeapon.Throw(origin);
                    InstanceService.Destroy(currentWeapon);
                    currentWeapon = InstanceService.Instantiate(new Fists());
                    currentWeaponDurability = 0;
                    break;
            }
            onWeaponEquipped?.Invoke(currentWeapon, secondaryWeapon);
        }

        public void Shoot(GameObject origin)
        {
            if(currentWeapon.type == WeaponType.Fists)
            {
                attackedWithFist?.Invoke();
            }
            if (currentWeapon.Shoot(origin))
            {
                onWeaponShot?.Invoke(currentWeapon);
                currentWeaponDurability = currentWeapon.durability;
            }

        }

        public void FlipSprite(bool flip)
        {
            shouldFlip = flip;
            if (currentWeapon.type == WeaponType.Fists) return;
            currentWeapon.renderer.FlipSpriteHorizontaly = flip;
        }

        public void EquipWeapon(WeaponType weaponType, GameObject origin)
        {
            if (weaponType == WeaponType.Fists) return;
            if(currentWeapon.type == WeaponType.Fists)
            {
                InstanceService.Destroy(currentWeapon);
                switch (weaponType)
                {
                    case WeaponType.Pistol:
                        currentWeapon = InstanceService.Instantiate(new Pistol());
                        currentWeaponDurability = currentWeapon.durability;
                        break;
                    case WeaponType.PlasmaLauncher:
                        currentWeapon = InstanceService.Instantiate(new PlasmaLauncher());
                        currentWeaponDurability = currentWeapon.durability;
                        break;
                    case WeaponType.MachineGun:
                        currentWeapon = InstanceService.Instantiate(new MachineGun());
                        currentWeaponDurability = currentWeapon.durability;
                        break;
                    case WeaponType.Shotgun:
                        currentWeapon = InstanceService.Instantiate(new ShotGun());
                        currentWeaponDurability = currentWeapon.durability;
                        break;
                    case WeaponType.Sniper:
                        currentWeapon = InstanceService.Instantiate(new Sniper());
                        currentWeaponDurability = currentWeapon.durability;
                        break;
                    case WeaponType.PlasmaSword:
                        currentWeapon = InstanceService.Instantiate(new PlasmaSword());
                        currentWeaponDurability = currentWeapon.durability;
                        break;
                }
                onWeaponEquipped?.Invoke(currentWeapon, secondaryWeapon);
                if (shouldFlip)
                {
                    currentWeapon.weaponPos = currentWeapon.weaponPos * new Vector2(1, -1);
                    currentWeapon.renderer.FlipSpriteHorizontaly = true;
                }
                return;
            }
            else
            {
                if(secondaryWeapon == WeaponType.Fists)
                {
                    switch (weaponType)
                    {
                        case WeaponType.Pistol:
                            secondaryWeapon = WeaponType.Pistol;
                            secondaryWeaponDurability = 15;
                            break;
                        case WeaponType.PlasmaLauncher:
                            secondaryWeapon = WeaponType.PlasmaLauncher;
                            secondaryWeaponDurability = 5;
                            break;
                        case WeaponType.MachineGun:
                            secondaryWeapon = WeaponType.MachineGun;
                            secondaryWeaponDurability = 40;
                            break;
                        case WeaponType.Shotgun:
                            secondaryWeapon = WeaponType.Shotgun;
                            secondaryWeaponDurability = 8;
                            break;
                        case WeaponType.Sniper:
                            secondaryWeapon = WeaponType.Sniper;
                            secondaryWeaponDurability = 4;
                            break;
                        case WeaponType.PlasmaSword:
                            secondaryWeapon = WeaponType.PlasmaSword;
                            secondaryWeaponDurability = 10;
                            break;
                    }
                    onWeaponEquipped?.Invoke(currentWeapon, secondaryWeapon);
                    return;
                }
                else
                {
                    ThrowAwayCurrentWeapon(origin);

                    switch (weaponType)
                    {
                        case WeaponType.Pistol:
                            currentWeapon = InstanceService.Instantiate(new Pistol());
                            currentWeaponDurability = currentWeapon.durability;
                            break;
                        case WeaponType.PlasmaLauncher:
                            currentWeapon = InstanceService.Instantiate(new PlasmaLauncher());
                            currentWeaponDurability = currentWeapon.durability;
                            break;
                        case WeaponType.MachineGun:
                            currentWeapon = InstanceService.Instantiate(new MachineGun());
                            currentWeaponDurability = currentWeapon.durability;
                            break;
                        case WeaponType.Shotgun:
                            currentWeapon = InstanceService.Instantiate(new ShotGun());
                            currentWeaponDurability = currentWeapon.durability;
                            break;
                        case WeaponType.Sniper:
                            currentWeapon = InstanceService.Instantiate(new Sniper());
                            currentWeaponDurability = currentWeapon.durability;
                            break;
                        case WeaponType.PlasmaSword:
                            currentWeapon = InstanceService.Instantiate(new PlasmaSword());
                            currentWeaponDurability = currentWeapon.durability;
                            break;
                    }
                    onWeaponEquipped?.Invoke(currentWeapon, secondaryWeapon);
                    if (shouldFlip)
                    {
                        currentWeapon.weaponPos = currentWeapon.weaponPos * new Vector2(1, -1);
                        currentWeapon.renderer.FlipSpriteHorizontaly = true;
                    }
                    return;

                }
            }
          
        }

        
    }
}
