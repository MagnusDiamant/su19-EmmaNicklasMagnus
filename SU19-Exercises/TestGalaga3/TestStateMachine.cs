using System.Collections.Generic;
using DIKUArcade.EventBus;
using GalagaGame;
using Galaga_Exercise_3.GalagaState;
using NUnit.Framework;
namespace TestGalaga3 {
    [TestFixture]
    public class TestStateMachine {
        private StateMachine stateMachine;
        private GameEventBus<object> galagaBus;
        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();
            galagaBus = GalagaBus.GetBus();
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent, // Message from the player
                GameEventType.GameStateEvent // Message about the GameStateEvent
            });
            stateMachine = new StateMachine();
            galagaBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
            
            // Here you should:
            // (1) Initialize a GalagaBus with proper GameEventTypes // (2) Instantiate the StateMachine
            // (3) Subscribe the GalagaBus to proper GameEventTypes // and GameEventProcessors

        }
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        [Test]
        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }
       
    } 
}