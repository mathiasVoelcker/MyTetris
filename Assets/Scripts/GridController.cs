using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//TODO: IMPLEMENT LOGIC FOR ADDING 0 IN ROW -1
//TODO: Score

public class GridController : MonoBehaviour
{
    public static readonly int LINES = 12;
    public static readonly int COLUMNS = 10;

    [SerializeField]
    private ScoreController ScoreController;

    [SerializeField]
    private GameController GameController;

    public Transform[,] Grid = new Transform[LINES, COLUMNS];

    public bool IsHighlightingBlocks { get; private set; } = false;

    public void CheckEquations()
    {
        var blocksToRemove = GridDomain.CheckEquationsHorizontal(Grid);

        if (blocksToRemove.Any())
        {
            GameController.AddScore(blocksToRemove);
            StartCoroutine(RemoveBlocks(blocksToRemove, null, null));
        }
    }

    public void CheckEquationsVertical()
    {
        //for (int c = 0; c < COLUMNS; c++)
        //{
        //    var equation = new Dictionary<int, Transform>();
        //    var blocksToRemove = new Dictionary<int, Transform>();
        //    var equationWithEqOperadtor = false;
        //    int operatorPosition = -1;
        //    int equationStart = -1;
        //    for (int l = 0; l < LINES; l++)
        //    {
        //        var block = Grid[l, c];
        //        if (block == null)
        //        {
        //            //check if equation has operator and if it has any number after the operator
        //            if (equationWithEqOperadtor && Grid[operatorPosition + 1, c] != null && EquationDomain.EquationIsValid(equation, isHorizontal: false))
        //                AddEquationsToRemove(equation, blocksToRemove);

        //            equation = new Dictionary<int, Transform>();
        //            equationWithEqOperadtor = false;
        //            continue;
        //        }
        //        //if block is an equality operator
        //        else if (Spawner.EqOperators.Any(x => x.ToString() == block.GetText()))
        //        {
        //            if (equation.Count == 0) continue; //TODO: IMPLEMENT LOGIC FOR ADDING 0 IN ROW -1
        //            //if equation already has an operator, validate it and disconsider what is left of the first operator, so the next equation can be verified
        //            else if (equationWithEqOperadtor) 
        //            {
        //                if (operatorPosition + 1 < l)
        //                {
        //                    if (EquationDomain.EquationIsValid(equation, isHorizontal: false))
        //                        AddEquationsToRemove(equation, blocksToRemove);
        //                    for (int i = equationStart; i <= operatorPosition; i++) equation.Remove(i);
        //                    //equation.RemoveRange(0, operatorPosition - equationStart + 1);
        //                }
        //            }
        //            else
        //                equationWithEqOperadtor = true;
        //            equation.Add(l, block);
        //            operatorPosition = l;
        //        }
        //        //if block is a number
        //        else
        //        {
        //            if (equation.Count == 0)
        //                equationStart = l;
        //            equation.Add(l, block);
        //        }
        //    }
        //    //check if equation has operator and if it has any number after the operator
        //    if (
        //        equationWithEqOperadtor
        //        && !Spawner.EqOperators.Any(x => x.ToString() == equation.OrderBy(y => y.Key).Last().Value.GetText()) //if the end of the equation is not a operator
        //        && EquationDomain.EquationIsValid(equation, isHorizontal: false)
        //        )
        //        AddEquationsToRemove(equation, blocksToRemove);
        //    if (blocksToRemove.Any())
        //        StartCoroutine(RemoveBlocks(blocksToRemove, null, c));
        //}
    }

    private void AddEquationsToRemove(List<Block> equation, List<Block> toRemoveList)
    {
        foreach(var block in equation)
        {
            if (!toRemoveList.Any(x => x.Column == block.Column && x.Line == block.Line))
                toRemoveList.Add(block);
        }
        //equation = equation.RemoveEqualElements(toRemoveList);
        //toRemoveList.AddRange(equation);
    }

    private IEnumerator RemoveBlocks(List<Block> blocksToRemove, int? line, int? column)
    {
        IsHighlightingBlocks = true;
        foreach (var block in blocksToRemove)
        {
            block.BlockObject.localScale = new Vector3(1.2f, 1.2f, 1);
        }
        yield return new WaitForSeconds(2f);
        IsHighlightingBlocks = false;
        blocksToRemove = blocksToRemove.OrderByDescending(x => x.Line).ToList();
        foreach (var block in blocksToRemove)
        {
            Destroy(block.BlockObject.gameObject);
            block.BlockObject.parent = null;
            Grid[line ?? block.Line, column ?? block.Column] = null;
            DropBlocksAbove(line ?? block.Line, column ?? block.Column);
        }
        FallUnattachedPieces();
        CheckEquations();
    }

    private void DropBlocksAbove(int line, int c, int fallLines = 1)
    {
        for (var l = line + 1; l < LINES; l++)
        {
            if (Grid[l,c] != null)
            {
                FallBlock(l, c);
            }
        }
    }

    void FallUnattachedPieces()
    {
        for (var l = 1; l < LINES; l++)
        {
            for (var c = 0; c < COLUMNS; c++)
            {
                if (Grid[l, c] != null)
                {
                    if ((l + 1 == LINES || Grid[l + 1, c] == null)
                        && Grid[l - 1, c] == null
                        && (c + 1 == COLUMNS || Grid[l, c + 1] == null)
                        && (c == 0 || Grid[l, c - 1] == null))
                        FallBlock(l, c);

                }
            }
        }
    }

    private void FallBlock(int l, int c)
    {
        Grid[l, c].localPosition += new Vector3(0, -60, 0);
        Grid[l - 1, c] = Grid[l, c];
        Grid[l, c] = null;
    }
}
