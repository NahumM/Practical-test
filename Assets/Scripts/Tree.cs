using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    private List<GameObject> trunks = new List<GameObject>();

    public void AddTrunkToTree(GameObject trunk)
    {
        trunks.Add(trunk);
    }

   public GameObject GetTrunkFromTree(int i)
    {
        return trunks[i];
    }

    public List<GameObject> GetTreeTrunks()
    {
        return trunks;
    }
}
