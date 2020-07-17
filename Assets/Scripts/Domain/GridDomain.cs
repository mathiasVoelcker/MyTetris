using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GridDomain
{
    public static readonly int LINES = 12;
    public static readonly int COLUMNS = 10;
    public static Transform[,] Grid = new Transform[LINES, COLUMNS];

    public static List<Block> CheckEquationsHorizontal(Transform[,] Grid)
    {
        var blocksToRemove = new List<Block>();
        for (int l = 0; l < LINES; l++)
        {
            var equation = new List<Block>();
            var equationWithEqOperadtor = false;
            int operatorPosition = -1;
            int equationStart = -1;
            for (int c = 0; c < COLUMNS; c++)
            {
                var block = Grid[l, c];
                if (block == null)
                {
                    //check if equation has operator and if it has any number after the operator
                    if (equationWithEqOperadtor && Grid[l, operatorPosition + 1] != null && EquationDomain.EquationIsValid(equation, isHorizontal: true))
                        AddEquationsToRemove(equation, blocksToRemove);

                    equation = new List<Block>();
                    equationWithEqOperadtor = false;
                    continue;
                }
                //if block is an equality operator
                else if (Spawner.EqOperators.Any(x => x.ToString() == block.GetText()))
                {
                    if (equation.Count == 0) continue; //TODO: IMPLEMENT LOGIC FOR ADDING 0 IN ROW -1
                    //if equation already has an operator, validate it and disconsider what is left of the first operator, so the next equation can be verified
                    else if (equationWithEqOperadtor)
                    {
                        if (operatorPosition + 1 < c)
                        {
                            if (EquationDomain.EquationIsValid(equation, isHorizontal: true))
                                AddEquationsToRemove(equation, blocksToRemove);
                            for (int i = equationStart; i <= operatorPosition; i++)
                                equation.Remove(equation.Find(x => x.Column == i && x.Line == l));
                            //equation.RemoveRange(0, operatorPosition - equationStart + 1);
                        }
                    }
                    else
                        equationWithEqOperadtor = true;
                    equation.Add(new Block(block, l, c));
                    operatorPosition = c;
                }
                //if block is a number
                else
                {
                    if (equation.Count == 0)
                        equationStart = c;
                    equation.Add(new Block(block, l, c));
                }
            }

            //check if equation has operator and if it has any number after the operator
            if (
                equationWithEqOperadtor
                && !Spawner.EqOperators.Any(x => x.ToString() == equation.OrderBy(y => y.Column).Last().BlockObject.GetText()) //if the end of the equation is not a operator
                && EquationDomain.EquationIsValid(equation, isHorizontal: true)
                )
                AddEquationsToRemove(equation, blocksToRemove);
        }
        return blocksToRemove;
    }

    private static void AddEquationsToRemove(List<Block> equation, List<Block> toRemoveList)
    {
        foreach (var block in equation)
        {
            if (!toRemoveList.Any(x => x.Column == block.Column && x.Line == block.Line))
                toRemoveList.Add(block);
        }
        //equation = equation.RemoveEqualElements(toRemoveList);
        //toRemoveList.AddRange(equation);
    }
}
