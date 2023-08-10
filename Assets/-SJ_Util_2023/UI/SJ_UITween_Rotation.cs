using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_UITween_Rotation : SJ_UITweenBase
{
    public  Vector3 from;
    public  Vector3 to;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FrameMove( Time.deltaTime );
    }

    public override void OnFrameMove()
    {
        Vector3 v = Vector3.Lerp( from , to , ratio_cur );
        transform.localRotation = Quaternion.Euler( v.x , v.y , v.z );
    }
}
