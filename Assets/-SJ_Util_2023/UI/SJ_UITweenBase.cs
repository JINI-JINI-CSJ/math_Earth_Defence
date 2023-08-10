using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_UITweenBase : MonoBehaviour
{

    public  float   PlayTime = 1.0f;
    public  bool    PingPong = true;

    public  bool    OncePlay = false;

    public  bool    play;    
    [HideInInspector]
    public  float   time_cur;
    [HideInInspector]
    public  bool    reverse_cur;
    [HideInInspector]
    public  float   ratio_cur;
    public  AnimationCurve  curve;

    public  _SJ_GO_FUNC     end_recv;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FrameMove( Time.deltaTime );
    }

    public  void    Play( bool force_start = false )
    {
        if( play && force_start == false ) return;
        play = true;
        time_cur = 0;
        ratio_cur = 0;
        reverse_cur = false;
        gameObject.SetActive(true);
    }

    public  void    Stop()
    {
        play = false;
        time_cur = 0;
        ratio_cur = 0;
        reverse_cur = false;
        OnFrameMove();
    }

    public  void    FrameMove( float t )
    {
        if(play == false) return;
        time_cur += t;
        bool loop_end = false;
        if( time_cur >= PlayTime )
        {
            time_cur -= PlayTime;
            if(PingPong)
            {
                reverse_cur = !reverse_cur;
                if( reverse_cur == false ) 
                {
                    loop_end = true;
                    end_recv.Func();
                }
            }else{
                loop_end = true;
                end_recv.Func();
            }
        }
        float r = time_cur / PlayTime;
        if( reverse_cur ) r = 1.0f - r;
        ratio_cur = curve.Evaluate(r);

        OnFrameMove();

        if( loop_end && OncePlay )
        {
            Stop();
        }
    }

    virtual public  void    OnFrameMove(){}
}
