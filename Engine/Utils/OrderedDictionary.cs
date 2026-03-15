using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Utils
{

	// code from copilot as last minute emergency fix
	public class OrderedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private readonly Dictionary<TKey, TValue> dict = new();
		private readonly List<TKey> order = new();

		public void Add(TKey key, TValue value)
		{
			dict.Add(key, value);
			order.Add(key);
		}

		public bool ContainsKey(TKey key) => dict.ContainsKey(key);

		public TValue this[TKey key]
		{
			get => dict[key];
			set => dict[key] = value;
		}

		public bool Remove(TKey key)
		{
			if (!dict.Remove(key))
				return false;

			order.Remove(key);
			return true;
		}

		// 🔥 Das macht foreach möglich
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (var key in order)
				yield return new KeyValuePair<TKey, TValue>(key, dict[key]);
		}

		public void Clear()
		{
			dict.Clear();
			order.Clear();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

}
