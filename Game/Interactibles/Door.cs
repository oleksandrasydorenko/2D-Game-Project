using RocketEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using JailBreaker.Player;
using System.Numerics;
using RocketEngine.Utils;

namespace JailBreaker.Interactibles
{
    public class Door : Interactible
    {

		protected enum DoorState
		{
			closed,
			open,
		}

		protected DoorState activeState = DoorState.closed;

		SpriteComponent renderer;
        SpriteAnimatorComponent animator;

        SpriteAnimation doorAnimation;

		private bool open;

        public event Action onDoorOpened; 
        public event Action onDoorClosed;
		public event Action onDoorTriggerEntered;

		AudioComponent doorOpenSound;
		AudioComponent doorCloseSound;

		public override void Construct()
        {
            base.Construct();


			Name = "Door";
			#region optics
			Sprite doorSprite = new Sprite("Game/Assets/Textures/door-Sheet.png", 13);
            renderer = new SpriteComponent(this, doorSprite, Raylib_cs.Color.White, "Door", 0, 0);
			#endregion

			doorOpenSound = new AudioComponent(this, "Game/Assets/Audio/Door/DoorOpen.wav", false, 0.5f, 1, true, true, 10, 200, true);
			doorCloseSound = new AudioComponent(this, "Game/Assets/Audio/Door/DoorClose.wav", false, 0.5f, 1, true, true, 10, 200, true);



			#region animations
			AnimationSignal firstFrameSignal = new AnimationSignal(1);
            AnimationSignal lastFrameSignal = new AnimationSignal(13);

			doorAnimation = new SpriteAnimation(doorSprite, 1f, loop: true, false, animationSignals: [firstFrameSignal, lastFrameSignal]);

            AnimationControllerState doorState = new AnimationControllerState("Open", doorAnimation);
            AnimationController doorController = new AnimationController(doorState);
            animator = new SpriteAnimatorComponent(this, renderer, doorController);
			#endregion


			//hitBox = new BoxCollider2D(this, width: doorSprite.FrameWidth * renderer.SpriteScale * .4f, height: doorSprite.FrameHeight * renderer.SpriteScale);
			hitBox.Size = new Vector2(doorSprite.FrameWidth * renderer.SpriteScale * .4f, doorSprite.FrameHeight * renderer.SpriteScale);
			hitBox.drawHitbox = true;

			animator.PauseAnimator(true);


			firstFrameSignal.onAnimationSignalTriggered += DoorClosingAnimationOver;

			lastFrameSignal.onAnimationSignalTriggered += DoorOpeningAnimationOver;

        }

        public sealed override void Interact(GameObject other)
        {

            base.Interact(other);

			InteractedWithDoor(other);

        }

		
	
		

		// Seperate function to still keep the base functionaly of interactible on interact for childs of the door
		public virtual void InteractedWithDoor(GameObject other)
		{
			if (other.Name != "Player") return;

			LaniasPlayer player = other as LaniasPlayer;

			if (player == null) return;

			if (activeState == DoorState.closed)
			{
				Open();
			}
			else
			{
				Close();
			}
		}

        public virtual void Open()
        {
            activeState = DoorState.open;
			doorAnimation.reverse = false;
			animator.PauseAnimator(false);
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			doorOpenSound.Pitch = pitch;
			doorOpenSound.Play();
		}

        public virtual void Close()
        {
            activeState = DoorState.closed;
			doorAnimation.reverse = true;
			animator.PauseAnimator(false);
			//hitBox.alwaysCheckTriggers = false;
			hitBox.onTriggerEntered -= PlayerInTrigger;
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			doorCloseSound.Pitch = pitch;
			doorCloseSound.Play();
		}

		public virtual void DoorOpeningAnimationOver()
		{
			if (activeState == DoorState.open)
			{
				animator.PauseAnimator(true);
				onDoorOpened?.Invoke();
				// since we use a looping animation if the lag is to big it could overshoot and be at a later frame 
				// so we need to force it to keep the right frame
				animator.SetActiveAnimtionFrame(13);

				Console.WriteLine("trigger actition request");
				hitBox.alwaysCheckTriggers = true;
				hitBox.onTriggerEntered += PlayerInTrigger;
			}
		}

		public virtual void DoorClosingAnimationOver()
		{
			if (activeState == DoorState.closed)
			{
				animator.PauseAnimator(true);
				onDoorClosed?.Invoke();
				// since we use a looping animation if the lag is to big it could overshoot and be at a later frame 
				// so we need to force it to keep the right frame
				animator.SetActiveAnimtionFrame(1);
			}
		}

		public virtual void PlayerInTrigger(BoxCollider2D other)
		{
            Console.WriteLine("object in trigger");

			if (other == null) return;

			if (!other.IsCollider) return;

			if (other.Parent.Name != "Player") return;

			onDoorTriggerEntered?.Invoke();

		}
	}
}
