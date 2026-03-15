using JailBreaker.Player;
using RocketEngine;
using System.Numerics;

namespace JailBreaker.Enemy
{
    public class Rat : Enemy
    {
        AudioComponent ratSound;
        AudioComponent injuredSound;
        float jumpForce = 200;
        float maxJumpSpeed = 200;
        float jumpcooldown = 1;
        float timer = 1;
        bool isColliding = false;
        public override void Construct()
        {
            base.Construct();
            Name = "Rat";
            ratSound = new AudioComponent(this, "Game/Assets/Audio/Rat/Rat.mp3", false, 0.5f, 1, true, true);
            injuredSound = new AudioComponent(this, "Game/Assets/Audio/Rat/InjuredRat.mp3", false, 0.5f, 1, true, true);

            Sprite idleSprite = new Sprite("Game/Assets/Textures/Rat.png", 4);
            Sprite deadSprite = new Sprite("Game/Assets/Textures/DeadRat.png", 6);

            renderer = new SpriteComponent(this, idleSprite, Raylib_cs.Color.White, name: "RatRenderer");

            SpriteAnimation animation = new SpriteAnimation(idleSprite, .5f);
            SpriteAnimation animation2 = new SpriteAnimation(deadSprite, .5f, false);

            AnimationControllerState idleState = new AnimationControllerState("Idle", animation);
            AnimationControllerState deadState = new AnimationControllerState("Dead", animation2);

            AnimationControllerState[] allStates =
            {
                idleState,
                deadState,
            };
            AnimationController controller = new AnimationController(allStates);

            animator = new SpriteAnimatorComponent(this, renderer, controller);

            renderer.SortingLayer = SortingLayers.Enemy;


            PhysicsComponent physics = new PhysicsComponent(this, 5f, "PhysicsComponent");
            physics.Groundy0 = false;
            this.physics = physics;
            physics.Drag = 4;
            walkForce = 50f;
            maxWalkSpeed = 50f;

            //Health
            MaxHealth = 1;
            Health = MaxHealth;


            collider = new BoxCollider2D(this, width: idleSprite.FrameWidth * renderer.SpriteScale - 12, height: idleSprite.FrameHeight * renderer.SpriteScale - 25);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Enemy;
            //Area where enemy detects player
            detectingSpace = new BoxCollider2D(this, width: idleSprite.FrameWidth * renderer.SpriteScale + 400, height: idleSprite.FrameHeight * renderer.SpriteScale + 100);
            detectingSpace.IsTrigger = true;
            detectingSpace.onTriggerEntered += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null && !isDead) { PlayerDetected(player); }; };

            //trigger when player touches enemy
            interactRange = new BoxCollider2D(this, offsetY: -5f, width: idleSprite.FrameWidth - 7, height: idleSprite.FrameHeight - 15);
            interactRange.IsTrigger = true;
            interactRange.onTriggerEntered += (BoxCollider2D other) =>
            {
                LaniasPlayer player = other.Parent as LaniasPlayer;
                if (player != null && !isDead) { isColliding = true; IsTouching(player); }
                else if (!isDead && other.IsCollider) { isColliding = true; };
            };
            interactRange.onTriggerExited += (BoxCollider2D other) =>
            {
                LaniasPlayer player = other.Parent as LaniasPlayer;
                if (player != null && !isDead) { isColliding = false; PlayerDetected(player); }
                else if (!isDead && other.IsCollider) { isColliding = false; };
            };

            onDamageTaken += (GameObject origin) => ApplyKnockback(origin);
            onDamageTaken += (GameObject origin) => { ratSound.Stop();  };

            onHealthZero += () => { currentState = EnemyState.Dead; injuredSound.Play(); };

        }
        public override void Update()
        {
            base.Update();
            knockbackTimer += Time.DeltaTime;
            if (knockbackTimer > 0.4f) inKnockback = false;
            timer += Time.DeltaTime;
            renderer.FlipSpriteVerticaly = direction < 0f;
            //jump when colliding whith ocstacle
            if (isColliding && timer > jumpcooldown && !isDead)
            {
                timer = 0f;
                this.physics.AddForce(new Vector2(0, -1), jumpForce, maxJumpSpeed);
                
            }

            switch (currentState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;

                case EnemyState.Attack:
                    Attack();
                    Chase();
                    break;

                case EnemyState.Dead:
                    Dead();
                    break;
            }
        }
        protected override void Chase()
        {
            if (target==null) return;
            if (Math.Abs(target.GetPositionX() - GetPositionX()) < 2f) return;

            base.Chase();
        }
        protected override void Attack()
        {
            base.Attack();
            if (timer > jumpcooldown && !isDead)
            {
                timer = 0f;
                this.physics.AddForce(new Vector2(0, -1), jumpForce, maxJumpSpeed);
            }
        }
        protected override void ApplyKnockback(GameObject other)
        {
            physics.Velocity = new Vector2();
            inKnockback = true;
            knockbackTimer = 0;
            physics.AddForce(new Vector2(0, -1), jumpForce, jumpForce);
        }
        /// <summary>
        /// when player is in range
        /// </summary>
        /// <param name="player"></param>
        private void PlayerDetected(LaniasPlayer player)
        {
            target = player;
			ratSound.PlayOneShot(ratSound.Volume, RocketEngine.Utils.MathUtils.RandomFloatInRange(.8f, 1.1f));
			currentState = EnemyState.Chase;

        }
        /// <summary>
        /// when player is colliding
        /// </summary>
        /// <param name="player"></param>
        private void IsTouching(LaniasPlayer player)
        {
            target = player;
            currentState = EnemyState.Attack;
        }
        protected override void Dead()
        {
            if (!isDead)
            {
                isDead = true;
                timer = 0;
                animator.SetState("Dead");
                collider.CollisionLayer = CollisionLayers.GhostEnemy;
                injuredSound.PlayOneShot(injuredSound.Volume, RocketEngine.Utils.MathUtils.RandomFloatInRange(.8f,1.1f));
            }
            this.physics.AddForce(new Vector2(0,1),1f,1f);
            if (timer >= 10)
                base.Dead();
        }
    }
}
