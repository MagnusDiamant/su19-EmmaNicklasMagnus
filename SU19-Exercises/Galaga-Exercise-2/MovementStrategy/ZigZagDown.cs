using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga_Exercise_2.MovementStrategy {
    // 2.8 Making a new class ZigZagDown that inherits from IMovementStrategy
    public class ZigZagDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            enemy.Direction(new Vec2F(0.0f, -0.001f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            throw new System.NotImplementedException();
        }
    }
}