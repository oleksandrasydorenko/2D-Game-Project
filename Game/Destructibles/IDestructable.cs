using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using RocketEngine;

namespace JailBreaker.Destructibles
{
    /// <summary>
    /// implements health an damage system
    /// </summary>
    public interface IDestructable
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Action onHealthZero { get; set; }
        public Action<RocketEngine.GameObject> onDamageTaken { get; set; }

        /// <summary>
        /// applies damage to destructible object
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="origin"></param>
        public void TakeDamage(int amount, GameObject origin)
        {
            Health -= amount;
            onDamageTaken?.Invoke(origin);

            if(Health <= 0)
            {
                Health = 0;
                onHealthZero?.Invoke();
            }
        }
        /// <summary>
        /// sets health to a specific value
        /// </summary>
        /// <param name="amount"></param>
        public void SetHealth(int amount)
        {
            if (amount >= MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health = amount;
                if(Health <= 0)
                {
					onHealthZero?.Invoke();
				}

            }
        }
        /// <summary>
        /// increases current health by the specified amount
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseHealth(int amount)
        {
            if (Health + amount > MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += amount;
            }
        }
    }
}
