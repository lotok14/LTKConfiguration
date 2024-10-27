using UnityEngine;

namespace LTKConfiguration.Utils
{
    public class CustomPoint
    {
        public string name;
        public float width;
        public Color color;
        public bool alwaysAward;

        public CustomPoint(string name, float width, Color color, bool alwaysAward)
        {
            this.name = name;
            this.width = width;
            this.color = color;
            this.alwaysAward = alwaysAward;
        }
    }
}