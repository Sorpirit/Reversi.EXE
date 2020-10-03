using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private Sprite blakDisk;
    [SerializeField] private Sprite whiteDisk;

    public void FlipToBlack()
    {
        sprite.sprite = blakDisk;
    }

    public void FlipToWhite()
    {
        sprite.sprite = whiteDisk;
    }
}
