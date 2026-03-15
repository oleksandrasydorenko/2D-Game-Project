using Raylib_cs;
using RocketEngine;

namespace JailBreaker
{
    public enum GameState
    {
        None,
        Menu,
        Play,
        Pause,
        GameOver,
    }
    public static class GameManager
    {

        private static float timeScaleBeforePaused = 1;
        public static Action onGameUnpaused { get; set; }
        public static Action<GameState> onGameStateChanged { get; set; }

        private static GameState currentState;
        public static GameState GameState
        {
            get => currentState;
            set
            {
                if (value == currentState) return;
                currentState = value;
                switch (currentState)
                {
                    case GameState.None:
                        break;
                    case GameState.Menu:
                        break;
                    case GameState.Play:
                        break;
                    case GameState.Pause:
                        break;
                    case GameState.GameOver:
                        break;
                }
                onGameStateChanged?.Invoke(currentState);
            }
        }




        private static bool showCursor = false;
        public static bool ShowCursor
        {
            get
            {
                return showCursor;
            }
            set
            {
                if (value == false)
                {
                    Raylib.HideCursor();
                }
                else
                {
                    Raylib.ShowCursor();
                }
            }
        }

        private static bool gamePaused;
        public static bool GamePaused
        {
            get
            {
                return gamePaused;
            }
            set
            {
                gamePaused = value;

                if (value == true)
                {
                    GameState = GameState.Pause;
                    timeScaleBeforePaused = Time.TimeScale;
                    Time.TimeScale = 0;
                }
                else
                {
					GameState = GameState.Play;
					Time.TimeScale = timeScaleBeforePaused;
                    onGameUnpaused?.Invoke();
                }
            }

        }

    }
}
