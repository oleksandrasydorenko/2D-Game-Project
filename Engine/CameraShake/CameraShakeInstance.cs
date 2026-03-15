using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine {
	/// <summary>
	/// 
	/// NOT IN USE RIGHT NOW AND PROBABLY WILL NOT BE 
	/// 
	/// the camera shake instance is for more controll over a speciffic shake we dont need that right now so pause and kill is not implimanted
	/// </summary>
	public class CameraShakeInstance : InstantiableComponent
	{	// here you would also need to change the origin if the transform is changed and send that to the listener
		CameraShakeArguments args;
		private int id;
		public CameraShakeInstance(ComponentBase parent, string name = "CameraShakeInstance") : base(parent, name)
		{

		}

		public void StartCameraShake()
		{
			

			id = CameraService.StartCameraShake(args);
		}

		public void StopCameraShake() 
		{ 
			// here we would just tell the camer service the id and it would send a notification to the listeners to set the duration to blend out if its unlimted and if its timed it would also do that but only if its not already fading out so bigger than the fade out time
		}

		public void KillCameraShake()
		{
			// gleiche wie stop just set the time to full duration or sth like that
		}
	}
}
