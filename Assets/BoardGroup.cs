using System.Collections.Generic;
using UnityEngine;

public class BoardGroup : MonoBehaviour
{
    private List<Board> boards;

    public void Subscribe(Board board)
    {
        if (boards == null)
        {
            boards = new List<Board>();
        }

        boards.Add(board);
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
