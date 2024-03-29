using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3{
    public class Score {
        private readonly Text display;

        private int score;

        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }

        // AddPoint increases the score by the given point.
        public void AddPoint(int point) {
            score += point;
        }

        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }
    }
}