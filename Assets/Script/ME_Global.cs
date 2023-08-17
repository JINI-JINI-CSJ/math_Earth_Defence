using System.Collections;
using System.Collections.Generic;
using System.Text;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class ME_Global : SJ_Singleton_Mono
{
    static  public  ME_Global g;
	override		public	SJ_Singleton_Mono	OnGetStatic() {return g; }
	override		public	void				OnSetStatic(SJ_Singleton_Mono s) { g = (ME_Global)s; }

    //public  string  url;

    public  SJ_GoogleLogin  sJ_GoogleLogin;


    public  void    Reg_Func()
    {
        this.sJ_GoogleLogin.OnSavedGameDataReadComplete = (status, bytes) => {
            if (status == SavedGameRequestStatus.Success)
            {
                string strCloudData = null;
 
                if (bytes.Length == 0)
                {
                    // strCloudData = string.Empty;
                    // txtCloudResult.text = "로드 성공, 데이터 없음";
                    Debug.Log("로드 성공, 데이터 없음");
                }
                else
                {
                    // strCloudData = Encoding.UTF8.GetString(bytes);
                    // this.gameInfo = JsonConvert.DeserializeObject<GameInfo>(strCloudData);
                    // this.txtNormalMonsterKillCount.text = this.gameInfo.normalMonsterKillCount.ToString();
                    // this.txtEliteMonsterKillCount.text = this.gameInfo.eliteMonsterKillCount.ToString();
                    // this.txtBossKillCount.text = this.gameInfo.killBossCount.ToString();
                    // this.txtHighScore.text = this.gameInfo.highScore.ToString();
                    // this.txtScore.text = this.score.ToString();
                    // txtCloudResult.text = "클라우드로부터 데이터를 불러왔습니다.";

                    strCloudData = Encoding.UTF8.GetString(bytes);
                    Debug.Log(strCloudData);

                    //ME_Account.user_Save = JsonUtility.FromJson<_User_Save>(strCloudData);
                    ME_Account.FromJson( strCloudData );

                    Debug.Log("로드 성공");
                }
            }
            else {
                //txtCloudResult.text = string.Format("로드 실패 : {0}", status);
                Debug.Log("로드 실패 :" + status);
            }
        };        

        this.sJ_GoogleLogin.OnSavedGameDataWrittenComplete = (status, game) => {
            if (status == SavedGameRequestStatus.Success)
            {
                //txtCloudResult.text = "클라우드에 저장 하였습니다.";
                Debug.Log("클라우드에 저장 하였습니다.");
            }
        };
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public  void    Login_Start()
    {
        g.Reg_Func();

        //g.sJ_GoogleLogin.func_data_Load_succ.SetMono( g , "OnLoad_PlayerData" );

        g.sJ_GoogleLogin.Init();
        g.sJ_GoogleLogin.GPGSLogin();


    }

    static  public  void    SetFunc_Login_Succ( MonoBehaviour mono , string func )
    {
        g.sJ_GoogleLogin.func_login_OK.SetMono( mono , func );
    }

    static public  void    SetFunc_Login_Fail( MonoBehaviour mono , string func )
    {
        g.sJ_GoogleLogin.func_login_Fail.SetMono( mono , func );
    }

    public  void    OnLoad_PlayerData( string str_data )
    {
        //Debug.Log( str_data );

        if( string.IsNullOrEmpty( str_data ) )return;

        ME_Account.user_Save = JsonUtility.FromJson<_User_Save>(str_data);

    }

    static public   void    Save_PlayerData()
    {
        string str_data = ME_Account.GetJson_User_Save();
        Debug.Log("Save_PlayerData : " + str_data);
        //g.sJ_GoogleLogin.SaveData( str_data );
        g.sJ_GoogleLogin.save_json_data = str_data;
        g.sJ_GoogleLogin.SaveData();
    }

}
