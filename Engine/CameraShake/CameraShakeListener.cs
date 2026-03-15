using RocketEngine.Scenemanagement;
using RocketEngine.Settings;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public class CameraShakeListener:InstantiableComponent, IUpdatable
	{

		private Dictionary<int, CameraShake> ActiveCamerShakes = new Dictionary<int, CameraShake>();
		public CameraShakeListener(ComponentBase parent, string name = "CameraShakeListener") : base(parent, name)
		{
			CameraService.shakeInstantiated += RegisterNewCameraShake;
		}

		List<int> shakesToRemove = new List<int>();
		public void Update()
		{
			Vector2 finalOffset = Vector2.Zero;

			if(ActiveCamerShakes.Count > 0)
			{
				foreach (var keyValuePair in ActiveCamerShakes)
				{
					Vector2 offset = keyValuePair.Value.GetShakeOffset(Time.DeltaTime, out Vector2 origin);
					if (offset == Vector2.Zero)// Vector2.Zero means shake is over 
					{
						shakesToRemove.Add(keyValuePair.Key); // we cant modify the dictionary while looping through it so we have to do it like that
						continue;
					}

					if (keyValuePair.Value.distanceBase)
					{
						float distanceToShake = (origin - SceneService.ActiveScene.mainCamera.Target.GetPosition()).Length();
						distanceToShake = MathF.Abs(distanceToShake);

                        Console.WriteLine("origin: " + origin);

						//Console.WriteLine("distance to shake: " + distanceToShake);

						float alpha = HelperFunctionsUtils.ReMap(distanceToShake, keyValuePair.Value.distance.X, keyValuePair.Value.distance.Y, 1, 0);

						alpha = Math.Clamp(alpha, 0, 1);

                       // Console.WriteLine("Alphaaaaaaa: " + alpha);

						offset *= alpha;
					}

					finalOffset += offset;
				}

				if (shakesToRemove.Count > 0)
				{
					foreach (int shakeID in shakesToRemove)
					{
						ActiveCamerShakes.Remove(shakeID);

					}
					shakesToRemove.Clear();
				}

				//Console.WriteLine("final offset: " + finalOffset);
			}
																					// this is of course not the best solution but it will work for now
			SceneService.ActiveScene.mainCamera.camera.Offset = MathUtils.Lerp(new Vector2(DisplaySettings.WINDOW_WIDTH/2, DisplaySettings.WINDOW_HEIGHT/2) + SceneService.ActiveScene.mainCamera.Offset, SceneService.ActiveScene.mainCamera.camera.Offset + finalOffset, .25f);
		}

		public void RegisterNewCameraShake(CameraShakeArguments arg, int id)
		{
			CameraShake shakeToAdd = new CameraShake(arg);
			ActiveCamerShakes.Add(id, shakeToAdd);	
		}
	}
}
