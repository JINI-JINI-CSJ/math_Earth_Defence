using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_UnityUIMng : MonoBehaviour
{
    static  public  SJ_UnityUIMng g;

    public  GameObject  go_Black;

    public  GameObject  go_LastSibling;

    //public  float   PopupAdd_Z = -500.0f;
    //public  float   PopupAdd_Z_Black = 10.0f;
    public  List<GameObject>    lt_Popup;

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


    static public   GameObject  OpenPopup( string str , bool show_go_LastSibling = false )
    {
        Transform tr = g.transform.Find(str);
        if( tr == null )
        {
            Debug.LogError( "에러!!! OpenPopup : " + str );
            return null;
        } 

        GameObject go = tr.gameObject;
        
        foreach( GameObject s in g.lt_Popup )
        {
            if( s == go )
            {
                Debug.LogError( "에러!!! OpenPopup : 이미 열려 있음!! " + str );
                return null;
            } 
        }

        g.lt_Popup.Add(go);

        go.transform.SetAsLastSibling();
        go.SetActive(true);
        SJ_Unity.SendMsg( go , "OpenPopup" );

        if( show_go_LastSibling && g.go_LastSibling != null )g.go_LastSibling.transform.SetAsLastSibling();

        return go;
    }

    static  public  void    ClosePopup()
    {
        if( g.lt_Popup.Count < 1 )
        {
            g.BlackPlane_ZPos();
            return;
        }

        GameObject go = g.lt_Popup[ g.lt_Popup.Count - 1 ];
        go.SetActive(false);
        g.lt_Popup.RemoveAt( g.lt_Popup.Count - 1 );
        g.BlackPlane_ZPos();

        Last_PopupMsg();
    }

    static public   void    Last_PopupMsg()
    {
        // 바로 아래 있던 팝업창에게 메세지
        if( g.lt_Popup.Count > 0 )
        {
            GameObject go = g.lt_Popup[g.lt_Popup.Count-1];
            SJ_Unity.SendMsg( go , "OnPopup_TopView" );
        }        
    }

    static public   GameObject  Find_Opened(string str)
    {
        foreach( GameObject s in g.lt_Popup )
        {
            if( s.name == str )
            {
                return s;
            }
        }
        return null;
    }

    static  public  void    ClosePopup( string str )
    {
        GameObject go = null;
        foreach( GameObject s in g.lt_Popup )
        {
            if( s.name == str )
            {
                go = s;
                break;
            }
        }
        if( go == null ) return;
        go.SetActive(false);
        SJ_Unity.SendMsg( go , "ClosePopup" );
        g.lt_Popup.Remove( go );

        Last_PopupMsg();
    }

    static  public  void    ClosePopup_All()
    {
        foreach( GameObject s in g.lt_Popup )
        {
            s.SetActive(false);
        }
        g.lt_Popup.Clear();
    }

    public  void    BlackPlane_ZPos()
    {
        // if( lt_Popup.Count < 1 )
        // {
        //     go_Black.SetActive(false);
        //     return;
        // }

        // Vector3 p = go_Black.transform.localPosition;
        // p.z = lt_Popup.Count * PopupAdd_Z + PopupAdd_Z_Black;
        // go_Black.transform.localPosition = p;

        // go_Black.SetActive( true );
    }

    public  void    ClosePopup_Common()
    {
        ClosePopup();
    }
}
