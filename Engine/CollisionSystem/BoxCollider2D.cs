
using Raylib_cs;
using System.Drawing;
using System.Numerics;

namespace RocketEngine
{
    /// <summary>
    /// improve on co´mponent aqdded and remnoved check 
    /// </summary>
    /// 
    struct RectangleInformation
    {
        public float oldLeft;
        public float oldTop;
        public float oldRight;
        public float oldBottom;
        public float currentLeft;
        public float currentTop;
        public float currentRight;
        public float currentBottom;
    }

    public class BoxCollider2D : Collider
    {
        private float width = 1;
        private float height = 1;
        private Raylib_cs.Rectangle hitbox;
        private RectangleInformation col;
        private static int[,] collisionMatrix = new int[12, 12];

        public Vector2 Size
        {
            get { return new Vector2(width, height); }
            set
            {
                width = value.X;
                height = value.Y;

                if (width < 0) width = 0;
                if (height < 0) height = 0;

				RecalculateRecInformation();

				CalculateHitbox();

			}
        }


        public CollisionLayers CollisionLayer { get; set; } = CollisionLayers.Default;
        public static List<BoxCollider2D> allColliders = new List<BoxCollider2D>();
        public List<BoxCollider2D> currentlyTriggeredObjects = new List<BoxCollider2D>();

        public static Action<BoxCollider2D> onColliderDeleted;

