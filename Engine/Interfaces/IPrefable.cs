using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public interface IPrefable
	{
		public Vector2 BoundingBoxSize { get; set; }
		public Vector2 Position { get; set; }
	}
}
