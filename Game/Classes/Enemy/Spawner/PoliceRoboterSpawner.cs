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
	public class PoliceRoboterSpawner : GameObject, IPrefable
	{
		
		public Vector2 BoundingBoxSize { get; set; } = new Vector2(17, 17);

		public override void Construct()
		{
			base.Construct();
			Name = "PoliceRoboterSpawner";
			if(EngineSerivce.isEditor)
			{
				SpriteComponent renderer;
				renderer = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Enemy/PolicePatrol2.png", 6, 3), Raylib_cs.Color.White);
			}
		}

		public override void Start()
		{
			base.Start();

			if (EngineSerivce.isEditor) return;

			PoliceRoboter robot = InstanceService.InstantiateWithPosition(new PoliceRoboter(), GetPosition());
			robot.patrolRange = new Vector2(robot.Position.X - 200, robot.Position.X + 200);
		}
	}
}
