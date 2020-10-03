using UnityEngine;

public class GameBoard
{

    public DiskType[] Board { get; private set; }
    public GridConverter Grid { get; private set; }

    public GameBoard()
    {
        SetupBoard();
    }

    private void SetupBoard()
    {
        Vector2Int boardSize = Vector2Int.one * 8;
        Vector2 cellSize = Vector2.one;

        Grid = new GridConverter(boardSize, cellSize);
        Board = new DiskType[Grid.CellsCount];

        FillBoardDefeult();
    }
    private void FillBoardDefeult()
    {
        for (int i = 0; i < Board.Length; i++)
        {
            Board[i] = DiskType.Default;
        }
    }
}
