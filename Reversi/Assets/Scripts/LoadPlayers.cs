using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayers : MonoBehaviour
{
    [SerializeField] private ReversyController controller;
    [SerializeField] private PlayerInput player;
    [SerializeField] private RandomMovesAI aiPlayer;
    [SerializeField] private Camera cam;

    private void Awake()
    {
        int inPlayType = PlayerPrefs.GetInt("playType");
        
        switch ((PlayType)inPlayType)
        {
            case PlayType.PlayerVzPlayer:
                GameObject player1 = Instantiate(player.gameObject, transform);
                player1.GetComponent<PlayerInput>().Init(controller,cam,DiskType.Black);
                GameObject player2 = Instantiate(player.gameObject, transform);
                player2.GetComponent<PlayerInput>().Init(controller, cam, DiskType.White);
                break;
            case PlayType.PlayerVsAI:
                DiskType playerType = (DiskType) Random.Range(1, 3);


                GameObject player3 = Instantiate(player.gameObject, transform);
                player3.GetComponent<PlayerInput>().Init(controller, cam,playerType);
                GameObject ai1 = Instantiate(aiPlayer.gameObject, transform);
                ai1.GetComponent<RandomMovesAI>().Init(controller, playerType == DiskType.Black ? DiskType.White : DiskType.Black);
                break;
            case PlayType.AIVsAI:
                DiskType ai1Type = (DiskType)Random.Range(1, 3);


                GameObject ai2 = Instantiate(aiPlayer.gameObject, transform);
                ai2.GetComponent<RandomMovesAI>().Init(controller, ai1Type);
                GameObject ai3 = Instantiate(aiPlayer.gameObject, transform);
                ai3.GetComponent<RandomMovesAI>().Init(controller, ai1Type == DiskType.Black ? DiskType.White : DiskType.Black);
                break;
        }
    }

    public enum PlayType
    {
        PlayerVzPlayer,AIVsAI,PlayerVsAI
    }
}
