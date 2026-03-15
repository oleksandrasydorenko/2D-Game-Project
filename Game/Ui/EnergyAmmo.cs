using RocketEngine;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Ui
{
    public class EnergyAmmo : UiElement
    {
        public SpriteComponent renderer;
        public SpriteAnimatorComponent animator;
        public override void Construct()
        {
            base.Construct();
			Sprite ammoSprite = new Sprite("Game/Assets/Textures/Weapons/EnergyAmmo.png", 5);

            renderer = new SpriteComponent(this, ammoSprite, Raylib_cs.Color.White);
            SpriteAnimation animation = new SpriteAnimation(ammoSprite, 0.5f, false);
            
            animation.onAnimationFinished += () =>
            {
				InstanceService.Destroy(this);
			};

            AnimationControllerState used = new AnimationControllerState("Used", animation);
            AnimationController controller = new AnimationController(used);
            animator = new SpriteAnimatorComponent(this, renderer, controller);
            
            renderer.SpriteScale = 2.0f;
            
            animator.PauseAnimator();
            
        }

        public void PlayUsedAnimation()
        {
            animator.SetState("Used");
            animator.PauseAnimator(false);
        }
    }
}
