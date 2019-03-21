using Galaga_Exercise_3.GalagaState;
using NUnit.Framework;

namespace TestGalaga3 {
    public class TestStateTransformer {
        // Assignment 2.0 
        [Test]
        public void TestTransformStateToStringRunning() {
            Assert.AreEqual("GAME_RUNNING", 
                GameStateType.StateTransformer.
                    TransformStateToString(GameStateType.GameStateTypes.GameRunning));
        }

        [Test]
        public void TestTransformStateToStringPaused() {
            Assert.AreEqual("GAME_PAUSED",
                GameStateType.StateTransformer.TransformStateToString
                (GameStateType.GameStateTypes
                    .GamePaused));
        }
        [Test]
        public void TestTransformStateToStringMenu() {
            Assert.AreEqual("MAIN_MENU",
                GameStateType.StateTransformer.TransformStateToString
                (GameStateType.GameStateTypes
                    .MainMenu));
        }

        [Test]
        public void TestTransformStringToStateRunning() {
            Assert.AreEqual(GameStateType.GameStateTypes.GameRunning,
                GameStateType.StateTransformer.TransformStringToState("GAME_RUNNING"));
        }
        [Test]
        public void TestTransformStringToStatePaused() {
            Assert.AreEqual(GameStateType.GameStateTypes.GamePaused,
                GameStateType.StateTransformer.TransformStringToState("GAME_PAUSED"));
        }
        [Test]
        public void TestTransformStringToStateMenu() {
            Assert.AreEqual(GameStateType.GameStateTypes.MainMenu,
                GameStateType.StateTransformer.TransformStringToState("MAIN_MENU"));
        }
       
    }
        
}