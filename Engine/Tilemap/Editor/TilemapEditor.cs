using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Data;
using RocketEngine.Tilemapsystem.UI;
using RocketEngine.Tilemapsystem;
using RocketEngine.DataStore;
using System.Reflection;
using System.Reflection.Metadata;
using System.Collections.Specialized;


namespace RocketEngine
{
	public class TilemapEditor : Scene
	{

		public enum EditorState
		{
			TilemapEditor,
			PrefabEditor,
		}

		private EditorState state;
		public EditorState currentEditorState { get { return state; } set { state = value; } }

		protected Vector2 cameraPosition;
		protected float cameraSpeed = 140;

		public TilemapEditor_UIPannel editorUi;

		public GameObject trackingTarget;

		#region TilemapEditor
		protected Tilemap[] tilemaps;
		Tilemap activeTilemap;
		public Tile previewTile;
		public int tileSheetID;
		private int previewTileIndex = 0;
		private int activeTileMapIndex = 0;
		private bool tinted;
		#endregion

		#region Prefab Editor
		public EditorPrefabPreview previewPrefab;
		protected PrefabDataPreset prefabData;
		public Utils.OrderedDictionary<int, EditorPrefabPreview> placedPreviewAssets;
		protected string prefabFilePath;
		private int previewPrefabIndex = 0;
		private int nextId = 0;
		List<int> openIds = new List<int>();
		int prefabCount = 0;
		#endregion



		public TilemapEditor(string name = "TilemapEditorScene") : base(name) { } 

		public sealed override void CreateScene()
		{
			base.CreateScene();

			EngineSerivce.isEditor = true;

			trackingTarget = InstanceService.Instantiate(new GameObject());
			mainCamera.Target = trackingTarget;

			CreateTilemapEditor();

			currentEditorState = EditorState.TilemapEditor;
			

			for (int i = 0; i < tilemaps.Length; i++)
			{
				tilemaps[i] = InstanceService.Instantiate(tilemaps[i]);
				tilemaps[i].ZIndex = (0 - i * 2); // we work in steps of evensteps of 2 for the maps and uneven for the preview tile
			}

			while(PrefabService.GetPrefabFromIndex(prefabCount) != null)
			{
				prefabCount++;
			}

			placedPreviewAssets = new Utils.OrderedDictionary<int, EditorPrefabPreview>();

			PlacePreviewPrefabsFromFile();

			openIds.Clear();
			int currentBiggestId = 0;
			foreach(var keyValuePair in placedPreviewAssets)
			{
				if (keyValuePair.Key > currentBiggestId)
				{
					currentBiggestId = keyValuePair.Key;
				}
			}

			for (int i = 0; i < currentBiggestId; i++)
			{
				if(!placedPreviewAssets.ContainsKey(i)) openIds.Add(i);
			}

			nextId = currentBiggestId + 1;

			

			editorUi = InstanceService.Instantiate(new TilemapEditor_UIPannel());
			editorUi.camSpeed.Text = $"Camera Speed: {MathF.Round(cameraSpeed / 10)}";
			editorUi.camZoom.Text = $"Camera Zoom: {mainCamera.Zoom}";
			
			SetAllPrefabsActive(false);
			InitTilemapEditor();

		}

		private void InitTilemapEditor()
		{
			previewTileIndex = 0;

			// using the first tilemap in the array as the on thats active first and gets renderd last
			SelectTilemapByIndex(0);

			// selecting the first tile by the id from the tilesheet
			SetPreviewTileFromCurrentTilePreviewIndex();

		//	TintAllUnactiveTilemaps(true);

			editorUi.ShowTilemapUI();

		}

		private void UnloadTilemapEditor()
		{
			if(previewTile!=null)InstanceService.Destroy(previewTile);
			previewTile = null;
		}

		private void InitPrefabEditor()
		{
			previewPrefabIndex = 0;

			SetPreviewPrefabFromIndex();
			
			TintAllUnactiveTilemaps(false);

			SetAllPrefabsActive(true);

			editorUi.ShowTilemapUI(true);
		}

