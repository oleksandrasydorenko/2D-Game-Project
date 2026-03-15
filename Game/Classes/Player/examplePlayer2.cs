using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;


namespace JailBreaker.Player
{
	public class examplePlayer2 : GameObject
	{

		public SpriteAnimatorComponent animator;
		public SpriteAnimatorComponent headAnimator;
		public SpriteAnimatorComponent armRAnimator;
	
		PlayerState currentState;
		public float walkSpeed = 50;

		public SpriteComponent bodyRenderer;
		public SpriteComponent headRenderer;
		public SpriteComponent armRRenderer;

		public GameObject armLeftPivot;
		public TestLeftArmToRotateWithGun armLeft;
		public TestGun TestGun;

		public override void Construct()
		{
			base.Construct();

			// for debug reasons disabled
			//Raylib.HideCursor(); // hides the cursor, later we need to use the cursor pos and draw a crosshair instead SAAAASHA NEEDS TO DO THAT :)

			Sprite idleBodySpite = new Sprite("Game/Assets/Textures/IdleTorsoLegsTest-Sheet.png", 6);
			
			Sprite walkingSprite = new Sprite("Game/Assets/Textures/Player_Walking.png", 8);
			Sprite jumpingSprite = new Sprite("Game/Assets/Textures/Player_Jump.png", 3);

			bodyRenderer = new SpriteComponent(this, idleBodySpite, Raylib_cs.Color.White, name: "PlayerRenderer");
			

			AnimationSignal middlePointReachedSignal = new AnimationSignal(4);
			middlePointReachedSignal.onAnimationSignalTriggered += () => Console.WriteLine("Hey das ist die mitte der animation");

			// animations signale um zu wissen wann ein frame in der animation erreicht wurde
            AnimationSignal[] signals =
			{
				middlePointReachedSignal,
			};

			SpriteAnimation animation = new SpriteAnimation(idleBodySpite, .5f);
			SpriteAnimation walkanim = new SpriteAnimation(walkingSprite, .5f, loop: true, animationSignals: signals);
			SpriteAnimation jumpAnim = new SpriteAnimation(jumpingSprite, .5f, loop:false);

			jumpAnim.onAnimationFinished += JumpDone; // wenn die jump animation fertig ist dann setzt ich den state auf idle 

			AnimationControllerState idleState = new AnimationControllerState("Idle",animation);
			idleState.onStateEntered += () => Console.WriteLine("idle enterd");

            AnimationControllerState walkingState = new AnimationControllerState("Walking", walkanim);
			AnimationControllerState jumpingState = new AnimationControllerState("Jumping", jumpAnim);


			AnimationControllerState[] allStates =
			{
				idleState,
				walkingState,
				jumpingState,
			};
			AnimationController controller = new AnimationController(allStates);

			#region head
			Sprite idleHeadSprite = new Sprite("Game/Assets/Textures/IdleHeadTest-Sheet.png", 6);
			SpriteAnimation idleHeadAnimation = new SpriteAnimation(idleHeadSprite, .5f);
			AnimationControllerState idleHeadState = new AnimationControllerState("Idle", idleHeadAnimation);

			AnimationControllerState[] allHeadStates =
			{
				idleHeadState,
			};
			AnimationController headController = new AnimationController(allHeadStates);

			headRenderer = new SpriteComponent(this, idleHeadSprite, Raylib_cs.Color.White, name: "PlayerHeadRenderer");
			headRenderer.TextureOffset = new Vector2(0, 0);
			headRenderer.SortingLayer = SortingLayers.Player;
			headRenderer.ZIndex = 4;
			headRenderer.colorTint = Color.Blue;
			headAnimator = new SpriteAnimatorComponent(this, headRenderer, headController);
		

			#endregion

			#region armRight
			Sprite idleArmRSprite = new Sprite("Game/Assets/Textures/IdleLeftArmTest-Sheet.png", 6);
			SpriteAnimation idleArmRAnimation = new SpriteAnimation(idleArmRSprite, .5f);
			AnimationControllerState idleArmRState = new AnimationControllerState("Idle", idleArmRAnimation);

			AnimationControllerState[] allArmRStates =
			{
				idleArmRState,
			};
			AnimationController armRController = new AnimationController(allArmRStates);

			armRRenderer = new SpriteComponent(this, idleArmRSprite, Raylib_cs.Color.White, name: "PlayerArmRRenderer");
			armRRenderer.TextureOffset = new Vector2(0, 0);
			armRRenderer.SortingLayer = SortingLayers.Player;
			armRRenderer.ZIndex = 5;
			armRRenderer.colorTint = Color.Yellow;
			armRAnimator = new SpriteAnimatorComponent(this, armRRenderer, armRController);


			#endregion

			#region armLeft

			armLeftPivot = InstanceService.Instantiate(new GameObject());
			armLeftPivot.SetPosition(GetPosition());

			armLeft = InstanceService.Instantiate(new TestLeftArmToRotateWithGun());
			TestGun = InstanceService.Instantiate(new TestGun());
	

			#endregion

			animator = new SpriteAnimatorComponent(this, bodyRenderer, controller);
			

			//render.spriteScale = 4; // wird etz über die camera geamcht

			bodyRenderer.SpriteScale = 1;
			bodyRenderer.colorTint = Color.Red;
			//renderer.SortingLayer = SortingLayers.Player;


			BoxCollider2D collider = new BoxCollider2D(this, width:idleBodySpite.FrameWidth * bodyRenderer.SpriteScale, height:idleBodySpite.FrameHeight * bodyRenderer.SpriteScale);
			collider.drawHitbox = true;
			collider.onTriggerEntered += TriggerEntered;
			collider.onTriggerExited += TriggerExited;
			collider.onCollider += OnCol;

			collider.drawHitbox = false;
			
		}

