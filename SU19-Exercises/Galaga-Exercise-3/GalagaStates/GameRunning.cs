using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using GalagaGame;
using Galaga_Exercise_3.MovementStrategy;

namespace Galaga_Exercise_3.GalagaState {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        
        // Creating an instance field player
        private readonly Player player;

        // Creating an instance field score
        private readonly Score score;
        
        // Creating a playerShots list
        public List<PlayerShot> playerShots { private set; get; }
        
        // Creating fields for explosions
        private readonly int explosionLength = 500;
        private readonly AnimationContainer explosions;
        private readonly List<Image> explosionStrides;
        
        // Creating fields for the enemies
        private List<Image> blueMonster;
        private List<Image> greenMonster;
        private GreenSquadron greenSquadron = new GreenSquadron();

        // Creating fields to make the enemies move
        private NoMove noMove;
        private Down down;
        private ZigZagDown zigzag;

        // Creating fields for the squadrons
        private List<Image> redMonster;
        private Squadron squadron = new Squadron();
        private SuperSquadron superSquadron = new SuperSquadron();
        

        public GameRunning() {
            // Instantiating player as a new Player
            player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), 
                    new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            // Making the blueMonster 
            blueMonster = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));

            // Making the redMonster
            redMonster = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));

            // Making the greenMonster
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

            // Instantiating the move fields
            noMove = new NoMove();
            down = new Down();
            zigzag = new ZigZagDown();
            
            // Creating the different squadrons 
            squadron.CreateEnemies(blueMonster);
            superSquadron.CreateEnemies(redMonster);
            greenSquadron.CreateEnemies(greenMonster);
        }

        // Returns an instance of GameRunning (and instantiates one if it has not already been)
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        // AddExplosion adds an explosion animation to the animation container
        // (added from the assignment description).
        public void AddExplosion(float posX, float posY, float extendX, float extendY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extendX, extendY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
        
        // CreateShot adds a shot to the playerShots list 
        public void CreateShot() {
            var playerShot = new PlayerShot(
                new DynamicShape(new Vec2F(player.Entity.Shape.Position.X + 0.047f,
                    player.Entity.Shape.Position.Y + 0.1f), new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            playerShots.Add(playerShot);
        }
        
        // Method Iterator iterates through an EntityContainer and deletes the relevant entities
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
                    score.AddPoint(5);
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

            // Making sure the enemies that have been hit are removed by using the Iterator-
            // Method on squadron.Enemies, superSquadron.Enemies and greenSquadron.Enemies
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
        
        public void GameLoop() {
            
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            // Making the player move
            player.Move();

            // 2.8 Making the enemies move
            noMove.MoveEnemies(squadron.Enemies);
            down.MoveEnemies(superSquadron.Enemies);
            zigzag.MoveEnemies(greenSquadron.Enemies);

            // Iterating shots
            IterateShots();
        }

        public void RenderState() {
            
            // Renders the player
            player.Entity.RenderEntity();

            // Rendering the squadron enemies
            foreach (Entity squad in squadron.Enemies) {
                squad.RenderEntity();
            }

            // Rendering the SuperSquandron enemies
            foreach (Entity superSquad in superSquadron.Enemies) {
                superSquad.RenderEntity();
            }

            // Rendering the GreenSquadron enemies
            foreach (Entity greenSquad in greenSquadron.Enemies) {
                greenSquad.RenderEntity();
            }

            // Renders the shots 
            foreach (var shot in playerShots) {
                shot.RenderEntity();
            }

            // Renders the explosions and the score 
            explosions.RenderAnimations();
            score.RenderScore();
        }

        // 3.3.2 - Works the same way as MainMenu.HandleKeyEvent except for some added features
        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                // If the escape key is pressed an event is created for all processors to change 
                // the activeState to GamePaused. 
                case "KEY_ESCAPE":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this,
                            "CHANGE_STATE", "GAME_PAUSED", ""));
                    break;
                // Sends a message to the event bus that the left key has been pressed
                case "KEY_LEFT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this,
                            "MOVE_LEFT", "", ""));
                    break;
                // Sends a message to the event bus that the right key has been pressed
                case "KEY_RIGHT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this,
                            "MOVE_RIGHT", "", ""));
                    break;
                // Calling CreateShot for the space key
                case "KEY_SPACE":
                    CreateShot();
                    break;
                }
            } else {
                switch (keyValue) {
                // Sends a message to the event bus that the right key has been released
                case "KEY_RIGHT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this,
                            "STOP", "", ""));
                    break;
                // Sends a message to the event bus that the left key has been released
                case "KEY_LEFT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this,
                            "STOP", "", ""));
                    break;
                }
            }
        }
    }
}