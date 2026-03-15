using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using System.Xml.Serialization;
using System.Numerics;

namespace RocketEngine.Tilemapsystem.UI
{
	public class TilemapEditor_UIPannel : UiElement
	{
		public UiText legend;
		public UiText camSpeed;
		public UiText camZoom;
		public UiText saveText;
		public UiText tileIndexText;
		public UiText tilemapText;
		public UiText prefabNameText;
		public UiText wordlCoordinatesText;

		private float displaySaveTextLenght = 0f;
		private float t;
		public override void Construct()
		{
			base.Construct();

			legend = InstanceService.Instantiate(new UiText(Color.White, " ", 20, 15, 15, AnchoringPosition.LeftTop));
			ActivateTilemapEditorLegend();

			camSpeed = InstanceService.Instantiate(new UiText(Color.White, "Camera Speed: ", 20, -100, 15, AnchoringPosition.RightTop));
			camZoom = InstanceService.Instantiate(new UiText(Color.White, "Camera Zoom: ", 20, -100, 25, AnchoringPosition.RightTop));
			saveText = InstanceService.Instantiate(new UiText(Color.White, "Tilemap Saved ", 20,-50,25, AnchoringPosition.Top));
			saveText.Visible = false;

			tileIndexText = InstanceService.Instantiate(new UiText(Color.White, "0", 20, 50, -40, AnchoringPosition.LeftBottom));
			tilemapText = InstanceService.Instantiate(new UiText(Color.White, "0", 20, 20, -25, AnchoringPosition.LeftBottom));

			prefabNameText = InstanceService.Instantiate(new UiText(Color.White, "0", 20, 50, -40, AnchoringPosition.LeftBottom));
			wordlCoordinatesText = InstanceService.Instantiate(new UiText(Color.White, "0", 20, -300, 15, AnchoringPosition.RightTop));

		}

		public void ActivateTilemapEditorLegend()
		{
			legend.Text =
				"WASD: Move" +
			   "\nMouseWheel/Trackpad: Zoom" +
			   "\nCTRL + MouseWheel/Trackpad: CamSpeed" +
			   "\nCTRL + S: Save Active Tilemap" +
			   "\nCTRL + Shift + S: Save All" +
			   "\nCTRL + F: Fill Active Tilemap" +
			   "\nCTRL + Delete: Clear Active Tilemap" +
			   "\nG: Toggle Game Mode" +
			   "\nH: Hide Active Tilemap" +
			   "\nAlt + H: Unhide Active Tilemap" +
			   "\nLeft Click: Place Tile" +
			   "\nUp/Down: Switch Tile" +
			   "\nLeft/Right: Switch Tilemap" +
			   "\nTab: Toggle Legend"+
				
				"\n"+
				"\nP: Prefab Editor"+ 
				"\nZero: Main Menu"
			   ; 
		}

		public void ActivatePrefabEditorLegend()
		{
			legend.Text =
			"WASD: Move" +
		   "\nMouseWheel/Trackpad: Zoom" +
		   "\nCTRL + MouseWheel/Trackpad: CamSpeed" +
		   "\nCTRL + S: Save Prefabs" +
		   "\nCTRL + Shift + S: Save All" +
		   "\nCTRL + Delete: Delete All Prefabs" +
		   "\nG: Toggle Game Mode" +
		   "\nLeft Click: Place Prefab" +
		   "\nUp/Down: Switch Prefab" +
		   "\nTab: Toggle Legend"+

		   "\n" +
		   "\nP: Tilemap Editor" +
		   "\nZero: Main Menu"
			;
		}

		public override void Start()
		{
			base.Start();

			UpdateService.DestroyUpdatable(this);
		}



		public void DisplaySaveText(bool tilemap)
		{
			saveText.Text = tilemap ? "Active Tilemap Saved": "Prefabs Saved";
			saveText.Visible = true;

			if (displaySaveTextLenght == 0)
			{
				UpdateService.CreateUpdatable(this);
			}

			displaySaveTextLenght = 3f;
			t = 0;
		}

		public void DisplaySaveAllTilemapsText()
		{
			saveText.Text = "Editor Saved";
			saveText.Visible = true;

			if(displaySaveTextLenght == 0)
			{
				UpdateService.CreateUpdatable(this);
			}

			displaySaveTextLenght = 3f;
			t = 0;
		}


		public void ShowTilemapUI(bool hide = false)
		{
		
			if(hide)
			{
				tileIndexText.Visible = false;
				prefabNameText.Visible = true;
				tilemapText.Visible = false;
				ActivatePrefabEditorLegend();
			}
			else
			{
				tileIndexText.Visible = true;
				prefabNameText.Visible = false;
				tilemapText.Visible = true;
				ActivateTilemapEditorLegend();
			}
		}

		public void UpdateTileIndexText(int newIndex)
		{
			tileIndexText.Text = newIndex.ToString();
		}

		public void UpdateTileMapNameText(string newName)
		{
			tilemapText.Text = newName;
		}

		public void UpdatePrefabNameText(string newName)
		{
			prefabNameText.Text = newName;
		}

		public void UpdateWorldCoordinates(float x, float y)
		{
			wordlCoordinatesText.Text = $"(X: {x,6:F2} | Y: {y,6:F2})";
		}

		public override void Update()
		{
			base.Update();

			if(displaySaveTextLenght > 0)
			{
				t += Time.DeltaTime;

				if(t >= displaySaveTextLenght)
				{
					displaySaveTextLenght = 0;
					UpdateService.DestroyUpdatable(this);
					saveText.Visible = false;	
				}
			}
			
		}

	}
}
