using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Physics
{
	public class DefaultGround:GroundProperty
	{
		public override GroundLayer Ground => GroundLayer.Default;
	}
}
