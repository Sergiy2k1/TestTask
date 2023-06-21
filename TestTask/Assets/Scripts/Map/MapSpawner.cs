using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _block;
    
    private MapGenerator _mapGenerator;
    private const float CHANCE_TO_INSTANTIATE_BLOCK = 0.35f;


    private void Start()
    {
        GenerateBlocks();
    }

    private void GenerateBlocks()
    {
        _mapGenerator = new MapGenerator();

        SetOffsets();

        MapGeneratorCell[,] map = _mapGenerator.GenerateMap();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                bool isCanInstantiate = CheckRandomlyChanceToInstantiate();

                if (isCanInstantiate)
                {
                    Instantiate(_block[Random.Range(0, _block.Length)], new Vector2(map[x, y].X, map[x, y].Y), Quaternion.identity);
                }
            }
        }
    }

    private void SetOffsets()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        _mapGenerator.HeightOffset = (int)height / 2;
        _mapGenerator.WidthOffset = (int)width / 2;
    }

    private bool CheckRandomlyChanceToInstantiate()
    {
        float random = Random.Range(0f, 1f);

        if (random < CHANCE_TO_INSTANTIATE_BLOCK)
        {
            return true;
        }

        return false;
    }
}
