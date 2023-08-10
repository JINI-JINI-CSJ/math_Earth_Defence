using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_UITween_RectPos : SJ_UITweenBase
{
    public  Vector2 from;
    public  Vector2 to;

    RectTransform   rectTransform;

    void Update()
    {
        FrameMove( Time.deltaTime );
    }

    public override void OnFrameMove()
    {
        Vector2 v = Vector3.Lerp( from , to , ratio_cur );
        if( rectTransform == null )rectTransform = GetComponent<RectTransform>();
        if( rectTransform != null )rectTransform.position = v;
    }
}
