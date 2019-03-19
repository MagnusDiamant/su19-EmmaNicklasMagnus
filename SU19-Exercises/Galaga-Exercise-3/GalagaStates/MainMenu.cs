using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaState {
        public class MainMenu : IGameState {
            private static MainMenu instance = null;
            private Entity backGroundImage;
            private Text[] menuButtons;
            private int activeMenuButton;
            private int maxMenuButtons;

            public static MainMenu GetInstance() {
                return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
            }

            public void GameLoop() {
                throw new System.NotImplementedException();
            }

            public void InitializeGameState() {
                throw new System.NotImplementedException();
            }

            public void UpdateGameLogic() {
                throw new System.NotImplementedException();
            }

            public void RenderState() {
                throw new System.NotImplementedException();
            }

            public void HandleKeyEvent(string keyValue, string keyAction) {
                throw new System.NotImplementedException();
            }
        }
    }