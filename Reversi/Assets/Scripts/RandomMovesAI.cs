using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovesAI : MonoBehaviour
{
    [SerializeField] private ReversyController controller;
    [SerializeField] private DiskType myType;


    private void Awake()
    {
        if (controller == null)
            return;

        controller.OnTurnStarted += (type) => {
            if (myType == type) 
                StartCoroutine(MakeMoveInTime(0.5f));
        };
    }

    public void Init(ReversyController controller,DiskType player)
    {
        this.controller = controller;
        myType = player;
        controller.OnTurnStarted += (type) => {
            if (myType == type)
                StartCoroutine(MakeMoveInTime(0.5f));
        };
    }

    private IEnumerator MakeMoveInTime(float secods)
    {
        Debug.Log("Start2");
        yield return new WaitForSeconds(secods);
        TakeTurn();
    }

    private void TakeTurn()
    {
        Debug.Log("Start");
        int[] moveIndexes = controller.validMoveIndexes;

        controller.MakeMove(moveIndexes[Random.Range(0, moveIndexes.Length)], myType);
    }
}
