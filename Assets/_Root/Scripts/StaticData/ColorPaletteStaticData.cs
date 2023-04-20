using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Editor
{
    [CreateAssetMenu(menuName = "StaticData/ColorPalette", fileName = "new Palette", order = 0)]
    public class ColorPaletteStaticData : ScriptableObject
    {
        public List<Color> Colors;
    }
}