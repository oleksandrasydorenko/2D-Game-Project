using RocketEngine;
using JailBreaker.Destructibles;
using Raylib_cs;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using JailBreaker.Player;

namespace JailBreaker.Effects
{
    public class FloorExplosion : GameObject
    {
        public Vector2 damageRange = new Vector2(10, 10);
        public float explosionDiameter = 50;
        private CameraShakeArguments shake = new CameraShakeArguments(5, 50, 1f, 0, .2f, true, 30, 200);
        public override void Construct()
        {
            base.Construct();

            Sprite explosionSpiteSheet = new Sprite("Game/Assets/Textures/Effects/FloorExplosion.png", 7);
            SpriteComponent renderer = new SpriteComponent(this, explosionSpiteSheet, Color.White);
            SpriteAnimation explosionAnimation = new SpriteAnimation(explosionSpiteSheet, .75f, false);
            AnimationControllerState explosionState = new AnimationControllerState("floorexplode", explosionAnimation);
            AnimationController animController = new AnimationController(explosionState);
            SpriteAnimatorComponent animator = new SpriteAnimatorComponent(this, renderer, animController);

            renderer.SortingLayer = SortingLayers.ForegroundElements1;

			AudioComponent explosionSound = new AudioComponent(this, "Game/Assets/Audio/Destructibles/ExplosionSound.wav", false, 0.5f, 1, true, true, 10, 300);
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			explosionSound.PlayOneShot(explosionSound.Volume, pitch);

			BoxCollider2D dmgRange = new BoxCollider2D(this, 0, 0, explosionDiameter, explosionDiameter);
            //dmgRange.drawHitbox = true;
            dmgRange.IsTrigger = true;
           // dmgRange.alwaysCheckTriggers = true;
            dmgRange.onTriggerEntered += ExplosionRangeEntered;
            dmgRange.CheckTrigger();


            explosionAnimation.onAnimationFinished += () => { InstanceService.Destroy(this); };

            shake.origin = GetPosition();
            CameraService.StartCameraShake(shake);
        }
        public void ExplosionRangeEntered(BoxCollider2D other)
        {
            LaniasPlayer target = other.Parent as LaniasPlayer;
            if (target != null)
            {
                IDestructable destructable = other.Parent as IDestructable;

                float distanceToExplosion = (target.GetPosition() - this.GetPosition()).Length();
                distanceToExplosion = MathF.Abs(distanceToExplosion);

                float dmg = HelperFunctionsUtils.ReMap(distanceToExplosion, 0, explosionDiameter * .5f, damageRange.Y, damageRange.X);

                destructable.TakeDamage((int)dmg, this);

            }
        }
    }
}
