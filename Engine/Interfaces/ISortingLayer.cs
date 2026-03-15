using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public interface ISortingLayer
	{

		public SortingLayers SortingLayer { get; set; }

		public int ZIndex { get; set; }
	}
}
