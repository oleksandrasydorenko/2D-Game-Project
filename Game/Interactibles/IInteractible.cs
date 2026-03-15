using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Interactibles
{
    public interface IInteractible 
    {
        void Interact(GameObject other);
    }
}
