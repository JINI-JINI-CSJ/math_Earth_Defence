using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// 2디 객체들 , 간단한 공간 분할
public class SJ_TileCell2D_Simple : MonoBehaviour
{
    public  Grid    grid;
    public  Tilemap    tilemap;

    public  int     chunk_size = 5;

    public  int     size_Field = 100000;

    // Key : 청크 사이즈로 나눈 단위
    // value: 객체 리스트
    public  Dictionary<Vector2Int,HashSet<SJ_TileCell2D_Simple_Unit>> dic_chunk_pos_set_unit = new Dictionary<Vector2Int, HashSet<SJ_TileCell2D_Simple_Unit>>();

    public  SJ_TileCell2D_Simple_Unit   cur_Unit_EditMode;

    public  Transform       tr_UnitPosCursor;

    public  float           offset_Y_editUnit;

    // 이미 설치되어 있던 객체를 수정하는 상태
    public  bool            recentEdit;
    public  Vector2Int      recentEdit_pos;
    public  bool            recentEdit_reserve_iso_tile;
    public  _SJ_GO_FUNC     func_Edit_Start_BuildObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {            

    }

    public  void    UpdatePos_CurUnit_MousePos()
    {
        Vector3 pos_mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        if( cur_Unit_EditMode != null )
        {
            UpdatePos_CurUnit( pos_mouse );
            Update_EditCursor();
        }
    }

    public  void    UpdatePos_CurUnit(int x , int y)
    {
        if( cur_Unit_EditMode != null )
        {
            UpdatePos_CurUnit( new Vector2Int( x , y ) );
            Update_EditCursor();
        }
    }

    public  void    Toggle_Reserve_CurUnit()
    {   
        if( cur_Unit_EditMode != null )
        {
            cur_Unit_EditMode.Toggle_IsoTile_Mirror();
        }
    }

    // 이미 설치된 것들에서 선택하여 에디트 모드 시작
    public  SJ_TileCell2D_Simple_Unit    Select_AlreadyBuildObj( Vector2Int v_pos_2d_cell )
    {
        // HashSet<SJ_TileCell2D_Simple_Unit> hs = GetSetUnit_WorldCellPos(v_pos_2d_cell);
        // if( hs != null )
        // {
        //     foreach( SJ_TileCell2D_Simple_Unit s in hs )
        //     {
        //         if( s.Contains_Vec2D( v_pos_2d_cell ) )
        //         {
        //             return s;
        //         }
        //     }
        // }

        HashSet<HashSet<SJ_TileCell2D_Simple_Unit>> hhs = GetSetUnit_List_ByBound( v_pos_2d_cell );

        foreach( HashSet<SJ_TileCell2D_Simple_Unit> s in hhs )
        {
            foreach( SJ_TileCell2D_Simple_Unit ss in s )
            {
                if( ss.Contains_Vec2D( v_pos_2d_cell ) )
                {
                    return ss;
                }
            }
        }

        return null;
    }

    public  SJ_TileCell2D_Simple_Unit        Check_Select_AlreadyBuildObj( Vector2Int v_pos_2d_cell )
    {
        SJ_TileCell2D_Simple_Unit unit = Select_AlreadyBuildObj( v_pos_2d_cell );
        if( unit != null )
        {
            RemoveUnit( unit );
            Add_EditMode_NewUnit(unit , false);
        }
        return unit;
    }


    public  bool    Add_EditMode_NewUnit( SJ_TileCell2D_Simple_Unit unit , bool update_pos_cam = true )
    {
        if( cur_Unit_EditMode != null )
        {
            Debug.LogError( "이미 작업중인 객체 있음!" );
            return false;            
        }

        cur_Unit_EditMode = unit;
        cur_Unit_EditMode.edit_mode_state = SJ_TileCell2D_Simple_Unit._EDIT_MODE_STATE.None;

        if( update_pos_cam )
        {
            UpdatePos_CurUnit(Camera.main.transform.position);
        }
        else
            Check_DuplCell();

        Update_EditCursor();
        return true;
    }

