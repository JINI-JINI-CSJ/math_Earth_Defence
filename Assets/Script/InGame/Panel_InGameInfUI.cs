using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel_InGameInfUI : MonoBehaviour
{
    static public   Panel_InGameInfUI   g;
    public  TMP_Text    text_score;
    public  Slider      slider_player_hp;
    public  Slider      slider_player_time;
    public TMP_Text     text_time;

    public TMP_Text     text_combo;

    public  float       time_start;

    public  float       play_total_time;
    public  float       play_total_time_cur;


    private void Awake() {
         g = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( InGameMain.g.play == false ) return;

        if( slider_player_time.gameObject.activeSelf )
        {
            play_total_time_cur -= Time.deltaTime;
            float f = play_total_time_cur / play_total_time;
            slider_player_time.value = f;

            int int_time = (int)play_total_time_cur + 1;
            text_time.text = int_time.ToString();            
            if( play_total_time_cur <= 0 )
            {
                InGameMain.OnEndPlay( true );
                text_time.text = "0";  
            }


        }
    }


    public  void    Start_Play( float time_play )
    {
        
        play_total_time = time_play;
        play_total_time_cur = time_play;
        time_start = Time.time;

        if( time_play < 0 )
        {
            slider_player_time.gameObject.SetActive(false);
        }else{
            slider_player_time.gameObject.SetActive(true);
        }
    }

    static public   void    Update_UI()
    {
        g.Update_UI_Score();
        g.Update_UI_PlayerHP();
    }

    public  void    Update_UI_Score()
    {
        InGamePlayer    p = InGamePlayer.g;
        text_score.text = p.score.ToString();

        
        if( InGamePlayer.g.combo > 1 )
        {
            text_combo.gameObject.SetActive(true);
            text_combo.text = "COMBO " + InGamePlayer.g.combo.ToString() + " !";
        }else{
            text_combo.gameObject.SetActive(false);
        }
    }

    public  void    Update_UI_PlayerHP()
    {
        InGamePlayer    p = InGamePlayer.g;
        slider_player_hp.value = (float)p.hp_cur / (float)p.HP;
    }


}
