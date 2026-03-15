using JailBreaker.Player;
using Raylib_cs;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public class Camera : IUpdatable
	{
		public Raylib_cs.Camera2D camera;

		private Vector2 offset;
		public Vector2 Offset { 
			get { return offset; } 
			set
			{
				offset = value;
				camera.Offset = offset + defaultOffset;
			}
		}
		private GameObject trackingTarget;
		public GameObject Target { 
			get { return trackingTarget; } 
			set { 
				trackingTarget = value;
				if (trackingTarget != null)
				{
					camera.Target = trackingTarget.GetPosition();
				}else
				{
					camera.Target = Vector2.Zero;
				}
			} 
		}
		public Vector2 BorderX {  get; set; }
		public Vector2 BorderY { get; set; }

		private Vector2 damping = new Vector2(10,10);
		public Vector2 Damping { 
			get { return damping; } 
			set 
			{ 
				damping = value;
				if (damping.X < 0) damping.X = 0;
				if (damping.Y < 0) damping.Y = 0;
			} 
		}

		private float zoom;
		public float Zoom
		{
			get { return zoom; }
			set { 
				zoom = value; 
				if(zoom < 0) zoom = 0; 
				camera.Zoom = zoom; 
			}
		}

		public bool UseCameraBorders {  get; set; }

		private Vector2 defaultOffset = new Vector2(RocketEngine.Settings.DisplaySettings.WINDOW_WIDTH / 2, RocketEngine.Settings.DisplaySettings.WINDOW_HEIGHT / 2);

		public Camera()
		{
			camera = new Camera2D();
			Target = null;

			Offset = Vector2.Zero;
			Zoom= 3.5f;

			UpdateService.CreateUpdatable(this);
		}

		public void Destroy()
		{
			UpdateService.DestroyUpdatable(this);
		}

		public void Update()
		{
			if (Target != null)
			{
				float xTarget = Target.GetPositionX();

				if (UseCameraBorders) xTarget = Math.Clamp(xTarget, BorderX.X, BorderX.Y);

				xTarget = MathUtils.Lerp(camera.Target.X, xTarget, damping.X * Time.DeltaTime);

				float yTarget = Target.GetPositionY();

				if(UseCameraBorders) yTarget = Math.Clamp(yTarget, BorderY.X, BorderY.Y);

				yTarget = MathUtils.Lerp(camera.Target.Y, yTarget, damping.Y * Time.DeltaTime);

				camera.Target = new Vector2(xTarget, yTarget);

				//Console.WriteLine("TARGET POS: " + camera.Target);
			}
		}


			
	}
}
