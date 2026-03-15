using JailBreaker.Destructibles;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Ui;
using JailBreaker.Interactibles;
using JailBreaker.Ui;
using JailBreaker.Weapons;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using RocketEngine.Utils;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;


namespace JailBreaker.Player
{
    /// <summary>
    /// TO DO:
    /// 1. When crosshair is implimented uncomment the show cursor false in the pause input
    /// 2. Show daniel phyiscs problem when climbing staris comment out velcoityY = 0 in jump force
    /// 3. Jump counter fixed double jumping but the stair problem is back because you cant add force any more idk
    /// </summary>


    public enum PlayerState
    {
        Idle,
        Walking,
        Jumping,
        Falling,
        Dead,
    }

    public enum InputState
    {
        Activated,
        Deactivated,
    }
    public class LaniasPlayer : GameObject, IDestructable
    {
        #region changable

        // walking
        public float walkForce = 50f;
        private float maxWalkSpeed = 100f;

        // jumping
        private float jumpForce = 115; // old value 130
        private float maxJumpForce = 190; // old vlaue 180
        private float maxJumpHeightTime = .2f;
        private float jumpBufferTime = .3f;
        private float coyoteTime = .2f;

        private float invincibleCooldown = 1f;

        private float defaultGravityMultiplier = 10;
        private float fallingGravityMultiplier = 20;

        public int MaxHealth { get; set; } = 100;

        private CameraShakeArguments shake = new CameraShakeArguments(10, 50, .5f, 0, .2f);

        #endregion

        #region public

        public bool hasKey;

        public WeaponManager weapons;

        private PhysicsComponent physicsComponent;
		public PhysicsComponent PhysicsComponent
		{
			get
			{
				return physicsComponent;
			}
			private set
			{
				physicsComponent = value;
			}
		}

        #region Health
        private int health;
        public int Health { get { return health; } set { health = value; playerPanel.UpdateHealthText(health, MaxHealth); } }

        public Action onHealthZero { get; set; }
        public Action<GameObject> onDamageTaken { get; set; }
        #endregion

        #endregion

        #region private

        private PlayerState previousState;
        private PlayerState currentState;

        private InputState currentInputState;

        private PlayerPanel playerPanel;

        private GameObject leftArmPivot;

        private float lastDamage=10;

        private float movement = 0f;
        private bool jumpPressed;
        private bool canJumpAgain = true;

        private float jumpTime = 0f;
        private DateTime lastTimeJumpPressedInAir;
        private DateTime timeStartedFalling;

        private Vector2 pistolOffset = new Vector2(3.4f, 5f);        
        private float knockbackTimer = 10f;
        private bool inKnockback = false;

        private bool isInvisible = false;
        private float flickerTimer;

        private bool attacking = false;
        private bool weaponPosFlipped = false;
        #region colliders
        private BoxCollider2D interactRange;
        private BoxCollider2D collider;
        private BoxCollider2D groundCheck;
        #endregion

        #region Player Optics
        // renderer for players limbs
        SpriteComponent HeadRenderer; //renderer for each drawn limb, regardless of their state
        SpriteComponent RightArmRenderer;
        SpriteComponent LeftArmRenderer;
        SpriteComponent TorsoLegsRenderer;

        SpriteAnimatorComponent HeadAnimator; //renderer for the animation
        SpriteAnimatorComponent RightArmAnimator;
        SpriteAnimatorComponent LeftArmAnimator;
        SpriteAnimatorComponent TorsoLegsAnimator;
		#endregion

		Random random = new Random();

		#region Player Sound
		public AudioComponent runSound;
		private string[] footstepSounds = { "Game/Assets/Audio/Footsteps/RunningSound3.wav", "Game/Assets/Audio/Footsteps/RunningSound4.wav", "Game/Assets/Audio/Footsteps/RunningSound5.wav", "Game/Assets/Audio/Footsteps/RunningSound6.wav", "Game/Assets/Audio/Footsteps/RunningSound7.wav" };
        int lastFootstepSoundIndex = 0;

		#endregion // new

		#endregion

