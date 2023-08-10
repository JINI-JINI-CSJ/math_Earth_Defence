using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_InstMatList : MonoBehaviour
{
    public  bool        noEnable;
    public  Renderer[]  lt_render;
    public  List<Material>  lt_inst_mat;

    public  bool    multi_material;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        if( noEnable ) return;
        Inst_Mat();
    }

    public void    Inst_Mat( bool force_init = false )
    {
        if( lt_render.Length < 1 || force_init )
        {
            foreach( Material s in lt_inst_mat )GameObject.Destroy( s );
            lt_inst_mat.Clear();

            lt_render = GetComponentsInChildren<Renderer>();
            foreach( Renderer s in lt_render )
            {
                if( multi_material == false )
                {
                    Material inst_m = GameObject.Instantiate( s.material );
                    lt_inst_mat.Add(inst_m);
                    s.material = inst_m;                    
                }else{
                    List<Material>  lt_inst_mat_cur_rd = new List<Material>();
                    for( int i = 0 ; i < s.materials.Length ; i++ )
                    {
                        Material mat = s.materials[i];
                        Material inst_m = GameObject.Instantiate( mat );
                        lt_inst_mat.Add(inst_m);
                        s.materials[i] = inst_m;
                    }
                }
            }            
        }
    }

    public  void    SetColor( string arg_name , Color col ) 
    {
        foreach( Material s in lt_inst_mat )
            s.SetColor( arg_name , col );
    }
    public  void    SetFloat( string arg_name , float f )   
    {
        foreach( Material s in lt_inst_mat )
            s.SetFloat( arg_name , f );
    }
}
