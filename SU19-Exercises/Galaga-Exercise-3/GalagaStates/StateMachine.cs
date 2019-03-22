using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using GalagaGame;


namespace Galaga_Exercise_3.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            // 3.2 - Subscribes every instance of StateMachine to the GalagaBus, so it will receive
            // GameStateEvents and InputEvents 
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            // The default state of the StateMachine is MainMenu 
            ActiveState = MainMenu.GetInstance();
        }

        // 3.2 - Switches the ActiveState depending on the input. 
        private void SwitchState(GameStateType.GameStateTypes stateType) {
            switch (stateType) {
            case GameStateType.GameStateTypes.GameRunning:
                // If the stateType is GameRunning, the ActiveState is set to an instance of 
                // GameRunning. 
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

        // 3.2 - Since the StateMachine is subscribed to the GalagaBus, it will receive its
        // messages/events. ProcessEvent takes either a GameStateEvent and then uses SwitchState
        // to change the ActiveState, or it takes an InputEvent and calls HandleKeyEvent with
        // the proper message and parameter1. 
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                SwitchState(
                    GameStateType.StateTransformer.TransformStringToState(gameEvent.Parameter1));
            } else if (eventType == GameEventType.InputEvent) {
                  ActiveState.HandleKeyEvent(gameEvent.Message,
                        gameEvent.Parameter1);
                }
            }
        }
    }
