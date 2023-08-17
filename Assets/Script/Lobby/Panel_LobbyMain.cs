using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panel_LobbyMain : MonoBehaviour
{
    public google_admob google_Admob;

    public  GameObject  bg;
    public  GameObject  loading;
    public  GameObject grid_Stage;

    private void Awake() {
        bg.SetActive(false);
        loading.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        ME_CSV.Load(this , "OnCSVLoad");

    }


    public  void    OnCSVLoad()
    {
        Init();

        ME_Global.SetFunc_Login_Succ( this , "OnGoogleLogin_Succ" );
        ME_Global.SetFunc_Login_Fail( this , "OnGoogleLogin_Fail" );
        ME_Global.Login_Start();
    }

    public  void    Init()
    {
        bg.SetActive(true);
        loading.SetActive(false);        

        List<int>   lt_stage = new List<int>();
        lt_stage.AddRange( ME_CSV.hs_stage_num );
        lt_stage.Remove(0); // 0 스테이지는 제외 ( 타임 리스 모드 )

        List<_MAKE_STAGE_Inf>   lt_MAKE_STAGE_Inf = new List<_MAKE_STAGE_Inf>();

        foreach( int s in lt_stage )
        {
            _MAKE_STAGE_Inf inst = new _MAKE_STAGE_Inf();
            inst.stage_num = s;
            lt_MAKE_STAGE_Inf.Add(inst);
        }

        List<object> lt_obj = SJ_UnityUI_Util.ListArgToListT( lt_MAKE_STAGE_Inf );
        SJ_UnityUI_Util.ListItem_Add( lt_obj , grid_Stage , "OnItem_Init" );
    }


    public  void    OnGoogleLogin_Succ()
    {
        google_Admob.Init_Request();
    }

    public  void    OnGoogleLogin_Fail()
    {
        google_Admob.Init_Request();
    }


    int load_after_update_count = 0;
    // Update is called once per frame
    void Update()
    {

    }

    public  void    OnClick_StageNum( GameObject go )
    {
        if( ME_CSV.loaded == false ) return;

        UIItem_StageNum uIItem_StageNum =  go.GetComponent<UIItem_StageNum>();
        
        GameObject go_win = SJ_UnityUIMng.OpenPopup("PanelPopup_StageDesc");
        go_win.GetComponent<PanelPopup_StageDesc>().SetStageInf( uIItem_StageNum.stage_num );
    }

    public void OnClick_Config()
    {
        SJ_UnityUIMng.OpenPopup("PanelPopup_Config");
    }
}
