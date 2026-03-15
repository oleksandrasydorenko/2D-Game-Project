using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;

namespace JailBreaker.Ui
{
    /// <summary>
    /// CreditsMenuPanel, shows game credits
    /// </summary>
    public class CreditsMenuPanel : UiElement
    {
        private UiText menuHeader;
        private UiText right;
        private UiText left;
        private float scrollSpeed = 30f; 
        private float scrollPosition = 0f;

        public override void Construct()
        {
            base.Construct();

            menuHeader = InstanceService.Instantiate(new UiText(Color.White,"CREDITS", 90, -80, -260, UiElement.AnchoringPosition.Center));
            right = InstanceService.Instantiate(new UiText(Color.White,
                "Sebastian Hillenbrand\nNicole Koller\nLania Taufiq\nDaniel Dudak\nOleksandra Sydorenko\n\n" +
                "Nicole Koller\nLania Taufiq\nDaniel Dudak\nOleksandra Sydorenko\n\n" +
                "Nicole Koller\nLania Taufiq\nSebastian Hillenbrand\nOleksandra Sydorenko\n\n" +
                "Lania Taufiq\nNicole Koller\nDaniel Dudak\nOleksandra Sydorenko\n\n" +
                "Daniel Dudak\n\n" +
                "Oleksandra Sydorenko\n",
                30, 75, -140, UiElement.AnchoringPosition.Center));
            left = InstanceService.Instantiate(new UiText(new Color(64, 224, 208),
                "Engine Programmers:\n\n\n\n\n\nGameplay Programmers:\n\n\n\n\nLevel Designers:\n\n\n\n\n2D-Artists:\n\n\n\n\nSound-Designer:\n\nUI-Designer:\n\n",
                30, -155, -140, UiElement.AnchoringPosition.Center));
        }

        public override void Update()
        {
            base.Update();
            //allows to move text on the screen
            scrollPosition -= scrollSpeed * Time.DeltaTime; 
            menuHeader.SetPositionY(GetPositionY() + scrollPosition);
            right.SetPositionY(GetPositionY() + scrollPosition);
            left.SetPositionY(GetPositionY() + scrollPosition);
        }
    }
}
