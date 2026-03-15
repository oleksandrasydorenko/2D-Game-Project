using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Effects
{
    public class ImpactEffect : GameObject
    {
        private SpriteComponent renderer;
        private String groundtype;
        private Vector2 dir;
        private AudioComponent effectSound;
        public ImpactEffect(String groundtype,Vector2 dir,Vector2 pos)
        {
            this.groundtype = groundtype;
            this.dir = dir;
            SetPosition(pos);
        }

        public override void Construct()
        {
            Sprite sprite;
            sprite = new Sprite("");
            base.Construct();
            switch (this.groundtype)
            {
                case "Blood":
                    sprite = new Sprite("Game/Assets/Textures/Effects/Blood.png", 5);
                    string[] enemieHitSounds = { "Game/Assets/Audio/Weapons/Splatt.mp3", "Game/Assets/Audio/Weapons/HitEnemy.mp3" };
                    Random random = new Random();
                    effectSound = new AudioComponent(this, enemieHitSounds[random.Next(0, enemieHitSounds.Length)]);
                    break;
                case "Metall":
                    sprite = new Sprite("Game/Assets/Textures/Effects/Metall.png", 5);
                    string[] enemieHitSounds2 = { "Game/Assets/Audio/Weapons/Metal1.mp3", "Game/Assets/Audio/Weapons/Metal2.mp3" };
                    Random random2 = new Random();
                    effectSound = new AudioComponent(this, enemieHitSounds2[random2.Next(0, enemieHitSounds2.Length)]);
                    break;
                case "Wood":
                    sprite = new Sprite("Game/Assets/Textures/Effects/Wood.png", 5);
                    effectSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/Wood.mp3");
                    break;
                case "Default":
                    sprite = new Sprite("Game/Assets/Textures/Effects/Metall.png", 5);
                    effectSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/DefaultImpact.wav");
                    break;
            }
            effectSound.DistanceRange = new Vector2(0, 300);
            effectSound.Use3DAudio = true;
            effectSound.UseDistanceBasedAudio = true;
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White);
            SpriteAnimation animation = new SpriteAnimation(sprite,0.5f,false);
            AnimationControllerState state = new AnimationControllerState("State", animation);
            AnimationController controller = new AnimationController(state);
            SpriteAnimatorComponent animator = new SpriteAnimatorComponent(this,renderer,controller);
            RotateTowards(dir);
            animation.onAnimationFinished += Destroyed;
            controller.SetState("State");
            if(groundtype == "Wood")
            {
                effectSound.Pitch = 1.5f;
            }
            effectSound.Play();
        }

        public void Destroyed()
        {
            InstanceService.Destroy(this);
        }
    }
}
