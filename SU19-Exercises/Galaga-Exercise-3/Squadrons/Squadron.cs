using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.Squadrons;

namespace Galaga_Exercise_3 {
    // 2.7 Making a new class Squadron that inherits from ISquadron
    public class Squadron : ISquadron {
        public EntityContainer<Enemy> Enemies { get; } = new EntityContainer<Enemy>();
        public int MaxEnemies { get; }

        public void CreateEnemies(List<Image> enemyStrides) {
            var v = 0.05f;
            for (var i = 0; i < 9; i++) {
                var enemy = new Enemy(new DynamicShape(new Vec2F(v, 0.9f),
                        new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, new List<Image>(enemyStrides)));
                Enemies.AddDynamicEntity(enemy);
                v += 0.1f;
            }
        }
    }
}