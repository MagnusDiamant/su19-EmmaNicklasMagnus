using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using GalagaGame;
using Galaga_Exercise_3.GalagaState;
using Galaga_Exercise_3.MovementStrategy;

namespace Galaga_Exercise_3 {
    public class Game : IGameEventProcessor<object> {
        // Creating an instance field eventBus
        private readonly GameEventBus<object> eventBus;

        // Creating an instance field gameTimer
        private readonly GameTimer gameTimer;

        // Creating an instance field win
        private readonly Window win;

        // Creating a field for the stateMachine
        private StateMachine stateMachine;
        

        public Game() {
            // Setting the window to a 500x500 resolution
            win = new Window("win", 500, 500);
            // Setting the timer to 60 ups and 120 fps
            gameTimer = new GameTimer(60, 120);

            // Added snippet of code concerning the eventBus from the assignment description
            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent, // Message from the player
                GameEventType.GameStateEvent // Message about the gameStateEvent
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            
            // Instantiating the stateMachine
            stateMachine = new StateMachine();

        }

        // ProcessEvent for game
        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    GameRunning.GetInstance().KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    GameRunning.GetInstance().KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic(); 
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }      
    }
}