		public override void Construct()
        {
            Name = "Player";

            base.Construct();

			// pivot to rotate gun and left arm around
			leftArmPivot = InstanceService.Instantiate(new GameObject());
			weapons = new WeaponManager();
            weapons.attackedWithFist += () => LeftArmAnimator.SetState("CloseCombat");
                
            playerPanel = InstanceService.Instantiate(new PlayerPanel());
            weapons.onWeaponEquipped += (WeaponBase weapon, WeaponType secondweapon) => playerPanel.UpdateWeaponAmmo(weapon, secondweapon);
            weapons.onWeaponShot += (WeaponBase weapon) => playerPanel.WeaponWasShot(weapon);

            onDamageTaken += (GameObject origin) => ApplyKnockback(origin);

            onHealthZero += () => { if (currentState != PlayerState.Dead) EnterDead(); };

			#region PlayerSound SetUp
			runSound = new AudioComponent(this, "Game/Assets/Audio/Footsteps/RunningSound3.wav", false, 0.5f, 1, true, true, 10, 200);
			#endregion

			#region PlayerOptics Setup

			// textures for each state
			// Idle Sprites
			Sprite idleHeadSprite = new Sprite("Game/Assets/Textures/IdleHead.png", 6);
            Sprite idleRightArmSprite = new Sprite("Game/Assets/Textures/IdleRightArm.png", 6);
            Sprite idleLeftArmSprite = new Sprite("Game/Assets/Textures/IdleLeftArm.png", 6);
            Sprite idleTorsoSprite = new Sprite("Game/Assets/Textures/IdleTorsoLegs.png", 6);

            // Walking Sprites
            Sprite walkHeadSprite = new Sprite("Game/Assets/Textures/WalkingHead.png", 6);
            Sprite walkRightArmSprite = new Sprite("Game/Assets/Textures/IdleRightArm.png", 6);
            Sprite walkLeftArmSprite = new Sprite("Game/Assets/Textures/IdleLeftArm.png", 6);
            Sprite walkTorsoSprite = new Sprite("Game/Assets/Textures/WalkingTorsoLegs.png", 6);

            // Falling Sprites
            Sprite fallHeadSprite = new Sprite("Game/Assets/Textures/IdleHead.png", 6);
            Sprite fallRightArmSprite = new Sprite("Game/Assets/Textures/IdleRightArm.png", 6);
            Sprite fallLeftArmSprite = new Sprite("Game/Assets/Textures/IdleLeftArm.png", 6);
            Sprite fallTorsoSprite = new Sprite("Game/Assets/Textures/JumpingTorsoLegs.png", 6);

            // Jumping Sprites
            Sprite jumpHeadSprite = new Sprite("Game/Assets/Textures/IdleHead.png", 6);
            Sprite jumpRightArmSprite = new Sprite("Game/Assets/Textures/IdleRightArm.png", 6);
            Sprite jumpLeftArmSprite = new Sprite("Game/Assets/Textures/IdleLeftArm.png", 6);
            Sprite jumpTorsoSprite = new Sprite("Game/Assets/Textures/JumpingTorsoLegs.png", 6);

            // Close Combat Sprites
            //Sprite closeCombatRightArmSprite = new Sprite("Game/Assets/Textures/CloseCombatRightArm.png", 3);
            Sprite closeCombatLeftArmSprite = new Sprite("Game/Assets/Textures/CloseCombatLeftArm.png", 4);
            

            // Death Sprite
            Sprite deathTorsoSprite = new Sprite("Game/Assets/Textures/DeathAnimationn.png", 18);


            // Render Sprites
            // 1.instanziate, 2. give color, 3&4. sort layer
            HeadRenderer = new SpriteComponent(this, idleHeadSprite, Color.White, "Head", 0, 0);
            //HeadRenderer.colorTint = Color.Red;
            HeadRenderer.SortingLayer = SortingLayers.Player;
            HeadRenderer.ZIndex = -1;

            RightArmRenderer = new SpriteComponent(this, idleRightArmSprite, Color.White, "RightArm", -1, 0); //shows the picture in the game/renderer
            //RightArmRenderer.colorTint = Color.Green;
            RightArmRenderer.SortingLayer = SortingLayers.Player;
            RightArmRenderer.ZIndex = -2;
            RightArmRenderer.TextureOffset = new Vector2(3, -5);

            LeftArmRenderer = new SpriteComponent(this, idleLeftArmSprite, Color.White, "LeftArm", 0, 0);
            //LeftArmRenderer.colorTint = Color.Blue;
            LeftArmRenderer.SortingLayer = SortingLayers.Player;
            LeftArmRenderer.ZIndex = 2;
            LeftArmRenderer.TextureOffset = new Vector2(-3, -5);

            TorsoLegsRenderer = new SpriteComponent(this, idleTorsoSprite, Color.White, "TorsoLegs", 0, 0);
            //TorsoLegsRenderer.colorTint = Color.Pink;
            TorsoLegsRenderer.SortingLayer = SortingLayers.Player;
            TorsoLegsRenderer.ZIndex = 0;

      

            //footstepSignals
            AnimationSignal footDownLeft = new AnimationSignal(2);
            footDownLeft.onAnimationSignalTriggered += () =>
            {
                PlayFootstepSound();
            };

            AnimationSignal footDownRight = new AnimationSignal(5);
			footDownRight.onAnimationSignalTriggered += () =>
            {
				PlayFootstepSound();
			};

			// Animate SpriteSheet
			// Idle Animation Sprites
			SpriteAnimation idleHeadAnimation = new SpriteAnimation(idleHeadSprite, 0.5f, loop: true);
            SpriteAnimation idleRightArmAnimation = new SpriteAnimation(idleRightArmSprite, 0.5f, loop: true);
            SpriteAnimation idleLeftArmAnimation = new SpriteAnimation(idleLeftArmSprite, 0.5f, loop: true);
            SpriteAnimation idleTorsoAnimation = new SpriteAnimation(idleTorsoSprite, 0.5f, loop: true);

            // Walking Animation Sprites
            SpriteAnimation walkHeadAnimation = new SpriteAnimation(walkHeadSprite, 0.5f, loop: true);
            SpriteAnimation walkRightArmAnimation = new SpriteAnimation(walkRightArmSprite, 0.5f, loop: true);
            SpriteAnimation walkLeftArmAnimation = new SpriteAnimation(walkLeftArmSprite, 0.5f, loop: true);
            SpriteAnimation walkTorsoAnimation = new SpriteAnimation(walkTorsoSprite, 0.5f, loop: true, animationSignals: [footDownLeft, footDownRight]);

            // Falling Animation Sprites
            SpriteAnimation fallHeadAnimation = new SpriteAnimation(walkHeadSprite, 0.5f, loop: true);
            SpriteAnimation fallRightArmAnimation = new SpriteAnimation(walkRightArmSprite, 0.5f, loop: true);
            SpriteAnimation fallLeftArmAnimation = new SpriteAnimation(walkLeftArmSprite, 0.5f, loop: true);
            SpriteAnimation fallTorsoAnimation = new SpriteAnimation(fallTorsoSprite, 0.5f, loop: true);

            // Jumping Animation Sprites
            SpriteAnimation jumpHeadAnimation = new SpriteAnimation(walkHeadSprite, 0.5f, loop: true);
            SpriteAnimation jumpRightArmAnimation = new SpriteAnimation(walkRightArmSprite, 0.5f, loop: true);
            SpriteAnimation jumpLeftArmAnimation = new SpriteAnimation(walkLeftArmSprite, 0.5f, loop: true);
            SpriteAnimation jumpTorsoAnimation = new SpriteAnimation(jumpTorsoSprite, 0.5f, loop: true);

            // Close Combat Animation Sprites
            //SpriteAnimation closeCombatRightArmAnimation = new SpriteAnimation(closeCombatRightArmSprite, 0.5f, loop: true);
            SpriteAnimation closeCombatLeftArmAnimation = new SpriteAnimation(closeCombatLeftArmSprite, 0.7f, loop: false);
            closeCombatLeftArmAnimation.onAnimationFinished = () =>
            {
                LeftArmAnimator.SetState("Idle");
            };

            // Death Animation Sprites
            SpriteAnimation deathTorsoAnimation = new SpriteAnimation(deathTorsoSprite, 0.5f, loop: false);
            deathTorsoAnimation.onAnimationFinished += () =>
            {
				ScreenFade fade = InstanceService.Instantiate(new ScreenFade());
				fade.onFadeInFinished += () =>
				{
					DeathMenuPanel deathMenuPanel = InstanceService.Instantiate(new DeathMenuPanel());
					GameManager.ShowCursor = true;
                    InstanceService.Destroy(playerPanel);
				};
				fade.Time = 1f;
				fade.FadeIn();
			};

            // for SpriteAnimation Renderer
            //  0: idle, 1: walking, 2:falling
            // Idle State
            AnimationControllerState[] allHeadStates =
            {
                new AnimationControllerState("Idle",    idleHeadAnimation),
                new AnimationControllerState("Walking", walkHeadAnimation),
                new AnimationControllerState("Falling", fallHeadAnimation),
                new AnimationControllerState("Jumping", jumpHeadAnimation),
            };

            AnimationControllerState[] allTorsoLegsStates =
            {
                new AnimationControllerState("Idle",    idleTorsoAnimation),
                new AnimationControllerState("Walking", walkTorsoAnimation),
                new AnimationControllerState("Falling", fallTorsoAnimation),
                new AnimationControllerState("Jumping", jumpTorsoAnimation),
                new AnimationControllerState("Death",   deathTorsoAnimation),
            };

            AnimationControllerState[] allLeftArmStates =
            {
                new AnimationControllerState("Idle",    idleLeftArmAnimation),
                new AnimationControllerState("Walking", walkLeftArmAnimation),
                new AnimationControllerState("Falling", fallLeftArmAnimation),
                new AnimationControllerState("Jumping", jumpLeftArmAnimation),
                new AnimationControllerState("CloseCombat", closeCombatLeftArmAnimation),
            };

            AnimationControllerState[] allRightArmStates =
            {
                new AnimationControllerState("Idle",    idleRightArmAnimation),
                new AnimationControllerState("Walking", walkRightArmAnimation),
                new AnimationControllerState("Falling", fallRightArmAnimation),
                new AnimationControllerState("Jumping", jumpRightArmAnimation),
                //new AnimationControllerState("CloseCombat", closeCombatLeftArmAnimation),
            };

            // so we can switch between states more easily
            AnimationController headController = new AnimationController(allHeadStates);
            AnimationController torsoLegsController = new AnimationController(allTorsoLegsStates);
            AnimationController rightArmController = new AnimationController(allRightArmStates);
            AnimationController leftArmController = new AnimationController(allLeftArmStates);

            HeadAnimator = new SpriteAnimatorComponent(this, HeadRenderer, headController);
            LeftArmAnimator = new SpriteAnimatorComponent(this, LeftArmRenderer, leftArmController);
            RightArmAnimator = new SpriteAnimatorComponent(this, RightArmRenderer, rightArmController);
            TorsoLegsAnimator = new SpriteAnimatorComponent(this, TorsoLegsRenderer, torsoLegsController);
            #endregion

           

            #region Colider SetUp
            collider = new BoxCollider2D(this, width: idleTorsoSprite.FrameWidth * TorsoLegsRenderer.SpriteScale * .5f, height: idleTorsoSprite.FrameHeight * TorsoLegsRenderer.SpriteScale);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Player;
            //collider.drawHitbox = true;

            interactRange = new BoxCollider2D(this, width: idleTorsoSprite.FrameWidth * TorsoLegsRenderer.SpriteScale + 10, height: idleTorsoSprite.FrameHeight * TorsoLegsRenderer.SpriteScale + 10);
            interactRange.IsTrigger = true;
            interactRange.IsCollider = false;
            //interactRange.drawHitbox = true;
            interactRange.CollisionLayer = CollisionLayers.InteractRadius;

            groundCheck = new BoxCollider2D(this, 0, (idleTorsoSprite.FrameWidth * TorsoLegsRenderer.SpriteScale * .5f), idleTorsoSprite.FrameHeight * TorsoLegsRenderer.SpriteScale * .3f, 5);
            groundCheck.IsTrigger = true;
            //groundCheck.drawHitbox = true;
            groundCheck.CollisionLayer = CollisionLayers.Player;

            groundCheck.onTriggerEntered += GroundCheckEntered;
            groundCheck.onTriggerExited += GroundCheckExited;
            #endregion

            PhysicsComponent = new PhysicsComponent(this, 5f, "physicsComponent");
            PhysicsComponent.Groundy0 = false;
        }

