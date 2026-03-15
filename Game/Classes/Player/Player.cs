using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;
using RocketEngine;
using System.Linq.Expressions;

namespace JailBreaker.Player
{
	public class Player : GameObject
	{
		SpriteComponent spriteComponent;
		SpriteAnimatorComponent animator;
		AnimationController ac;

		public override void Construct()
		{
            Console.WriteLine("Player init");
			spriteComponent = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Player_Idle.png", 4), Raylib_cs.Color.White, offsetX: 0, offsetY:0);

			spriteComponent.FlipSpriteVerticaly = true;
			spriteComponent.SpriteScale = 4;

			spriteComponent.ZIndex = 3;

			AnimationSignal middleSignal = new AnimationSignal(5);
			middleSignal.onAnimationSignalTriggered += Middle;

			AnimationSignal[] signals =
			{
				middleSignal,
			};

			SpriteAnimation jumpingAnimation = new SpriteAnimation(new Sprite("Game/Assets/Textures/Player_Jump.png", 3), loop: false);

			AnimationControllerState jumpingState = new AnimationControllerState("Jumping", jumpingAnimation);
			jumpingState.onStateEntered += EnteredJumpinh;

			jumpingAnimation.onAnimationFinished += FinishedJumping;
			jumpingAnimation.onAnimationStarted += () => Console.WriteLine("Jump Animation Started");



            AnimationControllerState walkingState = new AnimationControllerState("Walking", new SpriteAnimation(new Sprite("Game/Assets/Textures/Player_Walking.png", 8), 1, loop: true, reverse: false, animationSignals: signals));

			walkingState.onStateEntered += () => Console.WriteLine("Walking Enterd");
			walkingState.onStateExited += () => Console.WriteLine("WalkingExited");


            AnimationControllerState[] animationStates =
			{
				new AnimationControllerState("Idle", new SpriteAnimation(new Sprite("Game/Assets/Textures/Player_Idle.png", 4))),
				walkingState,
				jumpingState,
			};

			ac = new AnimationController(animationStates);

			animator = new SpriteAnimatorComponent(this, spriteComponent, ac);
			animator.SetState("Walking");

		}

		public void Middle()
		{
			Console.WriteLine("Middle");
			
		}

		public void EnteredJumpinh()
		{
            Console.WriteLine("Entered jumpingjevkmgk");
		}

		public void ExitedWalking()
		{
			Console.WriteLine("Exited Walking");
		}


		public void FinishedJumping()
		{
            Console.WriteLine("Jumping Done");
		}

		public override void Start()
		{
		

            Console.WriteLine("Player start");
			Name = "Player";

			int i = 100;
			SetPositionY(i);

		}


		private int xpos = 0;

		bool jumping = false;
		public override void Update()
		{
			base.Update();

			if (!jumping) xpos = 1;

			Vector2 Pos = GetPosition();
			Vector2 dir = (GetForwardVector() * xpos);

			SetPosition(Pos += dir);
			//SetRotation(180);

			if (GetPosition().X == 100)
			{
                Console.WriteLine("Hey");
                //Rotate(89);
                Console.WriteLine("up" + GetUpVector());

				Rotate(90);

				
			}

			if(xpos == 200)
			{
				animator.PauseAnimator(false);
			}
			
			if (xpos >= 250 && !jumping)
			{
				jumping = true;	
				animator.SetState("Jumping");
			}
		
			
			if (xpos >= 300)
			{
				if (spriteComponent != null)
				{
					/*
					sprite.Destroy();
					*/

					InstanceService.Destroy(this);

					Console.WriteLine("removed");
				}


				
				//Console.WriteLine(sprite.textureOffset.X);

			}

		
		}

		
		public override void ComponentAdded(InstantiableComponent component)
		{
			base.ComponentAdded(component);

            Console.WriteLine("component added");
		}

	}
}
