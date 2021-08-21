using DG.Tweening;
using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObject_AirplaneMinigame2 : MonoBehaviour
{
    public List<GesturePatternDraw> line = new List<GesturePatternDraw>();
    Vector3 scaleLine;
    private bool isBoss = false;

    private void Start()
    {
        scaleLine = line[0].transform.localScale;
        if (transform.GetChild(0).GetChild(0).childCount == 5)
        {
            isBoss = true;
        }
    }

    public void Open()
    {
        int j = 0;
        while (!line[j].IsActive())
        {
            j++;
        }
        line[j].transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() => 
        {
            line[j].gameObject.SetActive(false);
            if (!isBoss)
            {
                line.RemoveAt(0);
            }
        });
    }

    public void Sleep()
    {
        int j = 0;
        while (line[j].IsActive())
        {
            j++;
        }
        line[j].gameObject.SetActive(true);
        line[j].transform.DOScale(scaleLine, 0.5f).SetEase(Ease.OutCubic);
    }
}