        public override void Start()
        {
            base.Start();

            Health = MaxHealth;

            physicsComponent.IsGrounded = false;

            GameManager.GameState = GameState.Play;

            GameManager.ShowCursor = false;
        }

        public override void Update()
        {
            base.Update();

            knockbackTimer += Time.DeltaTime;
            if (knockbackTimer > 0.4f) inKnockback = false;


            if (GameManager.GamePaused) return;

            if (currentState == PlayerState.Dead) return;

            if (lastDamage <= invincibleCooldown) { lastDamage += Time.DeltaTime; flickerTimer += Time.DeltaTime;Flicker(); }
            else if (isInvisible) VisibleSprite();

            #region Movement Input

            if (!inKnockback)
            {
                // this will be triggered the next frame important for variable jump hight
                if (jumpPressed) canJumpAgain = false;

                if (IsKeyPressed(KeyboardKey.Space) && currentInputState == InputState.Activated)
                {
                    // saving the time if wa are already jumping or falling and pressing jump
                    if (!jumpPressed && (currentState == PlayerState.Falling || currentState == PlayerState.Falling)) lastTimeJumpPressedInAir = DateTime.Now;

                    jumpPressed = true;
                }

                if (IsKeyUp(KeyboardKey.Space))
                {
                    // if we jump and let go we dont want to be able to apply more jump force if we press jump again even if its still in the variable jump time frame
                    if (canJumpAgain == false && currentState == PlayerState.Jumping)
                    {
                        jumpTime += maxJumpHeightTime;
                    }

                    jumpPressed = false;
                    canJumpAgain = true;
                }

                movement = 0f;
                if (IsKeyDown(KeyboardKey.D) && currentInputState == InputState.Activated) movement += 1f;
                if (IsKeyDown(KeyboardKey.A) && currentInputState == InputState.Activated) movement -= 1f;
                #endregion

                if (IsKeyPressed(KeyboardKey.E) && currentInputState == InputState.Activated) CheckInteractRange();

                switch (currentState)
                {
                    case PlayerState.Idle:
                        Idle();

                        break;

                    case PlayerState.Walking:
                        Walking();

                        break;

                    case PlayerState.Jumping:
                        Jumping();

                        break;

                    case PlayerState.Falling: //in falling state can not switch to jumping
                        Falling();

                        break;
                }
            }

            #region Movement Force 

            if (!inKnockback)
            {

                if (MathF.Abs(movement) > 0.01f) // player requested to move in any direction
                {
                    // add force in the direction the player moves
                    PhysicsComponent.AddForce(new Vector2(movement, 0f), walkForce, maxWalkSpeed);

                    FlipPlayer(movement < 0);
                }

                // variable jump height apply jump force while holding space but it needs to be less then the regular jump force
                if (jumpPressed && currentState == PlayerState.Jumping && jumpTime > .1f && jumpTime < maxJumpHeightTime && canJumpAgain == false)
                {
                    PhysicsComponent.AddForce(new Vector2(0, -1), jumpForce, maxJumpForce);
                }
            }

            #endregion

            Vector2 mouseScreen = Raylib.GetMousePosition();

            if (playerPanel.crosshair != null)
            {
                playerPanel.crosshair.SetPosition(mouseScreen);
            }

            HandleGun();
        }

