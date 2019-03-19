using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga_Exercise_3.MovementStrategy {
    // 2.8 Making a new class NoMove that inherits from IMovementStrategy
    public class NoMove : IMovementStrategy {
        // 2.8 MoveEnemy doesn't move the enemy
        public void MoveEnemy(Enemy enemy) {
            enemy.Direction(new Vec2F(0.0f, 0.0f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}