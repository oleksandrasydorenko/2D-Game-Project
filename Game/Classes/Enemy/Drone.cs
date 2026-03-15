using JailBreaker.Effects;
using JailBreaker.Game.Classes.Weapons.Projectiles;
using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using System.Drawing;
using System.Numerics;

namespace JailBreaker.Enemy
{
    public class Drone : Enemy
    {
        float firerate = 2.5f;
        float timer=6f;
        BoxCollider2D attackingRange;
        Vector2 tempPatrolRange;
        SpriteAnimation animation;
        AudioComponent flySound;
        public override void Construct()
        {
            base.Construct();

            flySound = new AudioComponent(this, "Game/Assets/Audio/Drone/Drone.mp3", true, 1.5f, .9f, true, true,10, 350,true);
            flySound.Play();    

            #region Sprite
            Sprite idleSprite = new Sprite("Game/Assets/Textures/Drone.png", 4);

            renderer = new SpriteComponent(this, idleSprite, Raylib_cs.Color.White, name: "DroneRenderer");

            animation = new SpriteAnimation(idleSprite, .2f);

            AnimationControllerState idleState = new AnimationControllerState("Idle", animation);

            AnimationControllerState[] allStates =
            {
                idleState,
            };
            AnimationController controller = new AnimationController(allStates);

            animator = new SpriteAnimatorComponent(this, renderer, controller);

            renderer.SortingLayer = SortingLayers.Enemy;
            #endregion

            #region Physics
            PhysicsComponent physics = new PhysicsComponent(this, 5f, "PhysicsComponent");
            physics.Groundy0 = false;
            this.physics = physics;
            physics.Mass = 0f;
            physics.Drag = 5f;
            #endregion

            #region Collider
            collider = new BoxCollider2D(this, width: idleSprite.FrameWidth * renderer.SpriteScale - 10, height: idleSprite.FrameHeight * renderer.SpriteScale-10);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Enemy;
            //Area where enemy detects player
            detectingSpace = new BoxCollider2D(this, offsetY: idleSprite.FrameHeight * renderer.SpriteScale * 2, width: idleSprite.FrameWidth * renderer.SpriteScale + 150, height: idleSprite.FrameHeight * renderer.SpriteScale + 70);
            detectingSpace.IsTrigger = true;
            detectingSpace.onTriggerEntered += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null) { PlayerDetected(player); }; };
            detectingSpace.onTriggerExited += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null) { PlayerLost(player); } };

            //trigger when player touches enemy
            interactRange = new BoxCollider2D(this, width: idleSprite.FrameWidth * renderer.SpriteScale, height: idleSprite.FrameHeight * renderer.SpriteScale);
            interactRange.IsTrigger = true;
			interactRange.onTriggerEntered += (BoxCollider2D other) =>
            {
                LaniasPlayer player = other.Parent as LaniasPlayer;
                if (player != null && other.IsCollider) { IsTouching(player); }
                if (currentState == EnemyState.Patrol) direction = direction * -1 ;
                
            };
            interactRange.onTriggerExited += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null && other.IsCollider) { CheckState(other); } };

            attackingRange = new BoxCollider2D(this, offsetY: idleSprite.FrameHeight * renderer.SpriteScale * 2, width: idleSprite.FrameWidth * renderer.SpriteScale + 5, height: idleSprite.FrameHeight * renderer.SpriteScale + 70);
            attackingRange.IsTrigger = true;
            attackingRange.onTriggerEntered += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null) { AttackPlayer(player); } };
            attackingRange.onTriggerExited += (BoxCollider2D other) => { LaniasPlayer player = other.Parent as LaniasPlayer; if (player != null) { PlayerDetected(player); } };
            #endregion

            #region Health
            MaxHealth = 30;
            Health = MaxHealth;

            onDamageTaken += (GameObject origin) =>
            {
                timer = 0; LaniasPlayer player = origin as LaniasPlayer;
                if (player != null && !isDead)
                {
                    timer = 0;
                    tempPatrolRange = new Vector2(player.GetPositionX() - 10, player.GetPositionX() + 10);
                }
            };
            onDamageTaken += (GameObject origin) => ApplyKnockback(origin);

            onHealthZero += () =>
            {
                currentState = EnemyState.Dead; Dead(); 
                Explosion exp = InstanceService.InstantiateWithPosition(new Explosion(), GetPosition());
            };
            #endregion

            patrolRange = new Vector2(GetPositionX() - 20, GetPositionX() + 20);
        }

        public override void Update()
        {
            base.Update();
            timer += Time.DeltaTime;
            knockbackTimer += Time.DeltaTime;
            if (knockbackTimer > 0.1f) { inKnockback = false; animation.speed = currentState == EnemyState.Patrol ? 0.2f : 0.4f; renderer.colorTint = Raylib_cs.Color.White; }

            switch (currentState)
            {
                case EnemyState.Patrol:
                    if (timer < 3f) Patrol(tempPatrolRange);
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
        /// Enemy detect Player
        /// </summary>
        /// <param name="player"></param>
        private void PlayerDetected(LaniasPlayer player)
        {
            target = player;
            animation.speed = 0.4f;
            walkForce = 50f;
            maxWalkSpeed = 50f;
            currentState = EnemyState.Chase;

        }
        /// <summary>
        /// Enemy collides with Player
        /// </summary>
        /// <param name="player"></param>
        private void IsTouching(LaniasPlayer player)
        {
            target = player;
            Console.WriteLine("damage");
            currentState = EnemyState.Attack;
            player.TakeDamage(attackingForce, this);
        }
        /// <summary>
        /// Player out of detecting Range
        /// </summary>
        /// <param name="player"></param>
        private void PlayerLost(LaniasPlayer player)
        {
            if (target != null) 
            { 
                tempPatrolRange = new Vector2(player.GetPositionX() - 10, player.GetPositionX() + 10);
                timer = 0;
            }
            target = null;
            animation.speed = 0.2f;
            walkForce = 20f;
            maxWalkSpeed = 20f;
            currentState = EnemyState.Patrol;
            

        }
        /// <summary>
        /// Shoot
        /// </summary>
        protected override void Attack()
        {
            if (timer >= firerate)
            {
                Bomb mine = new Bomb();
                mine.SetPosition(GetPosition());
                mine.origin = this;
                InstanceService.Instantiate(mine);
                timer = 0;
            }
        }
        /// <summary>
        /// Chase Player
        /// </summary>
        protected override void Chase()
        {
            if (Math.Abs(target.GetPositionX() - GetPositionX()) < 5) return;

            base.Chase();
        }
        /// <summary>
        /// Visual changes when takes damage
        /// </summary>
        /// <param name="other"></param>
        protected override void ApplyKnockback(GameObject other)
        {
            knockbackTimer = 0;
            animation.speed = 2.7f;
            renderer.colorTint = Raylib_cs.Color.Beige;
        }
        /// <summary>
        /// Set Attack State
        /// </summary>
        /// <param name="player"></param>
        private void AttackPlayer(LaniasPlayer player)
        {
            target = player;
            currentState = EnemyState.Attack;

        }
        /// <summary>
        /// Calculates State
        /// </summary>
        /// <param name="player"></param>
        private void CheckState(BoxCollider2D player)
        {
            if (attackingRange.currentlyTriggeredObjects.Contains(player))
                AttackPlayer((LaniasPlayer)player.Parent);
            else if (detectingSpace.currentlyTriggeredObjects.Contains(player))
                PlayerDetected((LaniasPlayer)player.Parent);
            else
                PlayerLost((LaniasPlayer)player.Parent);
        }
    }
}
