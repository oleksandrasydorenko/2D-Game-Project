using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine {

	// hier müsssen die camera shakes geamcht 
	// kombiniert muss das aber im listener weil der distance mult
	// camera shake component muss nur ein signal senden und der shake wird dann hier in die liste eingefügt 
	// wir müssen mit einer shake id arbeiten

	

	public static class CameraService
	{
		public static int shakeID = -1;

		public static Action<CameraShakeArguments, int> shakeInstantiated;

		public static int StartCameraShake(CameraShakeArguments arg)
		{
			shakeID++;
			shakeInstantiated?.Invoke(arg, shakeID);

			return shakeID;	// shake id is needed to stop the shake
		}
	}
}
