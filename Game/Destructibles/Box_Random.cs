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
	public class Box_Random : Box
	{
		public override void Construct()
		{
			base.Construct();

			Name = "RandomBox";

			chestLoot = ChestLootOptions.Random;

			if (EngineSerivce.isEditor)
			{
				spriteComponent.colorTint = Raylib_cs.Color.Blue;
			}
		}
	}
}
