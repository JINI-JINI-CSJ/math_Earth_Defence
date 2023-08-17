using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PanelPopup_StageDesc : MonoBehaviour
{
    public TMP_Text text_Stage;
    public TMP_Text text_Desc;
    public TMP_Text text_HighScore;

    public int      stage_num;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPopup()
    {

    }

    public void SetStageInf( int _stage_num )
    {
        stage_num = _stage_num;
        CSV_StageDesc csv = ME_CSV.Get_CSV_StageDesc( stage_num );
        text_Stage.text = "STAGE " + stage_num.ToString();
        text_Desc.text = csv.desc_kr;

        text_HighScore.text = "HIGH SCORE : " + ME_Account.user_Save.Get_HighScoreStage( stage_num );
    }

    public void OnClick_Start()
    {
        ME_Account.StageNum = stage_num;
        SceneManager.LoadScene( "InGame" );
    }

}
