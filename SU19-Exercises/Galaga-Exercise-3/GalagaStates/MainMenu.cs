using System;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using GalagaGame;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaState {
        public class MainMenu : IGameState {
            private static MainMenu instance = null;
            private Entity backGroundImage;
            private Text newGame;
            private Text quit;
            private Text[] menuButtons = new Text[2];
            private int activeMenuButton = 0;
            private int maxMenuButtons = 1;
            private GameEventBus<object> eventBus = GalagaBus.GetBus();


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
                backGroundImage = new Entity(new StationaryShape(new Vec2F(0.0f, 0.0f), 
                        new Vec2F(1.0f,1.0f)), 
                    new Image(Path.Combine("Assets", "Images", "TitleImage.png")));  
                newGame = new Text("New Game", (new Vec2F(0.3f, 0.5f)),
                    new Vec2F(0.35f,0.25f) );
                quit = new Text("Quit", (new Vec2F(0.3f,0.4f)), 
                    new Vec2F(0.35f,0.25f));
                newGame.SetColor(Color.Green);
                quit.SetColor(Color.DarkRed);
                
                
                menuButtons[0] = newGame;
                menuButtons[1] = quit; 
                menuButtons[activeMenuButton].SetColor(Color.Gold);
                backGroundImage.RenderEntity();
                menuButtons[0].RenderText();
                menuButtons[1].RenderText(); 
                
            }

            public void HandleKeyEvent(string keyValue, string keyAction) {
                if (keyAction == "KEY_PRESS") {
                    switch (keyValue) {
                    case "KEY_UP":
                        if (activeMenuButton != 0) {
                            activeMenuButton -= 1;
                        }

                        break;
                    case "KEY_DOWN":
                        if (activeMenuButton != maxMenuButtons) {
                            activeMenuButton += 1;
                        }

                        break;
                    case "KEY_ENTER":
                        if (activeMenuButton == 0) {
                            eventBus.RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent, this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", ""));
                            break;
                        } else {
                            eventBus.RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.WindowEvent, this,
                                    "CLOSE_WINDOW", "", ""));
                            break;
                        }
                    }
                }
            }
        }
    }