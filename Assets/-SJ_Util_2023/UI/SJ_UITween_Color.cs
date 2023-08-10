using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SJ_UITween_Color : SJ_UITweenBase
{
    public  Color   col_from;
    public  Color   col_to;
    public  Image  image;
    public  SpriteRenderer  spriteRenderer;

    public  bool        noGetRenderer;


    public  SJ_MaterialPropertyBlock    sJ_MaterialPropertyBlock;
    public  SJ_InstMatList              sJ_InstMatList;
    public  string                      arg_Name_Color = "";



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FrameMove( Time.deltaTime );
    }

    public override void OnFrameMove()
    {
        // if( noGetRenderer == false && image == null && spriteRenderer == null )
        // {
        //     image = GetComponentInChildren<Image>();
        //     spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // }

        Color col = Color.Lerp( col_from , col_to , ratio_cur );
        if( image != null ) image.color = col;
        if( spriteRenderer != null ) spriteRenderer.color = col;

        if( sJ_MaterialPropertyBlock != null )
        {
            sJ_MaterialPropertyBlock.Block_Ready();
            sJ_MaterialPropertyBlock.block.SetColor( arg_Name_Color , col );
            sJ_MaterialPropertyBlock.Update_Block();
        }

        if( sJ_InstMatList != null ) 
        {
            sJ_InstMatList.Inst_Mat();
            sJ_InstMatList.SetColor( arg_Name_Color , col );
        }

    }

    public  Color   GetColor()
    {
        return Color.Lerp( col_from , col_to , ratio_cur );
    }

}
