using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DIKUArcade.Physics;


namespace Galaga_Exercise_1 {
    using DIKUArcade;
    using System;
    using DIKUArcade.EventBus;
    using DIKUArcade.Timers;
    using System.Runtime.CompilerServices;
    using DIKUArcade.Entities;
    using DIKUArcade.Graphics;
    using DIKUArcade.Math;

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;
        public int mode;
        public List<Image> enemyStrides = new List<Image>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<PlayerShot> playerShots = new List<PlayerShot>();

        public List<PlayerShot> PlayerShots {
            get => playerShots;
            set => playerShots = value;
        }

        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        public Score score; 


        public Game() {
// TODO: Choose some reasonable values for the window and timer constructor.
// For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
            win = new Window("win", 500, 500);
            
            gameTimer = new GameTimer(60, 120);
            
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>();
            
            playerShots = new List<PlayerShot>();
            
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(9);
            score = new Score(new Vec2F(0.43f, 0.40f), 
            new Vec2F(0.3f, 0.2f));


        }

        public void GameLoop() {
            AddEnemies(0.05f);
            AddEnemies(0.15f);
            AddEnemies(0.25f);
            AddEnemies(0.35f);
            AddEnemies(0.45f);
            AddEnemies(0.55f);
            AddEnemies(0.65f);
            AddEnemies(0.75f);
            AddEnemies(0.85f);
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
// Update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    player.RenderEntity();
                    eventBus.ProcessEvents();
                    if (player.Shape.Position.X > 0.0 && player.Shape.Position.X < 0.9) {
                        player.Move();
                    }

                    IterateShots();

                    foreach (Enemy x in enemies) {
                        x.RenderEntity();
                    }

                    foreach (PlayerShot shot in playerShots) {
                        shot.RenderEntity();
                    }
                    explosions.RenderAnimations();
                    score.RenderScore();

// Render gameplay entities here
                    win.SwapBuffers();
                    

                }

                if (gameTimer.ShouldReset()) {
// 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }

        }

        public void KeyPress(string key) {

            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this,
                        "CLOSE_WINDOW", "", ""));
                break;
            case "KEY_LEFT":
                player.Direction(new Vec2F(-0.01f, 0.0f));
                mode = 1;
                break;
            case "KEY_RIGHT":
                player.Direction(new Vec2F(0.01f, 0.0f));
                mode = 2;
                break;
            case "KEY_SPACE":
                player.CreateShot(); //SLET MULIGVIS
                break;
            }

        }

        public void KeyRelease(string key) {
            player.Direction(new Vec2F(0.0f, 0.0f));
        }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                default:
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

        public void AddEnemies(float x) {
            Enemy enemy = new Enemy(this, new DynamicShape(new Vec2F(x, 0.9f),
                    new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, new List<Image>(enemyStrides)));
            enemies.Add(enemy);
        }

        public void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                foreach (var enemy in enemies) {
                    if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape)
                        .Collision) {
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                            0.1f, 0.1f);
                        score.AddPoint();
                    }
                }
            }

            List<Enemy> newEnemies = new List<Enemy>();
            foreach (Enemy newEnemy in enemies) {
                if (!newEnemy.IsDeleted()) {
                    newEnemies.Add(newEnemy);
                }

            }

            enemies = newEnemies;
            List<PlayerShot> newShots = new List<PlayerShot>();
            foreach (PlayerShot newShot in playerShots) {
                if (!newShot.IsDeleted()) {
                    newShots.Add(newShot);
                }
            }
            playerShots = newShots;
        }
        
        public void AddExplosion(float posX, float posY, float extendX, float extendY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extendX, extendY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}

    


        




    

