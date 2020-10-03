using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateBoard : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 180), 10f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.InOutBack);
    }
}
