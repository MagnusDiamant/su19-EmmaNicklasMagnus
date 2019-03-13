using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Player : IGameEventProcessor<object> {
        public Entity Entity { get; private set; }
        
        private readonly Game game;
        private readonly GameEventBus<object> eventBus;
        

        public Player(Game game, DynamicShape shape, IBaseImage image) {
            this.game = game;
            Entity = new Entity(shape, image);
        }

        // Making the Direction method which sets the direction of the player as the given vector
        private void Direction(Vec2F vector) {
            Entity.Shape.AsDynamicShape().Direction = vector;
        }

        // Making the Move method which moves the player unless the player is trying to move
        // outside of the window
        public void Move() {
            if (Entity.Shape.Position.X + Entity.Shape.AsDynamicShape().Direction.X > 0.01f &&
                Entity.Shape.Position.X + Entity.Shape.AsDynamicShape().Direction.X < 0.9f) {
                Entity.Shape.Move();
            }
        }

        // CreateShot adds a shot to the playerShots list 
        public void CreateShot() {
            var playerShot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Entity.Shape.Position.X + 0.047f,
                    Entity.Shape.Position.Y + 0.1f), new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(playerShot);
        }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "MOVE_LEFT":
                    Direction(new Vec2F(-0.01f, 0.0f));
                    break;
                case "MOVE_RIGHT":
                    Direction(new Vec2F(0.01f, 0.0f));
                    break;
                case "STOP":
                    Direction(new Vec2F(0.0f, 0.0f));
                    break;
                }
            }
        }
    }
}