    public  void    Update_EditCursor()
    {
        if( cur_Unit_EditMode == null ) return;
        tr_UnitPosCursor.position = CellGrid_ToWorldPos( cur_Unit_EditMode.pos );

        cur_Unit_EditMode.transform.parent = tr_UnitPosCursor;

        Vector3 v = Vector3.zero;
        v.z -= 10.0f;
        v.y += offset_Y_editUnit;
        cur_Unit_EditMode.transform.localPosition = v;        
    }

    public  void    Cancel_EditMode_NewUnit( bool destroy = true )
    {
        if( cur_Unit_EditMode == null ) return;

        if( destroy )
            GameObject.Destroy( cur_Unit_EditMode.gameObject ); 

        cur_Unit_EditMode = null;
    }

    public  bool    UpdatePos_CurUnit( Vector3 world_pos ) 
    {        
        world_pos.z = 0;
        Vector3Int  v_cell      = grid.WorldToCell( world_pos );
        return UpdatePos_CurUnit( new Vector2Int( v_cell.x , v_cell.y ) );
    }
    public  bool    UpdatePos_CurUnit( Vector2Int v_pos_2d_cell ) 
    {
        Vector3     v_cell_pos  = grid.CellToWorld( new Vector3Int(v_pos_2d_cell.x , v_pos_2d_cell.y , 0) );

//Debug.Log( "UpdatePos_CurUnit : " + world_pos + ":" + v_pos_2d_cell );

        if( cur_Unit_EditMode.edit_mode_state == SJ_TileCell2D_Simple_Unit._EDIT_MODE_STATE.None ||
            Vector2Int.Equals( cur_Unit_EditMode.pos , v_pos_2d_cell ) == false )
        {
        }else{
            return false;
        }
        cur_Unit_EditMode.SetPos( v_pos_2d_cell );
        Check_DuplCell();
        return true;
    }

    virtual public  bool    OnFirstCheck_ObjCellAble( SJ_TileCell2D_Simple_Unit unit ){ return true; }

    public  bool    Check_InField( BoundsInt bd )
    {
        if( bd.min.x < -size_Field || size_Field < bd.max.x ) return false;
        if( bd.min.y < -size_Field || size_Field < bd.max.y ) return false;
        return true;
    }

    public  bool    Check_DuplCell()
    {
        bool b=false;
        cur_Unit_EditMode.edit_mode_state = SJ_TileCell2D_Simple_Unit._EDIT_MODE_STATE.Not;

        if( OnFirstCheck_ObjCellAble( cur_Unit_EditMode ) )
        {
            if( Check_InField( cur_Unit_EditMode.boundsInt ) )
            {
                if( Check_DuplCell(cur_Unit_EditMode.boundsInt) == false )
                {
                    cur_Unit_EditMode.edit_mode_state = SJ_TileCell2D_Simple_Unit._EDIT_MODE_STATE.Able;            
                    b = true;
                }
            }
        }
        cur_Unit_EditMode.OnState();      
        return b;  
    }


    public  SJ_TileCell2D_Simple_Unit    End_EditMode()
    {
        if( cur_Unit_EditMode == null ) return null;

        if( recentEdit )
        {
            cur_Unit_EditMode.SetPos(recentEdit_pos);
            cur_Unit_EditMode.reserve_iso_tile = recentEdit_reserve_iso_tile;
            cur_Unit_EditMode.MakeBounds();
            AddUnit(cur_Unit_EditMode);
            recentEdit = false;
        }

        SJ_TileCell2D_Simple_Unit r = cur_Unit_EditMode;
        cur_Unit_EditMode = null;
        return r;
    }


    public  Vector2Int  WorldCellPos_ToChunkPos( Vector2Int pos )
    {
        return new Vector2Int( pos.x / chunk_size , pos.y / chunk_size );
    }

    public  bool    Check_DuplCell( Vector2Int v_cell )
    {
        BoundsInt bd = new BoundsInt(v_cell.x, v_cell.y , 0 , 0,0,0);
        return Check_DuplCell(bd);
    }

