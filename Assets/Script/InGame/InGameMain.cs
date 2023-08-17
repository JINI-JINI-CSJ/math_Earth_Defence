using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMain : MonoBehaviour
{
    static  public  InGameMain g;

    public  Camera  cam_world;
    public  Camera  cam_UI;

    public  FallFormulaMng  fallFormulaMng;
    public  InGamePlayer    inGamePlayer;

    public  float               stagePlayTime = 60.0f;
    public  Panel_InGameInfUI   panel_InGameInfUI;

    public  bool                play;

    [System.Serializable]
    public class _INGAME_CONFIG
    {
        public  float   fix_fall_time = 1.0f;
    }

    public _INGAME_CONFIG   _ingame_config;

    private void Awake() {
        g = this;
        Application.targetFrameRate = 60;
        SJPool.InitMng();
    }

    // Start is called before the first frame update
    void Start()
    {
        ME_CSV.Load( this , "OnLoad_CSV" );
    }

    public  void    OnLoad_CSV()
    {
        Init();
        if( ME_Account.StageNum == 1 )
        {
            SJ_UnityUIMng.OpenPopup( "Panel_Tutorial" );
        }else{
            Play();            
        }
    }

    public  void    Init()
    {
        inGamePlayer.Init();
        fallFormulaMng.Init();

        if( ME_Account.StageNum == 0 )
        {
            panel_InGameInfUI.Start_Play( -1 );
        }else{
            panel_InGameInfUI.Start_Play( stagePlayTime ); 
        }
    }

    public void    Play()
    {
        play = true;
        fallFormulaMng.Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    static public   void    WorldToUI( GameObject tr_w , GameObject ui )
    {
        SJ_Unity.WorldPos_ToScreenPos( g.cam_world , g.cam_UI , tr_w.transform.position , ui.transform );
    }

    static public   void    OnEndPlay( bool clear_stage )
    {
        g._OnEndPlay(clear_stage);
    }    

    public bool stage_clear;
    public   void    _OnEndPlay(bool clear_stage)
    {
        if( play == false ) return;

        stage_clear = clear_stage;
        play = false;
        Time.timeScale = 0;

        InGamePlayer.g.StageResult();

        SJ_UnityUIMng.OpenPopup("Panel_InGameResult");
    }
}
