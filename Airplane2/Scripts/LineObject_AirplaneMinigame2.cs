using DG.Tweening;
using GestureRecognizer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObject_AirplaneMinigame2 : MonoBehaviour
{
    public List<GesturePatternDraw> line = new List<GesturePatternDraw>();
    Vector3 scaleLine;
    private bool isBoss = false;
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Idle, anim_DinhDon, anim_BossDinhDon, anim_BossMat1Mang;

    private void Start()
    {
        scaleLine = line[0].transform.localScale;
        if (transform.GetChild(0).GetChild(0).childCount == 5)
        {
            isBoss = true;
        }

        //anim.state.Complete += AnimComplete;
        //PlayAnim(anim, anim_Idle, true);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        //if (trackEntry.Animation.Name == anim_DinhDon || trackEntry.Animation.Name == anim_BossDinhDon)
        //{
        //    PlayAnim(anim, anim_Idle, true);
        //}      
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    public void Sleep()
    {
        if (!isBoss)
        {
            //PlayAnim(anim, anim_DinhDon, false);
        }
        else
        {
            //PlayAnim(anim, anim_BossDinhDon, false);
        }

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

    public void Open()
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
