using JailBreaker.Player;
using JailBreaker.Ui;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RocketEngine.Ui.UiElement;


namespace JailBreaker.Scenes
{
    public class CustomScene : Scene
    {
        public CustomScene(string name = "Editor") : base(name) { }
        Texture2D background;
        CustomPanel customPanel;

        /// <summary>
        /// gets called once at the beginning when the scene is created
        /// </summary>
        public override void CreateScene()
        {
            base.CreateScene();
            customPanel = InstanceService.Instantiate(new CustomPanel());
            background = TextureService.LoadTexture("Game/Assets/Textures/BGTest.png");
            customPanel.returnButton.OnClicked += ExitButtonClicked;
        }

        public void ExitButtonClicked()
        {
            Console.WriteLine("............................................");
            SceneService.LoadSceneByIndex(0);
        }
    }
}