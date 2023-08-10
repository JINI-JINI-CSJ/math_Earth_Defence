using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_InGameMain : MonoBehaviour
{
    static public  Panel_InGameMain g;

    public  bool    minus_checked;      // 마이너스 여부
    public  string  clicked_num_all = ""; // 숫자 입력한 상태 
    public  string  make_result;    // 위 2개를 합친  상태 , int 변환 예정

    public  TMP_InputField    text_show_Input;


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

    public  void    Update_UI()
    {
        text_show_Input.text = make_result;
    }

    public  void    OnClick_Clear()
    {
        make_result = "";
        clicked_num_all = "";
        minus_checked = false;
        Make_ResultText();
        Update_UI();
    }

    public  void    Make_ResultText()
    {
        make_result = clicked_num_all;
        if( string.IsNullOrEmpty(clicked_num_all) )
        {
            make_result = "0";
        }

        
        if( minus_checked )
        {
            make_result = "-" + make_result;
        }
    }

    public  void    OnClick_Minus()
    {
        minus_checked = !minus_checked;
        Make_ResultText();
        Update_UI();
    }

    public  void    OnClick_Number( string str )
    {
        clicked_num_all = clicked_num_all + str;
        Make_ResultText();
        Update_UI();
    }


    public  int     Parse_Input()
    {
        int r = 0;
        int.TryParse( make_result , out r );
        return r;
    }

    public  void    OnClick_Enter()
    {
        InGamePlayer.Input_Value( Parse_Input() );
        OnClick_Clear();
    }

}
