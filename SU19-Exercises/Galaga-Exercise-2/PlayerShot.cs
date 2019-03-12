using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class PlayerShot : Entity {
        private Game game;

        public PlayerShot(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            shape.AsDynamicShape().Direction = new Vec2F(0.0f, 0.01f);
        }
    }
}