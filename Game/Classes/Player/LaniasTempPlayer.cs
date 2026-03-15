using JailBreaker.Interactibles;
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
    /*public enum PlayerState2
    {
        Idle,
        Walking,
        Jumping,
    }*/

    public class LaniasTempPlayer : GameObject
    {

        private SpriteAnimatorComponent animator;
        PlayerState currentState;
        public float walkSpeed = 50;

        public bool hasKey;

        public override void Construct()
        {
            base.Construct();

            Sprite playerSpite = new Sprite("Game/Assets/Textures/Player_Idle.png", 4);
            Sprite walkingSprite = new Sprite("Game/Assets/Textures/Player_Walking.png", 8);
            Sprite jumpingSprite = new Sprite("Game/Assets/Textures/Player_Jump.png", 3);

            SpriteComponent render = new SpriteComponent(this, playerSpite, Raylib_cs.Color.White);

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

            jumpAnim.onAnimationFinished += JumpDone; // wenn die jump animation fertig ist dann setzt ich den state auf idle 

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

            animator = new SpriteAnimatorComponent(this, render, controller);

            //render.spriteScale = 4; // wird etz über die camera geamcht

            render.SpriteScale = 1;

            BoxCollider2D collider = new BoxCollider2D(this, width: playerSpite.FrameWidth * render.SpriteScale, height: playerSpite.FrameHeight * render.SpriteScale);
            collider.drawHitbox = true;
            collider.onTriggerEntered += TriggerEntered;
            collider.onTriggerExited += TriggerExited;
            collider.onCollider += OnCol;
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
            Console.WriteLine(Name + ": Collsision mit " + other.Name);
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


            if (Name == "Player2") return;

            float horizontal = 0f;
            float vertical = 0f;

            if (IsKeyDown(KeyboardKey.A)) { horizontal -= 1; }
            if (IsKeyDown(KeyboardKey.D)) { horizontal += 1; }
            if (IsKeyDown(KeyboardKey.W)) { vertical -= 1; }
            if (IsKeyDown(KeyboardKey.S)) { vertical += 1; }

            Vector2 inputVector = new Vector2(horizontal, vertical);
            float norm = MathF.Sqrt(MathF.Pow(inputVector.X, 2) + MathF.Pow(inputVector.Y, 2)); // normalizing the input vector 
            inputVector = norm != 0 ? 1f / norm * inputVector : Vector2.Zero; // if the norm is zero the division is not allowed default back to (0,0)

            if (IsKeyPressed(KeyboardKey.Space))
            {
                if (!(currentState == PlayerState.Jumping))
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


                    //////////////// Code von copilot zum abbilden eines einheits Vector 2s auf 0-360  

                    // -y wegen des komischen koordinaten systems
                    float angleRad = (float)Math.Atan2(inputVector.X, -inputVector.Y); // Ergebnis in Radiant (-π bis π)
                    float angleDeg = angleRad * (180f / (float)Math.PI); // Umrechnung in Grad

                    // Falls negativ, auf positiven Bereich abbilden
                    if (angleDeg < 0)
                        angleDeg += 360f;

                    SetRotation(angleDeg - 90);

                    //////////// 

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

            if (currentState != PlayerState.Idle)
            {
                Vector2 lastPos = GetPosition();

                Vector2 forward = GetForwardVector();

                // setzten der neuen position

                SetPosition(lastPos += (inputVector * walkSpeed * Time.DeltaTime));
            }

        }
    }
}