    // 겹치면 true
    public  bool    Check_DuplCell( BoundsInt bd )
    {
        HashSet< HashSet<SJ_TileCell2D_Simple_Unit> > hs_hs_unit = GetSetUnit_List_ByBound( bd );
        foreach( HashSet<SJ_TileCell2D_Simple_Unit> s in hs_hs_unit )
        {
            foreach( SJ_TileCell2D_Simple_Unit ss in s )
            {
//Debug.Log( "Check_DuplCell : " + ss.name );
                if( OverLap_BoundsInt2D( ss.boundsInt , bd ) ) return true;
            }
        }
        return false;
    }

    public  bool    AddUnit_CurEditUnit()
    {
        if( cur_Unit_EditMode == null ) return false;
        if( Check_DuplCell() == false ) return false;
        AddUnit(cur_Unit_EditMode);
        End_EditMode();

        return true;
    }

    public  void    AddUnit( SJ_TileCell2D_Simple_Unit  unit )
    {
        HashSet<SJ_TileCell2D_Simple_Unit> hs = GetSetUnit_WorldCellPos( unit.pos ,  true );
        hs.Add(unit);
        unit.hs_cash = hs;
        unit.edit_mode_state = SJ_TileCell2D_Simple_Unit._EDIT_MODE_STATE.Build;

        unit.transform.parent = null;
        unit.transform.position = CellGrid_ToWorldPos( unit.pos );

        Vector3 v = unit.transform.position;
        v.z = transform.position.z;
        unit.transform.position = v;

        unit.OnState();
        unit.OnBuildLayer( this );
        OnAddUnit( unit );
    }

    virtual public  void    OnAddUnit( SJ_TileCell2D_Simple_Unit  unit ){}

    public  void    RemoveUnit( SJ_TileCell2D_Simple_Unit  unit )
    {
        HashSet<SJ_TileCell2D_Simple_Unit> hs =  unit.hs_cash;
        if( hs == null ) return;
        hs.Remove(unit);
        unit.hs_cash = null;
        unit.OnState();
    }

    static public   bool    OverLap_BoundsInt2D( BoundsInt a , BoundsInt b )
    {
        if( OverLap_IntRange( a.min.x , a.max.x , b.min.x , b.max.x ) == false ) return false;
        if( OverLap_IntRange( a.min.y , a.max.y , b.min.y , b.max.y ) == false ) return false;

        return true;
    }

    static public   bool    OverLap_IntRange( int a1 , int a2 , int b1 , int b2 )
    {
        if( a1 <= b1 && b1 <= a2 ||
            a1 <= b2 && b2 <= a2
        ) return true;

        if( b1 <= a1 && a1 <= b2 ||
            b1 <= a2 && a2 <= b2
        ) return true;

        return false;
    }

    public HashSet< HashSet<SJ_TileCell2D_Simple_Unit> >    GetSetUnit_List_ByBound( Vector2Int pos )
    {
        BoundsInt bd = new BoundsInt( pos.x , pos.y , 0,0,0,0 );


        return GetSetUnit_List_ByBound(bd);
    }