		private void UnloadPrefabEditor()
		{
			if(previewPrefab != null)InstanceService.Destroy(previewPrefab);
			previewPrefab = null;

			SetAllPrefabsActive(false);
		}

		public virtual void CreateTilemapEditor(){}

		public override void UpdateScene()
		{
			base.UpdateScene();

			#region camera movement
			float horizontal = 0f;
			float vertical = 0f;

			if (Raylib.IsKeyDown(KeyboardKey.A)) { horizontal -= 1; }
			if (Raylib.IsKeyDown(KeyboardKey.D)) { horizontal += 1; }
			if (Raylib.IsKeyDown(KeyboardKey.W)) { vertical -= 1; }
			if (Raylib.IsKeyDown(KeyboardKey.S)) { vertical += 1; }

			Vector2 inputVector = new Vector2(horizontal, vertical);
			float norm = MathF.Sqrt(MathF.Pow(inputVector.X, 2) + MathF.Pow(inputVector.Y, 2)); // normalizing the input vector 
			inputVector = norm != 0 ? 1f / norm * inputVector : Vector2.Zero; // if the norm is zero the division is not allowed default back to (0,0)

			cameraPosition += inputVector * cameraSpeed * Time.UnscaledDeltaTime;

			trackingTarget.Position = cameraPosition;



			float mouseWheel = Raylib.GetMouseWheelMove();

			
			if (mouseWheel != 0)
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl))
				{
                   
					cameraSpeed = Math.Clamp(cameraSpeed + (-mouseWheel * 20), 50f, 700f);
					editorUi.camSpeed.Text = $"Camera Speed: {MathF.Round(cameraSpeed/10)}";
				}
				else
				{
					mainCamera.Zoom = (float)(Math.Round(Math.Clamp(mainCamera.Zoom + (mouseWheel * .2f), 1.5f, 5), 1));
					editorUi.camZoom.Text = $"Camera Zoom: {mainCamera.Zoom}";
				}
			}

			#endregion

			Vector2 worldMousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), mainCamera.camera);
			editorUi.UpdateWorldCoordinates(worldMousePos.X, worldMousePos.Y);

			switch (currentEditorState)
			{
				case EditorState.TilemapEditor:
					HandleTilemapEditorInput();
					break;
				case EditorState.PrefabEditor:
					HandlePrefabEditorInput(worldMousePos);
					break;
			}

			#region saving input

			if (Raylib.IsKeyPressed(KeyboardKey.S))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.LeftShift))
				{
					foreach(Tilemap tilemap in tilemaps)
					{
						bool save = tilemap.SaveTilemap();
					}

					SavePrefabsToFile();

					editorUi.DisplaySaveAllTilemapsText();
					
				}
			}

			#endregion

			if (Raylib.IsKeyPressed(KeyboardKey.Tab))
			{
				if(editorUi.legend.Visible)
				{
					editorUi.legend.Visible = false;
				}
				else
				{
					editorUi.legend.Visible = true;
				}
			}
			// needs to be at the bottom so the whole update loop finishes
			if (Raylib.IsKeyPressed(KeyboardKey.Zero))
			{
				SceneService.LoadSceneByIndex(0);
			}
		}

		private void HandleTilemapEditorInput()
		{
			Vector2 currentGridCell = GetCurrentGridCell();

			if (previewTile != null) previewTile.SetPosition(activeTilemap.Grid.CellSize * currentGridCell);

			#region placing and remvoing tiles input
			if (Raylib.IsMouseButtonDown(MouseButton.Left))
			{
				activeTilemap.PlaceTileInGridCell(tileSheetID, currentGridCell);
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Delete) || Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressed(KeyboardKey.X))
			{
				activeTilemap.DeleteTileInGridCell(currentGridCell);
			}

			if (Raylib.IsKeyPressed(KeyboardKey.F))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl))
				{
					activeTilemap.FillWholeTilemap(tileSheetID);
				}
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl))
				{
					activeTilemap.ClearWholeTilemap();
				}
			}
			#endregion

			#region switch Tile input
			if (Raylib.IsKeyPressed(KeyboardKey.Up))
			{
				SelectNextPreviewTile();
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Down))
			{
				SelectPreviousePreviewTile();
			}
			#endregion

			#region switch tilemap input
			if (Raylib.IsKeyPressed(KeyboardKey.Right))
			{
				SelectNextTilemap();
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Left))
			{
				SelectPreviouseTilemap();
			}
			#endregion

			#region saving active tilemap
			if (Raylib.IsKeyPressed(KeyboardKey.S))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl))
				{
					bool save = activeTilemap.SaveTilemap();
					editorUi.DisplaySaveText(true);
				}

			}
			else if (Raylib.IsKeyPressed(KeyboardKey.LeftControl) || Raylib.IsKeyPressed(KeyboardKey.RightControl))
			{
				if (Raylib.IsKeyDown(KeyboardKey.S))
				{
					bool save = activeTilemap.SaveTilemap();
					editorUi.DisplaySaveText(true);
				}
			}
			#endregion

			#region (un)hiding active tilemap

			if (Raylib.IsKeyPressed(KeyboardKey.H))
			{
				if (!(Raylib.IsKeyDown(KeyboardKey.LeftAlt)))
				{
					activeTilemap.HideTilemap(true);
				}
			}

			if (Raylib.IsKeyPressed(KeyboardKey.H))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
				{
					activeTilemap.HideTilemap(false);
				}
			}

			if (Raylib.IsKeyPressed(KeyboardKey.G))
			{
				if (activeTilemap.Grid.visible)
				{
					activeTilemap.Grid.visible = false;

					TintAllUnactiveTilemaps(false);
				}
				else
				{
					activeTilemap.Grid.visible = true;

					TintAllUnactiveTilemaps(true);
				}

			}

			#endregion

			if (Raylib.IsKeyPressed(KeyboardKey.P))
			{
				UnloadTilemapEditor();
				currentEditorState = EditorState.PrefabEditor;
				InitPrefabEditor();
			}
		}

		private void HandlePrefabEditorInput(Vector2 worldMousePos)
		{

			if (previewPrefab != null) previewPrefab.SetPosition((int)worldMousePos.X, (int)worldMousePos.Y);


			#region placing and remvoing tiles input

			if (Raylib.IsMouseButtonPressed(MouseButton.Left))
			{
				if (openIds.Count > 0)
				{
					foreach(int id in openIds) { Console.WriteLine("open: " + id); }
					PlacePrefab(previewPrefabIndex, worldMousePos, openIds[0]);
					openIds.RemoveAt(0);
				}
				else
				{
					PlacePrefab(previewPrefabIndex, worldMousePos, nextId);
					nextId++;
				}
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Delete) || Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressed(KeyboardKey.X))
			{
				DeletePreviewAssetsInArea();
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl))
				{
					DeleteAllPrefabs();
				}
			}
			#endregion

			if (Raylib.IsKeyPressed(KeyboardKey.G))
			{
				if (activeTilemap.Grid.visible)
				{
					activeTilemap.Grid.visible = false;
				}
				else
				{
					activeTilemap.Grid.visible = true;
				}

			}

			#region switch prefab input
			if (Raylib.IsKeyPressed(KeyboardKey.Up))
			{
				SelectNextPreviewPrefab();
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Down))
			{
				SelectPreviousePreviewPrefab();
			}
			#endregion

			#region Saving Only Prefabs
			if (Raylib.IsKeyPressed(KeyboardKey.S))
			{
				if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl))
				{
					editorUi.DisplaySaveText(false);
					SavePrefabsToFile();
				}

			}
			else if (Raylib.IsKeyPressed(KeyboardKey.LeftControl) || Raylib.IsKeyPressed(KeyboardKey.RightControl))
			{
				if (Raylib.IsKeyDown(KeyboardKey.S))
				{
					editorUi.DisplaySaveText(false);
					SavePrefabsToFile();
				}
			}
