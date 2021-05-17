using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int rows = 5;

    [SerializeField]
    private int cols = 8;

    [SerializeField]
    private int tileSize = 1;

    public Tuile referenceTile;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        //GameObject referenceTile = (GameObject) Instantiate(Resources.Load("grass_tile"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tuile tile = Instantiate(referenceTile, transform);

                float posX = col * tileSize;
                float posY = row * -tileSize;

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(referenceTile);

        float gridW = cols * tileSize;
        float gridH = rows * tileSize;

        transform.position = new Vector2(-gridW / 2 + (float) tileSize / 2, gridH / 2 - (float) tileSize / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
