using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public class RectangleLineRendererComponent : ShapeRendererComponent
	{
		public float lineThickness;

		public Rectangle rec = new Rectangle();
		public RectangleLineRendererComponent(ComponentBase parent, Color color, float sizeX = 32, float sizeY = 32, float lineThickness = 1, float offsetX = 0, float offsetY = 0, string name = "RectangleLineRenderer") : base(parent,color, offsetX, offsetY ,name)
		{
			this.lineThickness = lineThickness;
			rec.Size = new Vector2(sizeX, sizeY);
			rec.Position = parent.GetPosition();
		}

		protected override void TransformChanged()
		{
			base.TransformChanged();

			rec.X = Parent.GetPositionX() - rec.Width/2;
			rec.Y = Parent.GetPositionY() - rec.Height/2;

			rec.X = rec.X + textureOffset.X;
			rec.Y = rec.Y + textureOffset.Y;
		}

		protected override void Render()
		{
			base.Render();

			if (!visible) return;

			Raylib.DrawRectangleLinesEx(rec, lineThickness, color);
		}

	
	}
}
