using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private readonly Game game;

        public Player(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            
        }

        // Making the Direction method which sets the direction of the player as the given vector
        public void Direction(Vec2F vector) {
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
/// <summary>
/// HEJ Hva så, dette er en lille ændring 
/// </summary>
        // CreateShot adds a shot to the playerShots list 
        public void CreateShot() {
            var playerShot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Shape.Position.X + 0.047f,
                    Shape.Position.Y + 0.1f), new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(playerShot);
        }
    }
}