using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class _Stage_Record
{
    public int  stage_num;
    public int  high_score;
}

[System.Serializable]
public class _User_Save
{
    public   bool    config_sound = true;
    public   bool    config_Vibrate = true;    
    public   int     lastClear_Stage;

    public   List<_Stage_Record> lt_Stage_Record = new List<_Stage_Record>();// 세이브 용도
    public   Dictionary<int  ,_Stage_Record> dic_Stage_Record = new Dictionary<int, _Stage_Record>();

    // public  void    Save()
    // {
    //     string str_data = JsonUtility.ToJson(this);
        

    // }

    public  bool    Add_ClearStage( int stage_num , int high_score ) 
    {
        _Stage_Record stage_Record = null;
        if( dic_Stage_Record.TryGetValue( stage_num , out stage_Record ) )
        {
            if( stage_Record.high_score < high_score )
            {
                stage_Record.high_score = high_score;
                return true;
            }
            return false;
        }
        stage_Record = new _Stage_Record();
        stage_Record.stage_num = stage_num;
        stage_Record.high_score = high_score;
        dic_Stage_Record[stage_num] = stage_Record;
        return true;
    }

    public int  Get_HighScoreStage( int stage_num )
    {
        _Stage_Record stage_Record = null;
        if( dic_Stage_Record.TryGetValue( stage_num , out stage_Record ) )
        {
            return stage_Record.high_score;
        }
        return 0;
    }

    public  string    GetJson()
    {
        lt_Stage_Record.Clear();
        lt_Stage_Record.AddRange( dic_Stage_Record.Values );

        Debug.Log( "GetJson : " + lt_Stage_Record.Count);

        return JsonUtility.ToJson(this);
    }

    public  void    FromJson()
    {
        dic_Stage_Record.Clear();

        foreach( _Stage_Record s in lt_Stage_Record )
        {
            _Stage_Record new_s = new _Stage_Record();
            new_s.stage_num = s.stage_num;
            new_s.high_score = s.high_score;

            dic_Stage_Record[s.stage_num] = new_s;
        }
    }


}

public class ME_Account 
{
    static public   int     StageNum = 1;
    static public   _User_Save user_Save = new _User_Save();


    static public string GetJson_User_Save()
    {
        return user_Save.GetJson();
    }


    static public void FromJson( string json )
    {
        if( string.IsNullOrEmpty( json ) ) return;
        user_Save = JsonUtility.FromJson<_User_Save>( json );
        user_Save.FromJson();
    }

}
