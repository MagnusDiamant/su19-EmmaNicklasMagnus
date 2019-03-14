using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga_Exercise_2.MovementStrategy {
    // 2.8 Making a new class Down that inherits from IMovementStrategy
    public class Down : IMovementStrategy {
        // 2.8 MoveEnemy moves the enemy down by 0,001f
        public void MoveEnemy(Enemy enemy) {
            enemy.Direction(new Vec2F(0.0f, -0.001f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}