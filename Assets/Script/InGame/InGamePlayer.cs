using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePlayer : MonoBehaviour
{
    static  public  InGamePlayer    g;
    public  int     HP;
    public  int     hp_cur;
    public  int     score;

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
        Panel_InGameInfUI.Update_UI();
    }

    static public   void    Damage( int val )
    {
        g._Damage(val);
    }

    public  void    _Damage(int val)
    {
        hp_cur -= val;
        if( hp_cur < 0 )hp_cur = 0;
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
        while( true )
        {
            List<FallFormulaObj> lt_eq = FallFormulaMng.Get_All_FallObj( val );
            if( lt_eq.Count < 1 ) break;

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
}
