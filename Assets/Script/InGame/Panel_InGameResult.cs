using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Panel_InGameResult : MonoBehaviour
{
    public TMP_Text text_stage;
    public TMP_Text text_score;

    public TMP_Text text_max_combo;

    public GameObject go_Clear;
    public GameObject go_Fail;


    public  void    OpenPopup()
    {
        bool record_high_score = ME_Account.user_Save.Add_ClearStage( ME_Account.StageNum , InGamePlayer.g.stage_ScoreTotal );

        text_stage.text = "STAGE " + ME_Account.StageNum.ToString();
        text_score.text = "SCORE " + InGamePlayer.g.stage_ScoreTotal.ToString();
        text_max_combo.text = "MAX COMBO " + InGamePlayer.g.combo_MAX.ToString();

        if( InGameMain.g.stage_clear )
        {
            go_Clear.SetActive(true);
            go_Fail.SetActive(false);
        }else{
            go_Clear.SetActive(false);
            go_Fail.SetActive(true);
        }
        
        ME_Global.Save_PlayerData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void    OnClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene( "Lobby" );
    }
}
