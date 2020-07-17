using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextPiecePanel;

    [SerializeField]
    public GameObject _gamePanel;

    [SerializeField]
    private GridController _gridController;

    public List<Piece> Pieces;

    public static List<char> EqOperators = new List<char>() { '<', '>' };

    public static List<int> Numbers = new List<int> { -3, -2, -1, 0, 1, 2, 3 };

    private Piece _nextPiece;


    void Start()
    {
        //_gridController = (new GameObject("GridController")).AddComponent<GridController>();
        _nextPiece = CreateNextPiece();
        SummonNextPiece();
    }

    public void AddOperator(char op)
    {
        EqOperators.Add(op);
    }

    public void AddNumber(int i)
    {
        Numbers.Add(i);
        Numbers.Add(-i);
    }

    public void SummonNextPiece()
    {
        _nextPiecePanel.transform.DetachChildren();
        var spawnPosition = transform.localPosition;
        if (_nextPiece.transform.name.Contains("O") || _nextPiece.transform.name.Contains("I"))
        {
            spawnPosition.x += 30f;
            spawnPosition.y += 30f;
        }
        _nextPiece.transform.SetParent(_gamePanel.transform, false);
        _nextPiece.transform.localPosition = spawnPosition;
        _nextPiece.transform.localScale = new Vector3(1, 1, 1);
        _nextPiece.StartFalling();
        _nextPiece = CreateNextPiece();
    }

    private Piece CreateNextPiece()
    {
        var rndPiece = GetRndPiece();
        var piece = Instantiate(rndPiece);
        piece.transform.localPosition = new Vector3(0, 0, 0);
        piece.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        piece.transform.SetParent(_nextPiecePanel.transform, false);
        if (piece.name.Contains("T"))
            SetTValues(piece);
        else if (piece.name.Contains("I"))
            SetIValues(piece);
        else if (piece.name.Contains("L"))
            SetLJValues(piece);
        else if (piece.name.Contains("J"))
            SetLJValues(piece);
        else
            SetPieceValues(piece);

        piece.SetSpawner(this);
        piece.SetGridController(_gridController);
        //_nextPiecePanel.transform.DetachChildren();
        return piece;
    }

    private Piece GetRndPiece()
    {
        var rndPiece = Pieces[Random.Range(0, Pieces.Count)];
        //var rndPiece = Pieces[3];
        var spawnPosition = transform.position;
        if (rndPiece.name == "O")
        {
            spawnPosition.x += 0.5f;
            spawnPosition.y += 0.5f;
        }
        return rndPiece;
    }

    private void SetPieceValues(Piece piece)
    {
        CharType charType = GetRndChartype();
        
        foreach (Transform child in piece.transform)
        {
            SetRndValueToBlock(child, charType);
            if (charType == CharType.Operator) charType = CharType.Number;
            else charType = GetRndChartype();
        }
    }

    private void SetTValues(Piece piece)
    {
        
        for (int i = 0; i < 4; i++)
        {
            if (i == 1)
                SetRndValueToBlock(piece.transform.GetChild(i), CharType.Number);
            else
            {
                CharType charType = GetRndChartype();
                SetRndValueToBlock(piece.transform.GetChild(i), charType);
            }
        }
    }

    private void SetIValues(Piece piece)
    {

        for (int i = 0; i < 4; i++)
        {
            if (i == 1 || i == 2)
                SetRndValueToBlock(piece.transform.GetChild(i), CharType.Number);
            else
            {
                CharType charType = GetRndChartype();
                SetRndValueToBlock(piece.transform.GetChild(i), charType);

            }
        }
    }

    private void SetLJValues(Piece piece)
    {

        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
                SetRndValueToBlock(piece.transform.GetChild(i), CharType.Number);
            else
            {
                CharType charType = GetRndChartype();
                SetRndValueToBlock(piece.transform.GetChild(i), charType);

            }
        }
    }

    private void SetRndValueToBlock(Transform block, CharType charType)
    {
        var textComponent = block.GetTextTransform();
        string value;

        if (charType == CharType.Operator)
        {
            value = EqOperators[Random.Range(0, EqOperators.Count)].ToString();
        }
        else
        {
            var number = Numbers[Random.Range(0, Numbers.Count)];
            if (number > 0) value = $"+{number}";
            else value = $"{number}";
        }

        textComponent.gameObject.GetComponent<Text>().text = value;
    }

    private CharType GetRndChartype()
    {
        return Random.Range(1, 4) > 2 ? CharType.Operator : CharType.Number;
    }
}

public enum CharType
{
    Number = 1,
    Operator = 2,
}
