using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//``ID	스테이지	KOR	ENG																						
//1	1	튜토리얼 스테이지입니다.																							
public class CSV_StageDesc : SJ_CSV_BaseObj
{
    public  int         stage_num;
    public  string      desc_kr;
    public  string      desc_eng;

    public override void OnRead(SJ_CSV_BasePage _par, string[] _strs)
    {
        base.OnRead(_par, _strs);
        stage_num = Next_Int();
        desc_kr = Next();
        desc_eng = Next();
    }
}

public class CSV_StageDesc_Page : SJ_CSV_BasePage
{
    override	public	SJ_CSV_BaseObj	OnAlloc_Obj(){return new CSV_StageDesc();}
}
