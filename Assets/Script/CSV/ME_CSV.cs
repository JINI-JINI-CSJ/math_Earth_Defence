using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;



public class ME_CSV : SJ_Singleton_Mono
{
    static  public  int     load_count = 0;
    static  public  object  lock_load = new object();
    static  public  int     load_state = 0;
    static  public  bool    loaded;    
    public          _SJ_GO_FUNC         load_complete_func;    
    public          string  url_Base;


    static public   ME_CSV g;
	override		public	SJ_Singleton_Mono	OnGetStatic() {return (SJ_Singleton_Mono)g; }
	override		public	void				OnSetStatic(SJ_Singleton_Mono s) { g = s as ME_CSV; }



    static  public  CSV_Formula_Page    csv_Formula_Page = new CSV_Formula_Page();
    static  public  CSV_Formula_Page    csv_Formula_stage_Page = new CSV_Formula_Page();

    static  public  CSV_Formula_Page    csv_Formula_all = new CSV_Formula_Page();

    // 무한모드 빼고 나머지 스테이지 갯수
    static          public  int     stage_num;

    static  public  void    Load( MonoBehaviour mono = null , string func = "" )
    {
        if( mono != null )
        {
            g.load_complete_func.SetMono(mono , func);
        }

        if( loaded )
        {
            g.End_Load();
            return;           
        }

        loaded = true;
        load_state = 1;
        g.Load_GoogleSeat();
    }

    public   void    Load_GoogleSeat()
    {
        Debug.Log( "csv 로딩 시작" ); 
        StartCoroutine( load_Seat(csv_Formula_Page        , "타임리스모드"  , false ) );
        StartCoroutine( load_Seat(csv_Formula_stage_Page  , "스테이지"      , false ) );
    }

    public  IEnumerator    load_Seat( SJ_CSV_BasePage seat , string seat_name , bool id_int_str )
    {
        lock(lock_load)
        {
            load_count++;
        }

        Debug.Log( seat_name );
        UnityWebRequest www = UnityWebRequest.Get(url_Base + seat_name);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }else {
            Debug.Log(www.downloadHandler.text);
            seat.LoadCSV_Text( www.downloadHandler.text , seat_name , id_int_str );
        }

        lock(lock_load)
        {
            load_count--;

            if( load_count == 0 )
            {
                LoadAfter();                
                End_Load();
            }
        }
    }

    public  void    End_Load()
    {
        Debug.Log( "csv 로딩 완료" );      
        load_state = 2;
        load_complete_func.Func();        
    }

    static  public  bool    LoadCompleted()
    {
        if( load_state == 2 ) return true;
        return false;
    }

    static   public   void   LoadAfter()
    {
        csv_Formula_all.Add( csv_Formula_Page );
        csv_Formula_all.Add( csv_Formula_stage_Page );

        HashSet<int>    hs_stage_num = new HashSet<int>();
        foreach( CSV_Formula s in csv_Formula_all.dic_int.Values.Cast<CSV_Formula>())
        {
            hs_stage_num.Add( s.stage_num );
        }
        // 여기서 "0" 스테이지는 삭제
        if( hs_stage_num.Contains(0) )
        {
            hs_stage_num.Remove(0);
        }
        stage_num = hs_stage_num.Count;
        Debug.Log( "스테이지 갯수 : " + stage_num );
    }


    // static  public  CSV_Formula    Get_CalcFormula(  )
    // {
    //     CSV_Formula csv = csv_Formula_Page.Get_Per();

    //     // var compiledExpr = CodeWriter.ExpressionParser.FloatExpressionParser.Instance.Compile("1+2*54", null, true);
    //     // var result = compiledExpr.Invoke();
    //     // ref_result = (int)result;
    //     // Debug.Log( "결과 : " + result );

    //     return csv;
    // }
}
