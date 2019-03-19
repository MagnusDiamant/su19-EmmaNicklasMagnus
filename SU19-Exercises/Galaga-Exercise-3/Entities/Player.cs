using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    // 2.5 Making the Player inherit from IGameEventProcessor
    public class Player : IGameEventProcessor<object> {
        private readonly GameEventBus<object> eventBus;

        private readonly Game game;


        public Player(Game game, DynamicShape shape, IBaseImage image) {
            this.game = game;
            // 2.5 Instantiating Entity as a new Entity
            Entity = new Entity(shape, image);
        }

        // 2.5 Making an Entity of type Entity
        public Entity Entity { get; }

        // 2.5 ProcessEvent calls Direction for the different cases when the player pushes left and
        // right and releases the keys. 
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

        // Making the Direction method which sets the direction of the player as the given vector
        // 2.5 The Direction is now private
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
    }
}