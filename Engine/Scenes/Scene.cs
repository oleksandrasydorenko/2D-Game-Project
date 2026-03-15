using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;
using static RocketEngine.DrawService;
using static RocketEngine.UpdateService;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using JailBreaker.Data;
using RocketEngine.DataStore;
using RocketEngine.Tilemapsystem;
using JailBreaker.Destructibles;
using RocketEngine.Ui;

namespace RocketEngine
{
    /// <summary>
    /// REMOVE BG
    /// </summary>

    public abstract class Scene
	{
		public string Name { get; init; }

		protected Color backgroundColor = Color.Black;

		public List<Instantiable> instantiables = new List<Instantiable>();
		List<IUpdatable> updatables = new List<IUpdatable>();

		Dictionary<string, List<IDrawable>> drawables = new Dictionary<string, List<IDrawable>>();
		Dictionary<string, List<IUiDrawable>> uiDrawables = new Dictionary<string, List<IUiDrawable>>();

		public Camera mainCamera;

		public Scene(string name)
		{
			Name = name;
		}

		private void InstantiableObjectAddedToScene(Instantiable instantiable)
		{ instantiables.Add(instantiable); }
		private void InstantiableObjectRemovedFromScene(Instantiable instantiable)
		{ instantiables.Remove(instantiable); }

		private void DrawableAddedToDrawLoop(IDrawable drawable)
		{
			List<IDrawable> drawableList = drawables[drawable.SortingLayer.ToString()];

			drawableList.Add(drawable);

			if(drawableList.Count > 1)
			{
				for (int i = drawableList.Count - 1; i > 0; i--)
				{
					if (drawableList[i].ZIndex < drawableList[i - 1].ZIndex) // if the drawble in the list before this one has a greater Zindex we flip them
					{
						IDrawable temp = drawableList[i - 1];
						drawableList[i - 1] = drawableList[i];
						drawableList[i] = temp;
					}
					else // if not we dont need to continue looking
					{
						break;
					}
				}
			}
		}

		private void DrawableRemovedFromDrawLoop(IDrawable drawable)
		{

			List<IDrawable> drawableList = drawables[drawable.SortingLayer.ToString()];

			drawableList.Remove(drawable);
		}

		private void UiDrawableAddedToUiDrawLoop(IUiDrawable uiDrawable)
		{
			List<IUiDrawable> uiDrawableList = uiDrawables[uiDrawable.SortingLayer.ToString()];

			uiDrawableList.Add(uiDrawable);

			if (uiDrawableList.Count > 1)
			{
				for (int i = uiDrawableList.Count - 1; i > 0; i--)
				{
					if (uiDrawableList[i].ZIndex < uiDrawableList[i - 1].ZIndex) // if the ui drawble in the list before this one has a greater Zindex we flip them
					{
						IUiDrawable temp = uiDrawableList[i - 1];
						uiDrawableList[i - 1] = uiDrawableList[i];
						uiDrawableList[i] = temp;
					}
					else // if not we dont need to continue looking
					{
						break;
					}
				}
			}


		}

		private void UiDrawableRemovedFromUiDrawLoop(IUiDrawable uiDrawable)
		{
			List<IUiDrawable> uiDrawableList = uiDrawables[uiDrawable.SortingLayer.ToString()];

			uiDrawableList.Remove(uiDrawable);
		}

		private void UpdatableAddedToUpdateLoop(IUpdatable updatable)
		{ updatables.Add(updatable); }

		private void UpdatableRemovedFromUpdateLoop(IUpdatable updatable)
		{ updatables.Remove(updatable); }

		private void SubscribeToServices()
		{
			// programm has access to all gameobjects 
			onInstantiableObjectCreated += InstantiableObjectAddedToScene;
			onInstantiableObjectDestroyed += InstantiableObjectRemovedFromScene;

			// used for adding scripts to the run loop
			onDrawableCreated += DrawableAddedToDrawLoop;
			onDrawableDestroyed += DrawableRemovedFromDrawLoop;
			onUpdatableCreated += UpdatableAddedToUpdateLoop;
			onUpdatableDestroyed += UpdatableRemovedFromUpdateLoop;
			onUiDrawableCreated += UiDrawableAddedToUiDrawLoop;
			onUiDrawableDestroyed += UiDrawableRemovedFromUiDrawLoop;
		}

		private void UnsubscribeFromServices()
		{
			onInstantiableObjectCreated -= InstantiableObjectAddedToScene;
			onInstantiableObjectDestroyed -= InstantiableObjectRemovedFromScene;

			// used for removing scripts to the run loop
			onDrawableCreated -= DrawableAddedToDrawLoop;
			onDrawableDestroyed -= DrawableRemovedFromDrawLoop;
			onUpdatableCreated -= UpdatableAddedToUpdateLoop;
			onUpdatableDestroyed -= UpdatableRemovedFromUpdateLoop;
			onUiDrawableCreated -= UiDrawableAddedToUiDrawLoop;
			onUiDrawableDestroyed -= UiDrawableRemovedFromUiDrawLoop;
		}

		/// <summary>
		/// use this to do things once 
		/// </summary>
		public virtual void CreateScene()
		{


			SubscribeToServices();

			string[] sortingLayerNames = Enum.GetNames(typeof(SortingLayers));

			foreach (string layer in sortingLayerNames)
			{
				drawables.Add(layer, new List<IDrawable>());
				uiDrawables.Add(layer, new List<IUiDrawable>());
			}

			mainCamera = new Camera();

		}

		/// <summary>
		/// use this for calucations every frame
		/// </summary>
		public virtual void UpdateScene()
		{
			for (int i = 0; i < updatables.Count; i++)
			{
				updatables[i].Update();
			}
		}

		/// <summary>
		/// Use this to draw objects in the scene
		/// </summary>
		public virtual void DrawScene()
		{

			ClearBackground(backgroundColor);

			foreach (string s in drawables.Keys)
			{
				for (int i = 0; i < drawables[s].Count; i++)
				{
					drawables[s][i].Draw();
				}
			}

		}

		/// <summary>
		/// use this to draw objects on the screen
		/// </summary>
		public virtual void DrawUIScene()
		{
            

			foreach (string s in uiDrawables.Keys)
			{
				for (int i = 0; i < uiDrawables[s].Count; i++)
				{
					uiDrawables[s][i].DrawUi();
				}
			}
		}

		public void RunScene()
		{
			UpdateScene();
			BeginDrawing();
			BeginMode2D(mainCamera.camera);
			DrawScene();
			EndMode2D();
			DrawUIScene();
			EndDrawing();
		}

		public virtual void UnloadScene()
		{
			for (int i = 0; i < instantiables.Count; i++)
			{
				InstanceService.Destroy(instantiables[i]);
			}

			instantiables.Clear();
			drawables.Clear();
			updatables.Clear();
			uiDrawables.Clear();

			UnsubscribeFromServices();
            
			BoxCollider2D.allColliders.Clear();
		}


		public bool LoadPrefabs(string path)
		{
			bool success = true;

			PrefabDataPreset prefabData;

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

				foreach (PrefabDataStruct s in prefabData.presets)
				{
					//GameObject go = PrefabService.SpawnPrefabFromIndex(s.prefabIndex) as GameObject;
					//go.SetPosition(s.position);

					GameObject go = PrefabService.GetPrefabFromIndex(s.prefabIndex,true,s.position.X, s.position.Y) as GameObject;//PrefabService.GetPrefabFromIndex(s.prefabIndex);
                    Console.WriteLine("POSITION SET FOR: " + go.Name+ "   " + go.GetPosition());
				}
			}

			return success;
		}


	}
}
