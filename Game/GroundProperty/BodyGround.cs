using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketEngine.Physics;

namespace JailBreaker.Game.GroundLayers
{

	public class BodyGround : GroundProperty
	{
		public override GroundLayer Ground => GroundLayer.Body;
	}
}
