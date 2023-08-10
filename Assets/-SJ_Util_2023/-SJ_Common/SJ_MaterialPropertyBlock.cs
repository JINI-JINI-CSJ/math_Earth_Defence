using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_MaterialPropertyBlock : MonoBehaviour
{
    public  Renderer                rd;
    public  MaterialPropertyBlock   block;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void    Block_Ready()
    {
        if( block == null )
        {
            block = new MaterialPropertyBlock();
        }        
    }

    public  void   Update_Block()
    {
        if( rd == null )
        {
            rd = GetComponent<Renderer>();            
        }
        if( rd == null ) return;
        rd.SetPropertyBlock( block );
    }

}