    public HashSet< HashSet<SJ_TileCell2D_Simple_Unit> >    GetSetUnit_List_ByBound( BoundsInt bd )
    {
        HashSet< HashSet<SJ_TileCell2D_Simple_Unit> >  hs_hs = new HashSet<HashSet<SJ_TileCell2D_Simple_Unit>>();
        HashSet<SJ_TileCell2D_Simple_Unit> hs = null;

        Vector2Int cp = WorldCellPos_ToChunkPos( new Vector2Int( bd.min.x , bd.min.y ) );

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x - 1, cp.y - 1 ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x    , cp.y - 1 ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x + 1, cp.y - 1 ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x - 1, cp.y  ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x    , cp.y  ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x + 1, cp.y  ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x - 1, cp.y + 1 ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x    , cp.y + 1 ) );
        if( hs != null ) hs_hs.Add(hs);

        hs = GetSetUnit_ChunkPos( new Vector2Int( cp.x + 1, cp.y + 1 ) );

        return hs_hs;
    }

    public  HashSet<SJ_TileCell2D_Simple_Unit> GetSetUnit_WorldCellPos( Vector2Int pos , bool create = false )
    {
        Vector2Int chunk_pos = WorldCellPos_ToChunkPos( pos );
        HashSet<SJ_TileCell2D_Simple_Unit> h = null;
        if( dic_chunk_pos_set_unit.TryGetValue( chunk_pos ,out h ) )
        {
            return h;
        }

        HashSet<SJ_TileCell2D_Simple_Unit> hs = null;
        if( create )
        {
            hs = new HashSet<SJ_TileCell2D_Simple_Unit>();
            dic_chunk_pos_set_unit[chunk_pos] = hs;            
        }

        return hs;
    }

    public  HashSet<SJ_TileCell2D_Simple_Unit> GetSetUnit_ChunkPos( Vector2Int pos)
    {
        HashSet<SJ_TileCell2D_Simple_Unit> h = null;
        if( dic_chunk_pos_set_unit.TryGetValue( pos ,out h ) )
        {
            return h;
        }
        return null;
    }

    public  List<SJ_TileCell2D_Simple_Unit> GetAllUnit()
    {
        List<SJ_TileCell2D_Simple_Unit> lt = new List<SJ_TileCell2D_Simple_Unit>();

        foreach( HashSet<SJ_TileCell2D_Simple_Unit> s in dic_chunk_pos_set_unit.Values )
        {
            lt.AddRange( s );
        }
        return lt;
    }

    public  void    DeleteAll()
    {
        foreach( HashSet<SJ_TileCell2D_Simple_Unit> s in dic_chunk_pos_set_unit.Values )
        {
            foreach( SJ_TileCell2D_Simple_Unit obj in s )
            {
                GameObject.DestroyImmediate( obj.gameObject );
            }
        }

        dic_chunk_pos_set_unit.Clear();
    }

    public  void    Fill_TileMapBase( TileBase tb , Vector2Int v )
    {
        tilemap.SetTile( new Vector3Int( v.x,v.y,0 ) , tb);
    }

    public  void    MousePos_To_CellGrid( ref Vector2Int v_pos_2d_cell , ref Vector3 v_cell_pos  )
    {
        Vector3 pos_mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        pos_mouse.z = 0;
        WorldPos_To_CellGrid( pos_mouse , ref v_pos_2d_cell , ref v_cell_pos );
    }

    public  void    WorldPos_To_CellGrid( Vector3 world_pos , ref Vector2Int v_pos_2d_cell , ref Vector3 v_cell_pos )
    {
        Vector3Int  v_cell      = grid.WorldToCell( world_pos );
        v_pos_2d_cell = new Vector2Int( v_cell.x , v_cell.y );        
        v_cell_pos  = grid.CellToWorld( v_cell );
    }


    public  Vector3    CellGrid_ToWorldPos( Vector2Int v_pos_2d_cell )
    {       
        return grid.CellToWorld( new Vector3Int(v_pos_2d_cell.x , v_pos_2d_cell.y , 0) );
    }


    public  HashSet<Vector2Int> GetEmptyCell_ByChunk( Vector2Int pos_2d )
    {
        HashSet<Vector2Int>                 hs_pos_cell = new HashSet<Vector2Int>();
        HashSet<SJ_TileCell2D_Simple_Unit>  hs_unit     = GetSetUnit_WorldCellPos( pos_2d );
        Vector2Int                          chunk_pos   = WorldCellPos_ToChunkPos( pos_2d );

        if( hs_unit == null ) return null;

        for( int x = 0 ; x < chunk_size ; x++ )
        {
            for( int y = 0 ; y < chunk_size ; y++ )
            {
                Vector2Int v = new Vector2Int( chunk_pos.x * chunk_size + x , chunk_pos.y * chunk_size + y );
                hs_pos_cell.Add(v);
            }
        }

        foreach( SJ_TileCell2D_Simple_Unit s in hs_unit )
        {
            List<Vector2Int>    cell_size = s.GetBounds_V2D();
            foreach( Vector2Int v2 in cell_size )
            {
                if( hs_pos_cell.Contains( v2 ) )  hs_pos_cell.Remove(v2);
            }
        }

        return hs_pos_cell;
    }


}