        #region Player States
        private void EnterIdle()
        {

            //Console.WriteLine("IDLE");
            SetPlayerState(PlayerState.Idle);

            HeadAnimator.SetState("Idle");
            TorsoLegsAnimator.SetState("Idle");
            LeftArmAnimator.SetState("Idle");
            RightArmAnimator.SetState("Idle");

            jumpTime = 0f;

        }

        private void EnterWalking()
        {
            //Console.WriteLine("WALKING");
            SetPlayerState(PlayerState.Walking);

            HeadAnimator.SetState("Walking");
            TorsoLegsAnimator.SetState("Walking");
            LeftArmAnimator.SetState("Walking");
            RightArmAnimator.SetState("Walking");

            jumpTime = 0f;
        }

        private void EnterJumping()
        {
            //Console.WriteLine("JUMPING");
            SetPlayerState(PlayerState.Jumping);

            HeadAnimator.SetState("Jumping");
            TorsoLegsAnimator.SetState("Jumping");
            LeftArmAnimator.SetState("Jumping");
            RightArmAnimator.SetState("Jumping");

            physicsComponent.GravityMultiplier = defaultGravityMultiplier;

            //Console.WriteLine("FORCE BEFORE APPLY: " + PhysicsComponent.Velocity.Y);
            PhysicsComponent.Velocity = new Vector2(PhysicsComponent.Velocity.X, 0);
            PhysicsComponent.AddForce(new Vector2(0, -1), jumpForce * 2.2f, maxJumpForce * 2.2f);
            //Console.WriteLine("FORCE AFTER APPLY: " + PhysicsComponent.Velocity.Y);
        }

