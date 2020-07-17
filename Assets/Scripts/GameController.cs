using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private ScoreController ScoreController;

    [SerializeField]
    private LevelController LevelController;

    [SerializeField]
    private Spawner SpawnerController;

    internal void AddScore(List<Block> blocksToRemove)
    {
        var score = ScoreDomain.GetScore(blocksToRemove);
        var newScore = ScoreController.AddScore(score);
        if (newScore >= LevelController.ScoreToNextLevel)
        {
            var newLevel = LevelController.AddLevel();
            if (newLevel == 2)
                SpawnerController.AddNumber(4);
            if (newLevel == 3)
                SpawnerController.AddOperator('=');
            if (newLevel == 4)
                SpawnerController.AddNumber(5);
        }
    }
}
