using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuile : MonoBehaviour
{
    public string tilename;

    public Tuile(string _name)
    {
        tilename = _name;
    }
}

public class TuileEqualityComparer : IEqualityComparer<Tuile>
{
    public bool Equals(Tuile t1, Tuile t2)
    {
        if (t1 == null && t2 == null)
            return true;
        else if (t1 == null || t2 == null)
            return false;
        else
            return t1.gameObject.name.Equals(t2.gameObject.name);
    }

    public int GetHashCode(Tuile tuile)
    {
        return tuile.gameObject.name.GetHashCode();
    }
}
