using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SJ_InstMatSprite : MonoBehaviour
{
    public  Material        mat;
    public  MeshRenderer    meshRenderer;
    public  Material        inst_mat;   

    public  string          shader_name_arg_MainTex = "_MainTex";
    public  Sprite          sprite;
    public  Texture         recent_tex;

    public  string              shader_name_arg_addColor = "_AColor";
    public  SJ_UITween_Color    tween_Color_add;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Change_Sprite();

        if( tween_Color_add != null && inst_mat != null )
        {
            inst_mat.SetColor( shader_name_arg_addColor , tween_Color_add.GetColor() );    
        }
    }

    public  void    InstMat()
    {
        if( inst_mat != null ) return;
        inst_mat = GameObject.Instantiate( mat );
        meshRenderer.material = inst_mat;

    }

    public  void    Change_Sprite()
    {
        if( sprite == null || recent_tex == sprite.texture )
        {
            return;
        }

        if( meshRenderer == null )
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if( meshRenderer == null ) return;
            mat = meshRenderer.sharedMaterial;
        }

        if( mat == null ) return;

        InstMat();
        if( sprite != null )
        {
            inst_mat.SetTexture( shader_name_arg_MainTex , sprite.texture );     
            recent_tex = sprite.texture;       
        }
    }
}
