using DIKUArcade.Entities;

namespace Galaga_Exercise_2.MovementStrategy {
    public class NoMove : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            throw new System.NotImplementedException();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            throw new System.NotImplementedException();
        }
    }
}