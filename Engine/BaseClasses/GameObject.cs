using JailBreaker.Game.GroundLayers;
using Raylib_cs;
using RocketEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RocketEngine.Physics;

namespace RocketEngine
{
    public class GameObject: ComponentBase
	{
		public virtual GroundProperty GroundProperty { get; set; } = new DefaultGround();
		public GameObject() : base() { Name = "GameObject"; }
		public GameObject(float x = 0, float y = 0, string name = "GameObject") : base(x,y,name) {}
	}
}
