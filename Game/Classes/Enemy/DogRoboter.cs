using JailBreaker.Player;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Utils;
using System.Numerics;

namespace JailBreaker.Enemy
{
    public class DogRoboter : Enemy
    {
        float timer;
        float offsetTimer;
        float searchTimer = 10f;
        int triggerCount; //helps identifying in which trigger player is
        float cooldown = 0.4f;
        EnemyState? queuedState = null; //safes next state
        Vector2 targetPatrolRange;
        float jumpForce = 280;
        float maxJumpSpeed = 280;
        bool isColliding = false;
        // AudioComponent footstepAudio;
        AudioComponent biteAudio;
        AudioComponent deadAudio;
		AudioComponent chaseAudio;
		int countCollider;
        Random random;

        public override void Construct()
        {
            base.Construct();

            #region Audio
            //footstepAudio = new AudioComponent(this, "Game/Assets/Audio/keyPadTyping.mp3", use3DAudio: true, useDistanceBasedSound: true, maxDistance: 150);
            deadAudio = new AudioComponent(this, "Game/Assets/Audio/Dog/InjuredDog.mp3", false, 1.2f, 1f, true, true);
			biteAudio = new AudioComponent(this, "Game/Assets/Audio/Dog/DogAttack.mp3", false, .4f, 1f, true, true);
			chaseAudio = new AudioComponent(this, "Game/Assets/Audio/Dog/DogBark.wav", false, .5f, 1f, true, true);


			// AnimationSignal footdownFrame1signal = new AnimationSignal(1);
			// AnimationSignal footdownFrame4signal = new AnimationSignal(4);
			// AnimationSignal[] footstepSignals = { footdownFrame1signal, footdownFrame4signal };
			AnimationSignal biteSignal = new AnimationSignal(3);
            AnimationSignal[] biteSignals = { biteSignal };


           // footdownFrame1signal.onAnimationSignalTriggered += () => { footstepAudio.PlayOneShot(0.5f); };
           // footdownFrame4signal.onAnimationSignalTriggered += () => { footstepAudio.PlayOneShot(0.5f); };
            biteSignal.onAnimationSignalTriggered += () => { if (target != null && target.Health > 0) target.TakeDamage(attackingForce, this); biteAudio.PlayOneShot(biteAudio.Volume, MathUtils.RandomFloatInRange(.8f, 1.1f)); };
            #endregion

            #region Sprites
            Sprite idleSprite = new Sprite("Game/Assets/Textures/Dog3-Sheet.png", 6);
            Sprite walkingSprite = new Sprite("Game/Assets/Textures/Dog3-Sheet.png", 6);
            Sprite hurtingSprite = new Sprite("Game/Assets/Textures/DogHurt.png", 4);
            Sprite attackSprite = new Sprite("Game/Assets/Textures/DogAttack.png", 5);
            Sprite deadSprite = new Sprite("Game/Assets/Textures/DogDead3.png", 7);

            renderer = new SpriteComponent(this, idleSprite, Raylib_cs.Color.White, name: "PoliceRenderer");

            SpriteAnimation animation = new SpriteAnimation(idleSprite, .5f, loop: true);
            SpriteAnimation walkanim = new SpriteAnimation(walkingSprite, .5f, loop: true/*,animationSignals: footstepSignals*/);
            SpriteAnimation hurt = new SpriteAnimation(hurtingSprite, 1f, loop: false);
            SpriteAnimation attack = new SpriteAnimation(attackSprite, .5f, loop: true, animationSignals: biteSignals);
            SpriteAnimation dead = new SpriteAnimation(deadSprite, .5f, loop: false);

            AnimationControllerState idleState = new AnimationControllerState("Idle", animation);
            AnimationControllerState walkingState = new AnimationControllerState("Walking", walkanim);
            AnimationControllerState hurtingState = new AnimationControllerState("Hurt", hurt);
            AnimationControllerState attackState = new AnimationControllerState("Attack", attack);
            AnimationControllerState deadState = new AnimationControllerState("Dead", dead);

            AnimationControllerState[] allStates =
            {
                idleState,
                walkingState,
                hurtingState,
                attackState,
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
            collider = new BoxCollider2D(this, offsetY: -0.4f, width: idleSprite.FrameWidth * renderer.SpriteScale - 25, height: idleSprite.FrameHeight * renderer.SpriteScale);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Enemy;

            //Area where enemy detects player
            detectingSpace = new BoxCollider2D(this, width: idleSprite.FrameWidth * renderer.SpriteScale + 120, height: idleSprite.FrameHeight * renderer.SpriteScale + 80);
            detectingSpace.IsTrigger = true;
            detectingSpace.onTriggerEntered += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null && !isDead) { PlayerDetected(player); triggerCount = 1; }; };
            detectingSpace.onTriggerExited += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null && !isDead) { PlayerLost(player); triggerCount = 0; } };

            //trigger when player touches enemy
            interactRange = new BoxCollider2D(this, offsetY: -2, width: idleSprite.FrameWidth * renderer.SpriteScale - 5, height: idleSprite.FrameHeight * renderer.SpriteScale - 2);
            interactRange.IsTrigger = true;
            interactRange.CollisionLayer = CollisionLayers.Default;
            interactRange.onTriggerEntered += (BoxCollider2D other) =>
            {
                LaniasPlayer player = other.Parent as LaniasPlayer;
                if (player != null && !isDead/* && other.CollisionLayer == CollisionLayers.InteractRadius*/) { Console.WriteLine("IsTouchingggggggg"); IsTouching(player); triggerCount = 2; }
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
            };
            interactRange.onTriggerExited += (BoxCollider2D other) =>
           {
               LaniasPlayer player = other.Parent as LaniasPlayer;
               if (player != null && !isDead && other.CollisionLayer==CollisionLayers.InteractRadius) { PlayerDetected(player); triggerCount = 1; }
               else if (!isDead && other.IsCollider && other.CollisionLayer==CollisionLayers.Default) countCollider--;
           };
            #endregion

            #region Health
            MaxHealth = 30;
            Health = MaxHealth;

            //set hurt state and search player  when gets attacked
            onDamageTaken += (GameObject origin) =>
            {
                if (!isDead) { timer = 0f; TrySetState(EnemyState.Hurt); }

                LaniasPlayer player = origin as LaniasPlayer;
                if (player != null && !isDead)
                {
                    searchTimer = 0f;
                    targetPatrolRange = new Vector2(player.GetPositionX() - 20, player.GetPositionX() + 20);
                }
            };
            onDamageTaken += (GameObject origin) => ApplyKnockback(origin);

            onHealthZero += () => { currentState = EnemyState.Dead; };
            #endregion

            patrolRange = new Vector2(GetPositionX() - 60, GetPositionX() + 60);
            enemyLoot = EnemyLootOptions.Random;
        }
        public override void Update()
        {
            base.Update();

            knockbackTimer += Time.DeltaTime;
            if (knockbackTimer > 0.4f) inKnockback = false;

            offsetTimer += Time.DeltaTime;
            searchTimer += Time.DeltaTime;
            timer += Time.DeltaTime;

            renderer.FlipSpriteVerticaly = direction < 0f;

            //jump when colliding whith ocstacle
            if (isColliding && timer>1 &&!isDead)
            {
                timer = 0;
                this.physics.AddForce(new Vector2(0, -1), jumpForce, maxJumpSpeed);
                isColliding = false;
            }

            switch (currentState)
            {
                case EnemyState.Patrol:
                    //if is getting attacked search for player
                    if (searchTimer < 5f)
                        Patrol(targetPatrolRange);
                    else Patrol(patrolRange);
                    break;

                case EnemyState.Chase:
                    Chase();
                    break;

                case EnemyState.Attack:
                    break;

                case EnemyState.Hurt:
                    OnDamageTaken();
                    break;

                case EnemyState.Dead:
                    Dead();
                    break;
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
                deadAudio.Pitch = MathUtils.RandomFloatInRange(0.65f, .85f);
                deadAudio.Play();
                timer = 0;
                animator.SetState("Dead");
                collider.CollisionLayer = CollisionLayers.GhostEnemy;
                interactRange.Destroy();
                SpawnLoot();
            }
            if (timer >= 20)
            {
                InstanceService.Destroy(this);
            }
        }
        /// <summary>
        /// when player enters trigger
        /// </summary>
        /// <param name="player"></param>
        private void PlayerDetected(LaniasPlayer player)
        {
            target = player;
            chaseAudio.PlayOneShot(chaseAudio.Volume, MathUtils.RandomFloatInRange(0.7f,1f));
            TrySetState(EnemyState.Chase);

        }

        /// <summary>
        /// when player is out of sight
        /// </summary>
        /// <param name="player"></param>
        private void PlayerLost(LaniasPlayer player)
        {
            if (player != null)
                targetPatrolRange = new Vector2(player.GetPositionX() - 20, player.GetPositionX() + 20);
            searchTimer = 0f;
            target = null;
            TrySetState(EnemyState.Patrol);

        }

        /// <summary>
        /// when player is touching enemy
        /// </summary>
        /// <param name="player"></param>
        private void IsTouching(LaniasPlayer player)
        {
            TrySetState(EnemyState.Attack);
        }
        private void OnDamageTaken()
        {
            if (timer < cooldown) return;

            if (queuedState.HasValue)
            {
                var next = queuedState.Value;
                queuedState = null;
                ApplyState(next);
            }
            else
                CheckState();

        }
        /// <summary>
        /// check in which trigger player is and set state
        /// </summary>
        private void CheckState()
        {
            if (target == null) PlayerLost(target);
            else if (triggerCount == 1) PlayerDetected(target);
            else if (triggerCount == 2) IsTouching(target);
        }
        /// <summary>
        /// add to queue to prevent overriding other states
        /// </summary>
        /// <param name="next"></param>
        private void TrySetState(EnemyState next)
        {
            if (currentState == EnemyState.Hurt && timer < cooldown)
            {
                queuedState = next;
                return;
            }

            ApplyState(next);
        }
        /// <summary>
        /// set state
        /// </summary>
        /// <param name="next"></param>
        private void ApplyState(EnemyState next)
        {
            currentState = next;
            switch (next)
            {
                case EnemyState.Patrol:
                    animator.SetState("Walking");
                    target = null;
                    walkForce = maxWalkSpeed = 40f;
                    break;

                case EnemyState.Chase:
                    animator.SetState("Walking");
                    walkForce = maxWalkSpeed = 80f;
                    break;

                case EnemyState.Attack:
                    animator.SetState("Attack");
                    offsetTimer = 0;
                    break;

                case EnemyState.Hurt:
                    animator.SetState("Hurt");
                    break;

                case EnemyState.Dead:
                    Dead();
                    break;
            }

        }
        private void ChangeDirection()
        {
            if (countCollider > 1)
                return;
            direction *= -1;
        }
    }
}
