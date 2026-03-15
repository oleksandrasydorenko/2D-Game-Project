
using Raylib_cs;
using System.Numerics;

namespace RocketEngine
{


    public abstract class Collider : InstantiableComponent, IDrawable, IUpdatable
    {

        protected float offsetX = 0;
        protected float offsetY = 0;
        public bool drawHitbox = false;
        protected bool isColliding = false;
        private bool isTrigger = false;
        private bool isCollider = false;
        public bool alwaysCheckTriggers = false;
        public bool AlwaysCheckTrigger { get => alwaysCheckTriggers; set => alwaysCheckTriggers = value; }
        public bool IsTrigger { get => isTrigger; set => isTrigger = value; }
        public bool IsCollider { get => isCollider; set => isCollider = value; }
        protected Vector2 oldPosition;
        private bool isHandlingCollision = false;
        public Color hitboxColor = Color.White;
        public Color triggerColor = Color.Red;

        public Action<BoxCollider2D> onTriggerEntered;
        public Action<BoxCollider2D> onTriggerExited;
        public Action<BoxCollider2D> onCollider;
        

        private SortingLayers sortingLayer = SortingLayers.Default;
        public SortingLayers SortingLayer
        {
            get { return sortingLayer; }
            set { Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, false); }
        }

        private int zIndex = 0;
        public int ZIndex
        {
            get { return zIndex; }
            set { Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, false); }
        }

        

        public Collider(GameObject parent, float offsetX = 0, float offsetY = 0) : base(parent)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.oldPosition = Parent.GetPosition();
            
            parent.onTransformChanged += TransformChanged;

            TransformChanged();
        }
        private void TransformChanged()
        {
            RecalculateRecInformation();


            CalculateHitbox();
            if(IsCollider && !isHandlingCollision) //if the collider should physics-collision and the code is not already running
            {
                isHandlingCollision = true;
                CheckCollision();
                isHandlingCollision = false;
            }
            if(!alwaysCheckTriggers)
            {
                HandleTriggers();
            }

            oldPosition = Parent.GetPosition();
            

        }
        protected virtual void RecalculateRecInformation() { }
        protected virtual void CalculateHitbox() { }
        protected virtual void CheckCollision() { }
        protected virtual void DrawHitbox() { }
        protected virtual void HandleTriggers() { }
        
        


        public virtual void Draw()
        {
            if (drawHitbox)
            {

                DrawHitbox();
            }
        }

        public virtual void Update()
        {
            if(alwaysCheckTriggers)
            {
                HandleTriggers();
            }
           
        }
        public override void Destroy()
        {
            base.Destroy();
        }

        protected virtual void TriggerEntered(Collider col) { }
        protected virtual void TriggerExited(Collider col) { }
    }
}
