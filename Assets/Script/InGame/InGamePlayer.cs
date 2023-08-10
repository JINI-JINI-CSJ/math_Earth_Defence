using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePlayer : MonoBehaviour
{
    static  public  InGamePlayer    g;
    public  int     HP;
    public  int     hp_cur;


    public  int     score;

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

    static  public  bool    Input_Value( int val )
    {
        List<FallFormulaObj> lt_eq = FallFormulaMng.Get_All_FallObj( val );
        if( lt_eq.Count < 1 ) return false;

        // 
        foreach( FallFormulaObj s in lt_eq )
        {
            s.Return();
            g.score += 1;
        }

        //Panel_InGameMain.g.OnClick_Clear();

        Panel_InGameInfUI.Update_UI();

        return true;
    }
}
