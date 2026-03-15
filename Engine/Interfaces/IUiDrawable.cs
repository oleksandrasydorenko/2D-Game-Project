using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public interface IUiDrawable : ISortingLayer
	{
		void DrawUi();
	}
}
