using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RocketEngine.DataStore;

namespace RocketEngine.Tilemapsystem
{
	public class TilemapDataPreset: DataPreSet
	{
		public int[][] GridIndexSet { get; set; }
	}
}
