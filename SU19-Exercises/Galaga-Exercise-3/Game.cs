using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_3.GalagaState;
using Galaga_Exercise_3.MovementStrategy;

namespace Galaga_Exercise_3 {
    public class Game : IGameEventProcessor<object> {
        // Creating an instance field eventBus
        private readonly GameEventBus<object> eventBus;

        // Creating fields for explosions
        private readonly int explosionLength = 500;
        private readonly AnimationContainer explosions;
        private readonly List<Image> explosionStrides;

        // Creating an instance field gameTimer
        private readonly GameTimer gameTimer;

        // Creating an instance field player
        private readonly Player player;

        // Creating an instance field score
        private readonly Score score;

        // Creating an instance field win
        private readonly Window win;
        private Down down;

        // Creating fields for the enemies
        private List<Enemy> enemies;
        private List<Image> enemyStrides;
        private List<Image> greenMonster;
        private GreenSquadron greenSquadron = new GreenSquadron();

        // 2.8 Creating fields to make the enemies move
        private NoMove noMove;

        // 2.7 Creating fields for the squadrons
        private List<Image> redMonster;
        private Squadron squadron = new Squadron();
        private SuperSquadron superSquadron = new SuperSquadron();
        private ZigZagDown zigzag;



        public Game() {
            // Setting the window to a 500x500 resolution
            win = new Window("win", 500, 500);
            // Setting the timer to 60 ups and 120 fps
            gameTimer = new GameTimer(60, 120);

            // Instantiating player as a new Player
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));

            // Added snippet of code concerning the eventBus from the assignment description
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent, // Message from the player
                GameEventType.GameStateEvent // Message about the gameStateEvent
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            // 2.5 Making sure that player subcribes to the eventbus
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            // Added snippet of code from the assignment description (making a new enemy) 
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>();

            // 2.7 Making the redMonster
            redMonster = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));

            // 2.7 Making the greenMonster
            greenMonster = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "GreenMonster.png"));

            // Instantiating playerShots as a new PlayerShot list
            playerShots = new List<PlayerShot>();

            // Added snippet of code from the assignment description
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(9);

            // Instantiating score as a new Score
            score = new Score(new Vec2F(0.43f, 0.40f),
                new Vec2F(0.3f, 0.2f));

            // 2.8 Instantiating the move fields
            noMove = new NoMove();
            down = new Down();
            zigzag = new ZigZagDown();

            
        }

        // Creating a playerShots list
        public List<PlayerShot> playerShots { private set; get; }

        // Added snippet of code from the assignment description
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
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }

        public void GameLoop() {
            // 2.7 Creating the different squadrons 
            squadron.CreateEnemies(enemyStrides);
            superSquadron.CreateEnemies(redMonster);
            greenSquadron.CreateEnemies(greenMonster);

            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    // --------  Update game logic here  -----------

                    // Calling eventBus.ProcessEvents()
                    eventBus.ProcessEvents();

                    // Making the player move
                    player.Move();

                    // 2.8 Making the enemies move
                    noMove.MoveEnemies(squadron.Enemies);
                    down.MoveEnemies(superSquadron.Enemies);
                    zigzag.MoveEnemies(greenSquadron.Enemies);

                    // Iterating shots
                    IterateShots();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();

                    // --------  Render gameplay entities here --------

                    player.Entity.RenderEntity();

                    // 2.7 Rendering the squadron enemies
                    foreach (Entity squad in squadron.Enemies) {
                        squad.RenderEntity();
                    }

                    // 2.7 Rendering the SuperSquandron enemies
                    foreach (Entity superSquad in superSquadron.Enemies) {
                        superSquad.RenderEntity();
                    }

                    // 2.7 Rendering the GreenSquadron enemies
                    foreach (Entity greenSquad in greenSquadron.Enemies) {
                        greenSquad.RenderEntity();
                    }

                    foreach (var shot in playerShots) {
                        shot.RenderEntity();
                    }

                    explosions.RenderAnimations();
                    score.RenderScore();
                    MainMenu.GetInstance().RenderState();


                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        // Added snippet of code from the assignment description
        // 2.5 Making sure that when a key is pressed that the event is registered to the eventBus
        public void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this,
                        "CLOSE_WINDOW", "", ""));
                break;
            // 2.5 Sends a message to the event bus that the left key has been pressed
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this,
                        "MOVE_LEFT", "", ""));
                break;
            // 2.5 Sends a message to the event bus that the right key has been pressed
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this,
                        "MOVE_RIGHT", "", ""));
                break;
            // Calling CreateShot for the space key
            case "KEY_SPACE":
                player.CreateShot();
                break;
            }
        }

        // 2.5 Making sure that when a key is released that the event is registered to the eventBus
        public void KeyRelease(string key) {
            switch (key) {
            // 2.5 Sends a message to the event bus that the right key has been released
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this,
                        "STOP", "", ""));
                break;
            // 2.5 Sends a message to the event bus that the left key has been released
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this,
                        "STOP", "", ""));
                break;
            }
        }

        // AddEnemies creates the enemies and adds them to the enemy list. 
        public void AddEnemies(float x) {
            var enemy = new Enemy(new DynamicShape(new Vec2F(x, 0.9f),
                    new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, new List<Image>(enemyStrides)));
            enemies.Add(enemy);
        }

        // 2.7 Method Iterator iterates through an EntityContainer and deletes the relevant entities
        // if there has been a collision. Is to be used in IterateShots.
        public void Iterator(Entity entity) {
            foreach (var shot in playerShots) {
                // Checking if a shot has collided with the enemy.
                // If a collision has happened the shot and the enemy is deleted, an explosion is
                // shown and the score increases with one point.
                if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), entity.Shape)
                    .Collision) {
                    shot.DeleteEntity();
                    entity.DeleteEntity();
                    AddExplosion(entity.Shape.Position.X, entity.Shape.Position.Y,
                        0.1f, 0.1f);
                    score.AddPoint(1);
                }
            }
        }

        // IterateShots handles the updating and collision of the shots. 
        public void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }
            }

            // 2.7 Making sure the enemies that have been hit are removed by using the Iterator-
            // Method on squadron.Enemies
            squadron.Enemies.Iterate(Iterator);
            superSquadron.Enemies.Iterate(Iterator);
            greenSquadron.Enemies.Iterate(Iterator);

            // Making a new list without the shots that have hit an enemy or have left the window
            var newShots = new List<PlayerShot>();
            foreach (var newShot in playerShots) {
                if (!newShot.IsDeleted()) {
                    newShots.Add(newShot);
                }
            }

            playerShots = newShots;
        }

        // AddExplosion adds an explosion animation to the animation container
        // (added from the assignment description).
        public void AddExplosion(float posX, float posY, float extendX, float extendY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extendX, extendY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}