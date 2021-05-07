using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;

    public Node(bool walkable, Vector2 worldPosition){
        this.walkable = walkable;
        this.worldPosition = worldPosition;
    }
    
}
