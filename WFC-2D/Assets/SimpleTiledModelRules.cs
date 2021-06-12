using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    bool check(Tuile tile1, Tuile tile2, Direction direction)
    {
        return rules[tileIndices[tile1], tileIndices[tile2], (int) direction];
    }

    public void generateRules()
    {
        rules = new bool[numTiles, numTiles, NUM_DIRECTIONS];
        int rightCheck = width - 1;
        int bottomCheck = tiles.Length - width;
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
                rules[tileIndices[tiles[i]], tileIndices[tiles[i + 1]], (int) Direction.Right] = true;
            }

            // top
            if (i >= width)
            {
                rules[tileIndices[tiles[i - width]], tileIndices[tiles[i]], (int) Direction.Top] = true;
            }

            // bottom
            if (i < bottomCheck)
            {
                rules[tileIndices[tiles[i]], tileIndices[tiles[i + width]], (int) Direction.Bottom] = true;
            }
        }
    }

    public void generateIndices()
    {
        tileIndices = new Dictionary<Tuile, int>();
        numTiles = 0;
        foreach (Tuile tile in tiles)
        {
            if (!tileIndices.ContainsKey(tile))
            {
                tileIndices.Add(tile, numTiles++);
            }
        }
        Debug.Log(numTiles);
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
            Debug.Log(pair.Key.tilename + " " + pair.Value);
        }
        Debug.Log("numTiles " + numTiles);
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

            me.testSampleTiles();
            //me.sampleTiles();
            me.generateIndices();
            //me.testGenerateIndices();
            me.generateRules();
        }
        DrawDefaultInspector();
    }
}
#endif