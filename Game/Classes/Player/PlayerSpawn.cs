using JailBreaker.Enemy;
using JailBreaker.Player;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JailBreaker.Game.Classes.Player
{
	public class PlayerSpawn:GameObject, IPrefable
	{
		public Vector2 BoundingBoxSize { get; set; } = new Vector2(17, 17);

		public override void Construct()
		{
			base.Construct();
			Name = "PlayerSpawn";
			if (EngineSerivce.isEditor)
			{
				Sprite idleHeadSprite = new Sprite("Game/Assets/Textures/IdleHead.png", 6);
				Sprite idleRightArmSprite = new Sprite("Game/Assets/Textures/IdleRightArm.png", 6);
				Sprite idleLeftArmSprite = new Sprite("Game/Assets/Textures/IdleLeftArm.png", 6);
				Sprite idleTorsoSprite = new Sprite("Game/Assets/Textures/IdleTorsoLegs.png", 6);

				SpriteComponent HeadRenderer; //renderer for each drawn limb, regardless of their state
				SpriteComponent RightArmRenderer;
				SpriteComponent LeftArmRenderer;
				SpriteComponent TorsoLegsRenderer;

				// Render Sprites
				// 1.instanziate, 2. give color, 3&4. sort layer
				HeadRenderer = new SpriteComponent(this, idleHeadSprite, Raylib_cs.Color.White, "Head", 0, 0);
				//HeadRenderer.colorTint = Color.Red;
				HeadRenderer.SortingLayer = SortingLayers.Player;
				HeadRenderer.ZIndex = -1;

				RightArmRenderer = new SpriteComponent(this, idleRightArmSprite, Raylib_cs.Color.White, "RightArm", -1, 0); //shows the picture in the game/renderer
																												  //RightArmRenderer.colorTint = Color.Green;
				RightArmRenderer.SortingLayer = SortingLayers.Player;
				RightArmRenderer.ZIndex = -2;
				RightArmRenderer.TextureOffset = new Vector2(3, -5);

				LeftArmRenderer = new SpriteComponent(this, idleLeftArmSprite, Raylib_cs.Color.White, "LeftArm", 0, 0);
				//LeftArmRenderer.colorTint = Color.Blue;
				LeftArmRenderer.SortingLayer = SortingLayers.Player;
				LeftArmRenderer.ZIndex = 2;
				LeftArmRenderer.TextureOffset = new Vector2(-3, -5);

				TorsoLegsRenderer = new SpriteComponent(this, idleTorsoSprite, Raylib_cs.Color.White, "TorsoLegs", 0, 0);
				//TorsoLegsRenderer.colorTint = Color.Pink;
				TorsoLegsRenderer.SortingLayer = SortingLayers.Player;
				TorsoLegsRenderer.ZIndex = 0;
			}
		}

		public override void Start()
		{
			base.Start();

			if (EngineSerivce.isEditor) return;

			LaniasPlayer player = InstanceService.InstantiateWithPosition(new LaniasPlayer(), GetPosition());
		}
	}
}
