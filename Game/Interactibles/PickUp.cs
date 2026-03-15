using RocketEngine;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Interactibles
{
    public abstract class PickUp : Interactible, IPrefable
	{

        protected AudioComponent pickUpSound;
        private bool pickedUp;

        public Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);

        float rangeY = 7;
        float rangeLeft;
        float speed = 10;
        float LimitMovement;
        bool reverse;
		public override void Construct() //maybe for later
        {
			base.Construct();

			pickUpSound = new AudioComponent(this, "Game/Assets/Audio/PickUps/ItemPickUpSound.wav", false, 0.5f, 1);
            pickUpSound.onSoundFinished += () => { InstanceService.Destroy(this); };
           rangeLeft = rangeY;
        }
        public override void Update()
        {
            base.Update();
            rangeLeft -= speed * Time.DeltaTime; // Reverse direction at limits
            this.SetPositionY(GetPositionY() + (reverse ? -speed * Time.DeltaTime : +speed * Time.DeltaTime));
            if(rangeLeft <= 0)
            {
                rangeLeft = rangeY;
                reverse=!reverse;
            }
        }
        public override void Interact(GameObject other)
        {
            base.Interact(other);
        }

        public void PickUpItem()
        {
            if (pickedUp) return;

            interactPromptRenderer.Destroy();
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
            pickUpSound.Pitch = pitch;
            pickUpSound.Play();
            pickedUp = true;

		    hitBox.Destroy();

		}
    }
}
