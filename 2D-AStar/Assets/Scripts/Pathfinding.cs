using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarGrid))]
public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AStarGrid grid;
    private void Awake() {
        grid = GetComponent<AStarGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }
    public IEnumerator FindPath(Vector2 startPos, Vector2 targetPos){
        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = true;

        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        if(targetNode.walkable){
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            while(openSet.Count>0){
                
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if(currentNode == targetNode){
                    sw.Stop();
                    UnityEngine.Debug.Log("Path found:" + sw.ElapsedMilliseconds + "MS");
                    pathSuccess = true;
                    break;
                }

                foreach(Node neighbor in grid.GetNeighbors(currentNode)){
                    if((!neighbor.walkable)||(closedSet.Contains(neighbor))){
                        continue;
                    }
                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode,neighbor) + neighbor.movementPenalty;
                    if(newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)){
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if(!openSet.Contains(neighbor)){
                            openSet.Add(neighbor);
                        }
                        else{
                            openSet.UpdateItem(neighbor);
                        }
                    }
                
                }
            }
        }
        
        
        yield return null;
        if(pathSuccess){
            waypoints = RetracePath(startNode,targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints,pathSuccess);
    }

    public void StartFindPath(Vector2 startPos, Vector2 targetPos){
        StartCoroutine(FindPath(startPos,targetPos));
    }

    private Vector2[] RetracePath(Node start, Node end){
        List<Node> path = new List<Node>();
        Node currentNode = end;
        while(currentNode!=start){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }
    
    Vector2[] SimplifyPath(List<Node> path){
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i< path.Count; i++){
            Vector2 directionNew = new Vector2(path[i-1].gridPosX - path[i].gridPosX,path[i-1].gridPosY - path[i].gridPosY);
            if(directionNew!=directionOld){
                waypoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    
    private int GetDistance(Node A, Node B){
        int distanceX = Mathf.Abs(A.gridPosX - B.gridPosX);
        int distanceY = Mathf.Abs(A.gridPosY - B.gridPosY);
        if(distanceX>distanceY){
            return (14 * distanceY) + (10 * (distanceX-distanceY));
        }
        return (14 * distanceX) + (10 * (distanceY-distanceX));
    }
}
