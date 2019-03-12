using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : IGameEventProcessor<object> {
        private readonly Game game;
        private readonly GameEventBus<object> eventBus;
        

        public Player(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            game.eventBus.Subscribe(GameEventType.PlayerEvent, this);
        }

        // Making the Direction method which sets the direction of the player as the given vector
        private void Direction(Vec2F vector) {
            Shape.AsDynamicShape().Direction = vector;
        }

        // Making the Move method which moves the player unless the player is trying to move
        // outside of the window
        public void Move() {
            if (Shape.Position.X + Shape.AsDynamicShape().Direction.X > 0.01f &&
                Shape.Position.X + Shape.AsDynamicShape().Direction.X < 0.9f) {
                Shape.Move();
            }
        }

        // CreateShot adds a shot to the playerShots list 
        public void CreateShot() {
            var playerShot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Shape.Position.X + 0.047f,
                    Shape.Position.Y + 0.1f), new Vec2F(0.008f, 0.027f)),
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