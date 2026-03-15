using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Utils
{
    static class SortingLayerHelper
    {

        public static void ChangeSortingLayer(ISortingLayer caller, ref SortingLayers privateGetter,SortingLayers newLayer, bool isUi)
        {
			if (privateGetter == newLayer) return;

			if (isUi)
			{
				if (HelperFunctionsUtils.ImplimentsInterface(caller, "IUiDrawable"))
				{
					DrawService.DestroyUiDrawable((IUiDrawable)caller);
					privateGetter = newLayer;
					DrawService.CreateUiDrawable((IUiDrawable)caller);
				}
			}
			else
			{
				if (HelperFunctionsUtils.ImplimentsInterface(caller, "IDrawable"))
				{
					DrawService.DestroyDrawable((IDrawable)caller);
					privateGetter = newLayer;
					DrawService.CreateDrawable((IDrawable)caller);
				}
			}
			
			
			

			privateGetter = newLayer;
		}

		public static void ChangeZIndex(ISortingLayer caller, ref int privateGetter, int newIndex, bool isUi)
		{


			if (privateGetter == newIndex) return;

			if (isUi)
			{
				if (HelperFunctionsUtils.ImplimentsInterface(caller, "IUiDrawable"))
				{
					DrawService.DestroyUiDrawable((IUiDrawable)caller);
					privateGetter = newIndex;
					DrawService.CreateUiDrawable((IUiDrawable)caller);
				}
			}
			else
			{
				if (HelperFunctionsUtils.ImplimentsInterface(caller, "IDrawable"))
				{
					DrawService.DestroyDrawable((IDrawable)caller);
					privateGetter = newIndex;
					DrawService.CreateDrawable((IDrawable)caller);
				}
			}
		

			

			privateGetter = newIndex;
		}


	}


}

