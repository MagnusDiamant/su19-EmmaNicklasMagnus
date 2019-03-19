using DIKUArcade.EventBus;
using DIKUArcade.State;
using GalagaGame;


namespace Galaga_Exercise_3.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType.GameStateTypes stateType) {
            switch (stateType) {
                case GameStateType.GameStateTypes.GameRunning:
                    ActiveState = GameRunning.GetInstance();
                    break;
                case GameStateType.GameStateTypes.GamePaused:
                    ActiveState = GamePaused.GetInstance();
                    break;
                default:
                    ActiveState = MainMenu.GetInstance();
                    break;
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Parameter1) {
                    case "GAME_RUNNING":
                        SwitchState(GameStateType.GameStateTypes.GameRunning);
                        break;
                    case "GAME_PAUSED":
                        SwitchState(GameStateType.GameStateTypes.GamePaused);
                        break;
                    case "MAIN_MENU":
                        SwitchState(GameStateType.GameStateTypes.MainMenu);
                        break;
                }
            }
        }
    }
}