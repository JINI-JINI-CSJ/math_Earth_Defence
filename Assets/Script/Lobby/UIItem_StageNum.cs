using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public  class _MAKE_STAGE_Inf
{
    public  int     stage_num;
}

public class UIItem_StageNum : MonoBehaviour
{
    public  TMP_Text    text_stage_num;

    public  int stage_num;


    public  void    OnItem_Init( object arg )
    {
        _MAKE_STAGE_Inf s = (_MAKE_STAGE_Inf) arg;
        stage_num = s.stage_num;
        text_stage_num.text = stage_num.ToString();
    }

    // public  void    OnItem_StateNumInit( int arg )
    // {
    //     stage_num =  arg;
    //     text_stage_num.text = stage_num.ToString();
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
