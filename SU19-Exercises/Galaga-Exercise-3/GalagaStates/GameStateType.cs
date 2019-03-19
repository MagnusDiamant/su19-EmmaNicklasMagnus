using System;

namespace Galaga_Exercise_3.GalagaState {
    public class GameStateType {
        public enum GameStateTypes {
            GameRunning,
            GamePaused,
            MainMenu
        };

        public class StateTransformer {
            public static GameStateTypes TransformStringToState(string state) {
                switch (state) {
                case "GAME_RUNNING":
                    return GameStateTypes.GameRunning;
                case "GAME_PAUSED":
                    return GameStateTypes.GamePaused;
                case "MAIN_MENU":
                    return GameStateTypes.MainMenu;
                default:
                    throw new ArgumentException("Invalid argument - no match");
                }
            }

            public static string TransformStateToString(GameStateTypes state) {
                switch (state) {
                case GameStateTypes.GameRunning:
                    return "GAME_RUNNING";
                case GameStateTypes.GamePaused:
                    return "GAME_PAUSED";
                case GameStateTypes.MainMenu:
                    return "MAIN_MENU";
                default:
                    throw new ArgumentException("Invalid argument - no match");
                }
            }
        }
    }
}