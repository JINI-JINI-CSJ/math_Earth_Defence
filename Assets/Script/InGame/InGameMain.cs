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
        // string  str = "";
        // int     int_result = 0;
        // ME_CSV.Get_CalcFormula( ref str , ref int_result );
        // Debug.Log( "받은 결과 : " + int_result );

        ME_CSV.Load( this , "OnLoad_CSV" );
    }

    public  void    OnLoad_CSV()
    {
        Init();
        Play();
    }

    public  void    Init()
    {
        inGamePlayer.Init();
        fallFormulaMng.Init();
    }

    public void    Play()
    {
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

    
}
