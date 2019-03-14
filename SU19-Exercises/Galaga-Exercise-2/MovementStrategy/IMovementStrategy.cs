using DIKUArcade.Entities;

// 2.8 Making an interface MovementStrategy
// Added snippet of code from the assignment description

namespace Galaga_Exercise_2.MovementStrategy {
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
}