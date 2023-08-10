using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regen_FallObj : MonoBehaviour
{
    public  CSV_Formula     csv;

    public  List<FallFormulaObj>    lt_obj = new List<FallFormulaObj>();

    public  void    Init( CSV_Formula _csv )
    {
        csv = _csv;
    }

    public  void    Play()
    {
        CancelInvoke();
        Debug.Log( "Regen_FallObj : 타이머 : " + csv.first_regen_time + " : " + csv.regen_repeat_time );
        InvokeRepeating( "Timer_Inst" , csv.first_regen_time , csv.regen_repeat_time );
    }


    public  void    Timer_Inst()
    {
        if( SJ_Unity.PerRandom( csv.percent ) == false ) return;
        FallFormulaObj ff = FallFormulaMng.Inst_FallObj( this );
        lt_obj.Add(ff);
    }

    public  void    Remove_Obj( FallFormulaObj obj )
    {
        lt_obj.Remove(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
