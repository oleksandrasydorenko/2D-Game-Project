using JailBreaker.Game.Classes.Weapons;
using Raylib_cs;
using RocketEngine;
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
    public enum PlayerState3
    {
        Idle,
        Walking,
        Airborne,
    }

    public class DanielsPlayer : GameObject
    {

        SpriteAnimatorComponent animator;
        PlayerState3 currentState;
        
        PhysicsComponent physics;
        BoxCollider2D collider2D;

        SpriteComponent renderer;

        public override void Construct()
        {
            base.Construct();
            Console.WriteLine("Daniel's Player Constructed");
            Sprite playerSpite = new Sprite("Game/Assets/Textures/Player_Idle.png", 4);
            Sprite walkingSprite = new Sprite("Game/Assets/Textures/Player_Walking.png", 8);
            Sprite jumpingSprite = new Sprite("Game/Assets/Textures/Player_Jump.png", 3);

             renderer = new SpriteComponent(this, playerSpite, Raylib_cs.Color.White);
            
            AnimationSignal middlePointReachedSignal = new AnimationSignal(4);
            middlePointReachedSignal.onAnimationSignalTriggered += () => Console.WriteLine("Hey das ist die mitte der animation");

            // animations signale um zu wissen wann ein frame in der animation erreicht wurde
            AnimationSignal[] signals =
            {
                middlePointReachedSignal,
            };

            SpriteAnimation animation = new SpriteAnimation(playerSpite, .5f);
            SpriteAnimation walkanim = new SpriteAnimation(walkingSprite, .5f, loop: true, animationSignals: signals);
            SpriteAnimation jumpAnim = new SpriteAnimation(jumpingSprite, .5f, loop: false);


            AnimationControllerState idleState = new AnimationControllerState("Idle", animation);
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

            animator = new SpriteAnimatorComponent(this, renderer, controller);

            //render.spriteScale = 4; // wird etz über die camera geamcht
            PhysicsComponent physics = new PhysicsComponent(this, 5f, "PhysicsComponent");
            this.physics = physics;
 


            collider2D = new BoxCollider2D(this, width: playerSpite.FrameWidth * renderer.SpriteScale, height: playerSpite.FrameHeight * renderer.SpriteScale);
            collider2D.drawHitbox = true;
            collider2D.onTriggerEntered += TriggerEntered;
            collider2D.onTriggerExited += TriggerExited;
            collider2D.onCollider += OnCol;
            collider2D.IsTrigger = true;
            collider2D.IsCollider = true;

        }

        public void TriggerEntered(BoxCollider2D other)
        {
            Console.WriteLine("Hey ich bins der " + Name + " und der hat mein trigger betreten : " + other.Name);
        }
        public void TriggerExited(BoxCollider2D other)
        {
            Console.WriteLine("Hey ich bins der " + Name + " und der hat mein trigger verlassen : " + other.Name);
        }
        public void OnCol(BoxCollider2D other)
        {
            Console.WriteLine(Name + ": Collsision mit " + other.Name);
        }
        public override void Start()
        {
            base.Start();
            SetPosition(250, 250);
            physics.Drag = 2.0f;
            
        }

            float walkForce = 50f;
        float maxWalkSpeed = 100f;
        float jumpForce = 300f;
        float maxJumpSpeed = 1000f;
        

        public override void Update()
        {
            base.Update();

            if (Name == "Player2") return;
            

            float movement = 0f;
            if (IsKeyDown(KeyboardKey.D)) movement += 1f;
            if(IsKeyDown(KeyboardKey.A)) movement -= 1f;
            bool jumpPressed = IsKeyDown(KeyboardKey.Space);

            
            switch (currentState)
            {
                case PlayerState3.Idle:
                    if (physics.Velocity.Y != 0f)
                    {
                        currentState = PlayerState3.Airborne;
                        animator.SetState("Jumping");
                        physics.IsGrounded = false;
                    }
                    if (jumpPressed)
                    {
                        currentState = PlayerState3.Airborne;
                        animator.SetState("Jumping");
                        physics.AddForce(new Vector2(0, -1), jumpForce, maxJumpSpeed);
                        physics.IsGrounded = false;
                    }
                    
                    else if (Math.Abs(movement) > 0.1f)
                    {
                        currentState = PlayerState3.Walking;
                        animator.SetState("Walking");
                        physics.IsGrounded = true;
                    }
                    break;

                case PlayerState3.Walking:
                    if (physics.Velocity.Y != 0f)
                    {
                        currentState = PlayerState3.Airborne;
                        animator.SetState("Jumping");
                        physics.IsGrounded = false;
                    }
                    if (jumpPressed)
                    {
                        currentState = PlayerState3.Airborne;
                        animator.SetState("Jumping");
                        physics.AddForce(new Vector2(0, -1), jumpForce, maxJumpSpeed);
                        physics.IsGrounded = false;
                    }
                    
                    else if (Math.Abs(movement) <= 0.1f)
                    {
                        currentState = PlayerState3.Idle;
                        animator.SetState("Idle");
                    }
                    break;

                case PlayerState3.Airborne:
                    if (physics.Velocity.Y == 0f)
                    {
                        if (Math.Abs(movement) <= 0.1f)
                        {
                            currentState = PlayerState3.Idle;
                            animator.SetState("Idle");
                        }
                        else
                        {
                            currentState = PlayerState3.Walking;
                            animator.SetState("Walking");
                        }
                        physics.IsGrounded = true;
                    }

                    break;
            }


            if (MathF.Abs(movement) > 0.01f)
            {
                physics.AddForce(new Vector2(movement, 0f), walkForce, maxWalkSpeed);


                // einfache Richtungsausgabe (Anpassung möglich)
                float faceAngle = movement > 0 ? 0f : 180f;
                SetRotation(faceAngle);
            }





        }
    }
}
