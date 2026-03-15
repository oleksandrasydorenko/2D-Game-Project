using Raylib_cs;
using RocketEngine.Scenemanagement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Tilemapsystem
{ 
	public class EditorPrefabPreview :GameObject
	{
		private bool active = true;
		public bool Active
		{
			get
			{
				return active;
			}
			set
			{
				active = value;
				if(!active) lineRenderer.visible = false;
			}
		} 

		public Raylib_cs.Rectangle boundingBox;
		public int id;
		public int prefabIndex;
		public string prefabName;

		RectangleLineRendererComponent lineRenderer;

		List<SpriteComponent> spriteRenderersFromPrefab;

		public EditorPrefabPreview(int prefabIndex, string name,List<SpriteComponent> spriteRenderers, Vector2 boundingBoxSize) : base()
		{
			this.prefabIndex = prefabIndex;
			this.prefabName = name;

			boundingBox = new Raylib_cs.Rectangle(GetPositionX(), GetPositionY(), boundingBoxSize);

			spriteRenderersFromPrefab = spriteRenderers;
		}

		public override void Construct()
		{
			base.Construct();

			if(spriteRenderersFromPrefab.Count > 0)
			{
				for (int i = 0; i < spriteRenderersFromPrefab.Count; i++)
				{
					SpriteComponent spriteComponent = new SpriteComponent(this, spriteRenderersFromPrefab[i].sprite, spriteRenderersFromPrefab[i].colorTint);
					spriteComponent.SortingLayer = spriteRenderersFromPrefab[i].SortingLayer;
					spriteComponent.ZIndex = spriteRenderersFromPrefab[i].ZIndex;
					spriteComponent.TextureOffset = spriteRenderersFromPrefab[i].TextureOffset;
					spriteComponent.SpriteScale = spriteRenderersFromPrefab[i].SpriteScale;
				}
			}
			else
			{
				SpriteComponent spriteComponent = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Gizmo.png"), Raylib_cs.Color.White);
			}

			lineRenderer = new RectangleLineRendererComponent(this, Raylib_cs.Color.White, boundingBox.Width, boundingBox.Height);
			onTransformChanged += TransformChanged;
			lineRenderer.visible = false;
			lineRenderer.SortingLayer = SortingLayers.ForegroundElements1;

			spriteRenderersFromPrefab.Clear(); // savety clear to not keep references 
		}

		public void TransformChanged()
		{
			boundingBox.Position = GetPosition();
		}

		public override void Update()
		{
			if (!Active) return;

			base.Update();

			boundingBox.Position = GetPosition();

			Vector2 worldMousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), SceneService.ActiveScene.mainCamera.camera);
			Vector2 mousePos = Raylib.GetMousePosition();
			bool mouseIsOver = Raylib.CheckCollisionPointRec(worldMousePos, lineRenderer.rec);

			//lineRenderer.visible = false;

			if (mouseIsOver)
			{
				//Raylib.DrawRectangle((int)GetPositionX(),(int)GetPositionY(),32,32, Raylib_cs.Color.White);
				lineRenderer.visible = true;
			}
			else
			{
				lineRenderer.visible = false;
			}

		}

		public int CheckForInDeletionRadius(Vector2 worldMousePos)
		{
			bool mouseIsOver = Raylib.CheckCollisionPointRec(worldMousePos, lineRenderer.rec);

			if(!mouseIsOver) return -1;

			return id;
		}
	}
}
