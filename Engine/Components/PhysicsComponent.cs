using RocketEngine;
using RocketEngine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RocketEngine.Utils;

namespace RocketEngine
{

    public class PhysicsComponent : InstantiableComponent, IUpdatable
    {
        private bool isGrounded = false;
        private Vector2 velocity;
        private Vector2 acceleration;
        private float gravityMultiplier = 10.0f;
        private float drag = 1.0f;
        private bool groundy0 = true;

        public bool Groundy0 { get => groundy0; set => groundy0 = value; }
        public float Drag { get => drag; set => drag = value; }
        // mass of the object, higher mass means stronger gravity
        private float mass;

        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public float GravityMultiplier { get => gravityMultiplier; set => gravityMultiplier = value; }
        public float Mass { get => mass; set => mass = value; }

        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public PhysicsComponent(GameObject parent, float mass, string name = "PhysicsComponent", float drag = 1.0f) : base(parent, name)
        {
            this.Mass = mass;
            this.Drag = drag;
        }


        //applies gravity to the object
        private void Gravity(float gravity, float deltaTime, float multiplier)
        {
            acceleration.Y = multiplier * gravity * Mass * deltaTime;
            Velocity += acceleration;

            float groundY = 0f;
            const float eps = 0.001f;

            
            
                if (Parent.GetPositionY() > groundY - eps && Velocity.Y > 0f && Groundy0)
                {
                    Parent.SetPositionY(groundY);
                    Velocity = new Vector2(Velocity.X, 0f);
                }
            
        }


        /// <summary>
        /// Applies a force to the object in the specified direction, affecting its velocity.
        /// </summary>
        /// <remarks>The method adjusts the object's velocity by applying the specified force in the given
        /// direction. If the current velocity in any direction exceeds <paramref name="maxSpeed"/>, no additional force
        /// is applied in that direction.</remarks>
        
        public void AddForce(Vector2 direction, float force, float maxSpeed)
        {
            float yForce = force;
            float xForce = force;

            if (direction == Vector2.Zero) return;

            float effectiveMass = MathUtils.Lerp(1f, Mass, 0.1f);
            
            direction = Vector2.Normalize(direction);
            Vector2 forceVector = new Vector2(direction.X * xForce/effectiveMass , direction.Y * yForce/effectiveMass);

            if (forceVector.X != 0)
            {
                float velocityX = Velocity.X;
                bool isExceedingMaxX = maxSpeed != 0 && Math.Abs(velocityX) >= maxSpeed;
                bool sameDirectiomX = (velocityX > 0 && forceVector.X > 0) || (velocityX < 0 && forceVector.X < 0);

                if (!isExceedingMaxX || !sameDirectiomX)
                {
                    velocity.X += forceVector.X;
                    velocity.X = Math.Clamp(velocity.X, -maxSpeed, maxSpeed);
                }
            }
            
            if (forceVector.Y != 0)
            {
                float velocityY = Velocity.Y;
                bool isExceedingMaxY = maxSpeed != 0 && Math.Abs(velocityY) >= maxSpeed;
                bool sameDirectiomY = (velocityY > 0 && forceVector.Y > 0) || (velocityY < 0 && forceVector.Y < 0);

                if (!isExceedingMaxY || !sameDirectiomY)
                {
                    velocity.Y += forceVector.Y;
                    velocity.Y = Math.Clamp(velocity.Y, -maxSpeed, maxSpeed);
                }
            }

        }

       
        /// <summary>
        /// applies vertical drag to the object
        /// , drag is higher when grounded
        /// </summary>
        /// <param name="drag"></param>
        public void ApplyDrag(float drag)
        {
            if (isGrounded)
            {
                
                velocity.X -= Velocity.X * drag * 10 * Time.DeltaTime;
            }
            else
            {
                velocity.X -= Velocity.X * drag * Time.DeltaTime;
            }
        }


        public void Update()
        {
            Gravity(GameSettings.GRAVITY, Time.DeltaTime, GravityMultiplier);
            Parent.SetPosition(Vector2.Add(Parent.GetPosition(), Vector2.Multiply(Velocity, Time.DeltaTime)));
            ApplyDrag(Drag);
        }



    }


}
