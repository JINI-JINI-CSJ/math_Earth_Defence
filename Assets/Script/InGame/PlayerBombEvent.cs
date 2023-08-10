using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인게임 플레이어 폭탄

public class PlayerBombEvent : MonoBehaviour
{
    static public   PlayerBombEvent g;

    public  int     played_count;

    private void Awake() {
        g = this;
    }

    public  void Init()
    {
        played_count = 0;
    }


    public  bool    TryPlay()
    {
        if( played_count > 0 ) return false;
        return true;
    }

    // 광고 시청 후
    public  void    OnActive_Play()
    {
        played_count++;
        InGameMain.g._ingame_config.fix_fall_time = 0.7f;
        InGamePlayer.g.AddHP(10);
        InGamePlayer.g.Clear_All_FallObj();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
