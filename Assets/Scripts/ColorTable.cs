using System.Collections.Generic;
using UnityEngine;

namespace Comony
{
    [CreateAssetMenu(menuName = "ScriptableObject/ColorTable")]
    public class ColorTable : ScriptableObject
    {
        public List<Color> linearColor1;
        public List<Color> linearColor2;
    }
}
