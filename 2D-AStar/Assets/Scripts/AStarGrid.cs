using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class AStarGrid : MonoBehaviour
{
    #region Basic pathfinding
    private Node[,] grid;
    public BoundsInt gridWorldSize;
    public Vector2 nodeSize;
    public LayerMask collisionMask;

    public Grid tilemapGrid;
    public Tilemap collisionMap;
    #endregion
    #region Weights
    public bool enableWeightedPaths;
    
    /* Enable if your game is of the topdown view. This will change the orientation of the racast to cast
     * into the screen rather than downwards.  
     */
    public bool isTopdown;  
    public TerrainType[] walkableRegions;
    private LayerMask walkableLayers;
    Dictionary<int,int> walkableRegionsDict = new Dictionary<int, int>();
    #endregion

    public bool displayGridGizmos = false;
    private void Awake() {
        tilemapGrid = GetComponent<Grid>();
        nodeSize.x = collisionMap.cellSize.x;
        nodeSize.y = collisionMap.cellSize.y;
        gridWorldSize = collisionMap.cellBounds;
        if(enableWeightedPaths){
            foreach(TerrainType region in walkableRegions){
                walkableLayers.value |= region.terrainMask.value;
                walkableRegionsDict.Add((int)Mathf.Log(region.terrainMask.value,2.0f),region.terrainPenalty);
            }
        }
        CreateGrid();
    }
    private void CreateGrid(){
        grid = new Node[gridWorldSize.size.x,gridWorldSize.size.y];
        Vector2 worldBottomLeft = new Vector2(gridWorldSize.xMin,gridWorldSize.yMin);

        for(int x = 0; x<gridWorldSize.size.x;x++){
            for(int y = 0; y<gridWorldSize.size.y;y++){
                Vector2 worldPosition = worldBottomLeft + Vector2.right * (x*nodeSize.x+(nodeSize.x/2)) + Vector2.up * (y*nodeSize.y+(nodeSize.y/2));
                grid[x,y] = new Node(false, worldPosition,x,y,0);
                
                if(!(collisionMap.HasTile(collisionMap.WorldToCell(worldPosition)))){
                    grid[x,y].walkable = true;
                }
                int movementPenalty = 0;
                
                if(grid[x,y].walkable && enableWeightedPaths){
                   Ray ray = (isTopdown)? new Ray(new Vector3(worldPosition.x,worldPosition.y,5),Vector3.back):new Ray(new Vector3(worldPosition.x,worldPosition.y,0),Vector3.down);
                   Debug.DrawRay(new Vector3(worldPosition.x,worldPosition.y,5),Vector3.back);
                   RaycastHit hit;
                   if(Physics.Raycast(ray,out hit, 10, walkableLayers)){
                       walkableRegionsDict.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                       grid[x,y].movementPenalty = movementPenalty;
                   }
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
[System.Serializable]
public class TerrainType{
    public LayerMask terrainMask;
    public int terrainPenalty;
}