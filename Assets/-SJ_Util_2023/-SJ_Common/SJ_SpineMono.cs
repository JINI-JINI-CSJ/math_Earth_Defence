using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SJ_SpineMono : MonoBehaviour
{
    SkeletonAnimation   spine_skel_ani;
    SkeletonGraphic     spine_skel_grp;
    Skeleton            skeleton;
    public  Dictionary<string,string>   dic_part_skin = new Dictionary<string, string>();
    public  Dictionary<string,Color>   dic_part_skin_Color = new Dictionary<string, Color>();

    //public  List<Material>        lt_inst_mat;

    [System.Serializable]
    public  class _SKIN_DEFAULT
    {
        public  string      part;
        public  string      skin_name;
    }

    public  List<_SKIN_DEFAULT> lt_SKIN_DEFAULT;

    public  string  start_ani;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {

        check_SkelAni();
 
        First_Play_Ani();

        if( lt_SKIN_DEFAULT.Count > 0 )
        {
            foreach( _SKIN_DEFAULT s in lt_SKIN_DEFAULT )
            {
                Add_Skin( s.part , s.skin_name );
            }
            Update_Skin();
        }
    }

    public  void    First_Play_Ani()
    {
        if( string.IsNullOrEmpty( start_ani ) == false )
        {
            Spine.AnimationState ani_state  = GetAnimationState();
            ani_state.ClearTracks();
            Play_SpineAni( start_ani , false );
        }
    }

    void    check_SkelAni()
    {
        if( spine_skel_ani == null ) spine_skel_ani = GetComponent<SkeletonAnimation>();
        if( spine_skel_grp == null ) spine_skel_grp = GetComponent<SkeletonGraphic>();

        //spine_skel_ani.maskMaterials

        if( skeleton == null )
        {
            if( spine_skel_ani != null )skeleton = spine_skel_ani.skeleton;
            if( spine_skel_grp != null )skeleton = spine_skel_grp.Skeleton;            
        }

        
    }

    public  void    Add_Skin( string part , string skin )
    {
        dic_part_skin[part] = skin;
    }

    public  void    Del_Skin( string part )
    {
        dic_part_skin[part] = "";
    }

    public  void    Add_Skin_Color( string part , Color col )
    {
        dic_part_skin_Color[part] = col;
    }

    public  void    Del_Skin_Color( string part )
    {
        if( dic_part_skin_Color.ContainsKey(part) )
        {
            dic_part_skin_Color.Remove( part );
        }
    }

    public  void    Update_Skin()
    {
        check_SkelAni();
        Skin new_skin = new Skin("temp");

        if( skeleton == null )
        {
            return;
        }

        foreach( KeyValuePair<string,string> s in dic_part_skin )
        {
            string skin_name = s.Value;
            if( string.IsNullOrEmpty( skin_name ) == false )
            {
                Skin skin_find = skeleton.Data.FindSkin(skin_name);

                if( skin_find != null )
                {
                    Skin skin_inst = new Skin("sub");

                    skin_inst.CopySkin( skin_find );                    

                    Color col = Color.white;
                    if( dic_part_skin_Color.TryGetValue( s.Key , out col ) )
                    {

                        foreach( Skin.SkinEntry ss in skin_inst.Attachments )
                        {
                            Spine.MeshAttachment ma = ss.Attachment as MeshAttachment;
                            if(  ma != null )ma.SetColor( col );

                            Spine.RegionAttachment ra = ss.Attachment as RegionAttachment;
                            if(  ra != null )ra.SetColor(  col);
                        }                        
                    }
                    new_skin.AddSkin(skin_inst);                    
                }
            }
        }
        skeleton.SetSkin(new_skin);
        skeleton.SetSlotsToSetupPose();
    }

    public  void    GetAttachment_CurSkin( List<Spine.MeshAttachment> lt_ma , List<Spine.RegionAttachment> lt_ra )
    {
        check_SkelAni();
        if( skeleton == null )
        {
            return;
        }

        foreach( Skin skin in skeleton.Data.Skins )
        {
            foreach( Skin.SkinEntry ss in skin.Attachments )
            {
                Spine.MeshAttachment ma = ss.Attachment as MeshAttachment;
                if(  ma != null )lt_ma.Add(ma);

                Spine.RegionAttachment ra = ss.Attachment as RegionAttachment;
                if(  ra != null )lt_ra.Add(ra);
            }              
        }
    }

    public  void    Change_Color_CurSkin( Color col )
    {
        List<Spine.MeshAttachment>      lt_ma = new List<MeshAttachment>();
        List<Spine.RegionAttachment>    lt_ra = new List<RegionAttachment>();

        GetAttachment_CurSkin( lt_ma , lt_ra );
        foreach( Spine.MeshAttachment s in lt_ma ) s.SetColor( col );
        foreach( Spine.RegionAttachment s in lt_ra ) s.SetColor( col );
    }

    // public  void    NewInstMat()
    // {
    //     foreach( Material s in lt_inst_mat )GameObject.Destroy( s );
    //     lt_inst_mat.Clear();

    // }

    public  void    Change_Color_InstMat_TintColor( Color col_mul , Color col_add  )
    {
        // inst_mat.SetColor( "_Color" , col_mul );
        // inst_mat.SetColor( "_Black" , col_add );
        // if( lt_inst_mat.Count < 1 )
        // {
        //     //spine_skel_ani.
        // }

        foreach( Slot s in skeleton.Slots )
        {
            s.HasSecondColor = true;
            s.R2 = col_add.r;
            s.G2 = col_add.g;
            s.B2 = col_add.b;
            
            //s.SetColor(col_add);
        }

    }

    Coroutine coroutine_play_ani;

    public  Spine.AnimationState    GetAnimationState()
    {
        Spine.AnimationState ani_state = null;
        if( spine_skel_ani != null )ani_state = spine_skel_ani.AnimationState;
        if( spine_skel_grp != null )ani_state = spine_skel_grp.AnimationState;
        return ani_state;
    }



    public  bool    Check_CurAni( string ani , int track_idx = 0 )
    {
        Spine.AnimationState ani_state = GetAnimationState();

        TrackEntry te = ani_state.GetCurrent(track_idx);
        
        if( te != null && te.Animation.Name == ani) return true;
        return false;
    }

    public  void    Play_SpineAni( string ani , bool loop = true , string next_loop_ani = "" ,  int track_idx = 0 , MonoBehaviour mono_recv = null , string func_recv = "" )
    {
        if( string.IsNullOrEmpty(ani) ) return;

        check_SkelAni();

        Spine.AnimationState ani_state  = GetAnimationState();

        TrackEntry te = ani_state.GetCurrent(track_idx);
        
        if( te != null && te.Animation.Name == ani) return;
        
        ani_state.SetAnimation( track_idx , ani,loop );

        if( string.IsNullOrEmpty(next_loop_ani) == false || mono_recv != null )
        {
            Skeleton skeleton = null;
            if( spine_skel_ani != null )skeleton = spine_skel_ani.skeleton;
            if( spine_skel_grp != null )skeleton = spine_skel_grp.Skeleton;        

            Spine.Animation ani_spine = skeleton.Data.FindAnimation( ani );
            if( ani_spine == null ) return;            

            if( coroutine_play_ani != null )
                StopCoroutine( coroutine_play_ani );

            if( string.IsNullOrEmpty(next_loop_ani) == false )
                coroutine_play_ani = StartCoroutine( CO_PlayNext( ani_spine.Duration , next_loop_ani , track_idx ) );

            if( mono_recv != null )
            {
                StartCoroutine( CO_EndRecvAni( ani_spine.Duration , mono_recv , func_recv ) );
            }
        }
    }

    IEnumerator CO_PlayNext( float wait , string next_ani , int track_idx )
    {
        yield return new WaitForSeconds(wait);
        Play_SpineAni( next_ani , true , "" , track_idx );
    }

    IEnumerator CO_EndRecvAni(  float wait , MonoBehaviour mono , string func )
    {
        //Debug.Log( "CO_EndRecvAni : " + wait );
        yield return new WaitForSeconds(wait);
        mono.SendMessage( func , SendMessageOptions.DontRequireReceiver );
    }

    // private void HandleAnimationStateEvent_Complete (TrackEntry trackEntry) {

    //     Debug.Log( trackEntry );

    //     if( string.IsNullOrEmpty(temp_next_ani) == false )
    //     {
    //         Play_SpineAni( temp_next_ani , true , "" , temp_next_ani_track_idx );
    //         temp_next_ani = "";
    //     }
    // }

    public  void    Copy_Other_Skin( SJ_SpineMono sJ_Spine )
    {
        dic_part_skin = new Dictionary<string, string>( sJ_Spine.dic_part_skin );
        Update_Skin();
    }
}
