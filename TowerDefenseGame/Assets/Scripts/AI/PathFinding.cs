using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private bool calculateDiagonals = false;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private CustomGrid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public PathFinding(int width, int height)
    {
        grid = new CustomGrid<PathNode>(width, height, 1f, new Vector3(0,0), (CustomGrid<PathNode> g,int x,int y) => new PathNode(g, x, y));
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var startNode = grid.GetGridObect(startX, startY);
        var endNode = grid.GetGridObect(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode node = grid.GetGridObect(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.ParentNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                return CalculatedPath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(var neighborNode in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighborNode)) continue;

                if(!neighborNode.isWalkable)
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);

                if(tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.ParentNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if(!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }

        }
        // out of nodes
        return null;
    }

    private List<PathNode> GetNeighbors(PathNode currentNode)
    {
        List<PathNode> neighbors = new List<PathNode>();
        if(currentNode.x - 1 >= 0)
        {
            // Left
            neighbors.Add(GetNode(currentNode.x - 1, currentNode.y));

            if(calculateDiagonals)
            {
                // left down
                if(currentNode.y - 1 >= 0)
                {
                    neighbors.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                }

                // left up
                if (currentNode.y + 1 < grid.GetHeight())
                {
                    neighbors.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                }
            }
        }

        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbors.Add(GetNode(currentNode.x + 1, currentNode.y));

            if (calculateDiagonals)
            {
                // left down
                if (currentNode.y - 1 >= 0)
                {
                    neighbors.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                }

                // left up
                if (currentNode.y + 1 < grid.GetHeight())
                {
                    neighbors.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                }
            }
        }

        // down
        if(currentNode.y - 1 >= 0)
        {
            neighbors.Add(GetNode(currentNode.x, currentNode.y - 1));
        }

        if(currentNode.y +1 < grid.GetHeight())
        {
            neighbors.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbors;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObect(x, y);
    }

    private List<PathNode> CalculatedPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.ParentNode != null)
        {
            path.Add(currentNode.ParentNode);
            currentNode = currentNode.ParentNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for(int i = 1; i < pathNodes.Count; i++)
        {
            if(pathNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }

    public CustomGrid<PathNode> GetGrid()
    {
        return grid;
    }
}
