using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_FollowLerp : MonoBehaviour
{
    public  GameObject  go_Target;

    //public  float       follow_fix = 1f;

    public  float   move_speed = 1.0f;

    public  float   dec_distance = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 
    }

    private void LateUpdate() 
    {
      Update_Move( Time.deltaTime );
    }



    public  void  Update_Move( float time )
    {
        // float dist = Vector3.Distance( go_Target.transform.position , transform.position );
        // if( dist < 0.01f ) return;
        // Vector3 dir = go_Target.transform.position - transform.position;
        // dir.Normalize();
        // float speed_cur = move_speed;
        // if( dec_distance > dist )
        // {
        //     speed_cur = speed_cur * ((dist / dec_distance)* 0.9f);
        // }
        // transform.Translate( dir*speed_cur*time , Space.World );

        transform.position = Vector3.Slerp( transform.position , go_Target.transform.position , move_speed * time ); 
    }
}
