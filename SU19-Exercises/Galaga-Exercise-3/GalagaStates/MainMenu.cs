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

            // 3.3.1 - In order for the MainMenu class to be of the IGameState interface, there are 
            // a couple of methods we have to implement even though we don't use them. GameLoop, 
            // InitializeGameState and UpdateGameLogic are those methods. 
            public void GameLoop() {
                
            }

            public void InitializeGameState() {
                
            }

            public void UpdateGameLogic() {
                
            }

            // 3.3.1 - Shows the background image and the buttons in the window 
            public void RenderState() {
                // The background image is instantiated with a position, size and image called
                // TitleImage.png
                backGroundImage = new Entity(new StationaryShape(new Vec2F(0.0f, 0.0f), 
                        new Vec2F(1.0f,1.0f)), 
                    new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
                // The buttons newGame and quit are instantiated with a string displayed on the 
                // screen and a position and size 
                newGame = new Text("New Game", (new Vec2F(0.3f, 0.5f)),
                    new Vec2F(0.35f,0.25f) );
                quit = new Text("Quit", (new Vec2F(0.3f,0.4f)), 
                    new Vec2F(0.35f,0.25f));
                // The colors of the buttons are changed to make the interaction design more 
                // user friendly 
                newGame.SetColor(Color.Green);
                quit.SetColor(Color.DarkRed);
        
                // The buttons are inserted in the array menuButtons
                menuButtons[0] = newGame;
                menuButtons[1] = quit; 
                // The activeMenuButton (a.k.a. the button that you are "hovering" above) is 
                // colored gold so the user can see which button is about to be pressed 
                menuButtons[activeMenuButton].SetColor(Color.Gold);
                
                // The buttons and background image are rendered 
                backGroundImage.RenderEntity();
                menuButtons[0].RenderText();
                menuButtons[1].RenderText(); 
                
            }

            // 3.3.1 - If StateMachine.ProcessEvent has received an inputEvent HandleKeyEvent is 
            // called. HandleKeyEvent lets you choose a button with the up- and down-key and press 
            // it with the enter key 
            public void HandleKeyEvent(string keyValue, string keyAction) {
                if (keyAction == "KEY_PRESS") {
                    switch (keyValue) {
                    // If Key-Up is pressed 1 is subtracted from activeButton, unless its already 0
                    case "KEY_UP":
                        if (activeMenuButton != 0) {
                            activeMenuButton -= 1;
                        }

                        break;
                    // If Key-Down is pressed 1 is added to activeButton, unless activeButton is 
                    // bigger than the number of buttons in menuButtons. 
                    case "KEY_DOWN":
                        if (activeMenuButton != maxMenuButtons) {
                            activeMenuButton += 1;
                        }

                        break;
                    case "KEY_ENTER":
                        // If the enter key is pressed and the activeMenuButton = 0, a GameEvent
                        // is created for alle EventProcessors with the message to switch the
                        // state to GameRunning 
                        if (activeMenuButton == 0) {
                            eventBus.RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent, this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", ""));
                            break;
                        }
                        // If the activeButton is 1, a GameEvent is created to close the window. 
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