        private void EnterFalling()
        {
            //Console.WriteLine("FALLING");
            SetPlayerState(PlayerState.Falling);

            timeStartedFalling = DateTime.Now;

            HeadAnimator.SetState("Falling");
            TorsoLegsAnimator.SetState("Falling");
            LeftArmAnimator.SetState("Falling");
            RightArmAnimator.SetState("Falling");

            jumpTime = 0f;
            physicsComponent.GravityMultiplier = fallingGravityMultiplier;

        }

        private void EnterDead()
        {
            SetPlayerState(PlayerState.Dead);

            SetInputState(InputState.Deactivated);

            HeadRenderer.visible = false;
            LeftArmRenderer.visible = false;
            RightArmRenderer.visible = false;

			InstanceService.Destroy(weapons.currentWeapon);
			weapons.currentWeapon = null;

            TorsoLegsAnimator.SetState("Death");

            GameManager.GameState = GameState.GameOver;

        }

        private void Idle()
        {
            if (!physicsComponent.IsGrounded/*PhysicsComponent.Velocity.Y != 0f*/) //from idle to falling
            {
                EnterFalling();
            }
            else if (jumpPressed && canJumpAgain) //from idle to jumping
            {
                //Console.WriteLine("NormalJump");
                EnterJumping();
            }
            else if (Math.Abs(movement) > 0.1f) //from idle to walking
            {
                EnterWalking();
            }
        }

