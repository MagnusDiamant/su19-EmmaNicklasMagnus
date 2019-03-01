using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

namespace Galaga_Exercise_1
{
    public class Game : IGameEventProcessor<object>
    {
        // Creating an instance field player
        private readonly Player player;
        
        // Creating an enemies list
        public List<Enemy> enemies;
        
        // Creating an enemyStrides list
        public List<Image> enemyStrides;
        
        // Creating an instance field eventBus
        private readonly GameEventBus<object> eventBus;
        
        // Creating a field explosionLength and setting it to 500
        private readonly int explosionLength = 500;
        
        // Creating an instance field explosions
        private readonly AnimationContainer explosions;
        
        // Creating an explosionStrides list
        private readonly List<Image> explosionStrides;
        
        // Creating an instance field gameTimer
        private readonly GameTimer gameTimer;

        // Creating a Mode field for solving the problem when the player reaches the edges
        public int Mode;
        
        // Creating a playerShots list
        public List<PlayerShot> playerShots { private set; get; }
        
        // Creating an instance field score
        private readonly Score score;
        
        // Creating an instance field win
        private readonly Window win;


        public Game()
        {
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
            eventBus.InitializeEventBus(new List<GameEventType>
            {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent // messages to the window
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);

            // Added snippet of code from the assignment description 
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>();

            // Instantiating playerShots as a new PlayerShot list
            playerShots = new List<PlayerShot>();

            // Added snippet of code from the assignment description
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(9);
            
            // Instantiating score as a new Score
            score = new Score(new Vec2F(0.43f, 0.40f),
                new Vec2F(0.3f, 0.2f));
        }

        // Added snippet of code from the assignment description
        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.WindowEvent)
                switch (gameEvent.Message)
                {
                    case "CLOSE_WINDOW":
                        win.CloseWindow();
                        break;
                }
            else if (eventType == GameEventType.InputEvent)
            {
                switch (gameEvent.Parameter1)
                {
                    case "KEY_PRESS":
                        KeyPress(gameEvent.Message);
                        break;
                    case "KEY_RELEASE":
                        KeyRelease(gameEvent.Message);
                        break;
                }
            }
        }

        public void GameLoop()
        {
            // Adding enemies to the screen at different spots
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
                while (gameTimer.ShouldUpdate()) win.PollEvents();
                
                // Update game logic here
                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    
                    // Calling eventBus.ProcessEvents()
                    eventBus.ProcessEvents();
                    
                    // Making the player move
                    if (player.Shape.Position.X > 0.0 && player.Shape.Position.X < 0.9) {
                        player.Move();
                    }
                    
                    // Iterating shots
                    IterateShots();

                    // Render gameplay entities here
                    player.RenderEntity();
                    foreach (var x in enemies) x.RenderEntity();
                    foreach (var shot in playerShots) shot.RenderEntity();
                    explosions.RenderAnimations();
                    score.RenderScore();
                    
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset())
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
            }
        }

        // Added snippet of code from the assignment description
        public void KeyPress(string key) {
            switch (key) {
                case "KEY_ESCAPE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this,
                            "CLOSE_WINDOW", "", ""));
                    break;
                // Calling player direction for the left key
                case "KEY_LEFT":
                    player.Direction(new Vec2F(-0.01f, 0.0f));
                    Mode = 1;
                    break;
                // Calling player direction for the right key
                case "KEY_RIGHT":
                    player.Direction(new Vec2F(0.01f, 0.0f));
                    Mode = 2;
                    break;
                // Calling CreateShot for the space key
                case "KEY_SPACE":
                    player.CreateShot();
                    break;
            }
        }

        public void KeyRelease(string key)
        {
            player.Direction(new Vec2F(0.0f, 0.0f));
        }

        // AddEnemies creates the enemies and adds them to the enemy list. 
        public void AddEnemies(float x)
        {
            var enemy = new Enemy(this, new DynamicShape(new Vec2F(x, 0.9f),
                    new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, new List<Image>(enemyStrides)));
            enemies.Add(enemy);
        }

        // IterateShots handles the updating and collision of the shots. 
        public void IterateShots()
        {
            foreach (var shot in playerShots)
            {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) shot.DeleteEntity();

                // Iterating through the enemies and checking if a shot has collided with it
                // If a collision has happened the shot and the enemy is deleted, an explosion is
                // shown and the score increases with one point.
                foreach (var enemy in enemies)
                    if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape)
                        .Collision)
                    {
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                            0.1f, 0.1f);
                        score.AddPoint();
                    }
            }

            // Making a new list without the enemies that has been shot
            var newEnemies = new List<Enemy>();
            foreach (var newEnemy in enemies)
                if (!newEnemy.IsDeleted())
                    newEnemies.Add(newEnemy);
            enemies = newEnemies;
            
            // Making a new list without the shots that have hit an enemy or have left the window
            var newShots = new List<PlayerShot>();
            foreach (var newShot in playerShots)
                if (!newShot.IsDeleted())
                    newShots.Add(newShot);
            playerShots = newShots;
        }

        // AddExplosion adds an explosion animation to the animation container
        // (added from the assignment description).
        public void AddExplosion(float posX, float posY, float extendX, float extendY)
        {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extendX, extendY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}