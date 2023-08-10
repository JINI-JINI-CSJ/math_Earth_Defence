using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public	class _SJ_GO_FUNC
{
	public	bool	debug;

	public	GameObject		go;
	public	MonoBehaviour	mono;
	public	string			func;

	public	object			arg_obj;

	public	bool	IsEq( _SJ_GO_FUNC other )
	{
		if( other.go != go || other.func != func ) return false;
		return true;
	}

	public	void	Init()
	{
		go = null;mono = null;func="";arg_obj = null;
	}

	public	void	Set( GameObject _go = null , string _func = "" , object _arg = null)
	{
		go = _go;func = _func;arg_obj = _arg;
		mono = null;
	}

	public	void	SetMono( MonoBehaviour _mono = null , string _func = "" , object _arg = null)
	{
		mono = _mono;func = _func;arg_obj = _arg;
		go = null;
	}

	public	void	Func(object arg = null ) 
	{
		if( go != null )
		{ 
			if(arg_obj != null) SJ_Unity.SendMsg( go, func, arg_obj , debug );
			else				SJ_Unity.SendMsg( go ,func , arg , debug );
		}else if( mono != null ){
			if(arg_obj != null) SJ_Unity.SendMsg( mono, func, arg_obj , debug );
			else				SJ_Unity.SendMsg( mono ,func , arg , debug );
		}
	}
}

public class SJ_Unity
{
	static public bool SendMsg( MonoBehaviour mono , string func , object arg = null , bool debug = false )
	{
		if( mono == null || string.IsNullOrEmpty(func) )
		{
			return false; 
		}

		if( debug ) Debug.Log( "SendMsg : " + mono.name + " : " + func );

		if(arg == null)
		{
			mono.SendMessage(func , SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			mono.SendMessage(func, arg, SendMessageOptions.DontRequireReceiver);
		}
		return true;
	}

	static public bool SendMsg( GameObject mono , string func , object arg = null  , bool debug = false )
	{
		if( mono == null || string.IsNullOrEmpty(func) )
		{
			return false; 
		}

		//Debug.Log( "SendMsg : " + mono.name + " : " + func );

		if( debug ) Debug.Log( "SendMsg : " + mono.name + " : " + func );

		if(arg == null)
		{
			mono.SendMessage(func , SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			mono.SendMessage(func, arg, SendMessageOptions.DontRequireReceiver);
		}
		return true;
	}


	static	public	void	SetEqTrans( Transform self , Transform other = null , Transform par = null )
	{
		if( par != null )
		{
			self.parent = par;
		}

		if( other != null )
		{
			self.position = other.position;
			self.rotation = other.rotation;
			self.localScale = other.localScale;
		}
		else
		{
			self.localPosition = Vector3.zero;
			self.localRotation = Quaternion.identity;
			self.localScale = Vector3.one;
		}
	}

	public static float ObjColorLerpTime(GameObject go, float cur, float total, Color col)
	{
		if (cur < 0)
			return -1.0f;

		cur -= Time.deltaTime;
		float fRatio = 1.0f - cur / total;
		if (cur < 0.0f)
		{
			fRatio = 1.0f;
		}

		Color cur_col = Color.Lerp(col, new Color(1, 1, 1, 1), fRatio);

		SkinnedMeshRenderer[] sm_list = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer sm in sm_list)
		{
			sm.material.color = cur_col;
		}
		MeshRenderer[] ma_list = go.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer m in ma_list)
		{
			m.material.color = cur_col;
		}

		return cur;
	}

