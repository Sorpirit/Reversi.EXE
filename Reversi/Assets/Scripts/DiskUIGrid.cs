using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DiskUIGrid : MonoBehaviour
{
    [SerializeField] private GameObject diskInst;

    [SerializeField] private GameObject temp;
    [SerializeField] private TMP_Text playerLabel;
    [SerializeField] private TMP_Text whitePlayerS;
    [SerializeField] private TMP_Text blackPlayerS;
    [SerializeField] private TMP_Text winMessege;
    [SerializeField] private Transform diskParent;
    [SerializeField] private Transform tempParent;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Transform blackDisksStartingPoint;
    [SerializeField] private Transform whiteDisksStartingPoint;

    private DiskUI[] disks;
    private GridConverter grid;

    public Action OnAllAnimationsDone;

    private int animsCount;
    private Sequence animationS;

    public void Init(GridConverter grid)
    {
        this.grid = grid;
        disks = new DiskUI[grid.CellsCount];
        animationS = DOTween.Sequence();
    }

    public void SetDisk(int index, DiskType type)
    {
        if (index >= disks.Length || index < 0)
            return;

        if (disks[index] == null)
        {
            Place(index, type);
        }
        else
        {
            SwitchDisk(index, type);
        }
    }
    public void PlaceTemp(int index)
    {
        GameObject disk = Instantiate(temp, grid.IndexToWorldPos(index), Quaternion.identity,tempParent);
    }
    public void RemoveAllTemps()
    {
        foreach(Transform temp in tempParent)
        {
            Destroy(temp.gameObject);
        }
    }
    public void SetPlayerLabel(DiskType type)
    {
        switch (type)
        {
            case DiskType.Black:
                playerLabel.text = "Blacks turn";
                break;
            case DiskType.White:
                playerLabel.text = "Whites turn";
                break;
        }
    }
    public void UpdatePlayerScores(int blackP, int whiteP)
    {
        blackPlayerS.text = blackP.ToString();
        whitePlayerS.text = whiteP.ToString();
    }
    public void ShowWinScreen(DiskType type)
    {
        winMessege.text = type == DiskType.Black ? "Black wins" : "White wins";
        winScreen.SetActive(true);
    }

    private void Place(int index, DiskType type = DiskType.White)
    {
        GameObject disk = Instantiate(diskInst, Vector3.zero, Quaternion.identity, diskParent);

        disks[index] = disk.GetComponent<DiskUI>();
        switch (type)
        {
            case DiskType.Black:
                disks[index].FlipToBlack();
                disk.transform.position = blackDisksStartingPoint.position;
                break;
            case DiskType.White:
                disks[index].FlipToWhite();
                disk.transform.position = whiteDisksStartingPoint.position;
                break;
        }

        animsCount++;
        if (!animationS.active)
            animationS = DOTween.Sequence();
        animationS.Prepend(disk.transform.DOMove(grid.IndexToWorldPos(index), .5f).OnComplete(() => DecriptAnimation()));
    }
    private void SwitchDisk(int index, DiskType type)
    {
        switch (type)
        {
            case DiskType.Black:
                DoFlipAnimationBlack(index);
                break;
            case DiskType.White:
                DoFlipAnimationWhite(index);
                break;
        }
    }

    private void DoFlipAnimationBlack(int index)
    {
        animsCount++;
        Sequence flipAnimBlack = DOTween.Sequence();
        flipAnimBlack.Append(disks[index].transform.DORotate(new Vector3(0, 90), .3f));
        flipAnimBlack.AppendCallback(() => disks[index].FlipToBlack());
        flipAnimBlack.Append(disks[index].transform.DORotate(new Vector3(0, 180), .3f));
        flipAnimBlack.AppendCallback(() => {
            disks[index].transform.rotation = Quaternion.identity;
            DecriptAnimation();
        });
        if (!animationS.active)
            animationS = DOTween.Sequence();
        animationS.Append(flipAnimBlack);
    }
    private void DoFlipAnimationWhite(int index)
    {
        animsCount++;
        Sequence flipAnimWhite = DOTween.Sequence();
        flipAnimWhite.Append(disks[index].transform.DORotate(new Vector3(0, 90), .3f));
        flipAnimWhite.AppendCallback(() => disks[index].FlipToWhite());
        flipAnimWhite.Append(disks[index].transform.DORotate(new Vector3(0, 180), .3f));
        flipAnimWhite.AppendCallback(() => {
            disks[index].transform.rotation = Quaternion.identity;
            DecriptAnimation();
        });
        if (!animationS.active)
            animationS = DOTween.Sequence();
        animationS.Append(flipAnimWhite);
    }

    private void DecriptAnimation()
    {
        animsCount--;
        if(animsCount <= 0)
        {
            animsCount = 0;
            OnAllAnimationsDone?.Invoke();
        }
    }
    
}
