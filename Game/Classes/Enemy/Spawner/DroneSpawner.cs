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
	public class DroneSpawner : GameObject, IPrefable
	{
		
		public Vector2 BoundingBoxSize { get; set; } = new Vector2(17, 17);

		public override void Construct()
		{
			base.Construct();
			Name = "DroneSpawner";
			if(EngineSerivce.isEditor)
			{
				SpriteComponent renderer;
				renderer = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Drone.png", 4, 3), Raylib_cs.Color.White);
			}
		}

		public override void Start()
		{
			base.Start();

			if (EngineSerivce.isEditor) return;

			Drone robot = InstanceService.InstantiateWithPosition(new Drone(), GetPosition());
			robot.patrolRange = new Vector2(robot.Position.X - 200, robot.Position.X + 200);
		}
	}
}
