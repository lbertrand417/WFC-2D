using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MyTilePainter))]
public class SimpleTiledModelRules : MonoBehaviour
{
    private static int NUM_DIRECTIONS = 4;

    private Tuile[] tiles;
    private int width;
    private int height;
    private Dictionary<Tuile, int> tileIndices;
    private List<Tuile> indexTiles;
    private int numTiles;
    private bool[,,] rules;

    //private MyTilePainter myTilePainter;


    /*
        check(t1, t2, left)
        [t1][t2]

        check(t1, t2, right)
        [t2][t1]

        check(t1, t2, top)
        [t1]
        [t2]

        check(t1, t2, bottom)
        [t2]
        [t1]
    */

    public bool[,,] getRules()
    {
        return rules;
    }

    public Dictionary<Tuile, int> getDict()
    {
        return tileIndices;
    }

    public List<Tuile> getTuiles()
    {
        return indexTiles;
    }

    // Tile 1 : Neighbor
    // Tile 2 : Tile at the center
    // Tile 3 : Direction of the neighbor around the center tile
    public bool check(Tuile tile1, Tuile tile2, Direction direction)
    {
        return rules[tileIndices[tile1], tileIndices[tile2], (int) direction];
    }

    public void generateRules()
    {
        rules = new bool[numTiles, numTiles, NUM_DIRECTIONS];
        int rightCheck = width - 1;
        int topCheck = tiles.Length - width;
        for (int i = 0; i < tiles.Length; ++i)
        {
            // left
            if (i % width != 0)
            {
                rules[tileIndices[tiles[i - 1]], tileIndices[tiles[i]], (int) Direction.Left] = true;
            }

            // right
            if (i % width != rightCheck)
            {
                rules[tileIndices[tiles[i + 1]], tileIndices[tiles[i]], (int) Direction.Right] = true;
            }

            // bottom
            if (i >= width)
            {
                rules[tileIndices[tiles[i - width]], tileIndices[tiles[i]], (int) Direction.Bottom] = true;
            }

            // top
            if (i < topCheck)
            {
                rules[tileIndices[tiles[i + width]], tileIndices[tiles[i]], (int) Direction.Top] = true;
            }
        }
    }

    public void generateIndices()
    {
        TuileEqualityComparer equalityComparer = new TuileEqualityComparer();
        tileIndices = new Dictionary<Tuile, int>(equalityComparer);
        numTiles = 0;
        indexTiles = new List<Tuile>();
        foreach (Tuile tile in tiles)
        {
            if (!tileIndices.ContainsKey(tile))
            {
                indexTiles.Add(tile);
                tileIndices.Add(tile, numTiles++);
            }
        }
        Debug.Log("numTiles = " + numTiles);
    }



/*
0 0 1
2 1 1
3 3 3
1 1 1
*/
    public void sampleTiles()
    {
        GameObject go = new GameObject();
        Tuile t0 = go.AddComponent<Tuile>();
        t0.tilename = "t0";
        Tuile t1 = go.AddComponent<Tuile>();
        t1.tilename = "t1";
        Tuile t2 = go.AddComponent<Tuile>();
        t2.tilename = "t2";
        Tuile t3 = go.AddComponent<Tuile>();
        t3.tilename = "t3";
        width = 3;
        height = 4;
        tiles = new Tuile[] {t0, t0, t1, t2, t1, t1, t3, t3, t3, t1, t1, t1};
    }

    public void testSampleTiles()
    {
        foreach (Tuile tile in tiles)
        {
            Debug.Log(tile.tilename);
        }
    }

    public void testGenerateIndices()
    {
        foreach (KeyValuePair<Tuile, int> pair in tileIndices)
        {
            Debug.Log(pair.Key.gameObject.name + " " + pair.Value + " " + indexTiles[pair.Value]);
        }
        Debug.Log("numTiles " + numTiles);
    }

    public void testGenerateRules()
    {
        string[] tileNames = new string[numTiles];
        string[] directions = new string[] {"Left", "Right", "Top", "Bottom"};
        foreach (KeyValuePair<Tuile, int> pair in tileIndices)
        {
            Debug.Log("Tile " + pair.Key.gameObject.name + " index " + pair.Value);
            tileNames[pair.Value] = pair.Key.gameObject.name;
        }
        for (int t1 = 0; t1 < numTiles; ++t1)
        {
            for (int t2 = 0; t2 < numTiles; ++t2)
            {
                for (int d = 0; d < NUM_DIRECTIONS; ++d)
                {
                    if (rules[t1, t2, d])
                    {
                        Debug.Log("t1 = " + tileNames[t1] + ", t2 = " + tileNames[t2] + ", d = " + directions[d]);
                    }
                }
            }
        }
    }

    public void setTiles(Tuile[] t)
    {
        tiles = t;
    }

    public void setWidth(int w)
    {
        width = w;
    }

    public void setHeight(int h)
    {
        height = h;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SimpleTiledModelRules))]
public class SimpleTiledModelRulesEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SimpleTiledModelRules me = (SimpleTiledModelRules)target;
        GUILayout.Label("The canvas must be entirely filled up");
        GUILayout.Label("Generate Rules        : Generate rules using the canvas");
        if (GUILayout.Button("Generate Rules"))
        {

            me.setTiles(me.GetComponent<MyTilePainter>().tileobs);
            me.setWidth(me.GetComponent<MyTilePainter>().width);
            me.setHeight(me.GetComponent<MyTilePainter>().height);

            // me.testSampleTiles();
            //me.sampleTiles();
            me.generateIndices();
            // me.testGenerateIndices();
            me.generateRules();
            me.testGenerateRules();
        }
        DrawDefaultInspector();
    }
}
#endif