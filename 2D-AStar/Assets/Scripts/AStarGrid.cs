using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class AStarGrid : MonoBehaviour
{

    private Node[,] grid;
    public BoundsInt gridWorldSize;
    public Vector2 nodeRadius;
    public LayerMask collisionMask;

    public Grid tilemapGrid;
    public Tilemap collisionMap;

    private void Start() {
        tilemapGrid = GetComponent<Grid>();
        nodeRadius.x = collisionMap.cellSize.x;
        nodeRadius.y = collisionMap.cellSize.y;
        gridWorldSize = collisionMap.cellBounds;
        CreateGrid();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position,new Vector2(gridWorldSize.x,gridWorldSize.y));    
    }


    private void CreateGrid(){
        grid = new Node[gridWorldSize.size.x,gridWorldSize.size.y];
        
        //Test with bounds after
        Vector2 worldBottomLeft = transform.position - Vector3.right*gridWorldSize.size.x/2 - Vector3.up * gridWorldSize.size.y/2;
          
        for(int x = 0; x<gridWorldSize.size.x;x++){
            for(int y = 0; y<gridWorldSize.size.y;y++){

            }
        }

    }
}
