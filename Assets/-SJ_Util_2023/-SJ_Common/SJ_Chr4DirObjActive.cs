using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  enum SJ_4DIR_TYPE
{
    TOP = 0,
    BOTTOM ,
    LEFT ,
    RIGHT
}

public class SJ_Chr4DirObjActive : MonoBehaviour
{

    public  class  _ACTION
    {
        public  SJ_4DIR_TYPE    dir;
        public  string          name;
        public  GameObject      go;
    }
    public  List<_ACTION>   lt_ACTION;
    public  Dictionary<string,_ACTION>  dic_ACTION = new Dictionary<string, _ACTION>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static  public  string  STR_DirName( SJ_4DIR_TYPE dir , string name )
    {
        return dir.ToString() + "_" + name;
    }
    
    public  void    Init()
    {
        if( dic_ACTION.Count > 0 ) return;
         
    }


}
