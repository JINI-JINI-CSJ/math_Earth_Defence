using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_SpritePos_Ratio : MonoBehaviour
{
    static  public  SJ_SpritePos_Ratio  g;
    public  Camera  cam_2D;

    private void Awake() {
        g = this;
    }

    static public  void    Pos( Transform tr , Vector2 v2_view )
    {
        Vector3 p = Pos( v2_view );
        tr.position = p;
    }

    static  public Vector3  Pos( Vector2 v2_view  )
    {
        Vector3 p = g.cam_2D.ViewportToWorldPoint( v2_view );
        p.z = 0;
        return p;
    }

}
