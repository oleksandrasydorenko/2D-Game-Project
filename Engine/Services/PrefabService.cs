using JailBreaker.Destructibles;
using JailBreaker.Enemy.Spawner;
using JailBreaker.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;
using JailBreaker.Props;
using JailBreaker;
using JailBreaker.DeathArea;
using JailBreaker.Game.Destructibles;
using JailBreaker.Game;
using JailBreaker.Interactibles;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Interactibles.WeaponPickUps;
using JailBreaker.Game.Classes.Player;

namespace RocketEngine
{
	public static class PrefabService
	{
		public static object GetPrefabFromIndex(int index, bool spawn = false, float x = 0, float y = 0)
		{
			switch (index)
			{
				case 0: return spawn ? InstantiateWithPosition(new ExplosiveBarrel(), new Vector2(x, y)) : new ExplosiveBarrel();
				case 1: return spawn ? InstantiateWithPosition(new Box(), new Vector2(x, y)) : new Box();
				case 2: return spawn ? InstantiateWithPosition(new Box_Random(), new Vector2(x, y)) : new Box_Random();
				case 3: return spawn ? InstantiateWithPosition(new Box_HealthPack(), new Vector2(x, y)) : new Box_HealthPack();
				case 4: return spawn ? InstantiateWithPosition(new Box_Pistol(), new Vector2(x, y)) : new Box_Pistol();
				case 5: return spawn ? InstantiateWithPosition(new Box_KeyCard(), new Vector2(x, y)) : new Box_KeyCard();
				case 6: return spawn ? InstantiateWithPosition(new PoliceRoboterSpawner(), new Vector2(x,y)) : new PoliceRoboterSpawner();
				case 7: return spawn ? InstantiateWithPosition(new PoliceDogSpawner(), new Vector2(x, y)) : new PoliceDogSpawner();
				case 8: return spawn ? InstantiateWithPosition(new DroneSpawner(), new Vector2(x, y)) : new DroneSpawner();
				case 9: return spawn ? InstantiateWithPosition(new Monitors(), new Vector2(x, y)) : new Monitors();
                case 10: return spawn ? InstantiateWithPosition(new CellDoor(), new Vector2(x, y)) : new CellDoor();
                case 11: return spawn ? InstantiateWithPosition(new BloodShots(), new Vector2(x, y)) : new BloodShots();
                case 12: return spawn ? InstantiateWithPosition(new Cables1(), new Vector2(x, y)) : new Cables1();
                case 13: return spawn ? InstantiateWithPosition(new Cables2(), new Vector2(x, y)) : new Cables2();
                case 14: return spawn ? InstantiateWithPosition(new Cables3(), new Vector2(x, y)) : new Cables3();
                case 15: return spawn ? InstantiateWithPosition(new Cables4(), new Vector2(x, y)) : new Cables4();
                case 16: return spawn ? InstantiateWithPosition(new Cables5(), new Vector2(x, y)) : new Cables5();
                case 17: return spawn ? InstantiateWithPosition(new Cables6(), new Vector2(x, y)) : new Cables6();
                case 18: return spawn ? InstantiateWithPosition(new Cables7(), new Vector2(x, y)) : new Cables7();
                case 19: return spawn ? InstantiateWithPosition(new Graffiti(), new Vector2(x, y)) : new Graffiti();
                case 20: return spawn ? InstantiateWithPosition(new GunShots(), new Vector2(x, y)) : new GunShots();
                case 21: return spawn ? InstantiateWithPosition(new LightSolo(), new Vector2(x, y)) : new LightSolo();
                case 22: return spawn ? InstantiateWithPosition(new LightDouble(), new Vector2(x, y)) : new LightDouble();
                case 23: return spawn ? InstantiateWithPosition(new LightSwitch(), new Vector2(x, y)) : new LightSwitch();
                case 24: return spawn ? InstantiateWithPosition(new Pipe11(), new Vector2(x, y)) : new Pipe11();
                case 25: return spawn ? InstantiateWithPosition(new Pipe12(), new Vector2(x, y)) : new Pipe12();
                case 26: return spawn ? InstantiateWithPosition(new Pipe13(), new Vector2(x, y)) : new Pipe13();
                case 27: return spawn ? InstantiateWithPosition(new Pipe21(), new Vector2(x, y)) : new Pipe21();
                case 28: return spawn ? InstantiateWithPosition(new Pipe21(), new Vector2(x, y)) : new Pipe23();
                case 29: return spawn ? InstantiateWithPosition(new Pipe31(), new Vector2(x, y)) : new Pipe31();
                case 30: return spawn ? InstantiateWithPosition(new Pipe32(), new Vector2(x, y)) : new Pipe32();
                case 31: return spawn ? InstantiateWithPosition(new Pipe33(), new Vector2(x, y)) : new Pipe33();
                case 32: return spawn ? InstantiateWithPosition(new Scratches(), new Vector2(x, y)) : new Scratches();
                case 33: return spawn ? InstantiateWithPosition(new VentilatorOther(), new Vector2(x, y)) : new VentilatorOther();
                case 34: return spawn ? InstantiateWithPosition(new VentilatorLeft(), new Vector2(x, y)) : new VentilatorLeft();
                case 35: return spawn ? InstantiateWithPosition(new VentilatorMiddle(), new Vector2(x, y)) : new VentilatorMiddle();
                case 36: return spawn ? InstantiateWithPosition(new VentilatorRight(), new Vector2(x, y)) : new VentilatorRight();
                case 37: return spawn ? InstantiateWithPosition(new VentilatorSolo(), new Vector2(x, y)) : new VentilatorSolo();
                case 38: return spawn ? InstantiateWithPosition(new WarningSigns(), new Vector2(x, y)) : new WarningSigns();
                case 39: return spawn ? InstantiateWithPosition(new TrashCan2(), new Vector2(x, y)) : new TrashCan2();
				case 40: return spawn ? InstantiateWithPosition(new JumpPad(), new Vector2(x, y)) : new JumpPad();
                case 41: return spawn ? InstantiateWithPosition(new FanMoving(), new Vector2(x, y)) : new FanMoving();
				case 42: return spawn ? InstantiateWithPosition(new SmallDeathArea(), new Vector2(x, y)) : new SmallDeathArea();
				case 43: return spawn ? InstantiateWithPosition(new MediumDeathArea(), new Vector2(x, y)) : new MediumDeathArea();
				case 44: return spawn ? InstantiateWithPosition(new LargeDeathArea(), new Vector2(x, y)) : new LargeDeathArea();
				case 45: return spawn ? InstantiateWithPosition(new Mine(), new Vector2(x, y)) : new Mine();
                case 46: return spawn ? InstantiateWithPosition(new Platform(), new Vector2(x, y)) : new Platform();
				case 47: return spawn ? InstantiateWithPosition(new Box_Sniper(), new Vector2(x, y)) : new Box_Sniper();
				case 48: return spawn ? InstantiateWithPosition(new Box_ShotGun(), new Vector2(x, y)) : new Box_ShotGun();
				case 49: return spawn ? InstantiateWithPosition(new Box_PlasmaSword(), new Vector2(x, y)) : new Box_PlasmaSword();
				case 50: return spawn ? InstantiateWithPosition(new Box_PlasmaLauncher(), new Vector2(x, y)) : new Box_PlasmaLauncher();
				case 51: return spawn ? InstantiateWithPosition(new Box_MachineGun(), new Vector2(x, y)) : new Box_MachineGun();
				case 52: return spawn ? InstantiateWithPosition(new HealthPack(), new Vector2(x, y)) : new HealthPack();
				case 53: return spawn ? InstantiateWithPosition(new PistolPickUp(), new Vector2(x, y)) : new PistolPickUp();
				case 54: return spawn ? InstantiateWithPosition(new SniperPickUp(), new Vector2(x, y)) : new SniperPickUp();
				case 55: return spawn ? InstantiateWithPosition(new ShotGunPickUp(), new Vector2(x, y)) : new ShotGunPickUp();
				case 56: return spawn ? InstantiateWithPosition(new PlasmaSwordPickUp(), new Vector2(x, y)) : new PlasmaSwordPickUp();
				case 57: return spawn ? InstantiateWithPosition(new PlasmaLauncherPickUp(), new Vector2(x, y)) : new PlasmaLauncherPickUp();
				case 58: return spawn ? InstantiateWithPosition(new MachineGunPickUp(), new Vector2(x, y)) : new MachineGunPickUp();
				case 59: return spawn ? InstantiateWithPosition(new PlayerSpawn(), new Vector2(x, y)) : new PlayerSpawn();

			}
			return null;
		}
	}
}
