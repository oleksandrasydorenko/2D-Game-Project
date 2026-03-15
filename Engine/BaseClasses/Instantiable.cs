using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketEngine;

namespace RocketEngine
{
    public abstract class Instantiable : IUpdatable
    {
		public String Name { get; set; }

		public Instantiable() { }
		/// <summary>
		/// Gets Called when the Instantiables constructor runs
		/// </summary>
		public virtual void Construct() { }
		/// <summary>
		/// Gets Called Async after the Instantiable has been constructed and after init
		/// </summary>
		public virtual void Start() { }
        /// <summary>
        /// will run async once and after construct use this for  calculations that need to happen once and after the Object is instantiated
        /// </summary>
        public virtual void Update() { }
		/// <summary>
		/// Will run every frame use this to do draw textures in the world grid
		/// </summary>
		public virtual void Destroy() {}
    }


}
