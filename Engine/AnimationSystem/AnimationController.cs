using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
    public class AnimationController
    {
        /// <summary>
        /// Der Animation Controller beinhaltet Animation Controller States
        /// Er kümmert sich nur darum zwischen den States zu Wechseln
        /// </summary>

        private Dictionary<string, AnimationControllerState> states = new Dictionary<string, AnimationControllerState>();
        public AnimationControllerState ActiveState {  get; private set; }

        public AnimationController(AnimationControllerState state)
        {

            this.states.Add(state.name, state);

            ActiveState = this.states[state.name];

            ActiveState.onStateEntered?.Invoke();
        }

        public AnimationController(AnimationControllerState[] states) 
        {
            for (int i = 0; i < states.Length; i++)
            {
                this.states.Add(states[i].name, states[i]);
            }

			ActiveState = this.states[states[0].name];

            ActiveState.onStateEntered?.Invoke();
        }

        public void SetState(string name)
        {
			if (states.ContainsKey(name))
            {
                ActiveState.onStateExited?.Invoke();
				ActiveState = states[name];
                ActiveState.onStateEntered?.Invoke();
			}
		}


		public void UpdateController(float deltaTime)
        {
			ActiveState.UpdateState(deltaTime);
		}
    }

}
