using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace RocketEngine
{
	public class GridComponent : InstantiableComponent, IDrawable, IUiDrawable
	{
		/// <summary>
		/// if not visible we can actually remove the grid and the sprite renderer from draw and draw ui loop
		/// </summary>
		public int CellSize { get; init; }

		public int Columns { get; init; }
		public int Rows { get; init; }

		private float lineThickness = .3f;

		public bool visible = true;

		private SortingLayers sortingLayer = SortingLayers.ForegroundElements2;
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set { Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, parentIsUiElement); }
		}

		private int zIndex = 0;
		public int ZIndex
		{
			get { return zIndex; }
			set { Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, parentIsUiElement); }
		}


		private bool parentIsUiElement = false;
		public GridComponent(ComponentBase parent, int cellSize = 32, int rows = 50, int columns = 50, bool visible = true, string name = "GridComponent") : base(parent, name)
		{
			if (parent is Ui.UiElement) // if the parent is not a ui element then we draw the sprite in the world and not on the screen
			{
				parentIsUiElement = true;
				SortingLayer = sortingLayer;
				ZIndex = zIndex;
			}

			CellSize = cellSize;
			Columns = columns;
			Rows = rows;
			this.visible = visible;
		}

		
		public void Draw()
		{
			if (parentIsUiElement) return;
			Render();
		}

		public void DrawUi()
		{
			if (!parentIsUiElement) return;
			Render();
		}

		private void Render()
		{
			if (!visible) return;

			for (int x = 0; x < Columns; x++)
			{
				for (int y = 0; y < Rows; y++)
				{
					Rectangle r = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
					Raylib.DrawRectangleLinesEx(r, lineThickness, Color.Gray);
				}

			}
		}

		public Vector2 WorldToGridCell(float x, float y) 
		{
			int cellX = (int)(x / CellSize);
			int cellY = (int)(y / CellSize);

			// clamp to not go outside the grid
			cellX = Math.Clamp(cellX, 0, Columns -1);
			cellY = Math.Clamp(cellY, 0, Rows -1);

			return new Vector2(cellX, cellY);
		}

		public void HighLightGridCell(int x, int y) // for debuging
		{
			Rectangle r = new Rectangle(CellSize * x, CellSize * y, CellSize, CellSize);
			Vector2 origin = Vector2.Zero;
			Raylib.DrawRectanglePro(r, origin, 0, Color.Brown);
		}

	}
}

