using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;

namespace JailBreaker
{
	public class TestLeftArmToRotateWithGun : GameObject
	{
		public SpriteAnimatorComponent armLAnimator;
		public SpriteComponent armLRenderer;

		public override void Construct()
		{
			base.Construct();

			#region leftArm
			Sprite idleArmLSprite = new Sprite("Game/Assets/Textures/IdleRightArmTest-Sheet.png", 6);
			SpriteAnimation idleArmLAnimation = new SpriteAnimation(idleArmLSprite, .5f);
			AnimationControllerState idleArmLState = new AnimationControllerState("Idle", idleArmLAnimation);

			AnimationControllerState[] allArmLStates =
			{
				idleArmLState,
			};
			AnimationController armLController = new AnimationController(allArmLStates);

			armLRenderer = new SpriteComponent(this, idleArmLSprite, Raylib_cs.Color.White, name: "PlayerArmLRenderer");
			armLRenderer.TextureOffset = new Vector2(0, 0);
			armLRenderer.SortingLayer = SortingLayers.Player;
			armLRenderer.ZIndex = -2;
			armLRenderer.colorTint = Color.Pink;
			armLAnimator = new SpriteAnimatorComponent(this, armLRenderer, armLController);

			#endregion
		}
	}
}
