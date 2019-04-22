using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNav : MonoBehaviour
{

    Grid GridComponent;

    private void Awake()
    {
        GridComponent = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        //FindPath(StartPosition.position, TargetPosition.position);
    }

    public List<Node> FindPath(Vector3 PathStartPosition, Vector3 EndPosition)
    {
        Node StartNode = GridComponent.NodeFromWorldPoint(PathStartPosition);
        Node TargetNode = GridComponent.NodeFromWorldPoint(EndPosition);

        List<Node> OpenNodes = new List<Node>();
        List<Node> ClosedNodes = new List<Node>();
        List<Node> OutputPath = new List<Node>();
        OpenNodes.Add(StartNode);

        while (OpenNodes.Count > 0)
        {
            Node templowest = OpenNodes[0]; //make a node with the large default values to basically be a placeholder
                                          //check for the lowest fscore in open
            foreach (Node n in OpenNodes)
            {
                if (n.FScore <= templowest.FScore)
                {
                    if (n.GScore < templowest.GScore) templowest = n; //prefer the lower GScore
                }
            }
            Node CurrentNode = templowest;
            OpenNodes.Remove(CurrentNode);
            ClosedNodes.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                OutputPath = GetFinalPath(StartNode, TargetNode);
            }

            foreach(Node NeighborNode in GridComponent.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.IsWall || ClosedNodes.Contains(NeighborNode))
                {
                    continue;
                }

                int MoveCost = CurrentNode.GScore + CalcDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.GScore || !OpenNodes.Contains(NeighborNode))
                {
                    NeighborNode.GScore = MoveCost;
                    NeighborNode.HScore = CalcDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenNodes.Contains(NeighborNode))
                    {
                        OpenNodes.Add(NeighborNode);
                    }
                }
            }
        }
        return OutputPath;
    }

    List<Node> GetFinalPath(Node StartingNode, Node EndingNode)
    {
        List<Node> path = new List<Node>();
        Node CurrentNode = EndingNode;
        
        while (CurrentNode != StartingNode)
        {
            path.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
        path.Reverse();

        GridComponent.FinalPath = path;
        return path;
    }

    int CalcDistance(Node NodeFrom, Node NodeTo)
    {
        int ix = Mathf.Abs(NodeFrom.GridX - NodeTo.GridX);//x1-x2
        int iy = Mathf.Abs(NodeFrom.GridY - NodeTo.GridY);//y1-y2

        return ix + iy;//Return the sum
    }
}
