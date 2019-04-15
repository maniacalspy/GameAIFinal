using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool IsWall;
    public Node Parent;
    public Vector3 position;
    public int GScore;
    public int HScore;
    public int FScore { get { return GScore + HScore; } }
    public int GridX;
    public int GridY;

    public Node(bool IsAWall, Vector3 Pos, int ThisGridX, int ThisGridY)
    {
        IsWall = IsAWall;
        position = Pos;
        GridX = ThisGridX;
        GridY = ThisGridY;
    }
}
