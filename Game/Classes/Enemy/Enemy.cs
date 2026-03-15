using JailBreaker.Destructibles;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using RocketEngine;
using System.Numerics;
using static JailBreaker.Destructibles.Box;

namespace JailBreaker.Enemy
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Hurt,
        Dead,
    }
    public abstract class Enemy : GameObject, IDestructable
    {
        public enum EnemyLootOptions { Nothing, HealthPack, Key, Pistol, Sniper, MachineGun, ShotGun, PlasmaSword, PlasmaLauncher, Random }
        public EnemyLootOptions enemyLoot = EnemyLootOptions.Nothing;
        public EnemyLootOptions[] randomWeapons = { EnemyLootOptions.Pistol, EnemyLootOptions.Sniper, EnemyLootOptions.MachineGun, EnemyLootOptions.ShotGun, EnemyLootOptions.PlasmaLauncher, EnemyLootOptions.PlasmaSword };


        public SpriteAnimatorComponent animator;
        public SpriteComponent renderer;
        public Vector2 patrolRange = new Vector2(50, 200);
        public BoxCollider2D interactRange;
        public BoxCollider2D detectingSpace;
        public BoxCollider2D collider;

        protected EnemyState currentState = EnemyState.Patrol; 
        protected PhysicsComponent physics;
        protected int attackingForce = 1;
        protected int knockbackForce = 250;
        protected float walkForce = 20f;
        protected float maxWalkSpeed = 20f;
        public float WalkSpeed { get { return walkForce; } set { walkForce = value; } }
        protected int direction = 1;
        protected LaniasPlayer target;
        protected bool isDead = false;
        protected float knockbackTimer = 10f;
        protected bool inKnockback = false;


        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Action onHealthZero { get; set; }
        public Action<GameObject> onDamageTaken { get; set; }

        public void SpawnLoot()
        {
            if (enemyLoot != EnemyLootOptions.Nothing)
            {
                Vector2 spawnPosition = GetPosition() + GetUpVector()*10;

                switch (enemyLoot)
                {
                    case EnemyLootOptions.Random:
                        Random random = new Random();
                        int percent = random.Next(0, 101);

                        switch (percent)
                        {
                            case <= 70:
                                enemyLoot = EnemyLootOptions.Nothing;
                                break;
                            case <= 85:
                                enemyLoot = EnemyLootOptions.HealthPack;
                                break;
                            case <= 100:
                                int weaponIndex = random.Next(0, randomWeapons.Length);
                                enemyLoot = randomWeapons[weaponIndex];
                                break;
                        }
                        SpawnLoot();
                        break;

                    case EnemyLootOptions.HealthPack:
                        HealthPack healthPack = InstanceService.InstantiateWithPosition(new HealthPack(), spawnPosition);
                        break;

                    case EnemyLootOptions.Key:
                        Key key = InstanceService.InstantiateWithPosition(new Key(), spawnPosition);
                        break;
                }
            }
        }

        /// <summary>
        /// Move from left to right
        /// </summary>
        protected virtual void Patrol(Vector2 patrolRange)
        {
            if (inKnockback) return;
            float leftLimit = patrolRange.X;
            float rightLimit = patrolRange.Y;

            if (GetPositionX() <= leftLimit)
                direction = 1;
            else if (GetPositionX() >= rightLimit)
                direction = -1;

            physics.AddForce(new Vector2(direction, 0f), walkForce, maxWalkSpeed);

        }
        /// <summary>
        /// Chase player
        /// </summary>
        protected virtual void Chase()
        {
            if (target == null) return;
            if (inKnockback) return;
            if (Math.Abs(target.GetPositionX() - GetPositionX()) < 5) return;
            if (target.GetPositionX() < GetPositionX())
            {
                direction = -1;
            }
            else { direction = 1; }
            physics.AddForce(new Vector2(direction, 0f), walkForce, maxWalkSpeed);
        }
        /// <summary>
        /// Give Player damage
        /// </summary>
        protected virtual void Attack()
        {
            if (target != null)
            {
                target.TakeDamage(attackingForce, this);
            }
        }
        /// <summary>
        /// Destroy Enemy
        /// </summary>
        protected virtual void Dead()
        {
            SpawnLoot();
            InstanceService.Destroy(this);
        }
        /// <summary>
        /// Push Enemy back
        /// </summary>
        /// <param name="other"></param>
        protected virtual void ApplyKnockback(GameObject other)
        {
            if (isDead) return;
            physics.Velocity = new Vector2();
            inKnockback = true;
            knockbackTimer = 0;
            float dir = (GetPositionX() < other.GetPositionX()) ? -1.0f : 1.0f;
            physics.AddForce(new Vector2(dir, -1), knockbackForce, knockbackForce);
        }
    }
}