#endregion

			if (Raylib.IsKeyPressed(KeyboardKey.P))
			{
				UnloadPrefabEditor();
				currentEditorState = EditorState.TilemapEditor;
				InitTilemapEditor();
			}
		}

		/// <summary>
		/// Returns the current Grid Cell at the Mouse Position
		/// </summary>
		/// <returns></returns>
		private Vector2 GetCurrentGridCell()
		{
			Vector2 worldMousePosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), mainCamera.camera);
			return activeTilemap.Grid.WorldToGridCell(worldMousePosition.X, worldMousePosition.Y);   
		}

		private void TintAllUnactiveTilemaps(bool doTint)
		{
			if(doTint)
			{
				foreach (Tilemap tilemap in tilemaps)
				{
					if (tilemap != activeTilemap)
					{
						tilemap.TintTilemap(Color.Gray);
					}

				}
			}
			else
			{
				foreach (Tilemap tilemap in tilemaps)
				{
					tilemap.RemoveTilemapTint();
				}
			}
		}

		#region PreviewTile Selection

		private void SelectPreviewTileByIndex(int index)
		{
			if (index >= activeTilemap.TileSheet.tileDictionary.Count()) index = 0;
			if (index < 0) index = activeTilemap.TileSheet.tileDictionary.Count() - 1;

			previewTileIndex = index;

			SetPreviewTileFromCurrentTilePreviewIndex();

			editorUi.UpdateTileIndexText(previewTileIndex);
		}

		private void SelectNextPreviewTile()
		{
			SelectPreviewTileByIndex(previewTileIndex + 1);
		}

		private void SelectPreviousePreviewTile()
		{
			SelectPreviewTileByIndex(previewTileIndex - 1);
		}

		
		private void SetPreviewTileFromCurrentTilePreviewIndex()
		{
			int tileID = activeTilemap.TileSheet.tileDictionary.ElementAt(previewTileIndex).Key;

			if (previewTile != null)
			{
				InstanceService.Destroy(previewTile);
				previewTile = null;
			}

			previewTile = InstanceService.Instantiate(new Tile(activeTilemap.TileSheet.tileDictionary[tileID]));
			tileSheetID = tileID;

			previewTile.SetPosition(activeTilemap.Grid.CellSize * GetCurrentGridCell());

			// all the tiles in the tilemap have the same sorting layer and zindex 
			previewTile.spriteComponent.SortingLayer = activeTilemap.SortingLayer;
			previewTile.spriteComponent.ZIndex = activeTilemap.ZIndex + 1; // since the zIndex is always a multiplicant of 2 the uneaven numbers are for the preview tile


		}

		#endregion

		#region Tilemap Selection
		private void SelectTilemapByIndex(int index)
		{
			if (index >= tilemaps.Length) index = 0;
			if (index < 0) index = tilemaps.Length - 1;

			activeTileMapIndex = index;

			if (activeTilemap != null)
			{
				activeTilemap.Grid.visible = false;
				//activeTilemap.TintTilemap(Color.Gray);
				activeTilemap = null;
			}

			TintAllUnactiveTilemaps(false);

			activeTilemap = tilemaps[activeTileMapIndex];
			activeTilemap.Grid.visible = true;
			editorUi.UpdateTileMapNameText(activeTilemap.Name);

			//activeTilemap.RemoveTilemapTint();

			TintAllUnactiveTilemaps(true);

			SelectPreviewTileByIndex(0);

			// all the tiles in the tilemap have the same sorting layer and zindex 
			previewTile.spriteComponent.SortingLayer = activeTilemap.SortingLayer;
			previewTile.spriteComponent.ZIndex = activeTilemap.ZIndex + 1; // since the zIndex is always a multiplicant of 2 the uneaven numbers are for the preview tile

		}

		private void SelectNextTilemap()
		{
			SelectTilemapByIndex(activeTileMapIndex + 1);
		}

		private void SelectPreviouseTilemap()
		{
			SelectTilemapByIndex(activeTileMapIndex - 1);
		}

		public override void UnloadScene()
		{
			base.UnloadScene();

			if (activeTilemap != null)
			{
				InstanceService.Destroy(activeTilemap);
				activeTilemap = null;
			}
			if (previewTile != null)
			{
				InstanceService.Destroy(previewTile);
				previewTile = null;
			}
			if(previewPrefab != null)
			{
				InstanceService.Destroy(previewPrefab);
				previewPrefab = null;
			}

			if(trackingTarget != null)
			{
				InstanceService.Destroy(trackingTarget);
				trackingTarget = null;
			}

			tilemaps = null;

			previewPrefab = null;
			placedPreviewAssets.Clear();

			editorUi = null;

			EngineSerivce.isEditor = false;

		}

		#endregion

		#region prefabs


		public bool SavePrefabsToFile()
		{
			if (prefabFilePath == "" || prefabFilePath == string.Empty) return false;

			PrefabDataPreset newPreset = new PrefabDataPreset();

			foreach(var KeyValuePair in placedPreviewAssets)
			{
				RocketEngine.Tilemapsystem.PrefabDataStruct s = new Tilemapsystem.PrefabDataStruct(KeyValuePair.Value.id, KeyValuePair.Value.prefabIndex,KeyValuePair.Value.GetPosition());
				newPreset.presets.Add(s);
			}

			prefabData.presets = newPreset.presets;	

			return DataStoreService.SaveToJson<PrefabDataPreset>(prefabFilePath, prefabData);
		}

		
		public void PlacePreviewPrefabsFromFile()
		{
			LoadPrefabFile(prefabFilePath);

			/*
			if(prefabData.presets.Count != 0)
			{
				nextId = (prefabData.presets[prefabData.presets.Count - 1].id) + 1;
			}
			*/

			foreach(PrefabDataStruct str in prefabData.presets)
			{
                Console.WriteLine("id: " + str.id);
				PlacePrefab(str.prefabIndex, str.position, str.id);
			}
		}



		List<SpriteComponent> spriteRenderersOfGameobject = new List<SpriteComponent>();
		private void PlacePrefab(int index, Vector2 position, int id)
		{
			spriteRenderersOfGameobject.Clear();

			Vector2 positionToPlace = new Vector2((int)position.X, (int)position.Y);

			// getting the real object but only for reading out the needed data to give it to the preview object that will be placed at that point
			IPrefable prefabClass = PrefabService.GetPrefabFromIndex(index) as IPrefable;

			if (prefabClass == null) return;

			GameObject go = prefabClass as GameObject;

			if (go == null) return;

			go = InstanceService.Instantiate(go);

			// going through all the component to look for renderers 
			for (int i = 0; i < go.Components.Count; i++)
			{
				SpriteComponent renderer = go.Components[i] as SpriteComponent;

				if(renderer == null) continue;

				spriteRenderersOfGameobject.Add(renderer);

				Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(renderer.Name);
				Console.ForegroundColor = ConsoleColor.White;
			}

			// placing a new prefab preview object at that spot 
			EditorPrefabPreview preview = InstanceService.Instantiate(new EditorPrefabPreview(index, go.Name,spriteRenderersOfGameobject, prefabClass.BoundingBoxSize));
			preview.SetPosition(positionToPlace);
			preview.id = id;

			placedPreviewAssets.Add(id, preview);

			spriteRenderersOfGameobject.Clear();

            Console.WriteLine("placed id:" + id);

			InstanceService.Destroy(go);
		}

		private void SetPreviewPrefabFromIndex()
		{
			spriteRenderersOfGameobject.Clear();

			if (previewPrefab != null)
			{
				InstanceService.Destroy(previewPrefab);
				previewPrefab = null;
			}

			// getting the real object but only for reading out the needed data to give it to the preview object that will be placed at that point
			IPrefable prefabClass = PrefabService.GetPrefabFromIndex(previewPrefabIndex) as IPrefable;

			if (prefabClass == null) return;

			GameObject go = prefabClass as GameObject;

			if (go == null) return;

			go = InstanceService.Instantiate(go);

			// going through all the component to look for renderers 
			for (int i = 0; i < go.Components.Count; i++)
			{
				SpriteComponent renderer = go.Components[i] as SpriteComponent;

				if (renderer == null) continue;

				renderer.ZIndex++; // so that the preview prefab is always one above the placed prefabs
				spriteRenderersOfGameobject.Add(renderer);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(renderer.Name);
				Console.ForegroundColor = ConsoleColor.White;
			}

			// placing a new prefab preview object at that spot 
			previewPrefab = InstanceService.Instantiate(new EditorPrefabPreview(previewPrefabIndex, go.Name,spriteRenderersOfGameobject, prefabClass.BoundingBoxSize));
			previewPrefab.id = -1;

			Vector2 worldMousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), mainCamera.camera);

			previewPrefab.SetPosition((int)worldMousePos.X, (int)worldMousePos.Y);
			previewPrefab.Active = false;

			spriteRenderersOfGameobject.Clear();

			InstanceService.Destroy(go);

			editorUi.UpdatePrefabNameText(previewPrefab.prefabName);
		}

		private void DeletePreviewAssetsInArea()
		{
			Vector2 worldMousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), mainCamera.camera);

			int idToDelete = -1;
			int BiggestIndex = -1;
			
			foreach (var KeyValuePair in placedPreviewAssets)
			{
				Console.WriteLine("id " + KeyValuePair.Value.id);

				int deleteId = KeyValuePair.Value.CheckForInDeletionRadius(worldMousePos);

				if(deleteId == -1) continue;


				// Z Buffer like test
				if (KeyValuePair.Key <= BiggestIndex) continue;
				Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(KeyValuePair.Key);
				Console.ForegroundColor = ConsoleColor.White;
				BiggestIndex = placedPreviewAssets[KeyValuePair.Key].id;
				idToDelete = deleteId;
			}

			if(BiggestIndex != -1)
			{
				InstanceService.Destroy(placedPreviewAssets[idToDelete]);

				placedPreviewAssets.Remove(idToDelete);
			}
		}

		private void DeleteAllPrefabs()
		{
			foreach (var KeyValuePair in placedPreviewAssets)
			{
				Console.WriteLine("id " + KeyValuePair.Value.id);

				InstanceService.Destroy(placedPreviewAssets[KeyValuePair.Value.id]);
			}
			placedPreviewAssets.Clear();
			openIds.Clear();
			nextId = 0;
		}

		private void SetAllPrefabsActive(bool active)
		{
			foreach (var KeyValuePair in placedPreviewAssets)
			{
				KeyValuePair.Value.Active = active;
			}
		}

		public bool LoadPrefabFile(string path)
		{
			bool success = true;


			if (path == "" || path == string.Empty)
			{
				prefabData = new PrefabDataPreset();
				return success = false;
			}

			PrefabDataPreset? data = DataStoreService.LoadFromJson<PrefabDataPreset>(path);

			if (data == null) //if no data was found, we set the data to the default data
			{
				data = new PrefabDataPreset();
				DataStoreService.SaveToJson(path, data);
				prefabData = new PrefabDataPreset();
				success = false;
			}
			else // data found the prefab data is filled with the loaded value
			{
				prefabData = data;
			}

			return success;
		}

		private void SelectPrefabFromIndex(int index)
		{
			if (index >= prefabCount) index = 0;
			if (index < 0) index = prefabCount - 1;

			previewPrefabIndex = index;

			SetPreviewPrefabFromIndex();

		
		}

		private void SelectNextPreviewPrefab()
		{
			SelectPrefabFromIndex(previewPrefabIndex + 1);
		}

		private void SelectPreviousePreviewPrefab()
		{
			SelectPrefabFromIndex(previewPrefabIndex - 1);
		}

		#endregion
	}
}
