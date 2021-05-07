using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    float speed = 10f;
    Vector2[] path;
    int targetIndex;

    private void Start() {
        PathRequestManager.RequestPath(transform.position,target.position, OnPathFound);    
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccess){
        if(pathSuccess){
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    IEnumerator FollowPath(){
        Vector2 currentWaypoint = path[0];

        while(true){
            if((Vector2)transform.position == currentWaypoint){
                targetIndex++;
                if(targetIndex >= path.Length){
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            
            transform.position = Vector2.MoveTowards((Vector2)transform.position,currentWaypoint,speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos() {
        if(path != null){
            for(int i = targetIndex; i < path.Length;i++){
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[i],Vector2.one);

                if(i == targetIndex){
                    Gizmos.DrawLine(transform.position,path[i]);
                }
                else{
                    Gizmos.DrawLine(path[i-1],path[i]);
                }
            }
        }
    }
}