		public void TriggerEntered(BoxCollider2D other)
		{
            Console.WriteLine("HEy ich bins der player und jemand is in mein trigger" + Name);
		}
		public void TriggerExited(BoxCollider2D other)
		{
            Console.WriteLine("HEy ich bins der player und niemand is mehr in mein trigger" + Name);
        }
		public void OnCol(BoxCollider2D other)
		{
            Console.WriteLine(Name+": Collsision mit "+other.Name);
		}

		public override void Start()
		{
			base.Start();
			SetPosition(250, 250);
		}

		public void JumpDone()
		{
			if (currentState == PlayerState.Jumping)
			{
				currentState = PlayerState.Idle;
				animator.SetState("Idle");
			}
		}

		public override void Update()
		{
			base.Update();

			if(IsKeyPressed(KeyboardKey.P))
			{
				if(!GameManager.GamePaused)
				{
					GameManager.GamePaused = true;
				}
				else
				{
					GameManager.GamePaused = false;
				}
			}


			if (Name == "Player2") return;

			if (GameManager.GamePaused) return;

			float horizontal = 0f;
			float vertical = 0f;

			if (IsKeyDown(KeyboardKey.A)) { horizontal -= 1; }
			if (IsKeyDown(KeyboardKey.D)) { horizontal += 1; }
			if (IsKeyDown(KeyboardKey.W)) { vertical -= 1; }
			if (IsKeyDown(KeyboardKey.S)) { vertical += 1; }

			Vector2 inputVector = new Vector2(horizontal, vertical);
			float norm = MathF.Sqrt(MathF.Pow(inputVector.X, 2) + MathF.Pow(inputVector.Y, 2)); // normalizing the input vector 
			inputVector = norm != 0 ? 1f / norm * inputVector : Vector2.Zero; // if the norm is zero the division is not allowed default back to (0,0)

			if(IsKeyPressed(KeyboardKey.Space))
			{
				if(!(currentState == PlayerState.Jumping))
				{
					currentState = PlayerState.Jumping;
					animator.SetState("Jumping");
				}
			}

			if (currentState != PlayerState.Jumping)
			{

				// wenn wir uns bewegen und nciht sprigen
				if (inputVector != Vector2.Zero)
				{
					// dann setzten wir den state auf walking aber nur einmal um animations probleme zu vermeiden
					if (currentState == PlayerState.Idle)
					{
						currentState = PlayerState.Walking;
						animator.SetState("Walking");
					}
				}
				else // wir springen und bewegen uns nicht dann sind wir idle
				{
					// nur einmal in den state gehen um animations probleme zu umgehen
					if (currentState != PlayerState.Idle)
					{
						currentState = PlayerState.Idle;
						animator.SetState("Idle");
					}
				}

			}


			if(currentState != PlayerState.Idle)
			{
				Vector2 lastPos = GetPosition();

				Vector2 forward = GetForwardVector();

				// setzten der neuen position

				SetPosition(lastPos += (inputVector * walkSpeed * Time.DeltaTime));
			}

			if(bodyRenderer != null && inputVector.X < 0)
			{
				bodyRenderer.FlipSpriteVerticaly = true;
			}
			else
			{
				if(bodyRenderer != null && bodyRenderer.FlipSpriteVerticaly == true)
				{
					bodyRenderer.FlipSpriteVerticaly = false;
				}
			}

			// setting the pivot to the players center
			armLeftPivot.SetPosition(GetPosition());
			
			// calcuation the direction the players mouse is from the players center 
			Vector2 worldMousePosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), SceneService.ActiveScene.mainCamera.camera);
			Vector2 direction = worldMousePosition - GetPosition();
			
			// rotating the pivot towards the 
			armLeftPivot.RotateTowards(direction);

			armLeft.SetPosition(armLeftPivot.GetPosition());
			armLeft.RotateTowards(direction);

			// placing the gun infront of the arm with a offset
			TestGun.SetPosition(armLeftPivot.GetPosition() + (armLeftPivot.GetForwardVector() * 5) + (armLeftPivot.GetUpVector() * 2));
			// making the gun face the same direction as the arm
			TestGun.RotateTowards(direction);
		}

	}
}
