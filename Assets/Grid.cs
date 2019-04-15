using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform StartPosition;
    public LayerMask ObstacleMask;
    public Vector2 GridSize;
    public float NodeRadius;
    public float DistBetweenNodes;

    Node[,] Nodes;
    public List<Node> FinalPath;

    float NodeDiameter;
    int GridSizeX, GridSizeY;
    // Start is called before the first frame update
    void Start()
    {
        NodeDiameter = NodeRadius * 2;
        GridSizeX = Mathf.RoundToInt(GridSize.x / NodeDiameter);
        GridSizeY = Mathf.RoundToInt(GridSize.y / NodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        Nodes = new Node[GridSizeX, GridSizeY];
        Vector3 BottomLeft = transform.position - Vector3.right * GridSize.x / 2 - Vector3.forward * GridSize.y / 2;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 WorldPoint = BottomLeft + Vector3.right * (x * NodeDiameter + NodeRadius) + Vector3.forward * (y * NodeDiameter + NodeRadius);
                bool Wall = true;
                if (Physics.CheckSphere(WorldPoint, NodeRadius, ObstacleMask))
                {
                    Wall = false;
                }
                Nodes[x, y] = new Node(Wall, WorldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.GridX + 1;
        icheckY = a_NeighborNode.GridY;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodes[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.GridX - 1;
        icheckY = a_NeighborNode.GridY;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodes[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.GridX;
        icheckY = a_NeighborNode.GridY + 1;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodes[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.GridX;
        icheckY = a_NeighborNode.GridY - 1;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodes[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float xPos = ((a_vWorldPos.x + GridSize.x / 2) / GridSize.x);
        float yPos = ((a_vWorldPos.z + GridSize.y / 2) / GridSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((GridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((GridSizeY - 1) * yPos);

        return Nodes[x, y];
    }


    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, 1, GridSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (Nodes != null)//If the grid is not empty
        {
            foreach (Node n in Nodes)//Loop through every node in the grid
            {
                if (n.IsWall)//If the current node is a wall node
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.yellow;//Set the color of the node
                }


                if (FinalPath != null)//If the final path is not empty
                {
                    if (FinalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                    }

                }


                Gizmos.DrawCube(n.position, Vector3.one * (NodeDiameter - DistBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
}
