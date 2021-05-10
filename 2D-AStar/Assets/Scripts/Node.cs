using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    #region Pathfinding Base
    public bool walkable;
    public Vector2 worldPosition;
    public int gridPosX,gridPosY;
    public int gCost, hCost;
    public Node parent;
    #endregion
    #region Heap
    private int heapIndex;
    #endregion
    #region Weights
    public int movementPenalty;
    #endregion

    public Node(bool walkable, Vector2 worldPosition, int gridPosX, int gridPosY){
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridPosX = gridPosX;
        this.gridPosY = gridPosY;
        this.movementPenalty = 0;

    }
    public Node(bool walkable, Vector2 worldPosition, int gridPosX, int gridPosY, int movementPenalty){
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridPosX = gridPosX;
        this.gridPosY = gridPosY;
        this.movementPenalty = movementPenalty;
    }

    public int fCost{
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex{
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }
    
    public int CompareTo(Node nodeToCompare){
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0){
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare; 
    }
}
