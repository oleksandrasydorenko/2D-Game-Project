using RocketEngine.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Tilemapsystem
{
	public struct PrefabDataStruct
	{
		public int id;
		public int prefabIndex;
		public Vector2 position;

		public PrefabDataStruct(int id,int prefabIndex, Vector2 position)
		{
			this.prefabIndex = prefabIndex;
			this.position = position;
			this.id = id;
		}
	}

	public class PrefabDataPreset : DataPreSet
	{
		public List<PrefabDataStruct> presets = new List<PrefabDataStruct>();
	}
}
