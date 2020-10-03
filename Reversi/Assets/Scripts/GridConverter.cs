using UnityEngine;

public class GridConverter
{
    private int boardWidth;
    private int boardHight;
    private float cellWidth;
    private float cellHight;

    public int CellsCount => boardWidth * boardHight;

    public GridConverter(Vector2Int boardSize,Vector2 cellSize)
    {
        boardWidth = boardSize.x;
        boardHight = boardSize.y;
        cellWidth = cellSize.x;
        cellHight = cellSize.y;
    }

    public Vector2 IndexToWorldPos(int index)
    {
        Vector2Int cellPos = IndexToCelPos(index);
        return new Vector2(cellPos.x * cellWidth, cellPos.y * cellHight) + new Vector2(cellWidth / 2,cellHight/2);
    }
    public Vector2Int IndexToCelPos(int index)
    {
        int row = Mathf.FloorToInt(index / boardWidth);
        int colum = index - row * boardWidth;
        return new Vector2Int(row,colum);
    }
    public int WorldPosToIndex(Vector2 worlPos)
    {
        Vector2Int cellPos = WorldPosToCelPos(worlPos);
        return CellPosToIndex(cellPos);
    }
    public Vector2Int WorldPosToCelPos(Vector2 worlPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worlPos.x / cellWidth), Mathf.FloorToInt(worlPos.y / cellHight));
    }
    public int CellPosToIndex(Vector2Int cellPos)
    {
        return cellPos.x * boardWidth + cellPos.y;
    }
    public int GetCellNeghbourIndex(int cellIndex,NeighbourType type)
    {
        if (cellIndex < 0)
            return -1;

        switch (type)
        {
            case NeighbourType.U:
                return cellIndex >= boardWidth ? cellIndex - boardWidth : -1;
            case NeighbourType.D:
                return cellIndex + boardWidth < CellsCount ? cellIndex + boardWidth : -1;
            case NeighbourType.R:
                return Mathf.FloorToInt(cellIndex/boardWidth) == Mathf.FloorToInt((cellIndex + 1) / boardWidth) ? cellIndex + 1 : -1;
            case NeighbourType.L:
                return Mathf.FloorToInt(cellIndex / boardWidth) == Mathf.FloorToInt((cellIndex - 1) / boardWidth) ? cellIndex - 1 : -1;
            case NeighbourType.UR:
                return GetCellNeghbourIndex(GetCellNeghbourIndex(cellIndex, NeighbourType.R), NeighbourType.U);
            case NeighbourType.UL:
                return GetCellNeghbourIndex(GetCellNeghbourIndex(cellIndex, NeighbourType.L), NeighbourType.U);
            case NeighbourType.DR:
                return GetCellNeghbourIndex(GetCellNeghbourIndex(cellIndex, NeighbourType.R), NeighbourType.D);
            case NeighbourType.DL:
                return GetCellNeghbourIndex(GetCellNeghbourIndex(cellIndex, NeighbourType.L), NeighbourType.D);
        }

        return -1;
    }

    public enum NeighbourType
    {
        U,D,L,R,UL,UR,DL,DR
    }
}