        private void Walking()
        {

            if (!physicsComponent.IsGrounded/*PhysicsComponent.Velocity.Y != 0f*/)
            {
                EnterFalling();
            }
            else if (jumpPressed && canJumpAgain) //from walking to jumping
            {
                //Console.WriteLine("NormalJump");
                EnterJumping();
            }
            else if (Math.Abs(movement) <= 0.1f) //from walking to idle
            {
                EnterIdle();
            }
        }

        private void Jumping()
        {
            /*this does not work with jump buffering
            Console.WriteLine("VELOCITY: "+ physicsComponent.Velocity.Y);
            /*if (physicsComponent.IsGrounded && physicsComponent.Velocity.Y >= 0)
            
			{
                if (Math.Abs(movement) <= 0.1f) //from jumping to idle
                {
                    EnterIdle();
                }

                else if (Math.Abs(movement) > 0.1f) //from jumping to walking
                {
                    EnterWalking();
                }
			}
			*/

            if (PhysicsComponent.Velocity.Y > 0 || PhysicsComponent.IsGrounded) //from jumping to falling
            {
                EnterFalling();
            }

            jumpTime += Time.DeltaTime;
        }

        private void Falling()
        {

            if (physicsComponent.IsGrounded)
            {
                // JUMP BUFFER CHECK
                float lastTimeJumpPressedDifference = HelperFunctionsUtils.DateTimeDifferenceInSeconds(lastTimeJumpPressedInAir, DateTime.Now);
                if (lastTimeJumpPressedDifference < jumpBufferTime)
                {
                    //Console.WriteLine("JUMP BUFFER JUMP");
                    jumpTime = 0;
                    EnterJumping();
                }
                // END OF JUMP BUFFER CHECK
                else if (Math.Abs(movement) <= 0.1f) //from jumping to falling
                {
					PlayFootstepSound();
					EnterIdle();
                }
                else if (Math.Abs(movement) > 0.1f) //from jumping to walking
                {
                    EnterWalking();
                }
                physicsComponent.GravityMultiplier = defaultGravityMultiplier; // resetting the gravity multiplier back to the default value
            }

            // COYOTE TIME CHECK
            if (jumpPressed && canJumpAgain && previousState != PlayerState.Jumping)
            {
                float lastTimeJumpPressedDifference = HelperFunctionsUtils.DateTimeDifferenceInSeconds(timeStartedFalling, lastTimeJumpPressedInAir);
                if (lastTimeJumpPressedDifference < coyoteTime)
                {
                    //Console.WriteLine("COYTE TIME");
                    jumpTime = 0;
                    EnterJumping();
                }
            }
        }

        private void Dead()
        {

        }

        #endregion


        /// <summary>
        /// Updates the PlayerState variable and the previouse state
        /// actually switching to the state will happen the next frame
        /// </summary>
        /// <param name="state"></param>
        public void SetPlayerState(PlayerState state)
        {
            previousState = currentState;
            currentState = state;
        }

