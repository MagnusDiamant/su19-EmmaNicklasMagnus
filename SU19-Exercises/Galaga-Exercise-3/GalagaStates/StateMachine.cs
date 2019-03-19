using System;
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
                 SwitchState(GameStateType.StateTransformer.
                     TransformStringToState(gameEvent.Parameter1));
                 
                 
                
            }
        }
    }
}