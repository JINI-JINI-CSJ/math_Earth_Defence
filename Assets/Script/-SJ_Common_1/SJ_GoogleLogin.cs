using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi; //conf
using GooglePlayGames;  //platfom
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;
//using Newtonsoft.Json;
using System.IO;

public class SJ_GoogleLogin : MonoBehaviour
{
    public bool         noGoogleLogin;

    public _SJ_GO_FUNC  func_login_OK;
    public _SJ_GO_FUNC  func_login_Fail;

    public _SJ_GO_FUNC  func_data_Load_succ;
    public _SJ_GO_FUNC  func_data_Load_fail;

    public bool         state_init;
    public int          state_login;


    public Action<SavedGameRequestStatus, byte[]> OnSavedGameDataReadComplete;
    public Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameDataWrittenComplete;
    private bool isSaving;
    private const string FILE_NAME = "AppSmilejsuGameInfo.bin";


    public string   save_json_data = "";

    public void     Init()
    {
        if( state_init ) return;
        state_init = true;
#if UNITY_ANDROID 
        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .EnableSavedGames()
            .RequestServerAuthCode(false)
            //.RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
            .RequestIdToken()
            .Build();
        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 바꿔주세요.
        //GPGS 시작.
        PlayGamesPlatform.Activate();
#endif
    }

    public void GPGSLogin()
    {
        if( noGoogleLogin )
        {
            state_login = 1;
            func_login_OK.Func();
            return;
        }

#if UNITY_ANDROID
        if( state_login == 1 ) 
        {
            func_login_OK.Func();
            return;
        }

        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            state_login = 1;        
            func_login_OK.Func();

        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    state_login = 1;
                    //Load();
                    LoadData();
                    Debug.Log("Login success !!!");

                    func_login_OK.Func();
                }
                else
                {
                    state_login = -1;
                    // 로그인 실패
                    Debug.Log("Login failed for some reason~~~~");
                    func_login_Fail.Func();
                }
            });
        }
#else
        state_login = 1;
        func_login_OK.Func();
#endif

    }

    // 구글 토큰 받아옴
    public string GetTokens()
    {
#if UNITY_ANDROID

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫 번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두 번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return "";
        }
#else
        return "";
#endif
    }




#region 저장
    public void SaveData()
    {
        if (Social.localUser.authenticated)
        {
            Debug.Log( "SaveData 1");
            this.isSaving = true;
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(FILE_NAME, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
            Debug.Log( "SaveData 2");
        }
        else
        {
            this.SaveLocal();
        }
    }
 
    private void SaveLocal()
    {
        // var path = Application.persistentDataPath + "/AppSmilejsuGameInfo.bin";
        // var json = JsonConvert.SerializeObject(App.Instance.gameInfo);
        // byte[] bytes = Encoding.UTF8.GetBytes(json);
        // File.WriteAllBytes(path, bytes);
    }
 
    private void SaveGame(ISavedGameMetadata data)
    {
        Debug.Log( "SaveGame 1");
        this.SaveLocal();
 
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        var stringToSave = this.GameInfoToString();
        Debug.Log( "SaveGame : " + stringToSave );
        byte[] bytes = Encoding.UTF8.GetBytes(stringToSave);
        savedGameClient.CommitUpdate(data, update, bytes, OnSavedGameDataWrittenComplete);
        Debug.Log( "SaveGame 2");
    }
 
    #endregion
 
    #region 불러오기 
    public void LoadData()
    {
        if (Social.localUser.authenticated)
        {
            this.isSaving = false;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(FILE_NAME, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, this.OnSavedGameOpened);
        }
        else
        {
            this.LoadLocal();
        }
    }
 
    private void LoadLocal()
    {
        // var path = Application.persistentDataPath + "/AppSmilejsuGameInfo.bin";
        // byte[] bytes = File.ReadAllBytes(path);
        // var json = Encoding.UTF8.GetString(bytes);
 
        // this.StringToGameInfo(json);
    }
 
    private void LoadGame(ISavedGameMetadata data)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(data, OnSavedGameDataReadComplete);

    }
    #endregion
 
    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.LogFormat("OnSavedGameOpened : {0}, {1}", status, isSaving);
 
        if (status == SavedGameRequestStatus.Success)
        {
            if (!isSaving)
            {
                this.LoadGame(game);
            }
            else
            {
                this.SaveGame(game);
            }
        }
        // else
        // {
        //     if (!isSaving)
        //     {
        //         this.LoadLocal();
        //     }
        //     else
        //     {
        //         this.SaveLocal();
        //     }
        // }
    }
 
    // public void StringToGameInfo(string localData)
    // {
    //     if (localData != string.Empty)
    //     {
    //         //App.Instance.gameInfo = JsonConvert.DeserializeObject<GameInfo>(localData);
    //         save_json_data = localData;
    //     }
    // }
 
    private string GameInfoToString()
    {
        return save_json_data;
    }

//     public void SaveData( string myData_String )
//     {
// #if UNITY_ANDROID

//         Debug.Log("======= 구글 세이브 시작 ===== ");
//         Debug.Log(myData_String);

//         // //string myData_String = JsonUtility.ToJson(myData);
//         byte[] myData_Binary = Encoding.UTF8.GetBytes(myData_String);

//         SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
//         ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(myGame, update, myData_Binary, SaveCallBack);

//         //this.isSaving = true;
//         //ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
//         //savedGameClient.OpenWithAutomaticConflictResolution("FILE_NAME", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
// #endif
//     }


//     private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata game)
//     {
//         if (status == SavedGameRequestStatus.Success)
//         {
//             Debug.Log("구글 세이브 성공");
//             //Status_Display.text = "Save Successful.";
//             Load();
//         }
//         else
//         {
//             //Status_Display.text = "Save Failed.";
//         }
//     }

//     //////////
//     // LOAD //
//     //////////
//     public void Load() 
//     {
// #if UNITY_ANDROID
//         //Status_Display.text = "Loading...";
//         ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("mysave", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
// #endif
//     }

//     void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
//     {
//         if (status == SavedGameRequestStatus.Success)
//         {
//             myGame = game;
//             LoadData(myGame);
//         }
//     }

//     void LoadData(ISavedGameMetadata game)
//     {
// #if UNITY_ANDROID
//         ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, LoadDataCallBack);
// #endif
//     }

//     void LoadDataCallBack(SavedGameRequestStatus status, byte[] LoadedData)
//     {
// #if UNITY_ANDROID
//         if (status == SavedGameRequestStatus.Success)
//         {
//             try
//             {
//                 string  myData_String = Encoding.UTF8.GetString(LoadedData);

//                 Debug.Log("구글 플레이 로드 : " + myData_String);

//                 func_data_Load_succ.Func( myData_String );

//                 //myData = JsonUtility.FromJson<GameData>(myData_String);
//                 //Status_Display.text = "Loading Successful";
//                 //UpdateUI();
//             }
//             catch (Exception e)   
//             {
//                 //Status_Display.text = "Failed To Load Data. Using Default";
//                 //myData = new GameData();
//             }
//         }
// #else
//         func_data_Load_succ.Func( "" );
// #endif
//     }


//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
}
