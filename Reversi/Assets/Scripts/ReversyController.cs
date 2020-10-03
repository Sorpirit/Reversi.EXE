using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GridConverter;

public class ReversyController : MonoBehaviour
{
    [SerializeField] private DiskUIGrid ui;
    [SerializeField] private Camera cam;

    public int[] validMoveIndexes => movesCalculator.ValidMoves.Keys.ToArray();

    
    private ValidMovesCalculator movesCalculator;
    private GameBoard board;

    private DiskType currentAttaker;

    private int counterW = 2;
    private int counterB = 2;

    private bool isCompliteCalculatingMoves;
    private bool isAllAnimationsFinished;

    public Action<DiskType> OnTurnStarted;

    private bool onePlayerCantMove;

    private void Awake()
    {
        board = new GameBoard();

        ui.Init(board.Grid);
        movesCalculator = new ValidMovesCalculator(board.Grid);

        isAllAnimationsFinished = false;
        isCompliteCalculatingMoves = false;

        movesCalculator.OnFinishCalculating += CompliteCaluclatingValidMoves;
        ui.OnAllAnimationsDone += CompliteAllUIAnimations;

        PlaceStartingDisks();
        currentAttaker = DiskType.Black;
        movesCalculator.StartCalculate(currentAttaker, board.Board);
        ui.UpdatePlayerScores(counterB, counterW);
    }

    public bool MakeMove(Vector2 worldPos, DiskType playerType)
    {
        if (currentAttaker != playerType)
            return false;

        int index = board.Grid.WorldPosToIndex(worldPos);
        return MakeMove(index, playerType);
    }
    public bool MakeMove(int index,DiskType playerType)
    {
        if (currentAttaker != playerType)
            return false;

        foreach(int i in movesCalculator.ValidMoves.Keys)
        {
            if(index == i)
            {

                UpdatePlayersScores(index);

                isAllAnimationsFinished = false;
                isCompliteCalculatingMoves = false;

                SetDisk(index, currentAttaker);
                SwitchDisks(i,currentAttaker);
                ui.RemoveAllTemps();
                
                currentAttaker = DiskType.Black == currentAttaker ? DiskType.White : DiskType.Black;

                movesCalculator.StartCalculate(currentAttaker, board.Board);
                return true;
            }
        }
        return false;
    }
    public void ShowAvailabeMoves()
    {
        foreach (int validMove in movesCalculator.ValidMoves.Keys)
        {
            ui.PlaceTemp(validMove);
        }
    }

    private void PlaceStartingDisks()
    {
        int blakIndex1 = board.Grid.CellPosToIndex(new Vector2Int(3, 4));
        int blakIndex2 = board.Grid.CellPosToIndex(new Vector2Int(4, 3));
        int whiteIndex1 = board.Grid.CellPosToIndex(new Vector2Int(3, 3));
        int whiteIndex2 = board.Grid.CellPosToIndex(new Vector2Int(4, 4));

        SetDisk(blakIndex1, DiskType.Black);
        SetDisk(blakIndex2, DiskType.Black);
        SetDisk(whiteIndex1, DiskType.White);
        SetDisk(whiteIndex2, DiskType.White);
    }
    private void FinishGame()
    {
        DiskType winer = counterB > counterW ? DiskType.Black : DiskType.White;
        ui.ShowWinScreen(winer);
    }
    private void BeginTurn()
    {
        if (movesCalculator.ValidMoves.Keys.Count == 0)
        {
            currentAttaker = DiskType.Black == currentAttaker ? DiskType.White : DiskType.Black;
            
            if (onePlayerCantMove)
            {
                FinishGame();
                return;
            }
            else
            {
                onePlayerCantMove = true;
                movesCalculator.StartCalculate(currentAttaker, board.Board);
            }
            return;
        }
        else
        {
            onePlayerCantMove = false;
        }

        ui.SetPlayerLabel(currentAttaker);
        OnTurnStarted?.Invoke(currentAttaker);
    }
    private void UpdatePlayersScores(int moveIndex)
    {
        switch (currentAttaker)
        {
            case DiskType.Black:
                counterB += movesCalculator.ValidMoves[moveIndex].Count + 1;
                counterW -= movesCalculator.ValidMoves[moveIndex].Count;
                break;
            case DiskType.White:
                counterW += movesCalculator.ValidMoves[moveIndex].Count + 1;
                counterB -= movesCalculator.ValidMoves[moveIndex].Count;
                break;
        }
        ui.UpdatePlayerScores(counterB, counterW);
    }

    //Systems calbacks
    private void CompliteCaluclatingValidMoves()
    {
        isCompliteCalculatingMoves = true;

        if (isAllAnimationsFinished)
            BeginTurn();
    }
    private void CompliteAllUIAnimations()
    {
        isAllAnimationsFinished = true;

        if (isCompliteCalculatingMoves)
            BeginTurn();
    }

    //Controls Board&UI
    private void SwitchDisks(int moveIndex,DiskType attacker)
    {
        foreach(int disk in movesCalculator.ValidMoves[moveIndex])
        {
            SetDisk(disk,attacker);
        }
    }
    private void SetDisk(int index,DiskType type)
    {
        board.Board[index] = type;
        ui.SetDisk(index, type);
    }
}
