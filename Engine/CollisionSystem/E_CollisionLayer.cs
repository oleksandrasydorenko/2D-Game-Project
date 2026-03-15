using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
    public enum CollisionLayers
    {
        Default,//0
        Player,//1
        Enemy,//2
        GhostEnemy,//3
        EnvironmentalProjectile,//4
        PlayerProjectile,//5
        EnemyProjectile,//6
        Interactable,//7
        Obstacle,//8
        PassableObjects,//9
		OnlyPlayer,//10
		InteractRadius,//11
	};


}
