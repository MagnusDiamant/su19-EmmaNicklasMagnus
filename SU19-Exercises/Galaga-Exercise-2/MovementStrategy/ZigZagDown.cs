using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga_Exercise_2.MovementStrategy {
    // 2.8 Making a new class ZigZagDown that inherits from IMovementStrategy
    public class ZigZagDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            float s = 0.0003f;
            float p = 0.045f;
            float a = 0.05f;
            var x = enemy.Shape.Position.X;
            var y = enemy.Shape.Position.Y;
            var x0 = enemy.Vector.X;
            var y0 = enemy.Vector.Y;
            var newY = y + s;
            var newX = x0 + a * (float) Math.Sin((2 * Math.PI * (y0 - newY)) / p);
            enemy.Direction(new Vec2F(x + newX, y - newY));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}