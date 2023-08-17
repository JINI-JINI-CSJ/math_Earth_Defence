using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_UnityUI_Util : SJ_Singleton_Mono
{
    static public   List<object>    ListArgToListT<T>( List<T> lt_T )
    {
        List<object> lt_obj = new List<object>();
        foreach( T s in lt_T )
        {
            lt_obj.Add( s as object);
        }
        return lt_obj;
    }

    static public   void    ListItem_Add( List<object> lt_data , GameObject go_grid , string func_item )
    {
        if( go_grid.transform.childCount < 1 ) return;

        Transform tr_prf = go_grid.transform.GetChild(0);

        if( lt_data.Count > go_grid.transform.childCount )
        {
            int add = lt_data.Count - go_grid.transform.childCount;

            for( int i = 0 ; i < add ; i++ ) 
            {
                GameObject inst = GameObject.Instantiate(tr_prf.gameObject);
                inst.transform.parent           = go_grid.transform;                
                inst.transform.localPosition    = Vector3.zero;
                inst.transform.localScale       = Vector3.one;
                inst.transform.localRotation    = Quaternion.identity;
            }
        }

        for( int i = 0 ; i < go_grid.transform.childCount ; i++ )
        {
            Transform tr_item = go_grid.transform.GetChild(i);
            if( i < lt_data.Count )
            {
                object obj_arg = lt_data[i];           

                //Debug.Log( tr_item.name );

                tr_item.gameObject.SetActive(true);

                // 주의 !!!!
                // 반드시 엑티브 상태일것!!!!
                // 부모도 액티브인지 확인!!!! 
                tr_item.gameObject.SendMessage( func_item , obj_arg , SendMessageOptions.DontRequireReceiver );
            }else{
                tr_item.gameObject.SetActive(false);
            }
        }

    }
}
