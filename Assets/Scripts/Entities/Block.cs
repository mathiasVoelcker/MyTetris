using System;
using UnityEngine;

public class Block
{
    public Transform BlockObject { get; set; }

    public int Line { get; set; }

    public int Column { get; set; }

    public Block(Transform blockObject, int line, int column)
    {
        BlockObject = blockObject;
        Line = line;
        Column = column;
    }
}
