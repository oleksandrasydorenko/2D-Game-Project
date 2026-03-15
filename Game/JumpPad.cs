using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using JailBreaker.Player;
using RocketEngine;

namespace JailBreaker
{
	public class JumpPad: GameObject, IPrefable
	{
		// carefull i need to check every frame while he is cooled down if there is someone in the area 
		// but only if the area is not null so if no one is in the area



		// die force auf den player ist zu stark wenn ich drauf springe es geht nur wenn ich drauf laufe

		private float t = 0;
		private float cooldown = .3f;


		private bool isInCooldown = false;

		SpriteComponent renderer;

		private CameraShakeArguments shake = new CameraShakeArguments(10, 20, .5f, 0, .2f);

		public Vector2 BoundingBoxSize { get; set; } = new Vector2(14, 10);

		public override void Construct()
		{
			base.Construct();
			Name = "JumpPad";
			Sprite jumpPadSpriteSheet = new Sprite("Game/Assets/Textures/JumpPad.png", 3);
			renderer = new SpriteComponent(this,jumpPadSpriteSheet , Raylib_cs.Color.White);

			SpriteAnimation explosionAnimation = new SpriteAnimation(jumpPadSpriteSheet, .75f, true);
			AnimationControllerState explosionState = new AnimationControllerState("default", explosionAnimation);
			AnimationController animController = new AnimationController(explosionState);
			SpriteAnimatorComponent animator = new SpriteAnimatorComponent(this, renderer, animController);


			BoxCollider2D triggerArea = new BoxCollider2D(this, 0,0, 15,5);
			triggerArea.IsTrigger = true;
			//triggerArea.drawHitbox = true;
			triggerArea.alwaysCheckTriggers = true;
			triggerArea.CollisionLayer = CollisionLayers.OnlyPlayer;
			triggerArea.onTriggerEntered += TriggerEntered;

		}

		public void TriggerEntered(BoxCollider2D other)
		{
			if (isInCooldown) return;

			if (other.Parent == this) return;

			LaniasPlayer go = other.Parent as LaniasPlayer;

			if (go == null) return;

			AddForceToPlayer(go);
		}

		public void AddForceToPlayer(LaniasPlayer player)
		{
			CameraService.StartCameraShake(shake);

			isInCooldown = true;

			//player.SetPlayerState(PlayerState.Idle);
			player.PhysicsComponent.Velocity = Vector2.Zero;
			player.PhysicsComponent.AddForce(GetUpVector(), 800, 800);
		}

		public override void Update()
		{
			base.Update();

			if(isInCooldown)
			{
				t += Time.DeltaTime;
				if (t >= cooldown)
				{
					isInCooldown = false;
					t = 0;
				}
			}

		}
	}
}
