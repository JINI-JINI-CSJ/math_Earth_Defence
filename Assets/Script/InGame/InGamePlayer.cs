using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePlayer : MonoBehaviour
{
    static  public  InGamePlayer    g;
    public  int     HP;
    public  int     hp_cur;
    public  int     score;
    public  int     combo;
    public  int     combo_MAX;

    public  int     stage_ScoreTotal;

    public  GameObject  prf_bullet;

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

    public  void    Init()
    {
        hp_cur = HP;
        combo = 0;
        combo_MAX = 0;
        Panel_InGameInfUI.Update_UI();
    }

    static public   void    Damage( int val )
    {
        g._Damage(val);
    }

    public  void    _Damage(int val)
    {
        if( ME_Account.user_Save.config_Vibrate )
            SJ_AndroidVibration.Vibrate(100);

        combo = 0;
        hp_cur -= val;
        if( hp_cur <= 0 )
        {
            hp_cur = 0;            
            InGameMain.OnEndPlay( false );
        }

        Panel_InGameInfUI.Update_UI();
    }

    public  void    AddHP(int val)
    {
        hp_cur += val;
        if( hp_cur > HP )hp_cur = HP;
        Panel_InGameInfUI.Update_UI();
    }

    static public   void    AddScore( int val )
    {
        g.score += val;
        Panel_InGameInfUI.Update_UI();
    }

    static  public  bool    Input_Value( int val )
    {
        bool bomb = false;

        bool answer = false;

        while( true )
        {
            List<FallFormulaObj> lt_eq = FallFormulaMng.Get_All_FallObj( val );
            if( lt_eq.Count < 1 ) break;

            answer = true;

            FallFormulaObj s = lt_eq[0];
            s.AddScore();
            s.Return();

            g.FireBullet( s.transform );

            if( s.csv.type == 1 )
            {
                // 화면 클리어
                bomb = true;
                break;
            }
        }

        if( answer == false )  
        {
            g.combo = 0;
        }else{
            g.combo++;
            if( g.combo_MAX < g.combo ) g.combo_MAX = g.combo;
        }

        if( bomb )
        {
            g.Clear_All_FallObj();
        }

        

        Panel_InGameInfUI.Update_UI();
        return true;
    }

    public  void    Clear_All_FallObj()
    {
        List<FallFormulaObj> lt_eq = FallFormulaMng.Get_All_FallObj();
        foreach( FallFormulaObj s in lt_eq )
        {
            s.AddScore();
            s.Return();
        }            
    }

    public  void    FireBullet( Transform tr_target )
    {
        Quaternion qt = SJ_2DUitl.Rotation_Target( transform , tr_target );

        GameObject inst_bullet = SJPool.GetNewInst( prf_bullet );
        inst_bullet.transform.position = transform.position;
        inst_bullet.transform.localRotation = qt;

        BulletPlayer bulletPlayer = inst_bullet.GetComponent<BulletPlayer>();
        bulletPlayer.MoveStart( this , tr_target );
    }

    public  int     StageResult()
    {
        stage_ScoreTotal = score + combo_MAX;
        return stage_ScoreTotal;
    }


}
