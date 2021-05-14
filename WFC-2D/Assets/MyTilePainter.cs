using System;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(BoxCollider))]
public class MyTilePainter : MonoBehaviour
{

	public int gridsize = 1;
	private int width = 5;
	private int height = 5;
	public Vector3 cursor;
	public bool focused = false; // Display the red lines if true
	public GameObject[] tileobs;


	public UnityEngine.Object color = null;

#if UNITY_EDITOR

	static GameObject CreatePrefab(UnityEngine.Object fab, Vector3 pos)
	{
		GameObject o = PrefabUtility.InstantiatePrefab(fab as GameObject) as GameObject;
		o.transform.position = pos;
		return o;
	}

	public void Restore()
	{

		tileobs = new GameObject[width * height];
		int cnt = this.gameObject.transform.childCount;
		List<GameObject> trash = new List<GameObject>();
		for (int i = 0; i < cnt; i++)
		{
			GameObject tile = this.gameObject.transform.GetChild(i).gameObject;
			Vector3 tilepos = tile.transform.localPosition;
			int X = (int)(tilepos.x / gridsize);
			int Y = (int)(tilepos.y / gridsize);
			if (ValidCoords(X, Y))
			{
				tileobs[X + width * Y] = tile;
			}
			else
			{
				trash.Add(tile);
			}
		}
		for (int i = 0; i < trash.Count; i++)
		{
			if (Application.isPlaying) { Destroy(trash[i]); } else { DestroyImmediate(trash[i]); }
		}
	}

	public void Awake()
	{
		Restore();
	}

	public void OnEnable()
	{
		Restore();
	}

	void OnValidate()
	{
		BoxCollider bounds = this.GetComponent<BoxCollider>();
		bounds.center = new Vector3((width * gridsize) * 0.5f - gridsize * 0.5f, (height * gridsize) * 0.5f - gridsize * 0.5f, 0f);
		bounds.size = new Vector3(width * gridsize, (height * gridsize), 0f);
	}

	public Vector3 GridV3(Vector3 pos)
	{
		Vector3 p = transform.InverseTransformPoint(pos) + new Vector3(gridsize * 0.5f, gridsize * 0.5f, 0f);
		return new Vector3((int)(p.x / gridsize), (int)(p.y / gridsize), 0);
	}

	public bool ValidCoords(int x, int y)
	{
		if (tileobs == null) { return false; }

		return (x >= 0 && y >= 0 && x < width && y < height);
	}

	public Vector3 Local(Vector3 p)
	{
		return this.transform.TransformPoint(p);
	}

	public void Drag(Vector3 mouse, MyTileLayerEditor.TileOperation op)
	{
		if (tileobs == null | tileobs.Length == 0) {
			Restore(); }
		if (this.ValidCoords((int)cursor.x, (int)cursor.y))
		{
			DestroyImmediate(tileobs[(int)cursor.x + width * (int)cursor.y]);
			if (op == MyTileLayerEditor.TileOperation.Drawing)
			{
				if (color == null) { return; }
				GameObject o = CreatePrefab(color, new Vector3());
				o.transform.parent = this.gameObject.transform;
				o.transform.localPosition = (cursor * gridsize);
				tileobs[(int)cursor.x + width * (int)cursor.y] = o;
			}
		}
	}

	public void Clear()
	{
		tileobs = new GameObject[width * height];
		int childs = this.gameObject.transform.childCount;
		for (int i = childs - 1; i >= 0; i--)
		{
			DestroyImmediate(this.gameObject.transform.GetChild(i).gameObject);
		}
	}

	// To draw the red lines
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;
		if (focused)
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
			Gizmos.DrawRay((cursor * gridsize) + Vector3.forward * -49999f, Vector3.forward * 99999f);
			Gizmos.DrawRay((cursor * gridsize) + Vector3.right * -49999f, Vector3.right * 99999f);
			Gizmos.DrawRay((cursor * gridsize) + Vector3.up * -49999f, Vector3.up * 99999f);
			Gizmos.color = Color.yellow;
		}

		Gizmos.DrawWireCube(new Vector3((width * gridsize) * 0.5f - gridsize * 0.5f, (height * gridsize) * 0.5f - gridsize * 0.5f, 0f),
			new Vector3(width * gridsize, (height * gridsize), 0f));
	}
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(MyTilePainter))]
public class MyTileLayerEditor : Editor
{
	public enum TileOperation { None, Drawing};
	private TileOperation operation;

	public override void OnInspectorGUI()
	{
		MyTilePainter me = (MyTilePainter) target;
		GUILayout.Label("Assign a prefab to the color property");
		GUILayout.Label("drag        : paint tiles");
		if (GUILayout.Button("CLEAR"))
		{
			me.Clear();
		}
		DrawDefaultInspector();
	}

	private bool AmHovering(Event e)
	{
		// Retrieve the current tile painter
		MyTilePainter me = (MyTilePainter) target;
		RaycastHit hit;
		// If the cursor is inside the current tile painter)
		if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, Mathf.Infinity) &&
				hit.collider.GetComponentInParent<MyTilePainter>() == me)
		{
			// Autorize the display of the red line on the cursor
			me.cursor = me.GridV3(hit.point);
			me.focused = true;


			Renderer rend = me.gameObject.GetComponentInChildren<Renderer>();
			if (rend) EditorUtility.SetSelectedRenderState(rend, EditorSelectedRenderState.Wireframe);
			return true;
		}
		me.focused = false;
		return false;
	}

	public void ProcessEvents()
	{
		MyTilePainter me = (MyTilePainter) target;
		int controlID = GUIUtility.GetControlID(1778, FocusType.Passive);
		EditorWindow currentWindow = EditorWindow.mouseOverWindow;
		if (currentWindow && AmHovering(Event.current))
		{
			Event current = Event.current;
			bool leftbutton = (current.button == 0);
			switch (current.type)
			{
				case EventType.MouseDown:
					if (leftbutton)
					{
						operation = TileOperation.Drawing;
						me.Drag(current.mousePosition, operation);

						current.Use();
						return;
					}
					break;
				case EventType.MouseDrag:
					if (leftbutton)
					{
						if (operation != TileOperation.None)
						{
							me.Drag(current.mousePosition, operation);
							current.Use();
						}

						return;
					}
					break;
				case EventType.MouseUp:
					if (leftbutton)
					{
						operation = TileOperation.None;
						current.Use();
						return;
					}
					break;
				case EventType.MouseMove:
					current.Use();
					break;
				case EventType.Repaint:
					break;
				case EventType.Layout:
					HandleUtility.AddDefaultControl(controlID);
					break;
			}
		}
	}

	void OnSceneGUI()
	{
		ProcessEvents();
	}

	void DrawEvents()
	{
		Handles.BeginGUI();
		Handles.EndGUI();
	}
}
#endif

