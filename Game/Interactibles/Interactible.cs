using RocketEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

namespace JailBreaker.Interactibles
{
    public abstract class Interactible : GameObject, IInteractible
    {

		protected BoxCollider2D hitBox;

		protected SpriteComponent interactPromptRenderer;

        public override void Construct() //maybe for later
        {
            base.Construct();
			hitBox = new BoxCollider2D(this,0,0, 32,32);
			hitBox.IsTrigger = true;
			hitBox.alwaysCheckTriggers = true;
			hitBox.CollisionLayer = CollisionLayers.Interactable;
			hitBox.onTriggerEntered += PlayerEnteredTrigger;
			hitBox.onTriggerExited += PlayerExitedTrigger;

			interactPromptRenderer = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Prompts/InteractPrompt.png"), Raylib_cs.Color.White);
			interactPromptRenderer.visible = false;
			interactPromptRenderer.SortingLayer = SortingLayers.ForegroundElements2;
		}

		protected virtual void PlayerEnteredTrigger(BoxCollider2D col)
		{
			if (interactPromptRenderer != null) interactPromptRenderer.visible = true;
		}

		protected virtual void PlayerExitedTrigger(BoxCollider2D col)
		{
			if(interactPromptRenderer != null) interactPromptRenderer.visible = false;
		}

		public override void Update()
        {
            base.Update();
        }

		public virtual void Interact(GameObject other)
		{
			//Console.WriteLine($"{Name} was interacted with by {other.Name}");
		}
	}
}