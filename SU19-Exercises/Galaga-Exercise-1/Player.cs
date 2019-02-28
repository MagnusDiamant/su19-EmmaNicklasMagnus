using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private Game game;

        public Player(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
        }

        public void Direction(Vec2F vector) {
            Shape.AsDynamicShape().Direction = vector;
        }

        public void Move() {
            switch (game.mode) {
            case 1 when Shape.Position.X - 0.01 > 0.0:
            case 2 when Shape.Position.X + 0.01 < 0.9:
                Shape.Move();
                break;
            }
        }

        public void CreateShot() {
            PlayerShot playerShot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Shape.Position.X + 0.047f, 
                        Shape.Position.Y+ 0.1f), new Vec2F(0.008f, 0.027f)),
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(playerShot);
        }
    }
}
