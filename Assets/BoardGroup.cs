using System.Linq;
using UnityEngine;

public class BoardGroup : Group<Board>
{
    private float groupSize;
    [SerializeField] private Board boardPrefab;

    private void Awake()
    {
        groupSize = Initializator.Instance.GroupSize;
    }
    public override void OnBoardPointerExit(Board item)
    {
    }

    public override void OnBoardPointerEnter(Board item)
    {
    }

    public override void OnBoardPointerClick(Board board)
    {
        CameraController.Instance.MoveToBoard(board);
    }

    public (float width, Vector3 gridCenter) GetGridWidthAndCenter(float rows, float columns)
    {
        var singleBoardSize = Items.First().GetBoardSize();

        var verticalSize = singleBoardSize.vertical * (rows - 2);
        var horizontalSize = singleBoardSize.horizontal * columns;

        var horizontalShift = Vector3.right * (horizontalSize / 2);
        var verticalShift = Vector3.forward * (verticalSize / 2);

        var gridCenter = transform.position + horizontalShift - verticalShift;

        return (horizontalSize, gridCenter);
    }

    public void InstantiateMaps()
    {
        float columns = Mathf.Ceil(Mathf.Sqrt(groupSize));

        int currentRow = 0;
        int currentColumn = 0;

        for (int i = 1; i <= groupSize; i++)
        {
            var board = Board.InstantiateBoard(boardPrefab, this);
            board.PlaceBoardOnGrid(currentRow, currentColumn);

            currentColumn++;

            if (i % columns == 0)
            {
                currentRow++;
                currentColumn = 0;
            }
        }
    }

    public void DestroyCars()
    {
        foreach (var board in Items)
        {
            board.DestroyCar();
        }
    }

    public void ResetBoards()
    {
        foreach (var board in Items)
        {
            board.TurnOnBoard();
        }
    }

    public bool AreBoardsDisabled()
    {
        return Items.TrueForAll(x => x.IsDisabled);
    }

    public bool IsAnyBoardHidden()
    {
        return Items.Any(x => x.IsHidden);
    }
}
