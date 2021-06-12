using System;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(BoxCollider))]
public class MyTilePainter : MonoBehaviour
{

	private int gridsize = 1; // Size of one tile in the canvas
	public int width = 3; // Width of the canvas
	private int oldWidth = 3; 
	public int height = 4; // Height of the canvas
	private int oldHeight = 4;
	private Vector3 cursor;
	private bool focused = false; // Display the red lines if true
	public Tuile currentTile = null;

	//[HideInInspector]
	public Tuile[] tileobs;

#if UNITY_EDITOR

	static Tuile CreatePrefab(Tuile newtile, Vector2 pos)
	{
		// Create the tile
		Tuile o = Instantiate(newtile);
		o.transform.position = pos;
		return o;
	}


	void OnValidate()
	{
		BoxCollider bounds = this.GetComponent<BoxCollider>();
		bounds.center = new Vector3((width * gridsize) * 0.5f - gridsize * 0.5f, (height * gridsize) * 0.5f - gridsize * 0.5f, 0f);
		bounds.size = new Vector3(width * gridsize, (height * gridsize), 0f);

		if (height > 0 && width > 0)
		{
			if (oldHeight != height)
			{
				int diff = -(oldHeight - height) * gridsize;

				Tuile[] temp = new Tuile[width * height];

				for (int i = 0; i < tileobs.Length; i++)
				{
					if (tileobs[i] != null)
					{
						tileobs[i].transform.position = new Vector2(tileobs[i].transform.position.x, tileobs[i].transform.position.y + diff);
						if (i + diff * width < temp.Length && i + diff * width >= 0)
						{
							temp[i + diff * width] = tileobs[i];
						}
					}
				}

				oldHeight = height;
				tileobs = temp;

			}

			if (oldWidth != width)
			{
				int diff = - (oldWidth - width) * gridsize;

				Tuile[] temp = new Tuile[width * height];

				for (int i = 0; i < tileobs.Length; i++)
				{
					if (tileobs[i] != null)
					{
						// Gérer les bords à droite
						if (i % oldWidth < width)
						{
							temp[i + (int) diff * i / oldWidth] = tileobs[i];
						}
					}
				}

				oldWidth = width;
				tileobs = temp;

			}
		}
	}

	public Vector3 GridV3(Vector3 pos)
	{
		Vector3 p = transform.InverseTransformPoint(pos) + new Vector3(gridsize * 0.5f, gridsize * 0.5f, 0f);
		return new Vector3((int)(p.x / gridsize), (int)(p.y / gridsize), 0);
	}

	// Check if the point is inside the canvas
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
		// Initialization if it wasn't done before
		if (tileobs == null | tileobs.Length == 0) {
			tileobs = new Tuile[width * height];
		}

			// If the cursor is inside the canvas
		if (this.ValidCoords((int)cursor.x, (int)cursor.y))
		{
			// Destroy the previous tile if it exists
			if (tileobs[(int)cursor.x + width * (int)cursor.y] != null)
			{
				DestroyImmediate(tileobs[(int)cursor.x + width * (int)cursor.y].gameObject);
			}

			// If drawing mode
			if (op == MyTileLayerEditor.TileOperation.Drawing)
			{
				// If no tile selected do nothing
				if (currentTile == null) { return; }

				// Create the object
				Tuile o = CreatePrefab(currentTile, new Vector2());
				o.transform.parent = this.gameObject.transform;
				o.transform.localPosition = (cursor * gridsize);
				tileobs[(int)cursor.x + width * (int)cursor.y] = o;
			}
		}
	}

	bool inCanvas(Tuile t)
	{
		foreach (Tuile tile in tileobs)
		{
			if (tile != null)
			{
				if (t.GetInstanceID() == tile.GetInstanceID())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Clean()
	{
		int childs = this.gameObject.transform.childCount;
		for (int i = childs - 1; i >= 0; i--)
		{
			Debug.Log(inCanvas(this.gameObject.transform.GetChild(i).GetComponent<Tuile>()));
			if (!inCanvas(this.gameObject.transform.GetChild(i).GetComponent<Tuile>()))
			{
				DestroyImmediate(this.gameObject.transform.GetChild(i).gameObject);
			}
		}
	}

	public void Clear()
	{
		// Reset and delete all objects
		tileobs = new Tuile[width * height];
		int childs = this.gameObject.transform.childCount;
		for (int i = childs - 1; i >= 0; i--)
		{
			DestroyImmediate(this.gameObject.transform.GetChild(i).gameObject);
		}
	}

	public void OnDrawGizmos()
	{
		// Canvas border is white
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;
		if (focused)
		{
			// Draw red lines
			Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
			Gizmos.DrawRay((cursor * gridsize) + Vector3.forward * -49999f, Vector3.forward * 99999f);
			Gizmos.DrawRay((cursor * gridsize) + Vector3.right * -49999f, Vector3.right * 99999f);
			Gizmos.DrawRay((cursor * gridsize) + Vector3.up * -49999f, Vector3.up * 99999f);

			// Canvas border is yellow
			Gizmos.color = Color.yellow;
		}

		Gizmos.DrawWireCube(new Vector3((width * gridsize) * 0.5f - gridsize * 0.5f, (height * gridsize) * 0.5f - gridsize * 0.5f, 0f),
			new Vector3(width * gridsize, (height * gridsize), 0f));
	}

	public bool isFocused()
	{
		return focused;
	}

	public void setFocused(bool _focused)
	{
		focused = _focused;
	}

	public void setCursor(Vector3 _cursor)
	{
		cursor = _cursor;
	}

	public Tuile[] getCanvas()
	{
		return tileobs;
	}

	public int getGridsize()
	{
		return gridsize;
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
		GUILayout.Label("Assign a prefab to the tile object");
		GUILayout.Label("Drag        : Paint tiles");
		GUILayout.Label("CLEAN        : Remove tiles outside of the canvas");
		GUILayout.Label("CLEAR        : Delete all tile objects children of the canvas");
		if (GUILayout.Button("CLEAN"))
		{
			me.Clean();
		}
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
			// Round of the cursor
			me.setCursor(me.GridV3(hit.point));
			me.setFocused(true);


			Renderer rend = me.gameObject.GetComponentInChildren<Renderer>();
			if (rend) EditorUtility.SetSelectedRenderState(rend, EditorSelectedRenderState.Wireframe);
			return true;
		}
		me.setFocused(false);
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

