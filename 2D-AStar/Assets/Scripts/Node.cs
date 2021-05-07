using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridPosX,gridPosY;

    public int gCost, hCost;
    public Node parent;
    public Node(bool walkable, Vector2 worldPosition, int gridPosX, int gridPosY){
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridPosX = gridPosX;
        this.gridPosY = gridPosY;

    }

    public int fCost{
        get {
            return gCost + hCost;
        }
    }
    
}