        static BoxCollider2D()
        {   //Default 0                Player 1                   Enemy 2                   GhostEnemy 3              EnvironmentalProjectile 4   PlayerProjectile 5        EnemyProjectile 6            Interactable 7            Obstacle 8              PassableObjects 9              Only Player                 Interact Range
            collisionMatrix[0, 0] = 1; collisionMatrix[1, 0] = 1; collisionMatrix[2, 0] = 1; collisionMatrix[3, 0] = 1; collisionMatrix[4, 0] = 1; collisionMatrix[5, 0] = 1; collisionMatrix[6, 0] = 1; collisionMatrix[7, 0] = 0; collisionMatrix[8, 0] = 1; collisionMatrix[9, 0] = 1; collisionMatrix[10, 0] = 0; collisionMatrix[11, 0] = 0;
			collisionMatrix[0, 1] = 1; collisionMatrix[1, 1] = 0; collisionMatrix[2, 1] = 1; collisionMatrix[3, 1] = 0; collisionMatrix[4, 1] = 1; collisionMatrix[5, 1] = 0; collisionMatrix[6, 1] = 1; collisionMatrix[7, 1] = 0; collisionMatrix[8, 1] = 1; collisionMatrix[9, 1] = 0; collisionMatrix[10, 1] = 1; collisionMatrix[11, 1] = 0;
			collisionMatrix[0, 2] = 1; collisionMatrix[1, 2] = 1; collisionMatrix[2, 2] = 0; collisionMatrix[3, 2] = 0; collisionMatrix[4, 2] = 1; collisionMatrix[5, 2] = 1; collisionMatrix[6, 2] = 0; collisionMatrix[7, 2] = 0; collisionMatrix[8, 2] = 1; collisionMatrix[9, 2] = 0; collisionMatrix[10, 2] = 0; collisionMatrix[11, 2] = 0;
			collisionMatrix[0, 3] = 1; collisionMatrix[1, 3] = 0; collisionMatrix[2, 3] = 0; collisionMatrix[3, 3] = 0; collisionMatrix[4, 3] = 0; collisionMatrix[5, 3] = 0; collisionMatrix[6, 3] = 0; collisionMatrix[7, 3] = 0; collisionMatrix[8, 3] = 0; collisionMatrix[9, 3] = 0; collisionMatrix[10, 3] = 0; collisionMatrix[11, 3] = 0;
			collisionMatrix[0, 4] = 1; collisionMatrix[1, 4] = 1; collisionMatrix[2, 4] = 1; collisionMatrix[3, 4] = 0; collisionMatrix[4, 4] = 0; collisionMatrix[5, 4] = 0; collisionMatrix[6, 4] = 0; collisionMatrix[7, 4] = 0; collisionMatrix[8, 4] = 1; collisionMatrix[9, 4] = 0; collisionMatrix[10, 4] = 0; collisionMatrix[11, 4] = 0;
			collisionMatrix[0, 5] = 1; collisionMatrix[1, 5] = 0; collisionMatrix[2, 5] = 1; collisionMatrix[3, 5] = 0; collisionMatrix[4, 5] = 0; collisionMatrix[5, 5] = 0; collisionMatrix[6, 5] = 0; collisionMatrix[7, 5] = 0; collisionMatrix[8, 5] = 1; collisionMatrix[9, 5] = 1; collisionMatrix[10, 5] = 0; collisionMatrix[11, 5] = 0;
			collisionMatrix[0, 6] = 1; collisionMatrix[1, 6] = 1; collisionMatrix[2, 6] = 0; collisionMatrix[3, 6] = 0; collisionMatrix[4, 6] = 0; collisionMatrix[5, 6] = 0; collisionMatrix[6, 6] = 0; collisionMatrix[7, 6] = 0; collisionMatrix[8, 6] = 1; collisionMatrix[9, 6] = 0; collisionMatrix[10, 6] = 0; collisionMatrix[11, 6] = 0;
			collisionMatrix[0, 7] = 1; collisionMatrix[1, 7] = 1; collisionMatrix[2, 7] = 0; collisionMatrix[3, 7] = 0; collisionMatrix[4, 7] = 0; collisionMatrix[5, 7] = 0; collisionMatrix[6, 7] = 0; collisionMatrix[7, 7] = 0; collisionMatrix[8, 7] = 0; collisionMatrix[9, 7] = 0; collisionMatrix[10, 7] = 0; collisionMatrix[11, 7] = 1;
			collisionMatrix[0, 8] = 1; collisionMatrix[1, 8] = 1; collisionMatrix[2, 8] = 1; collisionMatrix[3, 8] = 0; collisionMatrix[4, 8] = 1; collisionMatrix[5, 8] = 1; collisionMatrix[6, 8] = 1; collisionMatrix[7, 8] = 0; collisionMatrix[8, 8] = 1; collisionMatrix[9, 8] = 1; collisionMatrix[10, 8] = 0; collisionMatrix[11, 8] = 0;
			collisionMatrix[0, 9] = 1; collisionMatrix[1, 9] = 0; collisionMatrix[2, 9] = 0; collisionMatrix[3, 9] = 0; collisionMatrix[4, 9] = 0; collisionMatrix[5, 9] = 1; collisionMatrix[6, 9] = 0; collisionMatrix[7, 9] = 0; collisionMatrix[8, 9] = 1; collisionMatrix[9, 9] = 0; collisionMatrix[10, 9] = 0; collisionMatrix[11, 9] = 0;
			collisionMatrix[0, 10] = 0; collisionMatrix[1, 10] = 0; collisionMatrix[2, 10] = 0; collisionMatrix[3, 10] = 0; collisionMatrix[4, 10] = 0; collisionMatrix[5, 10] = 0; collisionMatrix[6, 10] = 0; collisionMatrix[7, 10] = 0; collisionMatrix[8, 10] = 0; collisionMatrix[9, 10] = 0; collisionMatrix[10, 10] = 0; collisionMatrix[11, 11] = 0;
			collisionMatrix[0, 11] = 0; collisionMatrix[1, 11] = 0; collisionMatrix[2, 11] = 0; collisionMatrix[3, 11] = 0; collisionMatrix[4, 11] = 0; collisionMatrix[5, 11] = 0; collisionMatrix[6, 11] = 0; collisionMatrix[7, 11] = 1; collisionMatrix[8, 11] = 0; collisionMatrix[9, 11] = 0; collisionMatrix[10, 11] = 0; collisionMatrix[11, 11] = 0;


		}
		public BoxCollider2D(GameObject parent, float offsetX = 0, float offsetY = 0, float width = 1, float height = 1) : base(parent, offsetX, offsetY)
        {
            Size = new Vector2(width, height);

            onColliderDeleted += BoxColliderDestroyed;

            allColliders.Add(this); //remove should also be implemented when destroyed!

        }

