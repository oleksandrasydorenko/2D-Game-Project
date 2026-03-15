using JailBreaker.Interactibles;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JailBreaker.Player;
using static JailBreaker.Interactibles.DoorWithKey;

namespace JailBreaker.Interactibles
{
	public class RemoteDoor:Door
	{

		public GameObject sender = null;

		
		protected override void PlayerEnteredTrigger(BoxCollider2D col)
		{
		
			if (activeState == DoorState.open) return;
			base.PlayerEnteredTrigger(col);
		}
		

		public override void InteractedWithDoor(GameObject other)
		{

			if (other != sender) return;

			if (activeState == DoorState.closed)
			{
				Open();
			}
			else
			{
				Close();
			}
		}
	}
}
