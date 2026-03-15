using JailBreaker.Game.Classes.Weapons.Projectiles;
using JailBreaker.Player;
using RocketEngine;
using RocketEngine.Utils;
using System.Numerics;

namespace JailBreaker.Enemy
{

    public class PoliceRoboter : Enemy
    {
        public bool ShootHorizontal { get { return shootHorizontal; } set { shootHorizontal = value; } }
        bool shootHorizontal;
        float timer;
        float searchTimer = 10f;
        float shootTimer;
        Vector2 tempPatrolRange;
        float firerate = 2.5f;
        float jumpForce = 280;
        float maxJumpSpeed = 280;
        bool isColliding = false;
        float spriteOffset;
        int countCollider;

        Random random = new Random();

        #region Enemy Sound
        public AudioComponent runSound;
        private string[] footstepSounds = { "Game/Assets/Audio/Footsteps/RunningSound3.wav", "Game/Assets/Audio/Footsteps/RunningSound4.wav", "Game/Assets/Audio/Footsteps/RunningSound5.wav", "Game/Assets/Audio/Footsteps/RunningSound6.wav", "Game/Assets/Audio/Footsteps/RunningSound7.wav" };
        int lastFootstepSoundIndex = 0;

        public AudioComponent injuredSound;
		private string[] injuredSounds = { "Game/Assets/Audio/PoliceRobot/InjuredPolice.mp3", "Game/Assets/Audio/PoliceRobot/PoliceHurt.mp3" };

        #endregion