	public static void ObjRenderShowHide(GameObject go, bool bFlag)
	{
		SkinnedMeshRenderer[] sm_list = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer sm in sm_list)
		{
			sm.enabled = bFlag;
		}
		MeshRenderer[] ma_list = go.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer m in ma_list)
		{
			m.enabled = bFlag;
		}
	}

	public static void ObjTextureChange(GameObject go, Texture tex, string meshName = "" )
	{
		SkinnedMeshRenderer[] sm_list = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer sm in sm_list)
		{
			if( string.IsNullOrEmpty( meshName ) )
			{
				sm.material.SetTexture("_MainTex", tex);
				continue;
			}

			if ( sm.name == meshName) sm.material.SetTexture("_MainTex", tex);
		}
		MeshRenderer[] ma_list = go.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer m in ma_list)
		{
			if( string.IsNullOrEmpty( meshName ) )
			{
				m.material.SetTexture("_MainTex", tex);
				continue;
			}

			if (m.name == meshName)m.material.SetTexture("_MainTex", tex);
		}
	}

	public	static	void	ObjTextureChange(Renderer[] rd_list , Texture tex )
	{
		foreach (Renderer s in rd_list)
		{
			s.material.SetTexture("_MainTex", tex);
		}
	}

	public	static void		SetColor_Shader( Material mat , Color col , string param_name = "" )
	{
		if( string.IsNullOrEmpty( param_name ) )
		{
			param_name = "_MainColor";
		}

		mat.SetColor( param_name , col );
	}



	public	static	void	SetRender_RenderQ( GameObject go , int rdq )
	{
		Renderer[] rds = go.GetComponentsInChildren<Renderer>();
		foreach( Renderer s in rds ) s.material.renderQueue = rdq;
	}




	public	static	float	AnimationClip_CrossPlay( Animation ani , string ani_name , float speed=-1000.0f , float play_range=1.0f , 
		bool no_play = false , bool cross = true , bool queue = false )
	{
		if( ani == null || string.IsNullOrEmpty(ani_name)  ) return -1;

		AnimationState	st = ani[ani_name];

		if( st == null ) return -1;

		if( speed > -999.0f )
			st.speed = speed;

		AnimationClip clip = st.clip;

		float total_time = clip.length * st.speed * play_range;

		if( no_play == false )
		{
			if( queue == false )
			{
			 	if( cross )	ani.CrossFade( clip.name);
				else		ani.Play( clip.name);
			}
			else
			{
			 	if( cross )	ani.CrossFadeQueued( clip.name);
				else		ani.PlayQueued( clip.name);
			}
		}  

		return total_time;
	}

	public	static	float	AnimationClip_CrossPlay( Animation ani , AnimationClip clip , float speed=-1.0f , float play_range=1.0f , 
		bool no_play = false , bool cross = true , bool queue = false )
	{
		if( ani == null || clip == null ) return -1;
		AnimationState	st = ani[clip.name];
		float total_time = 0;
		if( st != null )
		{
			if( speed > 0 )
			{
				st.speed = speed;
			}
			else
			{
				speed = st.speed;
			}
		}
		total_time = clip.length * speed * play_range;
		total_time = Mathf.Abs(total_time);
		if( no_play == false )
		{
			if( queue == false )
			{
			 	if( cross )	ani.CrossFade( clip.name );
				else		ani.Play( clip.name);
			}
			else
			{
			 	if( cross )	ani.CrossFadeQueued( clip.name);
				else		ani.PlayQueued( clip.name);
			}
		}  
		return total_time;
	}


	// 애니메이터 스테이트 시간만큼 배속 변경해서 플레이
	// https://wergia.tistory.com/41
	public	static	void	AnimatorPlay_TotalTimeSpeed(Animator anit , AnimationClip clip , string state_name , string para_name ,  float playTime )
	{
		if( anit == null || clip == null ) return;
		float spd = clip.length / playTime;
		anit.SetFloat( para_name , spd );
		anit.Play(state_name);
	}


	public	static float	GetTime_AniClip(Animation ani , AnimationClip clip )
	{
		if( ani == null || clip == null ) return -1;
		AnimationState	st = ani[clip.name];

		if( st == null )
		{
			Debug.LogError( "Error!! : GetTime_AniClip : st == null : clip.name : " + clip.name );
			return 0;
		}

		return clip.length * st.speed;
	}

	// 애니 플래이
	// 지정된 시간 , 지정된 애니 정규화 된 시간
	// 애니의 normalize_time 만큼 time 동안 재생
	public	static	void	AnimationClip_Play_Time_Ratio( Animation ani , AnimationClip clip , float time , float normalize_time=1.0f , bool cross = false )
	{
		if( ani == null || clip == null ) return;

		AnimationState	st = ani[clip.name];
		float total_time = st.length * normalize_time;

		st.speed = total_time / time;

		if( cross )	ani.CrossFade( clip.name);
		else		ani.Play( clip.name);
	}


	public	static	void	Particle_Active( GameObject go , bool b )
	{
		if( go == null ) return;

		if( b ) go.SetActive(true);

		ParticleSystem[] par_list = go.GetComponentsInChildren<ParticleSystem>();
		foreach( ParticleSystem s in par_list )
		{
			if( b )
				s.Play();
			else
				s.Stop();
		}
	}


	// 랜덤 리스트 계열
	public	static	object	Array_RandomObj( object[] ar )
	{
		if(	ar.Length < 1 )return null;
		int i = UnityEngine.Random.Range( 0 , ar.Length );
		return ar[i];
	}

	public	static T GetArray_Random<T>( T[] arr )
	{
		if( arr.Length < 1 )
			return default(T);

		int sel = UnityEngine.Random.Range( 0 , arr.Length );
		return arr[sel];
	}


	public	static	int		List_Add_Or_Set<T>(List<T> list , T t ,int index )
	{
		if( index < 0 )
		{
			list.Add(t);
			return 1;
		}
		if(	list.Count <= index )
		{
			int add_count = (index+1) - list.Count;
			for( int i = 0 ; i < add_count ; i++ )list.Add(t);
			return add_count;
		}
		list[index] = t;
		return 1;
	}

	//
	// 리스트에 있는걸 갯수만큼 섞어서 리턴
	//
	public	static	List<T>	GetRandom_ListMix<T>( List<T> list , int count )
	{
		List<T> lt = new List<T>( list );
		List<T> lt_t = new List<T>();

		for( int i = 0 ; i < count ; i++ )
		{
			if( lt.Count < 1) return lt_t;
			int idx = UnityEngine.Random.Range( 0, lt.Count );
			T t = lt[idx];
			lt.RemoveAt( idx );
			lt_t.Add( t );
		}

		return lt_t;
	}


	public	static T GetRandom_Pop<T>( List<T> lt )
	{
		if( lt.Count < 1 ) return default(T);
		int idx = UnityEngine.Random.Range( 0, lt.Count );
		T t = lt[idx];
		lt.RemoveAt(idx);
		return t;
	}


	public static void GameObj_OneSetActive(GameObject[] go_ar, string go_name)
	{
		foreach (GameObject go in go_ar)
		{
			if (go.name == go_name) go.SetActive(true);
			else go.SetActive(false);
		}
	}

	public	static	void	Child_OneSetActive(GameObject go_parent, string go_name)
	{
		for( int i = 0 ; i < go_parent.transform.childCount ; i++ )
		{
			Transform tr_ch = go_parent.transform.GetChild(i);
			if (tr_ch.gameObject.name == go_name) tr_ch.gameObject.SetActive(true);
			else tr_ch.gameObject.SetActive(false);
		}
	}


	public static string GetAppPathFile(string filename , bool asset_folder = false )
	{
#if UNITY_EDITOR
		string f = "";
		if (asset_folder == false)
		{
			f = Application.persistentDataPath + "/";
			f += filename;
		}
		else
		{
			f = Application.dataPath;
			f = f.Substring(0, f.LastIndexOf('/'));
			f += "/Assets/Resources/";
		}

		Debug.Log(f);

		return (f);
#else

		if (!Directory.Exists(Application.persistentDataPath ))Directory.CreateDirectory(Application.persistentDataPath);

		string path = Application.persistentDataPath + "/" + filename;

		Debug.Log("and_Path : " + path );
		return path;
#endif
	}

	public	static	StreamWriter	FileCreateWriteTxt( string filename , bool asset_folder = false )
	{
		string path_file =	GetAppPathFile( filename , asset_folder );
		StreamWriter sw =	File.CreateText( path_file );
		return sw;
	}


	public	static	StreamReader 	FileOpenReadTxt( string filename , bool asset_folder = false )
	{
		string path_file =	GetAppPathFile( filename , asset_folder );
		if( !File.Exists(path_file) )
		{
			return null;
		}
		StreamReader sr;
		sr =	File.OpenText( path_file );
		return sr;
	}

	public	static	string 	FileOpenReadTxt_All( string filename , bool asset_folder = false )
	{
		string path_file =	GetAppPathFile( filename , asset_folder );
		if( !File.Exists(path_file) )
		{
			return null;
		}
		StreamReader sr;
		sr =	File.OpenText( path_file );
		string str_data = sr.ReadToEnd();
		sr.Close();
		return str_data;
	}

	public	static	string FileOpen_ReadLine( string filename )
	{
		StreamReader sr = FileOpenReadTxt(filename);
		if (sr == null) 
		{
			return "";
		}
		string	str_list =	sr.ReadLine();
		sr.Close();
		return str_list;
	}

	public	static	bool FileCreate_WriteLine(string filename , string text , bool new_create = true )
	{
		string path_file =	GetAppPathFile( filename );

		StreamWriter sw = null;

		if( new_create )
		{
			sw =	File.CreateText( path_file );
			Debug.Log( " FileCreate_WriteList :  " + path_file );
		}
		else 
			sw =	File.AppendText( path_file );
		//Debug.Log( " FileCreate_WriteList :  " + path_file );

		if( sw == null )
		{
			Debug.Log( "Error!!!!!!! FileCreate_WriteList : " + path_file );
			return false;
		}

		sw.WriteLine( text );
		sw.Close();

		return true;
	}

	public	static	BinaryWriter  FileCreate_Bin(string filename )
	{
		string path_file =	GetAppPathFile( filename );
		FileStream fs = File.Create( path_file );
		if( fs == null ) return null;
		BinaryWriter bw = new BinaryWriter(fs);
		return	bw;
	}

	public	static	BinaryReader  FileLoad_Bin(string filename )
	{
		string path_file =	GetAppPathFile( filename );

		if( File.Exists(path_file) == false ) return null;

		FileStream fs =	File.OpenRead( path_file );
		if( fs == null ) return null;
		BinaryReader br = new BinaryReader( fs );
		return br;
	}

	public	static	byte[]  FileLoad_Bin_Buff(string filename )
	{
		string path_file =	GetAppPathFile( filename );
		if( File.Exists(path_file) == false ) return null;
		FileStream fs =	File.OpenRead( path_file );
		if( fs == null ) return null;
		if( fs.Length < 1 )
		{
			fs.Close();
			return null;
		}
		byte[] buff = new byte[ fs.Length ];
		fs.Read(buff , 0 , (int)fs.Length);
		fs.Close();
		return buff;
	}

	public static	bool	PerRandom( int ratio , int max = 100 )
	{
		int r = UnityEngine.Random.Range( 0 , max );
		if( r < ratio ) return true;
		return false;
	}

	public static	bool	PerRandom( float ratio , float max = 100.0f )
	{
		float r = UnityEngine.Random.Range( 0 , max );
		if( r < ratio ) return true;
		return false;
	}

	// public static void SetList<T>( List<T> list )

	public static T Random_RangeStepList_T<T>(List<T> list)
	{
		List<int>	list_int = new List<int>();
		foreach( T s in list )
		{
			RANDOM_RANGE_STEP_BASE b = s as RANDOM_RANGE_STEP_BASE;
			list_int.Add(b.ratio);
		}
		int idx = Random_RangeStepList( list_int.ToArray() );
		return list[idx];
	}

	public static int Random_RangeStepList(int[] nRatioList)
	{
		if (nRatioList.Length < 1)
			return 0;
		int[] step_list = new int[nRatioList.Length];
		int nAcc = 0;
		for (int i = 0; i < nRatioList.Length; i++)
		{
			nAcc += nRatioList[i];
			step_list[i] = nAcc;
		}
		int nVal = UnityEngine.Random.Range(0, nAcc);
		int pre_step = 0;
		for (int i = 0; i < nRatioList.Length; i++)
		{
			if (step_list[i] != pre_step && step_list[i] > nVal)
				return i;
			pre_step = step_list[i];
		}
		return 0;
	}


	public	static	void	SetLayer_Obj( GameObject go , string layer_name , bool child )
	{
		go.layer = LayerMask.NameToLayer( layer_name );
		if( child )
		{
			for( int i = 0 ; i < go.transform.childCount ; i++ )
			{
				go.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer( layer_name );
			}
		}
	}

	public	static	bool	Collider_RayCheck_ByWorld( Vector3 pos_world , Collider coll )
	{
		pos_world.z = -2000.0f;
		Ray ray = new Ray(pos_world ,new Vector3( 0,0,1 ) );
		RaycastHit	hit;
		if(	coll.Raycast(ray , out hit , 100000 ) )
		{
			//Debug.Log(	"레이케스트!!! Collider_RayCheck_ByWorld" );
			return true;
		}
		//Debug.Log(	"안됨~~~ 레이케스트   Collider_RayCheck_ByWorld" );
		return false;
	}

	public	static	bool	Collider_RayCheck_ByMouse( Collider coll , Camera _cam = null )
	{
		Camera cam = Camera.main;
		if( _cam != null ) cam = _cam;

		Ray ray = cam.ScreenPointToRay( Input.mousePosition );
		RaycastHit	hit;
		if(	coll.Raycast(ray , out hit , 100000 ) )
		{
			//Debug.Log(	"레이케스트!!! Collider_RayCheck_ByWorld" );
			return true;
		}
		//Debug.Log(	"안됨~~~ 레이케스트   Collider_RayCheck_ByWorld" );
		return false;
	}

	public static Vector3? Point_RayCheck_ByMouse(Collider coll, Camera _cam = null)
	{
		Camera cam = Camera.main;
		if (_cam != null) cam = _cam;

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (coll.Raycast(ray, out hit, 100000))
		{
			return hit.point;
		}
		return null;
	}

	public	static	Collider PickMouse_RayCast( int layer_mask )
	{
		RaycastHit hit;
		Ray ray =	Camera.main.ScreenPointToRay( Input.mousePosition );
		if(	Physics.Raycast( ray , out hit , 10000 , layer_mask ) )
		{
			return hit.collider;
		}
		return null;
	}

	public	static	RaycastHit[] PickMouse_RayCastAll( int layer_mask )
	{
		Ray ray =	Camera.main.ScreenPointToRay( Input.mousePosition );
		return 	Physics.RaycastAll( ray , 10000 , layer_mask );
	}

	public static Collider2D PickMouse_RayCast2D(int layer_mask)
	{
		Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Ray2D ray = new Ray2D(wp, Vector2.zero);
		RaycastHit2D hit = Physics2D.Raycast( ray.origin , ray.direction , 10000 , layer_mask);
		//RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
		if (hit.collider != null)
		{
			return hit.collider;
		}
		return null;
	}


	static	public	Vector3 WorldPos_ToScreenPos( Camera cam_w , Camera cam_s , Vector3 obj_pos_w , Transform tr , Vector2 v_off = new Vector2() )
	{
		Vector3 vp = cam_w.WorldToViewportPoint( obj_pos_w );
		Vector3 vs = cam_s.ViewportToWorldPoint( vp );
		tr.position = vs;

		Vector3 vl = tr.localPosition;
		vl.z = 0;
		vl += new Vector3(v_off.x , v_off.y , 0 ) ;
		tr.localPosition = vl;
		
		return vs;
	}


	public static RaycastHit2D[] PickMouse_RayCast2DAll(int layer_mask)
	{
		Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Ray2D ray = new Ray2D(wp, Vector2.zero);
		RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 10000, layer_mask);
		return hits;
	}


		public	static	void	Delete_Child( Transform tr , bool sjpool_return = false )
	{
		if( tr == null ) return;

		List<Transform> list_ch = new List<Transform>();

		for( int i = 0 ; i < tr.childCount ; i++ )
		{
			list_ch.Add( tr.GetChild(i) );
		}

		foreach( Transform s in list_ch )
		{
			if( sjpool_return == false )
				GameObject.DestroyImmediate(s.gameObject);
			else 
				SJPool.ReturnInst_Or_Destroy( s.gameObject );
		}
	}

	public static	Transform	FindChild_All( Transform tr , string str )
	{
		Transform tr_child = tr.Find( str );
		if( tr_child != null ) return tr_child;

		for( int i=0; i < tr.childCount ; i++ )
		{
			tr_child = FindChild_All( tr.GetChild(i) , str );
			if( tr_child != null ) return tr_child;
		}
		return null;
	}

	static public string 	RemoveString_ForNum( string str , bool end_point_remove = true )
	{
		string str_new = str.Trim();

		str_new =	str_new.Replace( " " ,"");
		str_new =	str_new.Replace( "," ,"");
		str_new =	str_new.Replace( "\"" ,"");
		str_new =	str_new.Replace( "%" ,"");

		if( end_point_remove )
			str_new =	str_new.Replace( "." ,"");

		return str_new;
	}

	static public float TryParseFloat( string str )
	{
		float val = 0;
		float.TryParse( str , out val );
		return val;
	}

	static public int TryParseInt( string str )
	{
		if( str.Length < 1 ) return 0;

		int val = 0;
		int.TryParse( str , out val );
		try
		{
			int m = int.Parse(str);
		}
		catch (FormatException e)
		{
			Debug.Log(" TryParseInt " + "[" + str + "] " + e.Message);
		}

		return val;
	}

	static	public	bool	Check_TryParseInt(string str)
	{
		int val = -1;
		if(	int.TryParse( str , out val ) ) return true;
		return false;
	}


	static public List<string> Read_1Dim_String( string str )
	{
		List<string> list_str = new List<string>();
		if( string.IsNullOrEmpty( str ) ) return list_str;
		str =	str.Replace( "(" , "" );
		str =	str.Replace( ")" , "" );
		string[]	 str_ar = str.Split(',');
		list_str.AddRange( str_ar );
		return list_str;
	}

	static public  List<List<string>>	Read_2Dim_String( string str )
	{
		List<List<string>> list_par = new List<List<string>>();

		if (str.Length < 1)
		{
			//Debug.Log( "return!! Read_2Dim_String : str.Length < 1 " );
			return list_par;
		}

		string	str_count_1 = str.Substring( 0, 1 );
		int		count_1 = 0;
		if (int.TryParse(str_count_1, out count_1) == false)
		{
			Debug.Log( "return!! Read_2Dim_String : int.TryParse(str_count_1, out count_1) == false " );
			return null;
		}

		int find_start_idx = 0;
		for (int i = 0; i < count_1; i++)
		{
			if( str.Length <= find_start_idx ) break;

			int find_s = str.IndexOf( '(' , find_start_idx );
			int find_e = str.IndexOf( ')' , find_start_idx );

			if (find_s < 0 || find_e < 0)
			{
				Debug.Log( "return!! Read_2Dim_String : find_s < 0 || find_e < 0 " );
				break;
			}

			int read_s = find_s+1;
			int read_e = find_e;
			int read_len = read_e - read_s;
			if (read_len < 1)
			{
				Debug.Log( "return!! Read_2Dim_String : read_len < 1 " );
				Debug.Log( "find_s : " + find_s.ToString() );
				Debug.Log( "find_e : " + find_e.ToString() );
				break;
			}

			string		str_part = str.Substring( read_s , read_len );
			string[]	str_unit_ar = str_part.Split( ',' );
			List<string> str_unit_list = new List<string>();
			str_unit_list.AddRange( str_unit_ar );
			list_par.Add( str_unit_list );

			find_start_idx = find_e + 1;
		}

		return list_par;
	}


	static	public	GameObject	ResInst_Default( string _path , string _default_res , string _res )
	{
		string path = "";
		if( string.IsNullOrEmpty( _res ) == false ) path = _path + "/" + _res;
		else										path = _path + "/" + _default_res;
		GameObject prf_model = Resources.Load( path , typeof(GameObject) ) as GameObject;
		if( prf_model == null ) return null;
		GameObject go_model = GameObject.Instantiate( prf_model );
		return go_model;
	}


	static	public	void		Random_CreateBatch( Transform tr_par , Vector3 v3bb , int count , List<GameObject>	list_obj )
	{
		for( int i = 0 ; i < count ; i ++ )
		{
			Vector3 pos = SJ_Cood.Random_BoxBound(v3bb);
			GameObject go =	GetArray_Random<GameObject>(list_obj.ToArray());
			GameObject inst = GameObject.Instantiate( go );
			inst.transform.parent = tr_par;
			inst.transform.localPosition = pos;
		}
	}

	static	public	bool		SetAnit_LayerName( Animator anit , string layer , float weight = 1.0f )
	{
		if( anit == null ) return false;
		int idx =	anit.GetLayerIndex(layer);
		if( idx < 0 )return false;

		//Debug.Log( "=================== SetAnit_LayerName : " + layer + " : " + weight );

		anit.SetLayerWeight(idx , weight );
		return true;
	}


	static	public	bool	Check_InCamera( Vector3 pos , Camera _cam = null , float offset = 0 )
	{
		Camera cam = Camera.current;
		if( _cam != null ) cam = _cam;

		Vector3 v =	cam.WorldToViewportPoint( pos );
		if( v.z <= 0 ) return false;
		if( v.x < 0+offset || v.x > 1-offset || v.y < 0+offset || v.y > 1-offset ) return false;
		return true;
	}
	
	/// <summary>
	/// 
	/// 시작위치에서 타겟위치로 이동거리만큼 이동하고 
	/// 이동후 거리를 리턴한다.
	/// 
	/// </summary>
	/// <param name="tar"></param>
	/// <param name="start"></param>
	/// <param name="moveLen"></param>
	/// <returns></returns>

	static	public	float	CheckPlane_MoveLen( Vector3 tar , Vector3 start , float moveLen , ref Vector3 s_to_t_nor )
	{
		Vector3 t_to_s = start - tar;
		Vector3 s_to_t = tar - start;
		t_to_s.Normalize();
		s_to_t.Normalize();

		s_to_t_nor = s_to_t;

		Vector3 move_pos = start + (s_to_t*moveLen);
		Plane pl = new Plane( t_to_s , tar );
		return pl.GetDistanceToPoint( move_pos );
	}

	// 수치 범위 
	// 작은 수치부터 큰수치로 찾기
	static	public	int		Find_RangeList_INT_S_TO_L(List<int> list ,int val )
	{
		for( int i = 0 ; i < list.Count ; i++ )
		{
			int _v = list[i];
			if( _v <= val )
			{
				return i;
			}
		}
		return -1;
	}

	static	public	int		Find_RangeList_FLOAT_S_TO_L(List<float> list ,float val )
	{
		for( int i = 0 ; i < list.Count ; i++ )
		{
			float _v = list[i];
			if( _v <= val )
			{
				return i;
			}
		}
		return -1;
	}

	public	static	List<object>		ListT_TO_ListObj<T>(List<T> list )
	{
		List<object> lt_o = new List<object>();
		foreach( T t in list )
		{
			lt_o.Add( t );
		}
		return lt_o;
	}


	// 자식갯수만큼 액티브 나머지는 비 활성화
	static 	public	void 	Active_Child( Transform tr , int c )
	{
		if( tr == null ) return;
		for( int i = 0 ; i < tr.childCount ; i++ )
		{
			if( i < c )
			{
				tr.GetChild(i).gameObject.SetActive(true);
			}else{
				tr.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	static public	List<Transform>	GetChild( Transform tr_par )
	{
		List<Transform> lt = new List<Transform>();
		for( int i = 0 ; i < tr_par.childCount ; i++ )
		{
			Transform tr = tr_par.GetChild(i);
			lt.Add(tr);
		}
		return lt;
	}

	// 객체의 하위 갯수 
	static public	List<GameObject>	GetChildMake_Count( GameObject go_par , int count )
	{
		if( go_par.transform.childCount < 1 ) return null;
		List<GameObject> lt = new List<GameObject>();

		if( go_par.transform.childCount > count )
		{
			List<Transform> lt_del = new List<Transform>();
			for( int i = count ; i < go_par.transform.childCount ; i++ )
			{
				lt_del.Add( go_par.transform.GetChild( i ) );
			}

			foreach( Transform s in lt_del )
			{
				GameObject.DestroyImmediate( s.gameObject );
			}
		}else if( go_par.transform.childCount < count ){
			int add = count - go_par.transform.childCount;
			for( int i = 0 ; i < add ; i++ )
			{
				GameObject inst = GameObject.Instantiate( go_par.transform.GetChild( 0 ).gameObject );
				inst.transform.parent = go_par.transform;
			}
		}

		for( int i = 0 ; i < go_par.transform.childCount ; i++ )
		{
			lt.Add( go_par.transform.GetChild( i ).gameObject );
		}
		return lt;
	}

	static public	string 	Get_FileNameTime( string _name = "" , string ext = "" )
	{
		string year  =	DateTime.Now.Year.ToString();
		string month =  DateTime.Now.Month.ToString();
		string day   =  DateTime.Now.Day.ToString();
		string hour  =  DateTime.Now.Hour.ToString();
		string min   = 	DateTime.Now.Minute.ToString();
		string sec   =  DateTime.Now.Second.ToString();
		string mil_sec= DateTime.Now.Millisecond.ToString();

		string time_s = "_" + year + "-" + month + "-" + day + "-" + hour + "-" + min + "-" + sec + "-" + mil_sec;

		return _name + time_s + ext;
	}

	// 컴포넌트 문자열로 추가하기 
	static	public	Component	AddComponentStr( GameObject go , string str )
	{
		System.Type componentType = System.Type.GetType( str );
		if( componentType == null )
		{
			Debug.LogError( "에러!!! AddComponentStr : " + str   );
			return null;
		}
		Component comp = go.GetComponent(componentType);
		if( comp == null )
		{
			comp = go.AddComponent(componentType);
		}
		return comp;
	}

	// 카메라 거리
	// https://docs.unity3d.com/kr/2020.3/Manual/FrustumSizeAtDistance.html

	// 일정 거리만큼 떨어진 절두체의 높이(두 값 모두 월드 단위)는 다음 공식을 통해 구할 수 있습니다.
	//  var frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
	// 또한 이 과정을 반대로 하면 특정 절두체 높이일 때의 거리를 계산할 수 있습니다.
	//  var distance = frustumHeight * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
	// 거리와 높이를 모두 알고 있을 때에는 FOV 각도를 계산할 수도 있습니다.
	//  var camera.fieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / distance) * Mathf.Rad2Deg;
	// 각 계산식의 결과를 얻으려면 절두체의 높이를 알아야 하며, 절두체의 높이는 절두체의 너비를 이용하여 쉽게 구할 수 있습니다(절두체의 높이를 이용하여 너비를 계산할 수도 있음).
	// var frustumWidth = frustumHeight * camera.aspect;
	// var frustumHeight = frustumWidth / camera.aspect;

	static	public	float Get_CameraDistanceFrustum( Camera cam , float frustumHeight )
	{
		return frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
	}

	/// <summary>
	/// 에디터 함수
	/// </summary>

	static	public	void	ClearDevConsole()
	{
#if UNITY_EDITOR
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		var type = assembly.GetType("UnityEditor.LogEntries");
		var method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
#endif

	}


#if UNITY_EDITOR

	static	public	GameObject	Prefab_Load( string path , string name )
	{
		string localPath = "Assets/" + path + "/" + name + ".prefab";

		//Check if the Prefab and/or name already exists at the path
		GameObject obj = AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)) as GameObject;
		if( obj == null )
		{
			obj = new GameObject( name );
			obj = PrefabUtility.SaveAsPrefabAssetAndConnect( obj , localPath , InteractionMode.AutomatedAction );
		}
		else
		{
			obj =	GameObject.Instantiate( obj );
		}
		return obj;
	}

	static	public	void	Prefab_Apply( GameObject obj , bool delete_inst = false )
	{
		PrefabUtility.ApplyPrefabInstance( obj , InteractionMode.AutomatedAction );
		if( delete_inst )
		{
			GameObject.Destroy( obj );
		}
	}

	static	public	void	Prefab_Apply( MonoBehaviour mono )
	{
		PropertyModification[] prop_mod = PrefabUtility.GetPropertyModifications(mono);
		PrefabUtility.SetPropertyModifications(mono.gameObject, prop_mod);
	}
#endif



}
/// <summary>
/// 리스트 각 항목에  랜덤 확률을 넣고 
/// 그 확률만큼 나오게 하기
/// T 는 RANDOM_RANGE_STEP_BASE 를 상속 받은 항목
/// </summary>
public	class RANDOM_RANGE_STEP_BASE{public	int		ratio;}
public	class Random_RangeStepList
{

	static	public	List<int>	temp_int = new List<int>();
	static	public	object		obj_list_set;

	public static void SetList<T>( List<T> list )
	{
		obj_list_set = (object)list;
		int nAcc = 0;
		temp_int.Clear();
		foreach( T s in list )
		{
			RANDOM_RANGE_STEP_BASE b = s as RANDOM_RANGE_STEP_BASE;
			nAcc += b.ratio;
			temp_int.Add(nAcc);
		}
	}

	public	static T GetRandom<T>()
	{
		int nVal = UnityEngine.Random.Range(0, temp_int[temp_int.Count-1]);
		int pre_step = 0;
		int sel = 0;
		for (int i = 0; i < temp_int.Count; i++)
		{
			if (temp_int[i] != pre_step && temp_int[i] > nVal)
			{
				sel = i;
				break;
			}
			pre_step = temp_int[i];
		}

		List<T> list = obj_list_set as List<T>;
		return list[sel];
	}
}