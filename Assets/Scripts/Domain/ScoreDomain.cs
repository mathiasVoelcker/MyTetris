using System;
using System.Collections.Generic;
using System.Linq;

public static class ScoreDomain
{
    public static int GetScore(List<Block> blocks)
    {
        var scoreValue = 0;
        var multiplier = 1;
        foreach (var block in blocks)
        {
            var stringValue = block.BlockObject.GetText();
            if (stringValue == "=") multiplier = 2;
            var numberValue = stringValue.ExtractNumber();
            var absoluteValue = Math.Abs(numberValue);
            scoreValue += absoluteValue;
        }
        scoreValue *= multiplier;
        return scoreValue;
    }
}
