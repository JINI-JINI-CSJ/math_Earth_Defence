using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SJ_UnityUI_CommonPopup : MonoBehaviour
{
    public  Text    text_title;
    public  Text    text_MSG;
    public  Text    text_OK;
    public  Text    text_Cancel;

    public  GameObject      go_BT_Cancel;

    public  _SJ_GO_FUNC     func_OK;
    public  _SJ_GO_FUNC     func_Cancel;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void    _Active_BTCancel( bool b )
    {
        go_BT_Cancel.SetActive(b);
    }

    public  void    _SetText( string _text_title , string _text_msg )
    {
        if( text_title != null ) text_title.text = _text_title;
        if( text_MSG != null ) text_MSG.text = _text_msg;
    }

    public  void    _SetFunc( string _text_msg , MonoBehaviour mono = null , string func_ok = "" , string func_cancel = "" , string _text_ok = "" , string _text_cancel = "" )
    {
        if( text_MSG != null ) text_MSG.text = _text_msg;
        func_OK.SetMono( mono , func_ok );
        func_Cancel.SetMono( mono , func_cancel );
        if( text_OK != null ) text_OK.text = _text_ok;
        if( text_Cancel != null ) text_Cancel.text = _text_cancel;
    }

    static  public  void    OpenCommonMsg(string _text_msg , MonoBehaviour mono  = null , string func_ok  = "" , string func_cancel = "" , string _text_ok = "" , string _text_cancel = "")
    {
        GameObject go_SJ_UnityUI_CommonPopup = SJ_UnityUIMng.OpenPopup( "SJ_UnityUI_CommonPopup" );
        if( go_SJ_UnityUI_CommonPopup == null )
        {
            Debug.LogError( "go_SJ_UnityUI_CommonPopup == null" );
            return;            
        }
        SJ_UnityUI_CommonPopup sJ_UnityUI_CommonPopup = go_SJ_UnityUI_CommonPopup.GetComponent<SJ_UnityUI_CommonPopup>();
        if( sJ_UnityUI_CommonPopup == null )
        {
            Debug.LogError( "sJ_UnityUI_CommonPopup == null" );
            return;            
        }

        if( string.IsNullOrEmpty( func_cancel ) && string.IsNullOrEmpty( _text_cancel ) )
        {
            sJ_UnityUI_CommonPopup._Active_BTCancel(false);
        }else{
            sJ_UnityUI_CommonPopup._Active_BTCancel(true);
        }

        sJ_UnityUI_CommonPopup._SetFunc( _text_msg , mono , func_ok , func_cancel , _text_ok  , _text_cancel );  
    }

    public  void    OnOK()
    {
        func_OK.Func();
        SJ_UnityUIMng.ClosePopup();
    }

    public  void    OnCancel()
    {
        func_Cancel.Func();
        SJ_UnityUIMng.ClosePopup();
    }
}
