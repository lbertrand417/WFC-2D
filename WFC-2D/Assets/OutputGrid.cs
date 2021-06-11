using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class OutputGrid : MonoBehaviour
{

    public List<Tuile> globalList;

    // Raw values
    private int width = 2;
    private int height = 2;
    private int tileSize = 1;

#if UNITY_EDITOR
    void OnValidate()
    {
        // Should be in the "main" script
        BoxCollider bounds = this.GetComponent<BoxCollider>();
        bounds.size = new Vector3(width * tileSize, (height * tileSize), 0f);
    }

    public bool isPossible(List<Tuile> possibilities, Tuile myTuile)
    {
        foreach (Tuile p in possibilities)
        {
            //if (myTuile.tilename.Equals(p.tilename))
            if (myTuile.GetInstanceID() == p.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateGrid(List<Tuile>[] tuile, List<Tuile> dictTuile)
    {
        // Retrieve number of rows and cols and tilesize

        // Size of the canvas
        float gridW = width * tileSize;
        float gridH = height * tileSize;

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                // Give the position of the tile
                // First term : Find the right location for the given "big" tile
                // Second term + third term : translation of the entire tile grid to the corner of the canvas
                //      - second term : center of the first tile to the corner of the canvas
                //      - third term : shift to put the corner of the first tile to the corner of the canvas
                float posX = col * tileSize - gridW / 2 + (float) tileSize / 2;
                float posY = row * -tileSize + gridH / 2 - (float) tileSize / 2;

                Debug.Log(tuile[col + width * row].Count);

                // If final tile found
                if (tuile[col + width * row].Count == 1)
                {
                    Tuile tile = Instantiate(tuile[col + width * row][0], transform);

                    tile.transform.localPosition = new Vector2(posX, posY);


                } else
                {
                    // Size of the subgrid (divid x divid)
                    int divid = (int) Math.Ceiling(Math.Sqrt(dictTuile.Count));
                    Debug.Log("divid :" + divid);

                    for (int i = 0; i < dictTuile.Count; i++)
                    {
                        // If the tile is a possibility for this spot
                        if(isPossible(tuile[col + width * row], dictTuile[i]))
                        {
                            Tuile tile = Instantiate(dictTuile[i], transform);

                            // Change the scale of the tile
                            tile.transform.localScale = tile.transform.localScale / divid;

                            // Indices in the sub grid of the given "small" tile
                            int indicex = (int) i % divid;
                            int indicey = (int) i / divid;

                            // Give position of the given "small" tile
                            // First term : position of the "big" tile (the subgrid starts at the center of the big tile)
                            // Second term + third term : translation of the entire sub grid to the corner of the "big" tile
                            //      - second term : center of the first "small" tile to the corner of the "big" tile
                            //      - third term : shift to put the corner of the first "small" tile to the corner of the "big" tile
                            // Fourth term : Find the right location for the given "small" tile
                            float localPosX = posX - (float) divid / 2 * tile.transform.localScale.x + (float) tile.transform.localScale.x / 2 + indicex * tile.transform.localScale.x;
                            float localPosY = posY + (float) divid / 2 * tile.transform.localScale.y - (float) tile.transform.localScale.y / 2 - indicey * tile.transform.localScale.y;

                            tile.transform.localPosition = new Vector2(localPosX, localPosY);
                            
                        }
                    }
                }
            }
        }
    }

    public void Clear()
    {
        // Reset and delete all objects
        int childs = this.gameObject.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            DestroyImmediate(this.gameObject.transform.GetChild(i).gameObject);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(OutputGrid))]
public class OutputGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        OutputGrid me = (OutputGrid)target;
        if (GUILayout.Button("RUN"))
        {

            List<Tuile>[] myGrid = new List<Tuile>[4];

            List<Tuile> list1 = new List<Tuile>();
            list1.Add(me.globalList[4]);
            myGrid[0] = list1;

            List<Tuile> list2 = new List<Tuile>();
            list2.Add(me.globalList[2]);
            list2.Add(me.globalList[5]);
            myGrid[1] = list2;

            List<Tuile> list3 = new List<Tuile>();
            list3.Add(me.globalList[6]);
            myGrid[2] = list3;

            List<Tuile> list4 = new List<Tuile>();
            list4.Add(me.globalList[2]);
            list4.Add(me.globalList[7]);
            myGrid[3] = list4;

            me.Clear();
            me.UpdateGrid(myGrid, me.globalList);
        }
        DrawDefaultInspector();
    }

}
#endif
