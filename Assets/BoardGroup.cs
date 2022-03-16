using System.Collections.Generic;
using UnityEngine;

public class BoardGroup : MonoBehaviour
{
    [HideInInspector] public List<Board> Boards { get; private set; }

    public void Subscribe(Board board)
    {
        if (Boards == null)
        {
            Boards = new List<Board>();
        }

        Boards.Add(board);
    }

    public void OnBoardPointerExit(Board board)
    {

    }

    public void OnBoardPointerEnter(Board board)
    {

    }

    public void OnBoardPointerClick(Board board)
    {

    }
}
