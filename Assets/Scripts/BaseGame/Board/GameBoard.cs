using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoard : MonoBehaviour
{
    public Cell[] _cells;

    public TextMeshProUGUI _token;
    public Button _newGame;
    public Draw _draw;
    public Victory _victory;

    private string[] _tokens = new string[] { "X", "0" };
    private int _currentToken = 0;

    private bool _playing = false;

    private GameEngine _engine = new GameEngine();

    void Awake()
    {
        foreach (var cell in _cells)
        {
            cell.Click += (o, e) =>
            {
                var index = Array.IndexOf(_cells, o);
                Clicked(index);
            };
            cell.SetText("");
        }
    }

    public void NewGame()
    {
        _currentToken = 0;
        _token.text = _tokens[_currentToken];
        _newGame.interactable = false;
        _playing = true;
        _engine.NewGame();

        foreach (var cell in _cells)
        {
            cell.SetText("");
            cell.IsEnabled = true;
        }
    }

    private void Clicked(int index) // Clicked/MadeMove
    {
        if (!_playing)
        {
            return;
        }

        //print(index);
        _cells[index].SetText(_tokens[_currentToken]);
        _currentToken++;
        _currentToken %= 2;
        _token.text = _tokens[_currentToken];

        _engine.Place(index);
        _cells[index].IsEnabled = false;

        // Check for Victory
        var winner = _engine.IsVictory();
        if (winner == -1)
        {
            DrawGame();
        }
        else if (winner > 0)
        {
            WinnerIs(winner - 1);
        }

    }

    private void WinnerIs(int id)
    {
        EndOfGame();
        _victory.Show(_tokens[id]);
    }


    private void DrawGame()
    {
        EndOfGame();
        _draw.Show();
    }

    private void EndOfGame()
    {
        _newGame.interactable = true;
        foreach (var cell in _cells)
        {
            cell.IsEnabled = false;
        }
    }

}