        private void BoxColliderDestroyed(BoxCollider2D collider)
        {
            currentlyTriggeredObjects.Remove(collider);
        }

        protected override void RecalculateRecInformation()
        {
            col.oldLeft = oldPosition.X - width / 2 + offsetX;
            col.oldTop = oldPosition.Y - height / 2 + offsetY;
            col.oldRight = oldPosition.X + width / 2 + offsetX;
            col.oldBottom = oldPosition.Y + height / 2 + offsetY;
            col.currentLeft = Parent.GetPositionX() - width / 2 + offsetX;
            col.currentTop = Parent.GetPositionY() - height / 2 + offsetY;
            col.currentRight = Parent.GetPositionX() + width / 2 + offsetX;
            col.currentBottom = Parent.GetPositionY() + height / 2 + offsetY;
        }


        protected override void DrawHitbox()
        {
            Raylib_cs.Color outlineColor = isColliding ? triggerColor : hitboxColor;
            Raylib.DrawRectangleLinesEx(hitbox, 2, outlineColor);

        }
        protected override void CalculateHitbox()
        {
            base.CalculateHitbox();
            hitbox = new Raylib_cs.Rectangle((Parent.GetPositionX() + offsetX) - width / 2, (Parent.GetPositionY() + offsetY) - height / 2, width, height);

        }

        protected override void CheckCollision() //Check for collision and handle it if found
        {

            base.CheckCollision();
            foreach (var other in allColliders.ToArray())
            {
                if (collisionMatrix[(int)CollisionLayer,(int)other.CollisionLayer] != 1) continue; //check collision matrix if these layers should collide
                if (other == this) continue;
                if (!other.IsCollider) continue; //if the other collider shouldnt have physics collision stop running
                if (other.Parent == this.Parent) continue;//if collider share the same parent dont run , else very buggy
                if(CheckTunneling(other)) continue; //if tunneling was detected and handled skip normal collision check
                if(!CheckAlignment(other)) continue; //skip collision check if not aligned on atleast one axis and within reasonable range on the other axis
                if (Raylib.CheckCollisionRecs(this.hitbox, other.hitbox))
                {
                    HandleCollision(other);
                }
            }
        }

        //Used help from AI to improve collision resolution issues, works better now
        private void HandleCollision(BoxCollider2D other)
        {
            Vector2 otherVelocity = Vector2.Zero;
            PhysicsComponent physics = null;

            foreach (InstantiableComponent component in other.Parent.Components)
            {
                if (component is PhysicsComponent p)
                    otherVelocity = p.Velocity;
            }

            foreach (InstantiableComponent component in Parent.Components)
            {
                if (component is PhysicsComponent p)
                    physics = p;
                
            }

            if (physics == null) return;

            // Calculate penetration depths
            float overlapLeft = col.currentRight - other.col.currentLeft;
            float overlapRight = other.col.currentRight - col.currentLeft;
            float overlapTop = col.currentBottom - other.col.currentTop;
            float overlapBottom = other.col.currentBottom - col.currentTop;

            float minOverlapX = Math.Min(overlapLeft, overlapRight);
            float minOverlapY = Math.Min(overlapTop, overlapBottom);

            const float MIN_OVERLAP = 0.001f;
            if (minOverlapX <= MIN_OVERLAP || minOverlapY <= MIN_OVERLAP)
            {
                return;
            }

           

            
            const float MIN_WALL_VERTICAL_OVERLAP = 3.0f;
            const float MIN_FLOOR_HORIZONTAL_OVERLAP = 3.0f;

            bool canBeWallCollision = minOverlapY >= MIN_WALL_VERTICAL_OVERLAP;
            bool canBeFloorCollision = minOverlapX >= MIN_FLOOR_HORIZONTAL_OVERLAP;

            // Decide resolution axis
            bool resolveHorizontally = false;
            bool resolveVertically = false;

            if (canBeWallCollision && canBeFloorCollision)
            {
                
                if (minOverlapX < minOverlapY)
                {
                    resolveHorizontally = true;
                }
                else
                {
                    resolveVertically = true;
                }
            }
            else if (canBeWallCollision)
            {
                resolveHorizontally = true;
            }
            else if (canBeFloorCollision)
            {
                resolveVertically = true;
            }
            else
            {
                
                if (minOverlapX < minOverlapY)
                    resolveHorizontally = true;
                else
                    resolveVertically = true;
            }

            if (resolveHorizontally)
            {
                if (overlapLeft < overlapRight)
                {
                    if (physics.Velocity.X > 0)
                    {
                        Parent.SetPositionX(other.col.currentLeft - width / 2);
                        physics.Velocity = new Vector2(otherVelocity.X, physics.Velocity.Y);
                    }
                }
                else
                {
                    if (physics.Velocity.X < 0)
                    {
                        Parent.SetPositionX(other.col.currentRight + width / 2);
                        physics.Velocity = new Vector2(otherVelocity.X, physics.Velocity.Y);
                    }
                }
            }
            else if (resolveVertically)
            {
                if (overlapTop < overlapBottom)
                {
                    if (physics.Velocity.Y > 0)
                    {
                        Parent.SetPositionY(other.col.currentTop - height / 2);
                        physics.Velocity = new Vector2(physics.Velocity.X, otherVelocity.Y);
                    }
                }
                else
                {
                    if (physics.Velocity.Y < 0)
                    {
                        Parent.SetPositionY(other.col.currentBottom + height / 2);
                        physics.Velocity = new Vector2(physics.Velocity.X, otherVelocity.Y);
                    }
                }
            }

            onCollider?.Invoke(other);
        }

