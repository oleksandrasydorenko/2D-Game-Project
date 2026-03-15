using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;
using RocketEngine;
using System.Linq.Expressions;
using RocketEngine.Scenemanagement;
using RocketEngine.Utils;


namespace JailBreaker.Game.Ui
{
    public class GameText : GameObject, ISortingLayer
    {
        private TextComponent textComponent;
        public TextComponent TextComponent { get { return textComponent; } set { textComponent = value; } }
        private string text;

        private bool visible;
        public bool Visible { get => visible ; set { visible = value; textComponent.visible = visible; } }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                textComponent.text = text;
            }
        }
        private Color color;
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                textComponent.color = color;
            }
        }
        private int size;
        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                textComponent.size = size;
            }
        }

        #region changing the texts sorting layer, changes the text components sorting layer at the same time 
        protected SortingLayers sortingLayer = SortingLayers.Default;
        public SortingLayers SortingLayer
        {
            get { return sortingLayer; }
            set
            {
                RocketEngine.Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, false);

                if (textComponent != null)
                {
                    textComponent.SortingLayer = value;
                }

            }
        }
        protected int zIndex = 0;
        public int ZIndex
        {
            get { return zIndex; }
            set
            {

                RocketEngine.Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, false);

                if (textComponent != null)
                {
                    textComponent.ZIndex = value;
                }

            }
        }
        #endregion


        public GameText(Color color, string text = "Text", int fontSize = 20, float x = 0, float y = 0, string name = "UiText") : base(x, y, name)
        {
            this.text = text;
            this.color = color;
            this.size = fontSize;
            textComponent = new TextComponent(this, color, text, size, x, y);
        }
    }
}

