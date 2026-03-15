using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using RocketEngine.DataStore;
using JailBreaker.Weapons;
using JailBreaker.Game.Classes.Weapons;

namespace JailBreaker.Data
{
    public class GameDataPreSet : DataPreSet //varibales that need to be saved
    {
        public int Level { get; set; } = 1; //default value

        public Vector2 PlayerPosition { get; set; }

        public int CurrentWeapon { get; set; } = 0;

        public int SecondWeapon { get; set; } = 0;

        //no constructur, as it only acts as a "container" and not as a "controller"
    }
}
