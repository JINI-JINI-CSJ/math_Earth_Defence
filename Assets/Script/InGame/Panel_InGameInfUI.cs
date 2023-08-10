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
    }

    public  void    Update_UI_PlayerHP()
    {
        InGamePlayer    p = InGamePlayer.g;
        slider_player_hp.value = (float)p.hp_cur / (float)p.HP;
    }
}
