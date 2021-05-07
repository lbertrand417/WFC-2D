using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MyTilePainter : MonoBehaviour
{

    public int gridsize = 1;
    public int width = 20;
    public int height = 20;
    public GameObject tiles;
    public Vector3 cursor;
    public Tuile[,] tileobs;
    public List<Tuile> palette = new List<Tuile>();

	public Tuile CreateTuile(Tuile t, Vector3 pos)
	{
		Tuile o = Instantiate(t , transform);
		o.transform.position = pos;
		return o;
	}

	public bool ValidCoords(int x, int y)
	{
		if (tileobs == null) { return false; }

		return (x >= 0 && y >= 0 && x < tileobs.GetLength(0) && y < tileobs.GetLength(1));
	}


	public void Awake()
	{
	}

	public void OnEnable()
	{
	}

	void OnValidate()
	{
		//_changed = true;
		BoxCollider bounds = this.GetComponent<BoxCollider>();
		bounds.center = new Vector3((width * gridsize) * 0.5f - gridsize * 0.5f, (height * gridsize) * 0.5f - gridsize * 0.5f, 0f);
		bounds.size = new Vector3(width * gridsize, (height * gridsize), 0f);
	}

}
