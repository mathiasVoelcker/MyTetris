using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private GameObject ScoreText;

    public int AddScore(int score)
    {
        var text = ScoreText.gameObject.GetComponent<Text>().text;
        var newScore = int.Parse(text) + score;
        ScoreText.gameObject.GetComponent<Text>().text = "" + newScore;

        return newScore;
    }
}
