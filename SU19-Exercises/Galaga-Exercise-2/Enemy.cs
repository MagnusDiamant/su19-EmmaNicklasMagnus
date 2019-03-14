using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Enemy : Entity {
        private Game game;
        // 2.6 Making a get-only property vector of type Vec2F
        private Vec2F Vector { get; }


        public Enemy(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            // 2.6 Instantiating Vector as a new Vec2F
            Vector = shape.Position;
        }
    }
}