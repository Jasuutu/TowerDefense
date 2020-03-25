using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    PathFinding pathFinding;
    Vector3 startPosition = new Vector3(0, 5);
    // Start is called before the first frame update
    void Start()
    {
        pathFinding = new PathFinding(11, 12, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPositon();
            pathFinding.GetGrid().GetXY(mousePosition, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath((int)startPosition.x, (int)startPosition.y, x, y);

            if(path != null)
            {
                for(int i = 0; i < path.Count - 1; i++)
                    Debug.DrawLine(new Vector3(path[i].x + transform.position.x - pathFinding.GetGrid().GetWidth()/2, path[i].y + transform.position.y - pathFinding.GetGrid().GetHeight()/2) * 1f + Vector3.one * .5f, 
                                    new Vector3(path[i+1].x + transform.position.x - pathFinding.GetGrid().GetWidth()/2, path[i+1].y + transform.position.y - pathFinding.GetGrid().GetHeight()/2) * 1f + Vector3.one * .5f, Color.green, 5);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = GetMouseWorldPositon();
            pathFinding.GetGrid().GetXY(mousePosition, out int x, out int y);
            pathFinding.GetNode(x, y).SetIsWalkable(!pathFinding.GetNode(x, y).isWalkable);
        }
    }

    private Vector3 GetMouseWorldPositon()
    {
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3.z = 0f;
        return v3;
    }
}

