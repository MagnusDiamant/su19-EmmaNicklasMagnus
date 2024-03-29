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
    public class GamePaused : IGameState {
            private static GamePaused instance = null;
            private Entity backGroundImage;
            private Text continueGame;
            private Text mainMenu;
            private Text quit;
            private Text[] menuButtons = new Text[3];
            private int activeMenuButton = 0;
            private int maxMenuButtons = 2;
            private GameEventBus<object> eventBus = GalagaBus.GetBus();

            // 3.3.3 - Works the exact same way as the constructor for the MainMenu except with a
            // different background image and a new button that makes it possible to access the
            // Main Menu from the pause screen
            public GamePaused() {
                backGroundImage = new Entity(new StationaryShape(new Vec2F(0.0f, 0.0f), 
                        new Vec2F(1.0f,1.0f)), 
                    new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))); 
                continueGame = new Text("Continue Game", (new Vec2F(0.3f, 0.5f)),
                    new Vec2F(0.35f,0.25f) );
                mainMenu = new Text("Main Menu", (new Vec2F(0.3f,0.4f)), 
                    new Vec2F(0.35f,0.25f));
                quit = new Text("Quit", (new Vec2F(0.3f,0.3f)), 
                    new Vec2F(0.35f,0.25f));
                menuButtons[0] = continueGame;
                menuButtons[1] = mainMenu;
                menuButtons[2] = quit;
            }

            public static GamePaused GetInstance() {
                return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
            }

            // Empty methods because of the IGameState interface 
            public void GameLoop() {
                
            }

            public void InitializeGameState() {
                
            }

            public void UpdateGameLogic() {
                
            }

            // 3.3.3 - Shows the background image and the buttons in the window
            public void RenderState() {
                // Making the different buttons different colours
                continueGame.SetColor(Color.Green);
                mainMenu.SetColor(Color.Blue);
                quit.SetColor(Color.DarkRed);
                // Making the activeMenuButton Golden
                menuButtons[activeMenuButton].SetColor(Color.Gold);
                
                // The buttons and background image are rendered
                backGroundImage.RenderEntity();
                menuButtons[0].RenderText();
                menuButtons[1].RenderText(); 
                menuButtons[2].RenderText();
            }

            // 3.3.3 - Works the same way as MainMenu.HandleKeyEvent except it has an option for 
            // accessing the Main Menu as well. 
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
                        } else if (activeMenuButton == 1) {
                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this,
                                "CHANGE_STATE",
                                "MAIN_MENU", ""));
                        break;
                    }
                        else {
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