using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Props
{
    public class FanMoving : Prop
    {
        public override Sprite Sprite { get; set; } 
        private SpriteComponent renderer;
        private SpriteAnimation animation;
        private AnimationController controller;
        private AnimationControllerState state;
        private SpriteAnimatorComponent animator;
        public override Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);

        public override void Construct()
        {
            Sprite = new Sprite("Game/Assets/MapDecoration/FanMoving.png", 4);
            renderer = new SpriteComponent(this, Sprite, Raylib_cs.Color.White);
            animation = new SpriteAnimation(Sprite, loop: true);
            state = new AnimationControllerState("FanMoving", animation);
            controller = new AnimationController(state);
            animator = new SpriteAnimatorComponent(this, renderer, controller);

            base.Construct();
        }
    }
}