        public override void Construct()
        {
            base.Construct();
            #region Audio
            runSound = new AudioComponent(this, "Game/Assets/Audio/Footsteps/RunningSound3.wav", false, 0.5f, 1, true, true, 10, 300);
            injuredSound = new AudioComponent(this, injuredSounds[random.Next(0,injuredSounds.Length)], false, .4f, 1, true, true, 10, 300);


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
            #endregion

            #region Sprite
            Sprite idleSprite = new Sprite("Game/Assets/Textures/Enemy/PolicePatrol2.png", 6);
            Sprite attackingSprite = new Sprite("Game/Assets/Textures/Enemy/PoliceAttack2.png", 6);
            Sprite deadSprite = new Sprite("Game/Assets/Textures/PoliceDead.png", 11);
            spriteOffset = idleSprite.FrameWidth / 2;

            renderer = new SpriteComponent(this, idleSprite, Raylib_cs.Color.White, name: "PoliceRenderer");

            SpriteAnimation animation = new SpriteAnimation(idleSprite, .5f, animationSignals: [footDownLeft, footDownRight]);
            SpriteAnimation attackanim = new SpriteAnimation(attackingSprite, .4f, loop: true, animationSignals: [footDownLeft, footDownRight]);
            SpriteAnimation deadanim = new SpriteAnimation(deadSprite, 0.5f, loop: false);

            AnimationControllerState idleState = new AnimationControllerState("Idle", animation);
            AnimationControllerState attackingState = new AnimationControllerState("Attack", attackanim);
            AnimationControllerState deadState = new AnimationControllerState("Dead", deadanim);

            AnimationControllerState[] allStates =
            {
                idleState,
                attackingState,
                deadState,
            };
            AnimationController controller = new AnimationController(allStates);

            animator = new SpriteAnimatorComponent(this, renderer, controller);

            renderer.SortingLayer = SortingLayers.Enemy;
            #endregion

            #region Physics
            PhysicsComponent physics = new PhysicsComponent(this, 5f, "PhysicsComponent");
            physics.Groundy0 = false;
            this.physics = physics;
            this.physics.Drag = 5f;

            walkForce = 40f;
            maxWalkSpeed = 40f;
            #endregion

            #region Collider
            collider = new BoxCollider2D(this, offsetY: -0.4f, width: idleSprite.FrameWidth * renderer.SpriteScale - 20, height: idleSprite.FrameHeight * renderer.SpriteScale);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Enemy;
            //Area where enemy detects player
            detectingSpace = new BoxCollider2D(this, width: idleSprite.FrameWidth * renderer.SpriteScale + 150, height: idleSprite.FrameHeight * renderer.SpriteScale + 30);
            detectingSpace.IsTrigger = true;
            detectingSpace.onTriggerEntered += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null && !isDead) { PlayerDetected(player); }; };
            detectingSpace.onTriggerExited += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null && !isDead) { PlayerLost(player); } };

            //trigger when player touches enemy
            interactRange = new BoxCollider2D(this, offsetY: -3f, width: idleSprite.FrameWidth * renderer.SpriteScale - 15, height: idleSprite.FrameHeight * renderer.SpriteScale + 5);
            interactRange.IsTrigger = true;
            interactRange.onTriggerEntered += (BoxCollider2D other) =>
            {
                if (!isDead && other.IsCollider)
                {
                    if (other.CollisionLayer == CollisionLayers.Default)
                    {
                        countCollider++;
                        if (currentState == EnemyState.Patrol) ChangeDirection();
                        else isColliding = true;
                    }
                    else if (other.CollisionLayer == CollisionLayers.Obstacle)
                        isColliding = true;
                }
                LaniasPlayer player = other.Parent as LaniasPlayer;
                if (player != null && !isDead) IsTouching(player); 
            };

            interactRange.onTriggerExited += (BoxCollider2D other) =>
            {
                LaniasPlayer player = other.Parent as LaniasPlayer;
                if (player != null && !isDead) { PlayerDetected(player); }
                else if (!isDead && other.IsCollider && other.CollisionLayer == CollisionLayers.Default) countCollider--;
            };
            #endregion

            #region Health
            MaxHealth = 30;
            Health = MaxHealth;

            onDamageTaken += (GameObject origin) =>
            {
                ApplyKnockback(origin);

                timer = 0;
                LaniasPlayer player = origin as LaniasPlayer;
                if (player != null && !isDead)
                {
                    searchTimer = 0;
                    tempPatrolRange = new Vector2(player.GetPositionX() - 50, player.GetPositionX() + 80);
                }
            };

            onHealthZero += () => { currentState = EnemyState.Dead; Dead(); };
            #endregion

            patrolRange = new Vector2(GetPositionX() - 50, GetPositionX() + 50);
            enemyLoot = EnemyLootOptions.Random;
            shootHorizontal = true;
        }

        public override void Update()
        {
            base.Update();

            knockbackTimer += Time.DeltaTime;
            if (knockbackTimer > 0.7f) inKnockback = false;

            timer += Time.DeltaTime;
            searchTimer += Time.DeltaTime;
            shootTimer += Time.DeltaTime;

            renderer.FlipSpriteVerticaly = direction < 0f;

            //jump when colliding whith ocstacle
            if (isColliding && timer > 1 && !isDead)
            {
                timer = 0f;
                this.physics.AddForce(new Vector2(0, -1), jumpForce, maxJumpSpeed);
                isColliding = false;
            }

            switch (currentState)
            {
                case EnemyState.Patrol:
                    if (searchTimer < 5f)
                    {
                        Patrol(tempPatrolRange);
                    }
                    else Patrol(patrolRange);
                    break;

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
        /// <summary>
        /// Chase and shoot Player
        /// </summary>
        protected override void Chase()
        {
            base.Chase();
            if (shootTimer >= firerate)
            {
                shootTimer = 0;
                if (shootHorizontal)
                {
                    InstanceService.Instantiate(new Bullet(150, new Vector2(direction, 0), GetPosition(), 2, 1, this));
                    return;
                }
                if (target == null) return;
                Vector2 dir = target.GetPosition() - GetPosition();
                InstanceService.Instantiate(new Bullet(150, dir, new Vector2(GetPositionX() + spriteOffset * direction, GetPositionY()), 2, 1, this));
            }
        }
        /// <summary>
        /// Enemy dies
        /// </summary>
        protected override void Dead()
        {
            if (!isDead)
            {
                isDead = true;
                timer = 0;
                animator.SetState("Dead");
                collider.CollisionLayer = CollisionLayers.GhostEnemy;
                SpawnLoot();
                injuredSound.Play();
            }
            if (timer >= 20) InstanceService.Destroy(this);
        }
        /// <summary>
        /// Enemy detects Player
        /// </summary>
        /// <param name="player"></param>
        private void PlayerDetected(LaniasPlayer player)
        {
            animator.SetState("Attack");
            target = player;
            maxWalkSpeed = walkForce / 3f;
            currentState = EnemyState.Chase;

        }

        /// <summary>
        /// when player is out of sight
        /// </summary>
        /// <param name="player"></param>
        private void PlayerLost(LaniasPlayer player)
        {
            animator.SetState("Idle");
            tempPatrolRange = new Vector2(player.GetPositionX() - 20, player.GetPositionX() + 20);
            searchTimer = 0;
            target = null;
            maxWalkSpeed = walkForce;
            currentState = EnemyState.Patrol;

        }

        /// <summary>
        /// when player is touching enemy
        /// </summary>
        /// <param name="player"></param>
        private void IsTouching(LaniasPlayer player)
        {
            Console.WriteLine("Attack");
            currentState = EnemyState.Attack;
        }
        private void ChangeDirection()
        {
            if (countCollider > 1)
                return;
            direction *= -1;
        }
        /// <summary>
        /// Appy Footstep Sound
        /// </summary>
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
