using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_TileCell2D_Simple_Unit : MonoBehaviour
{
    public  enum _EDIT_MODE_STATE
    {
        None = 0,
        Build , 
        Able , 
        Not 
    }
    public  _EDIT_MODE_STATE    edit_mode_state;
    // 추가 사이즈
    // 만약 1x1 짜리라면  ,  (0,0) 값
    public  Vector2Int  size;
    public  BoundsInt   boundsInt;
    public  Vector2Int  pos;
    // 아이소매트릭스 타일 기준 , 반전 여부
    // 이미지는 맨아래 기준..

    public  int         default_x_scale = 1;
    public  bool        reserve_iso_tile;
    public  bool        self_image = true;
    public  HashSet<SJ_TileCell2D_Simple_Unit>  hs_cash;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public  void    SetPos( Vector2Int p )
    {
        pos = p;
        MakeBounds();
    }

    public  void    Set_Mirror( bool b )
    {
        reserve_iso_tile = b;
        MakeBounds();
    }

    public  void    Toggle_IsoTile_Mirror()
    {
        reserve_iso_tile = !reserve_iso_tile;
        MakeBounds();
    }

    public  void    MakeBounds()
    {
        Vector2Int size_cur = size;
        if( reserve_iso_tile )
        {
            size_cur.x = size.y;
            size_cur.y = size.x;
        }

        boundsInt.SetMinMax(
         new Vector3Int( pos.x , pos.y ) 
        ,new Vector3Int( pos.x + size_cur.x , pos.y + size_cur.y ) );    

        if( self_image )    
        {
            float x_scale = default_x_scale;

            if( reserve_iso_tile == false )
            {
                //transform.localScale = Vector3.one;
            }else{
                //transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
                x_scale *= -1.0f;
            }

            transform.localScale = new Vector3(x_scale,1.0f,1.0f);
        }
        OnState();
    }

    public  List<Vector2Int>    GetBounds_V2D()
    {
        List<Vector2Int> lt = new List<Vector2Int>();

        for( int x = boundsInt.min.x ; x < boundsInt.max.x + 1 ; x++ )
        {
            for( int y = boundsInt.min.y ; y < boundsInt.max.y + 1 ; y++ )
            {
                Vector2Int v = new Vector2Int( x , y );
                lt.Add(v);
            }
        }
        return lt;
    }

    public  bool    Contains_Vec2D( Vector2Int v2 )
    {
        BoundsInt bd_check = new BoundsInt();
        Vector3Int v_max = boundsInt.max + Vector3Int.one;
        bd_check.SetMinMax( boundsInt.min , v_max );   

        if( bd_check.Contains( new Vector3Int( v2.x , v2.y , 0 ) ) ) return true;

        return false;

    }

    virtual public  void    OnState(){}

    virtual public  void    OnBuildLayer( SJ_TileCell2D_Simple tile_layer ){}
} 
