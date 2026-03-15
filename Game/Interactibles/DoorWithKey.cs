using JailBreaker.Interactibles;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JailBreaker.Player;
using RocketEngine.Utils;

namespace JailBreaker.Interactibles
{
	public class DoorWithKey:Door
	{
		public enum LockedState
		{
			locked, open,
		}


		public LockedState lockedState = LockedState.locked;

		AudioComponent doorUnlockSound;

		public override void Construct()
		{
			base.Construct();
			
			doorUnlockSound = new AudioComponent(this, "Game/Assets/Audio/PickUps/KeyCardSound2.wav", false, 0.5f, 1, true, true, 10, 200, true);

		}

		public override void InteractedWithDoor(GameObject other)
		{
			if(lockedState == LockedState.locked && activeState == DoorState.closed)
			{
				if (other.Name != "Player") return;

				LaniasPlayer player = other as LaniasPlayer;

				if (player == null) return;

				if (player.hasKey)
				{
					float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
					doorUnlockSound.Pitch = pitch;
					doorUnlockSound.Play();
					Open();
					lockedState = LockedState.open;
				}
			}
			else
			{
				// if the door is unlocked then it behaves like any other door
				base.InteractedWithDoor(other);
			}

		

		}
	}
}
