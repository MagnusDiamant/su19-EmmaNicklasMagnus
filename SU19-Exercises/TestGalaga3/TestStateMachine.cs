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
            // 3.2.1 - Initializes a GalagaBus 
            galagaBus = GalagaBus.GetBus();
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent, // Message from the player
                GameEventType.GameStateEvent // Message about the GameStateEvent
            });
            // 3.2.1 - Intantiates a StateMachine
            stateMachine = new StateMachine();
            // 3.2.1 - Subscribes said stateMachine to the galagaBus 
            galagaBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
            

        }
        
        // 3.2.1 - Tests that the initial ActiveState is MainMenu
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        
        // 3.2.1 - Tests that the ActiveState can be changed from MainMenu to GamePaused
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