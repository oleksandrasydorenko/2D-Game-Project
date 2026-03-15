using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
    public class AnimationSignal
    {
        private int frame;
        public int Frame { get { return frame; } init { if (value < 0) value = 0; frame = value; } }
        public Action onAnimationSignalTriggered { get; set; }
        public AnimationSignal(int frame) 
        {
            Frame = frame;
        }
    }
}
