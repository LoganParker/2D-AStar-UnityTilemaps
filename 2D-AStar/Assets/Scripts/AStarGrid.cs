using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class AStarGrid : MonoBehaviour
{
    private Node[,] grid;
    public BoundsInt gridWorldSize;
    public Vector2 nodeSize;
    public LayerMask collisionMask;

    public Grid tilemapGrid;
    public Tilemap collisionMap;

    public bool displayGridGizmos = false;
    private void Awake() {
        tilemapGrid = GetComponent<Grid>();
        nodeSize.x = collisionMap.cellSize.x;
        nodeSize.y = collisionMap.cellSize.y;
        gridWorldSize = collisionMap.cellBounds;
        CreateGrid();
    }
    private void CreateGrid(){
        grid = new Node[gridWorldSize.size.x,gridWorldSize.size.y];
        Vector2 worldBottomLeft = new Vector2(gridWorldSize.xMin,gridWorldSize.yMin);

        for(int x = 0; x<gridWorldSize.size.x;x++){
            for(int y = 0; y<gridWorldSize.size.y;y++){
                
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x*nodeSize.x+(nodeSize.x/2)) + Vector2.up * (y*nodeSize.y+(nodeSize.y/2));
                //print(worldPoint);
                grid[x,y] = new Node(false, worldPoint,x,y);
                
                if(!(collisionMap.HasTile(collisionMap.WorldToCell(grid[x,y].worldPosition)))){
                    grid[x,y].walkable = true;
                }

            }
        }
    }

    public List<Node> GetNeighbors(Node node){
        List<Node> neighbors = new List<Node>();

        for(int x = -1;x<= 1;x++){
            for(int y = -1;y<=1;y++){
                if(x==0 && y==0){
                    continue;
                }
                int xNeighbor = node.gridPosX + x;
                int yNeighbor = node.gridPosY + y;
                if((xNeighbor >= 0 && xNeighbor < gridWorldSize.size.x) && 
                    (yNeighbor >= 0 && yNeighbor < gridWorldSize.size.y)){
                    neighbors.Add(grid[xNeighbor,yNeighbor]);
                }
            }
        }
        return neighbors;
    }
    
    public Node NodeFromWorldPosition(Vector2 worldPos){
        int x = Mathf.RoundToInt(worldPos.x - 1 + (gridWorldSize.size.x / 2));
        int y = Mathf.RoundToInt(worldPos.y + (gridWorldSize.size.y / 2));
        return grid[x,y];
    }

    public int MaxSize{
        get{
            return gridWorldSize.size.x * gridWorldSize.size.y;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.size.x,gridWorldSize.size.y,0));
        if(grid != null && displayGridGizmos){
            foreach (Node n in grid){
                Gizmos.color = (n.walkable)? Color.white:Color.red;
                Gizmos.DrawCube(n.worldPosition,Vector3.one*(nodeSize.x-0.1f));
            }
        }
    }
}
