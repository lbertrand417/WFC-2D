using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SemiInteractiveGrid : MonoBehaviour
{
    public Tuile[] tuiles;
    public float tuilesize = 1;
    public int width = 5;
    public int height = 5;

    public void myfunction()
    {
        Debug.Log("Coucou");
        Tuile[] randomTuiles = FindObjectsOfType<Tuile>();
        tuiles = new Tuile[width * height];

        foreach (Tuile t in randomTuiles)
        {
            int indicex = (int) ((t.transform.position.x - transform.position.x) / tuilesize);
            int indicey = (int)((t.transform.position.y - transform.position.y) / tuilesize);

            Vector2 pos = t.transform.position;
            pos.x = indicex * tuilesize + transform.position.x;
            pos.y = indicey * tuilesize + transform.position.y;
            t.transform.position = pos;

            if (0 <= indicex && 0 <= indicey && indicex < width && indicey < height)
            {
                int indice = indicex + width * indicey;
                tuiles[indice] = t;
                Debug.Log("Tuile trouvée à " + indicex + " et " + indicey);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SemiInteractiveGrid))]
public class SemiInteractiveGridEditor : Editor
{
	public override void OnInspectorGUI()
	{
		SemiInteractiveGrid me = (SemiInteractiveGrid) target;
		if (GUILayout.Button("RUN"))
		{
            me.myfunction();
		}
		DrawDefaultInspector();
	}

}
#endif