        /// <summary>
        /// The amount will be subtracted from the players health if the health drops below 1 
        /// Player will enter the Dead state
        /// </summary>
        /// <param name="amount"></param>
		public void TakeDamage(int amount, GameObject origin)
        {
            DeathArea.DeathArea area = origin as DeathArea.DeathArea;
            if (area != null) lastDamage = invincibleCooldown + 1;

            if (lastDamage >= invincibleCooldown)
            {
                Health -= amount;
                onDamageTaken?.Invoke(origin);

                Console.WriteLine("HITTTTTTTTTTTTTTTTTTT");
                CameraService.StartCameraShake(shake);

                if (Health <= 0)
                {
                    Health = 0;
                    onHealthZero?.Invoke();
                }

                lastDamage = 0f;
            }
        }

		/// <summary>
		/// If true it flips the player sprites and the gun
		/// </summary>
		/// <param name="left"></param>
		private void FlipPlayer(bool left)
		{
			if (left)
			{
				HeadRenderer.FlipSpriteVerticaly = true;
				LeftArmRenderer.FlipSpriteVerticaly = true;
				RightArmRenderer.FlipSpriteVerticaly = true;
				TorsoLegsRenderer.FlipSpriteVerticaly = true;
				weapons.FlipSprite(true);

				RightArmRenderer.ZIndex = -2;
				LeftArmRenderer.ZIndex = 2;
				RightArmRenderer.TextureOffset = new Vector2(-1, -5); //right is turning to the left arm -> less drawing
				LeftArmRenderer.TextureOffset = new Vector2(3, -5); //left is turning to the right arm
				pistolOffset = new Vector2(-3f, 5f);
				if (!weaponPosFlipped) 
				{ 
					weapons.currentWeapon.weaponPos = weapons.currentWeapon.weaponPos * new Vector2(1, -1);
                    weaponPosFlipped = true;
                }
            }
			else
			{
				HeadRenderer.FlipSpriteVerticaly = false;
				LeftArmRenderer.FlipSpriteVerticaly = false;
				RightArmRenderer.FlipSpriteVerticaly = false;
				TorsoLegsRenderer.FlipSpriteVerticaly = false;
				weapons.FlipSprite(false);

                RightArmRenderer.ZIndex = -2;
				LeftArmRenderer.ZIndex = 2;
				RightArmRenderer.TextureOffset = new Vector2(0, -5);
				LeftArmRenderer.TextureOffset = new Vector2(-3, -5);
				pistolOffset = new Vector2(3.4f, 5);
                if (weaponPosFlipped)
                {
                    weapons.currentWeapon.weaponPos = weapons.currentWeapon.weaponPos * new Vector2(1, -1);
                    weaponPosFlipped = false;
                }
            }
		}

        /// <summary>
        /// Checks the Interact range for interactibles and interacts with the first one found
        /// </summary>
        private void CheckInteractRange()
        {
            // go through all interactivles and check if player is interacting with them
            foreach (BoxCollider2D col in interactRange.currentlyTriggeredObjects.ToArray())
            {
                if (col.Parent == this) continue;
                IInteractible interactible = col.Parent as IInteractible; //returns IInteractibel or if none: 0
                if (interactible == null) continue;


                Console.WriteLine("interact range: " + col.Parent.Name);

                interactible.Interact(this);
                break;
            }

            Console.WriteLine("---------------------------");
        }

        private void GroundCheckEntered(BoxCollider2D other)
        {
            if (other.Parent == this) return; // ignore self collisions
            if (!other.IsCollider) return;
            PhysicsComponent.IsGrounded = true;
        }

        private void GroundCheckExited(BoxCollider2D other)
        {
            foreach (BoxCollider2D col in groundCheck.currentlyTriggeredObjects)
            {
                if (col.Parent == this) continue; //ignore self collisions
                if (col.IsCollider)
                {
                    return;
                }
            }

            if (other.Parent == this) return; //ignore self collisions
            PhysicsComponent.IsGrounded = false;
        }

		#region Weapon 
		/// <summary>
		/// Rotates the left art towards the mouse and the current gun
		/// </summary>
		private void PointGunToMouse()
		{
			//Player holding Pistol in Hand implementation
			Vector2 mousePosToWorld = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), SceneService.ActiveScene.mainCamera.camera);
			Vector2 direction = Vector2.Normalize(mousePosToWorld - GetPosition());
			leftArmPivot.SetPosition(GetPosition() - pistolOffset);
			leftArmPivot.RotateTowards(direction);


