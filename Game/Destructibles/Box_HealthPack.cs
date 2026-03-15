using JailBreaker.Destructibles;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Destructibles
{
	public class Box_HealthPack : Box
	{
		public override void Construct()
		{
			base.Construct();

			Name = "HealtBox";

			chestLoot = ChestLootOptions.HealthPack;

			if(EngineSerivce.isEditor)
			{
				spriteComponent.colorTint = Raylib_cs.Color.Red;
			}
		}
	}
}
