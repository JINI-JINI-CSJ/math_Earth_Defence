using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_ResPool : MonoBehaviour
{
    static  public  InGame_ResPool  g;



    public  GameObject  prf_FallObjUI;

    public GameObject   tr_ui_par;

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

    //static public   GameObject   Inst_FallObj(){return SJPool.GetNewInst( g.prf_FallObj );}
    static public   GameObject   Inst_FallObjUI()
    {
        GameObject inst = SJPool.GetNewInst( g.prf_FallObjUI );

        SJ_Unity.SetEqTrans( inst.transform , null , g.tr_ui_par.transform );
        
        return inst;
    }
}
