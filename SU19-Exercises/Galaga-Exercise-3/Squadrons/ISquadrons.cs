using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

// 2.7 Making an unified interface Squadrons
// Added snippet of code from the assignment description

namespace Galaga_Exercise_3.Squadrons {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; }
        int MaxEnemies { get; }
        void CreateEnemies(List<Image> enemyStrides);
    }
}