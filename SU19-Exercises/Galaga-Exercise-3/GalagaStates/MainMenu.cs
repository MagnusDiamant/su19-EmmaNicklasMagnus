using System.Drawing;
using System.IO;
using System.Net.Sockets;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaState {
        public class MainMenu : IGameState {
            private static MainMenu instance = null;
            private Entity backGroundImage;
            private Text[] menuButtons;
            private int activeMenuButton;
            private int maxMenuButtons;

            /* public MainMenu() {
                //backGroundImage.Image = new 
                  //  Image(Path.Combine("Assets", "Images", "TitleImage.png"));
                Text newGame = new Text("New Game", Vec2F.Normalize(new Vec2F()),
                    Vec2F.Normalize(new Vec2F()) );
                Text quit = new Text("Quit", Vec2F.Normalize(new Vec2F()), 
                    Vec2F.Normalize(new Vec2F()));
                newGame.SetColor(Color.Green);
                quit.SetColor(Color.DarkRed);
                menuButtons[0] = newGame;
                menuButtons[1] = quit;
            } */

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
                backGroundImage.RenderEntity();
                menuButtons[0].RenderText();
                menuButtons[1].RenderText();
            }

            public void HandleKeyEvent(string keyValue, string keyAction) {
                throw new System.NotImplementedException();
            }
        }
    }