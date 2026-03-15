using RocketEngine;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public abstract class InstantiableComponent
	{
		public string Name { get; set; }

		private ComponentBase parent;
		public ComponentBase Parent {

			get
			{
				return parent;
			}
			set 
			{
				AttachToParent(value);
			} 
		}

		public Action<InstantiableComponent> onDestroyed;

		public InstantiableComponent(ComponentBase parent, string name = "InstantiableComponent")
		{
			AttachToParent(parent);
			this.Name = name;


			// if a component imlients the i drawable interface it will be automaticly added to the main draw loop this is easier than handling it manually 
			if (Utils.HelperFunctionsUtils.ImplimentsInterface(this, "IDrawable"))
			{
				if (parent as GameObject != null)
					DrawService.CreateDrawable((IDrawable)this);
			}

			if (Utils.HelperFunctionsUtils.ImplimentsInterface(this, "IUiDrawable"))
			{ 
				if(parent as UiElement != null)
					DrawService.CreateUiDrawable((IUiDrawable)this);
			}

			if (Utils.HelperFunctionsUtils.ImplimentsInterface(this, "IUpdatable"))
			{
				UpdateService.CreateUpdatable((IUpdatable)this);
			}
			
		}

		public virtual void Destroy()
		{
			
			if (Utils.HelperFunctionsUtils.ImplimentsInterface(this, "IDrawable"))
			{
				DrawService.DestroyDrawable((IDrawable)this);
			}

			if (Utils.HelperFunctionsUtils.ImplimentsInterface(this, "IUiDrawable"))
			{
				DrawService.DestroyUiDrawable((IUiDrawable)this);
			}

			if (Utils.HelperFunctionsUtils.ImplimentsInterface(this,"IUpdatable"))
			{
				UpdateService.DestroyUpdatable((IUpdatable)this);
			}

			if (parent != null)
			{
				parent.ComponentDestroyRequested(this);
			}

			onDestroyed?.Invoke(this); // if other then the parent someone has a reference to this compoennt and is subscirbed he will be notified  
		}

		// private Functions that make switching parents easier

		private void AttachToParent(ComponentBase newParent)
		{
			if (parent != null)
			{
				newParent.onDestroyed -= ParentDestroyed;
				newParent.ComponentDestroyRequested(this);
			}

			parent = newParent;
			parent.ComponentAdded(this);
			parent.onDestroyed += ParentDestroyed;
		}

		private void ParentDestroyed(ComponentBase parent)
		{
			parent = null;

			Destroy();
		}

	}
}
