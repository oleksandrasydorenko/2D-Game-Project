using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public struct CameraShakeArguments
	{
		public float strenght = 10;
		public float speed = 50;
		public float duration = 1;
		public float blendInTime = .2f;
		public float blendOutTime = .2f;
		public bool distanceBased = false;
		public Vector2 distance = Vector2.Zero;
		public Vector2 origin = Vector2.Zero;

		public CameraShakeArguments(float strenght = 10, float speed = 50, float duration = 1, float blendInTime = .2f, float blendOutTime = .2f, bool distanceBased = false, float minDistance = 10, float maxDistance = 100, float originX = 0, float originY = 0)
		{
			this.strenght = strenght;
			this.speed = speed;
			this.duration = duration;
			this.blendInTime = blendInTime;
			this.blendOutTime = blendOutTime;
			this.distanceBased = distanceBased;
			this.distance = new Vector2(minDistance, maxDistance);
			this.origin = new Vector2(originX, originY);	
		}
	}
}
