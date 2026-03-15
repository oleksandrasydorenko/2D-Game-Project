using JailBreaker.Game.Ui;
using JailBreaker.Player;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JailBreaker.Game.TextTriggers
{
	public class TextTrigger: GameObject
	{

		private BoxCollider2D triggerRange;

		public bool useDestroyTimer = true;
		public float destroyTime = 7f;

		private Vector2 triggerSize = new Vector2(50, 50);
		public Vector2 TriggerSize
		{
			get { return triggerSize; }
			set { triggerSize = value; triggerRange.Size = triggerSize; }
		}

		private bool drawBorder;
		public bool DrawBorder
		{
			private get { return drawBorder;}
			set { drawBorder = value;
				if (triggerRange != null)
				{ triggerRange.drawHitbox = drawBorder; }
				else
				{drawBorder = false;}
			}
		}


		private GameText textComponent;
		private string text = "Text";
		public string Text
		{
			get { return text; }
			set { text = value; textComponent.Text = text; }
		}

		private Raylib_cs.Color textColor = Raylib_cs.Color.White;
		public Raylib_cs.Color TextColor
		{
			get { return textColor; }
			set { textColor = value; textComponent.Color = textColor; }
		}

		private int fontSize = 20;
		public int FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				fontSize = value;
				textComponent.Size = fontSize;
			}
		}

		private Vector2 textPosition = Vector2.Zero;
		public Vector2 TextPosition
		{
			get
			{
				return textPosition;
			}
			set
			{
				textPosition = value;
				textComponent.SetPosition(textPosition);
			}
		}

		private float aliveTime = 0f;
		private bool playerEnteredTriggerWhileUseDestroyTimerIsTrue;

		public override void Construct()
		{
			base.Construct();

			Name = "TextTrigger";

			triggerRange = new BoxCollider2D(this, 0, 0, TriggerSize.X, TriggerSize.Y);
			triggerRange.IsTrigger = true;
			triggerRange.alwaysCheckTriggers = true;
			triggerRange.CollisionLayer = CollisionLayers.OnlyPlayer; // not working

			textComponent = InstanceService.InstantiateWithPosition(new GameText(TextColor,Text,FontSize), GetPosition());
			textComponent.Visible = false;

			triggerRange.onTriggerEntered += PlayerInRange;
			triggerRange.onTriggerExited += PlayerExitedRange;

		}

		public void PlayerInRange(BoxCollider2D col)
		{
			LaniasPlayer player = col.Parent as LaniasPlayer;

			if (player == null) return;

			if (playerEnteredTriggerWhileUseDestroyTimerIsTrue) return;

			textComponent.Visible = true;

			if (useDestroyTimer) playerEnteredTriggerWhileUseDestroyTimerIsTrue = true;

		}

		public void PlayerExitedRange(BoxCollider2D col)
		{
			LaniasPlayer player = col.Parent as LaniasPlayer;

			if (player == null) return;

			if (playerEnteredTriggerWhileUseDestroyTimerIsTrue) return;

			textComponent.Visible = false;
		}

		public override void Destroy()
		{
			base.Destroy();

			InstanceService.Destroy(textComponent);	
		}

		public override void Update()
		{
			base.Update();

			if (!useDestroyTimer) return;

			if(!playerEnteredTriggerWhileUseDestroyTimerIsTrue) return;

			aliveTime += Time.DeltaTime;

			if(aliveTime > destroyTime)
			{
				InstanceService.Destroy(this);
			}
		}

	}
}
