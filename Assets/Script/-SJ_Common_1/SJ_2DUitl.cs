using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_2DUitl 
{
    static public Quaternion Rotation_Target( Transform tr_src , Transform tr_target )
    {
        Vector3 v_dir = tr_target.position - tr_src.position;

        v_dir.z = 0;
        v_dir.Normalize();
        float ang = SJ_Cood.GetAngle( Vector3.up , v_dir , _XYZ.Z );
        Quaternion qt = Quaternion.Euler( 0,0,ang );
        tr_src.localRotation = qt;
        return qt;
    }


}
