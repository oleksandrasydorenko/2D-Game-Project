using RocketEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Enemy.Spawner
{
	public class PoliceDogSpawner : GameObject, IPrefable
	{
		
		public Vector2 BoundingBoxSize { get; set; } = new Vector2(17, 17);

		public override void Construct()
		{
			base.Construct();
			Name = "PoliceDogSpawner";
			if(EngineSerivce.isEditor)
			{
				SpriteComponent renderer;
				renderer = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Dog3-Sheet.png", 6), Raylib_cs.Color.White);
			}
		}

		public override void Start()
		{
			base.Start();

			if (EngineSerivce.isEditor) return;

			DogRoboter robot = InstanceService.InstantiateWithPosition(new DogRoboter(), GetPosition());
			robot.patrolRange = new Vector2(robot.Position.X - 200, robot.Position.X + 200);
		}
	}
}
