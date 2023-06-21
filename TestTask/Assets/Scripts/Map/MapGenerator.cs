using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int _width = 21;
    private int _height = 13;
    public int WidthOffset { get; set; }
    public int HeightOffset { get; set; }

    public MapGeneratorCell[,] GenerateMap()
    {
        MapGeneratorCell[,] map = new MapGeneratorCell[_width, _height];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = new MapGeneratorCell { X = x - WidthOffset, Y = y - HeightOffset };
            }
        }

        return map;
    }
}