			weapons.currentWeapon.SetPosition(leftArmPivot.GetPosition() + (leftArmPivot.GetForwardVector() * weapons.currentWeapon.weaponPos.X) + (leftArmPivot.GetUpVector() * weapons.currentWeapon.weaponPos.Y));
			
			weapons.currentWeapon.RotateTowards(direction);

            if (LeftArmAnimator.GetState() != "CloseCombat")
            {
                LeftArmRenderer.LocalRotation = leftArmPivot.GetRotationInDeg() + 270;
            }
            else
            {
                if (LeftArmRenderer.FlipSpriteVerticaly)
                {
                    LeftArmRenderer.LocalRotation = leftArmPivot.GetRotationInDeg() + 180;
                }
                else
                {
                    LeftArmRenderer.LocalRotation = leftArmPivot.GetRotationInDeg();
                }
            }
        }

        /// <summary>
        /// Controls the active weapon in the players Hand
        /// </summary>
        private void HandleGun()
        {
            PointGunToMouse();

			if (Raylib.IsMouseButtonDown(MouseButton.Left) && weapons.currentWeapon.IsAutomatic && currentInputState == InputState.Activated)
			{
				weapons.Shoot(this);
			}
			else
			{
				if (Raylib.IsMouseButtonPressed(MouseButton.Left) && currentInputState == InputState.Activated)
				{
					weapons.Shoot(this);
				}
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Q) && currentInputState == InputState.Activated) 
			{
				weapons.ThrowAwayCurrentWeapon(this);
			}

            if (Raylib.IsKeyPressed(KeyboardKey.F) && currentInputState == InputState.Activated)
            {
                weapons.SwitchWeapons();
            }
		}
		#endregion

        public override void Destroy()
        {
            base.Destroy();

			if(weapons.currentWeapon != null)
			{
				InstanceService.Destroy(weapons.currentWeapon);
				weapons.currentWeapon = null;
			}

            if (playerPanel != null)
            {
                InstanceService.Destroy(playerPanel);
                playerPanel = null;
            }

            if (leftArmPivot != null)
            {
                InstanceService.Destroy(leftArmPivot);
                leftArmPivot = null;
            }
        }
        /// <summary>
        /// kicks player back
        /// </summary>
        /// <param name="other"></param>
        private void ApplyKnockback(GameObject other)
        {
            if (health<=0) return;
            physicsComponent.Velocity = new Vector2();
            inKnockback = true;
            knockbackTimer = 0;
            float dir = (GetPositionX() < other.GetPositionX()) ? -1.0f : 1.0f;
            physicsComponent.AddForce(new Vector2(dir, -1), 300, 300);
        }

        /// <summary>
        /// make sprite invisible
        /// </summary>
        private void Invisible()
        {
            if (!isInvisible)
            {
                isInvisible = true;
                HeadRenderer.visible = false;
                RightArmRenderer.visible = false;
                LeftArmRenderer.visible = false;
                TorsoLegsRenderer.visible = false;
            }
        }
        /// <summary>
        /// make sprite visible
        /// </summary>
        private void VisibleSprite()
        {
            if (isInvisible)
            {
                isInvisible = false;
                HeadRenderer.visible = true;
                RightArmRenderer.visible = true;
                LeftArmRenderer.visible = true;
                TorsoLegsRenderer.visible = true;
            }
        }
        /// <summary>
        /// flicking sprite
        /// currently using when player takes damage
        /// </summary>
        private void Flicker()
        {
            float flickerIntervall = 0.07f;
            if (flickerTimer < flickerIntervall) { Invisible();return; }
            else if (flickerTimer < flickerIntervall*3) { VisibleSprite();return; }
            flickerTimer = 0;
        }

        public void SetInputState(InputState inputState)
        {
            currentInputState = inputState;
        }

        public void PlayFootstepSound()
        {
			int randomSoundIndex = lastFootstepSoundIndex;

			while (randomSoundIndex == lastFootstepSoundIndex)
			{
				randomSoundIndex = random.Next(0, footstepSounds.Length);
			}

			lastFootstepSoundIndex = randomSoundIndex;

			runSound.FilePath = footstepSounds[randomSoundIndex];
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			runSound.PlayOneShot(runSound.Volume, pitch);
		}
    }
}