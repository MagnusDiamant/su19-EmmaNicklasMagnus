using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Enemy : Entity {
        public Entity enemy;
        private Game game;


        public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
            game = game;
            // 2.6 Vector is now a copy of the shape's position
            Vector = shape.Position.Copy();
            // 2.8 Instantiating enemy as a new Entity
            enemy = new Entity(shape, image);
        }

        // 2.6 Making a get-only property vector of type Vec2F
        public Vec2F Vector { get; }

        // 2.8 Making the Direction method which sets the direction of the enemy as the given vector
        public void Direction(Vec2F vector) {
            enemy.Shape.AsDynamicShape().Direction = vector;
            enemy.Shape.Move();
        }
    }
}