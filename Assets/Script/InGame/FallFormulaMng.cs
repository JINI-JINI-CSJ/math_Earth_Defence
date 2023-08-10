using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallFormulaMng : MonoBehaviour
{
    static public   FallFormulaMng  g;

    // 뷰포트 0->1
    public  float   start_y_vp = 0.8f;
    public  float   start_y_vp_random_add = 0.1f;
    public  float   end_y_vp = 0.2f;
    public  float   width_x = 0.25f;
    public  float   fall_time = 10.0f;

    public  float   check_empty_radius = 2;

    public  GameObject  prf_fall_obj;

    public  GameObject  tr_inst_par;

    public  List<Regen_FallObj> lt_Regen_FallObj;

    private void Awake() {
        g = this;
    }

    public  void    Init()
    {
        lt_Regen_FallObj.Clear();
        SJ_Unity.Delete_Child( transform );

        foreach( CSV_Formula s in ME_CSV.csv_Formula_Page.dic_int.Values.Cast<CSV_Formula>())
        {
            GameObject inst_regen = new GameObject("regen");
            Regen_FallObj c = inst_regen.AddComponent<Regen_FallObj>();
            c.Init( s );
            lt_Regen_FallObj.Add(c);
            SJ_Unity.SetEqTrans( inst_regen.transform , null , transform );
        }
    }

    public  void    Play()
    {
        foreach( Regen_FallObj s in lt_Regen_FallObj ) s.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  Vector2 Get_Pos_Empty()
    {
        int c = 0;

        Vector2 pos = Vector2.zero;

        while( true )
        {
            float x_vp = UnityEngine.Random.Range( -width_x , width_x ) + 0.5f;
            float y_vp = start_y_vp - UnityEngine.Random.Range(0,start_y_vp_random_add);
            pos.x = x_vp;
            pos.y = y_vp;

            Vector2 pos_w = SJ_SpritePos_Ratio.Pos( pos );

            int lc = LayerMask.NameToLayer( "Aster" );
            int layerMask = 1 << lc;
            Collider2D[] cs = Physics2D.OverlapCircleAll( pos_w , check_empty_radius , layerMask );
            if( cs.Length < 1 )break;

            c++;
            if( c > 20 ) break;
        }

        Debug.Log( "Get_Pos_Empty : " + c );

        return pos;
    }

    static public   FallFormulaObj    Inst_FallObj( Regen_FallObj regen )
    {

        // float pos_vp_x = UnityEngine.Random.Range( -g.width_x , g.width_x ) + 0.5f;
        // float y = g.start_y_vp - UnityEngine.Random.Range(0,g.start_y_vp_random_add);

        Vector2 pos_vp = g.Get_Pos_Empty();


        GameObject inst = SJPool.GetNewInst( g.prf_fall_obj );
        inst.transform.parent = g.tr_inst_par.transform;
        FallFormulaObj ff = inst.GetComponent<FallFormulaObj>();

        ff.Init( pos_vp , regen );
        return ff;
    }

    static public   List<FallFormulaObj>    Get_All_FallObj()
    {
        List<FallFormulaObj> lt = new List<FallFormulaObj>();
        foreach( Regen_FallObj s in g.lt_Regen_FallObj )
        {
            lt.AddRange( s.lt_obj );
        }
        return lt;
    }

    static public   List<FallFormulaObj>    Get_All_FallObj( int val )
    {
        List<FallFormulaObj> lt = new List<FallFormulaObj>();
        foreach( Regen_FallObj s in g.lt_Regen_FallObj )
        {
            foreach( FallFormulaObj ss in s.lt_obj )
            {
                if( ss.Check_Result( val ) )
                {
                    lt.Add(ss);
                }
            }
        }
        return lt;
    }




}
