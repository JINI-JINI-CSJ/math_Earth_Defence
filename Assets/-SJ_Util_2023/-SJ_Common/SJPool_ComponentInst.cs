using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJPool_ComponentInst
{
    public  delegate    void    Dlg_Func( GameObject obj , object arg );

    static  public  GameObject  GetInst( string str_component , Dlg_Func func = null , object arg = null )
    {
        if(  SJPool.FindMng(str_component) == false )
        {
            System.Type componentType = System.Type.GetType( str_component ); 
            GameObject obj = new GameObject( str_component );
            obj.AddComponent<SJGoPoolObj>();
            obj.AddComponent(componentType);

            if( func != null )
            {
                func( obj , arg );
            }

            return SJPool.GetNewInst(obj);
        }else{
            return SJPool.GetNewInst(str_component);
        }

        return null;
    }
}
