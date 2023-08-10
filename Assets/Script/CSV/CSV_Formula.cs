using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//ID	타입	패턴(게임표시)							난이도컨트롤																								
//		a자리수	기호1	b자리수	기호2	c자리수	기호3	d자리수	최초생성시간	리젠간격(초)	등장확률(%)	낙하속도																																																												

public class CSV_Formula : SJ_CSV_BaseObj
{
    public  int         type;
    public  int         num_scale_1;
    public  string      oper_1;
    public  int         num_scale_2;
    public  string      oper_2;
    public  int         num_scale_3;
    public  string      oper_3;
    public  int         num_scale_4;

    public  float       first_regen_time;
    public  float       regen_repeat_time;
    public  int         percent;
    public  float       fall_speed;

    public  int         stage_num;
    public  int         damage;


    override	public	void	OnRead( SJ_CSV_BasePage _par , string[] _strs )
    {
        base.OnRead( _par ,  _strs );
        type        = Next_Int();
        num_scale_1 = Next_Int();
        oper_1      = Next();
        num_scale_2 = Next_Int();
        oper_2      = Next();
        num_scale_3 = Next_Int();
        oper_3      = Next();
        num_scale_4 = Next_Int();
        first_regen_time    = Next_Float();
        regen_repeat_time   = Next_Float();
        percent             = Next_Int();
        fall_speed          = Next_Float() * 0.01f;

        stage_num = Next_Int();
        damage = Next_Int();
    }

    public  string  Make_Str()
    {
        string str = "";
        str = Make_Number( num_scale_1 ); // 첫번째 상수
        str += Make_Oper( oper_1 );
        str += Make_Number( num_scale_2 ); // 두번째 상수

        if( string.IsNullOrEmpty( Make_Oper(oper_2) ) )// 연산자 없으면 리턴
        {
            Debug.Log( str );
            return str;
        }

        str += Make_Oper( oper_2 );
        str += Make_Number( num_scale_3 );

        if( string.IsNullOrEmpty( Make_Oper(oper_3) ) )// 연산자 없으면 리턴
        {
            Debug.Log( str );
            return str;
        }

        str += Make_Oper( oper_3 );
        str += Make_Number( num_scale_4 );


        Debug.Log( str );
        return str;
    }

    public  string  Make_Number( int scale )
    {
        int v1 = (int)Mathf.Pow( 10 , scale-1 );
        int v2 = (int)Mathf.Pow( 10 , scale );

        int vv = UnityEngine.Random.Range( v1 , v2 );
        return vv.ToString();
    }

    public  string Make_Oper( string oper )
    {
        if( string.IsNullOrEmpty( oper ) || oper == "0" ) return "";
        if( oper == "X" || oper == "x" ) return "*";
        return oper;
    }

}


public class CSV_Formula_Page : SJ_CSV_BasePage
{
    override  public	SJ_CSV_BaseObj	OnAlloc_Obj(){return new CSV_Formula();} 

    // 후처리
    // 확률들 정리

    // 일반 확률 모음
    public  List<CSV_Formula>   lt_normal = new List<CSV_Formula>();

    // 특수 확률들
    public  List<CSV_Formula>   lt_spc_per = new List<CSV_Formula>();

    public  void    Prc_After()
    {
        lt_normal.Clear();
        lt_spc_per.Clear();

        foreach( CSV_Formula s in dic_int.Values.Cast<CSV_Formula>())
        {
            lt_normal.Add(s);
        }
    }

    public  CSV_Formula    Get_Per()
    {
        return SJ_Unity.GetArray_Random<CSV_Formula>( lt_normal.ToArray() );
    }
}