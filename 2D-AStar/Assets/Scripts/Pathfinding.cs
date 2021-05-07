using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarGrid))]
public class Pathfinding : MonoBehaviour
{
    AStarGrid grid;
    public Transform seeker, target;

    private void Update() {
        FindPath(seeker.position,target.position);
    }


    private void Awake() {
        grid = GetComponent<AStarGrid>();
    }
    private void FindPath(Vector2 startPos, Vector2 targetPos){
        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        while(openSet.Count>0){
            Node currentNode = openSet[0];
            for(int i = 1; i<openSet.Count;i++){
                if(openSet[i].fCost<currentNode.fCost || 
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)){
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode){
                RetracePath(startNode,targetNode);
                
                return;
            }

            foreach(Node neighbor in grid.GetNeighbors(currentNode)){
                if((!neighbor.walkable)||(closedSet.Contains(neighbor))){
                    continue;
                }
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode,neighbor);
                if(newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)){
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if(!openSet.Contains(neighbor)){
                        openSet.Add(neighbor);
                    }
                }
            
            }
        }
    }

    private void RetracePath(Node start, Node end){
        List<Node> path = new List<Node>();
        Node currentNode = end;
        while(currentNode!=start){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
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
