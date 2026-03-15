using JailBreaker.Player;
using JailBreaker.Ui;
using Raylib_cs;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Interactibles
{
    public class KeyPad : Interactible
    {
        enum KeypadState
        {
            Locked,
            Unlocked
        }

        private KeypadState activeState = KeypadState.Locked;

        public IInteractible reactor = null;
        private KeyPadPanel keypad;

        SpriteComponent renderer;

        private bool inUse = false;

		private bool InputStillDownError = false; // when the keypad ui gets created the update loop gets called after so it closes the ui instantly so we have to wait one frame
		public override void Construct()
        {
            base.Construct();

            Name = "KeyPad";

            Sprite keypadSprite = new Sprite("Game/Assets/Textures/KeyPad.png");

            renderer = new SpriteComponent(this, keypadSprite, Raylib_cs.Color.White); //´game starts with leaver being off

			hitBox.Size = new Vector2(64, 39); //= new BoxCollider2D(this, width: leaverSprite.FrameWidth * renderer.SpriteScale, height: leaverSprite.FrameHeight * renderer.SpriteScale);

			interactPromptRenderer.TextureOffset = new Vector2(0, -16);
		}


        public override void Interact(GameObject other)
        {
            base.Interact(other);

            LaniasPlayer player = other as LaniasPlayer;

            if (player == null) return;

            player.SetInputState(InputState.Deactivated);

            if (activeState == KeypadState.Locked) ShowKeyPadUI(player);
        }

        public void ShowKeyPadUI(LaniasPlayer player)
        {
			
			if (keypad == null)
            {
				
				keypad = InstanceService.Instantiate(new KeyPadPanel());
                keypad.Password = "7563";
                keypad.onCorrectInputEntered += KeyPadPasswortCorrect;
                keypad.onDestroyed += (ComponentBase cb) => { keypad = null; player.SetInputState(InputState.Activated); inUse = false;};
                player.SetInputState(InputState.Deactivated);
				inUse = true;
                InputStillDownError = true;
			}
            else if(inUse) // player is interacting again so we destroy the keypad
            {
                InstanceService.Destroy(keypad);
                keypad = null;
			}
        }

        private void KeyPadPasswortCorrect()
        {
            activeState = KeypadState.Unlocked;
            reactor?.Interact(this);
        }


		public override void Update()
		{
			base.Update();
			if (!inUse) return;

			if (Raylib.IsKeyPressed(KeyboardKey.E) && !InputStillDownError)
            {   
                inUse = false;
				InstanceService.Destroy(keypad);
				keypad = null;
                
			}

            InputStillDownError = false;
		}
	}
}