        public void CheckTrigger()
        {
            HandleTriggers();
        }

        protected override void HandleTriggers()
        {
            base.HandleTriggers();
            foreach (var other in allColliders.ToArray())//check for all colliders
            {
                if (collisionMatrix[(int)CollisionLayer, (int)other.CollisionLayer] != 1) continue;
                if (other == this) continue;//exclude self
                if (other.Parent == this.Parent) continue; //do not trigger for Collider with the same parent
                if (Raylib.CheckCollisionRecs(this.hitbox, other.hitbox))
                {
                    if (!currentlyTriggeredObjects.Contains(other) && IsTrigger)//only trigger once if not already triggered
                    {
                        currentlyTriggeredObjects.Add(other);
                        onTriggerEntered?.Invoke(other);



                        isColliding = true;
                    }

                }
                else
                {
                    if (currentlyTriggeredObjects.Contains(other) && IsTrigger)//only trigger once if was previously triggered
                    {
                        currentlyTriggeredObjects.Remove(other);
                        onTriggerExited?.Invoke(other);
                        if (currentlyTriggeredObjects.Count == 0)
                        {
                            isColliding = false;
                        }
                    }
                    
                }
            }
        }


        //Tunneling detection for every direction when moving with high speed, so no "warping" through colliders occurs, may still be buggy in some edge cases e.g when moving diagonally very fast
        private bool CheckTunneling(BoxCollider2D other)
        {
            // Get physics component
            PhysicsComponent physics = null;
            foreach (InstantiableComponent component in Parent.Components)
            {
                if (component is PhysicsComponent p)
                {
                    physics = p;
                    break;
                }
            }

            if (physics == null) return false;

            bool tunneled = false;

            // Calculate movement distances
            float verticalMovement = Math.Abs(col.currentBottom - col.oldBottom);
            float horizontalMovement = Math.Abs(col.currentRight - col.oldRight);

            float verticalThreshold = Math.Max(height * 0.3f, 5f);
            float horizontalThreshold = Math.Max(width * 0.3f, 5f);

            // Swept bounds for checking overlap during entire movement
            float minX = Math.Min(col.oldLeft, col.currentLeft);
            float maxX = Math.Max(col.oldRight, col.currentRight);
            float minY = Math.Min(col.oldTop, col.currentTop);
            float maxY = Math.Max(col.oldBottom, col.currentBottom);

            bool xOverlap = maxX > other.col.currentLeft && minX < other.col.currentRight;
            bool yOverlap = maxY > other.col.currentTop && minY < other.col.currentBottom;

            // Handle vertical tunneling if X overlap exists and significant vertical movement
            if (xOverlap && verticalMovement >= verticalThreshold)
            {
                // DOWNWARD TUNNELING (falling through floors)
                if (physics.Velocity.Y > 0)
                {
                    bool wasAbove = col.oldBottom <= other.col.currentTop;
                    bool crossedSurface = col.currentBottom > other.col.currentTop;

                    if (wasAbove && crossedSurface)
                    {
                        Parent.SetPositionY(other.col.currentTop - height / 2);
                        physics.Velocity = new Vector2(physics.Velocity.X, 0);
                        tunneled = true;
                        Console.WriteLine($"TUNNELING DOWN: Snapped {Parent.Name} to top of {other.Parent.Name}");
                    }
                }
                // UPWARD TUNNELING (jumping through ceilings)
                else if (physics.Velocity.Y < 0)
                {
                    bool wasBelow = col.oldTop >= other.col.currentBottom;
                    bool crossedSurface = col.currentTop < other.col.currentBottom;

                    if (wasBelow && crossedSurface)
                    {
                        Parent.SetPositionY(other.col.currentBottom + height / 2);
                        physics.Velocity = new Vector2(physics.Velocity.X, 0);
                        tunneled = true;
                        Console.WriteLine($"TUNNELING UP: Snapped {Parent.Name} to bottom of {other.Parent.Name}");
                    }
                }
            }

            // Handle horizontal tunneling if Y overlap exists and significant horizontal movement
            if (yOverlap && horizontalMovement >= horizontalThreshold)
            {
                // RIGHTWARD TUNNELING (moving through left walls)
                if (physics.Velocity.X > 0)
                {
                    bool wasLeft = col.oldRight <= other.col.currentLeft;
                    bool crossedSurface = col.currentRight > other.col.currentLeft;

                    if (wasLeft && crossedSurface)
                    {
                        Parent.SetPositionX(other.col.currentLeft - width / 2);
                        physics.Velocity = new Vector2(0, physics.Velocity.Y);
                        tunneled = true;
                        Console.WriteLine($"TUNNELING RIGHT: Snapped {Parent.Name} to left of {other.Parent.Name}");
                    }
                }
                // LEFTWARD TUNNELING (moving through right walls)
                else if (physics.Velocity.X < 0)
                {
                    bool wasRight = col.oldLeft >= other.col.currentRight;
                    bool crossedSurface = col.currentLeft < other.col.currentRight;

                    if (wasRight && crossedSurface)
                    {
                        Parent.SetPositionX(other.col.currentRight + width / 2);
                        physics.Velocity = new Vector2(0, physics.Velocity.Y);
                        tunneled = true;
                        Console.WriteLine($"TUNNELING LEFT: Snapped {Parent.Name} to right of {other.Parent.Name}");
                    }
                }
            }

            // Update collision data if tunneling was corrected
            if (tunneled)
            {
                RecalculateRecInformation();
                CalculateHitbox();
            }

            return tunneled;
        }

        private bool CheckAlignment(BoxCollider2D other) //check if colliders are aligned on atleast one axis and within reasonable range on the other axis
        {
            bool aligned = false;
            if(col.currentRight >= other.col.currentLeft && col.currentLeft <= other.col.currentRight)
            {   
                if(Math.Abs(other.Parent.GetPositionY() - Parent.GetPositionY()) < 200) aligned = true;
            }
            if(col.currentTop <= other.col.currentBottom && col.currentBottom >= other.col.currentTop)
            {
                if (Math.Abs(other.Parent.GetPositionX() - Parent.GetPositionX()) < 200) aligned = true;
            }
            return aligned;
        }

        public override void Destroy()
        {
            base.Destroy();

            Console.WriteLine("REMOVED COLLIDER:" + this.Parent.Name);

            onColliderDeleted?.Invoke(this);

            allColliders.Remove(this);

        }

    }
}
