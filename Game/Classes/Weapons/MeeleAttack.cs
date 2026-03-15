using JailBreaker.Destructibles;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Classes.Weapons
{
    public class MeeleAttack : GameObject
    {
        private float aliveTime = 0.1f;
        private float range = 40f;
        private int damage = 2;
        private GameObject origin;
        private BoxCollider2D hitBox;
        public MeeleAttack(float range, int damage, GameObject origin, Vector2 position)
        {
            this.range = range;
            this.damage = damage;
            this.origin = origin;
            SetPosition(position);
            Name = "MeeleAttack";
        }

        public override void Construct()
        {
            base.Construct();
            hitBox = new BoxCollider2D(this, 0, 0, range, range);
            hitBox.IsTrigger = true;
            hitBox.onTriggerEntered += HitBox_OnTriggerEnter;
            hitBox.CollisionLayer = CollisionLayers.PlayerProjectile;
            hitBox.alwaysCheckTriggers = true;
        }

        public override void Update()
        {
            base.Update();
            aliveTime -= Time.DeltaTime;
            if (aliveTime <= 0)
            {
                InstanceService.Destroy(this);
            }
        }

        public void HitBox_OnTriggerEnter(BoxCollider2D other)
        {
            if (other.Parent == origin) return;
            IDestructable destructable = other.Parent as IDestructable;
            if (destructable != null && other.IsCollider)
            {
                destructable.TakeDamage(damage, origin);
                foreach (InstantiableComponent component in other.Parent.Components)
                {
                    if (component is PhysicsComponent p)
                    {
                        p.AddForce(Vector2.Normalize(other.Parent.GetPosition() - origin.GetPosition()), 100f, 500f);
                    }
                }
            }
        }
    }
}
