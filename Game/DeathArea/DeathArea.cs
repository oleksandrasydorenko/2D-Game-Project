using JailBreaker.Player;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.DeathArea
{
    public abstract class DeathArea: GameObject
    {
        protected float width;
        protected float height;
        protected BoxCollider2D trigger;
        
        /// <summary>
        /// game over when collider is player
        /// </summary>
        /// <param name="other"></param>
        protected void CheckCollider(BoxCollider2D other) {
            LaniasPlayer player = other.Parent as LaniasPlayer;
            if (player!=null)
            {
                player.TakeDamage(player.MaxHealth,this);
            }
        }
    }
}
