using Raylib_cs;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using RocketEngine.Utils;

namespace JailBreaker.Interactibles
{
    public class Leaver : Interactible
    {
        enum leaverState
        {
            LeaverOn,
            LeaverOff
        }

        private leaverState activeState = leaverState.LeaverOff;

        public IInteractible reactor = null;

        SpriteComponent renderer;
		// BoxCollider2D hitBox;
		AudioComponent doorOpenSound;

		public override void Construct()
        {
            base.Construct();

			Name = "Leaver";

			Sprite leaverSprite = new Sprite("Game/Assets/Textures/LeaverOffOn-Sheet.png", 2); 

            renderer = new SpriteComponent(this, leaverSprite, Raylib_cs.Color.White); //´game starts with leaver being off

            hitBox.Size = new Vector2(64, 39); //= new BoxCollider2D(this, width: leaverSprite.FrameWidth * renderer.SpriteScale, height: leaverSprite.FrameHeight * renderer.SpriteScale);

            interactPromptRenderer.TextureOffset = new Vector2(0,-16);

			doorOpenSound = new AudioComponent(this, "Game/Assets/Audio/Door/TrioSound.wav", false, 0.5f, 1, true, true, 10, 200, true);


		}


		public override void Interact(GameObject other)
        {
            base.Interact(other);

            if (other.Name != "Player") return;

            if (activeState == leaverState.LeaverOff) LeaverOn();
            else LeaverOff();
        }

        public void LeaverOn()
        {
            renderer.sprite.CurrentFrame = 1; //1: leaver is down (on)
            activeState = leaverState.LeaverOn;
            reactor?.Interact(this);
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			doorOpenSound.PlayOneShot(doorOpenSound.Volume, pitch);
        }

        public void LeaverOff()
        {
            renderer.sprite.CurrentFrame = 0; //leaver is up (off)
            activeState = leaverState.LeaverOff;
            reactor?.Interact(this);
			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			doorOpenSound.PlayOneShot(doorOpenSound.Volume, pitch);
		}
    }
}
