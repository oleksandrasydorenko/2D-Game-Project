using JailBreaker.Destructibles;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Effects
{
	public class Explosion:GameObject
	{
		private Vector2 damageRange = new Vector2(0, 100); // min dmg and max dmg
		private float explosionDiameter = 150;
		private CameraShakeArguments shake = new CameraShakeArguments(50, 50, 1f, 0, .2f, true, 30,200);

		private List<IDestructable> objectsHit = new List<IDestructable>();

		public override void Construct()
		{
			base.Construct();

			Sprite explosionSpiteSheet = new Sprite("Game/Assets/Textures/Effects/Explosion.png", 9);
			SpriteComponent renderer = new SpriteComponent(this,explosionSpiteSheet,Color.White);
			SpriteAnimation explosionAnimation = new SpriteAnimation(explosionSpiteSheet, .75f, false);
			AnimationControllerState explosionState = new AnimationControllerState("explode", explosionAnimation);
			AnimationController animController = new AnimationController(explosionState);
			SpriteAnimatorComponent animator = new SpriteAnimatorComponent(this,renderer,animController);

			AudioComponent explosionSound = new AudioComponent(this, "Game/Assets/Audio/Destructibles/ExplosionSound.wav", false,0.5f, 1, true, true, 10, 500,true);
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			explosionSound.PlayOneShot(explosionSound.Volume,pitch);

		    BoxCollider2D dmgRange = new BoxCollider2D(this, 0,0, explosionDiameter, explosionDiameter);
			//dmgRange.drawHitbox = true;
			dmgRange.IsTrigger = true;

			dmgRange.onTriggerEntered += ExplosionRangeEntered;
			dmgRange.CheckTrigger();
	

			explosionAnimation.onAnimationFinished += () => {InstanceService.Destroy(this);};

			shake.origin = GetPosition();
			CameraService.StartCameraShake(shake);
		}


		public void ExplosionRangeEntered(BoxCollider2D other)
		{
			Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("enterd");
			Console.ForegroundColor = ConsoleColor.White;

			if (other.Parent == this) return;

			if (other.IsTrigger) return;

			IDestructable destructable = other.Parent as IDestructable;

			if (destructable == null) return;

			if (objectsHit.Contains(destructable)) return;

			GameObject go = other.Parent as GameObject;

			if (go == null) return;

			float distanceToExplosion = (go.GetPosition() - this.GetPosition()).Length();
			distanceToExplosion = MathF.Abs(distanceToExplosion);

			Console.WriteLine("distance to explosion: " + distanceToExplosion);

			float dmg = HelperFunctionsUtils.ReMap(distanceToExplosion, 0, explosionDiameter * .5f, damageRange.Y, damageRange.X);

            Console.WriteLine("damage from Explosion: " + dmg);

			destructable.TakeDamage((int)dmg, this);

			objectsHit.Add(destructable);

		}
	}
}
