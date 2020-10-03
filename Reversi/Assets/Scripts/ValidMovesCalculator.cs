using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GridConverter;

public class ValidMovesCalculator
{
    private GridConverter grid;
    private DiskType[] tempBoard;

    public Dictionary<int, HashSet<int>> ValidMoves { get; private set; }
    public Action OnFinishCalculating;

    public ValidMovesCalculator(GridConverter grid)
    {
        this.grid = grid;

        ValidMoves = new Dictionary<int, HashSet<int>>();
    }

    public async void StartCalculate(DiskType attacker, DiskType[] board)
    {
        await Task.Run(() => CalculateValidMoves(attacker,board));
        OnFinishCalculating?.Invoke();
    }

    private void CalculateValidMoves(DiskType attacker,DiskType[] board)
    {
        ValidMoves.Clear();
        tempBoard = board;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != attacker)
                continue;
            AnalyseCellNeigbours(i, attacker);
        }
    }
    private void AnalyseCellNeigbours(int cellIndex, DiskType attacker)
    {
        Neghbour(cellIndex, NeighbourType.U, attacker);
        Neghbour(cellIndex, NeighbourType.D, attacker);
        Neghbour(cellIndex, NeighbourType.L, attacker);
        Neghbour(cellIndex, NeighbourType.R, attacker);
        Neghbour(cellIndex, NeighbourType.UL, attacker);
        Neghbour(cellIndex, NeighbourType.UR, attacker);
        Neghbour(cellIndex, NeighbourType.DR, attacker);
        Neghbour(cellIndex, NeighbourType.DL, attacker);
    }
    private void Neghbour(int cellIndex, NeighbourType type, DiskType attacker)
    {
        int neghbourIndex = grid.GetCellNeghbourIndex(cellIndex, type);
        HashSet<int> flipPath = new HashSet<int>();

        while (neghbourIndex >= 0 && tempBoard[neghbourIndex] != attacker && tempBoard[neghbourIndex] != DiskType.Default)
        {
            flipPath.Add(neghbourIndex);
            neghbourIndex = grid.GetCellNeghbourIndex(neghbourIndex, type);

        }
        if (neghbourIndex == -1 || tempBoard[neghbourIndex] == attacker || flipPath.Count < 1)
            return;

        if (ValidMoves.ContainsKey(neghbourIndex))
            ValidMoves[neghbourIndex].UnionWith(flipPath);
        else
            ValidMoves.Add(neghbourIndex, flipPath);
    }
}
