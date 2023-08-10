using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public SJ_TransUpdate   transUpdate;

    public  Vector3         pos_src;
    public  Vector3         pos_tar;

    public  float           move_time = 0.3f;
    public  float           move_time_cur;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Update_Pos();
    }

    public  void    MoveStart( InGamePlayer player , Transform tr_target )
    {
        //transUpdate.SetTargetMode( true , null , 0.3f , tr_target.position , gameObject , "OnEnd_Move" );

        move_time_cur = 0;
        pos_src = player.transform.position;
        pos_tar = tr_target.position;
    }

    public  void    Update_Pos()
    {
        move_time_cur += Time.deltaTime;
        float r = move_time_cur / move_time;
        transform.position = Vector3.Lerp( pos_src , pos_tar , r );
        if( r >= 1.0f )
        {
            OnEnd_Move();
        }
    }

    public  void    OnEnd_Move()
    {
        SJPool.ReturnInst( gameObject );
    }

}
