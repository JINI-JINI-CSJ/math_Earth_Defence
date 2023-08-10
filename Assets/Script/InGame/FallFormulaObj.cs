using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FallFormulaObj : MonoBehaviour
{
    public  TMP_InputField    text_str;
    public  string      str_text;
    public  CSV_Formula csv;
    public  Vector2     pos_vp; // 뷰포트 좌표

    public  List<GameObject>    lt_image;
    public  GameObject          go_ufo;
    public  GameObject          go_Bomb;

    public  int         val_result;

    public  GameObject  inst_ui;

    public  float       fall_time_cur;
    public  float       pos_x;

    public  float       pos_y_start;

    public  Regen_FallObj   regen_FallObj_par;

    public  bool    play;

    public  void    Init( Vector2 pos_vp , Regen_FallObj regen_FallObj )
    {
        play = true;

        csv = regen_FallObj.csv;        

        regen_FallObj_par = regen_FallObj;

        fall_time_cur = 0;
        pos_x = pos_vp.x;
        pos_y_start = pos_vp.y;

        str_text = csv.Make_Str();

        inst_ui = InGame_ResPool.Inst_FallObjUI();
        text_str = inst_ui.GetComponentInChildren<TMP_InputField>();
        text_str.text = str_text;
        

        var compiledExpr = CodeWriter.ExpressionParser.FloatExpressionParser.Instance.Compile( str_text , null, true);
        var result = compiledExpr.Invoke();
        val_result = (int)result;

        go_ufo.SetActive(false);
        go_Bomb.SetActive(false);
        foreach( GameObject s in lt_image ) s.SetActive(false);

        switch( csv.type )
        {
            case 0: 
            {
                GameObject img_active = SJ_Unity.GetArray_Random( lt_image.ToArray() );
                img_active.SetActive(true);
            }
            break;

            case 1:
                go_Bomb.SetActive(true);
            break;

            case 2:
                go_ufo.SetActive(true);
            break;
        }

        Update_Pos();
        Update_UI();
    }

    public  bool    IsLive()
    {
        return play;
    }

    public  bool    Check_Result( int check )
    {
        if( IsLive() == false ) return false;

        if( val_result == check )
        {
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Update_Pos();
    }

    public  void    Update_Pos()
    {
        fall_time_cur += Time.deltaTime * csv.fall_speed * InGameMain.g._ingame_config.fix_fall_time;
        float ratio = fall_time_cur / FallFormulaMng.g.fall_time;
        float y = Mathf.Lerp( pos_y_start , FallFormulaMng.g.end_y_vp , ratio );
        pos_vp.x = pos_x;
        pos_vp.y = y;

        if( ratio >= 1.0f )
        {
            if( csv.type == 0 )
                InGamePlayer.Damage(10);

            // 도달
            Return();
        }
        SJ_SpritePos_Ratio.Pos( transform , pos_vp );
    }

    public  void    Return()
    {
        if( play == false ) return;

        play = false;

        regen_FallObj_par.Remove_Obj(this);
        SJPool.ReturnInst( inst_ui );
        SJPool.ReturnInst(gameObject);
    }


    public  void    AddScore()
    {
        if( IsLive() == false ) return;
        switch( csv.type )
        {
            case 0:
            {
                InGamePlayer.AddScore(1);
            }
            break;

            case 1:
            {
                InGamePlayer.AddScore(1);
            }
            break;

            case 2:
            {
                InGamePlayer.AddScore(5);
            }
            break;
        }
    }

    private void FixedUpdate() {

        Update_UI();
    }

    public  void    Update_UI()
    {
        if( inst_ui != null ) 
            InGameMain.WorldToUI( gameObject , inst_ui );
    }


    
}
