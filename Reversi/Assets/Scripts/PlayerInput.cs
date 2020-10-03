using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private ReversyController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private DiskType myType;


    private void Awake()
    {
        if(controller != null)
            controller.OnTurnStarted += OnMoveBegin;
    }

    public void Init(ReversyController controller, Camera cam,DiskType player)
    {
        this.controller = controller;
        this.cam = cam;
        myType = player;
        controller.OnTurnStarted += OnMoveBegin;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            bool result = controller.MakeMove(mousePos,myType);
            if (result)
            {
                Debug.Log("Good moove");
            }
            else
            {
                Debug.Log("Invalid moove");
            }
        }
    }

    private void OnMoveBegin(DiskType player)
    {
        if (myType != player)
            return;

        controller.ShowAvailabeMoves();
    }
}
