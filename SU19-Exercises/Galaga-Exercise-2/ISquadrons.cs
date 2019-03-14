using System.Collections.Generic;
using DIKUArcade.Entities;
using System.Drawing;
using Image = DIKUArcade.Graphics.Image;

// 2.7 Making a unified interface Squadrons
// Added snippet of code from the assignment description

namespace Galaga_Exercise_2.Squadrons {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; }
        int MaxEnemies { get; }
        void CreateEnemies(List<Image> enemyStrides);
    }
}