using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SJGoPoolMng : MonoBehaviour 
{
	public	int 		m_nPoolObjType  = 0;
	public	int 		m_nAllocCount = 100;
	public	GameObject	m_go_BaseObj = null;
	public	List<GameObject> 	m_ltAllocObj = new List<GameObject>();
	public	Queue<GameObject>	q_UseAbleObj = new Queue<GameObject>();

	public	Dictionary<int ,GameObject >	dic_Obj = new Dictionary<int, GameObject>();

	int		new_Inst_First_Last; // 풀인덱스에서 어디 먼저 가져올껀지...  0 : 첫번째  , 1 : 마지막꺼

	void Awake()
	{

	}
	
	public	void InitMng()
	{
		if( m_go_BaseObj == null )
			return;
		
		for(int i = 0 ; i < m_nAllocCount ; i++ )
		{
			Add_Inst();
		}
	}

	public	void	Destroy_Mng()
	{
		foreach( GameObject s in m_ltAllocObj )
		{
			GameObject.DestroyImmediate( s );
		}
		m_ltAllocObj.Clear();
		q_UseAbleObj.Clear();
		dic_Obj.Clear();
	}

	void	Add_Inst()
	{
		GameObject obj = Instantiate( m_go_BaseObj ) as GameObject;
		SJGoPoolObj pool_obj = obj.GetComponent<SJGoPoolObj>();
		if( pool_obj == null )
		{
			Debug.LogError( "Error!!! SJGoPoolMng : SJGoPoolObj == null! : " + obj.name );
			return;
		}

		pool_obj.AllocInstSJ( m_go_BaseObj );
		pool_obj.m_cPoolMsg = this;
		pool_obj.UID = m_ltAllocObj.Count;
		pool_obj.m_bUse = false;
		obj.name = m_go_BaseObj.name;
		obj.SetActive(false);
		obj.transform.parent = transform;
		m_ltAllocObj.Add( obj );
		q_UseAbleObj.Enqueue( obj );
		dic_Obj[pool_obj.UID] = obj;

		// 비활성이라서 실행 안됨
		//obj.SendMessage( "OnAllocInstSJ" , SendMessageOptions.DontRequireReceiver );
	}

	
	public	GameObject	Find_ID( int _id )
	{
		GameObject f_obj = null;
		if( dic_Obj.TryGetValue( _id , out f_obj ) )
		{
			return f_obj;
		}
		return null;
	}
	
	public	GameObject GetNewInst( bool bParentNull = true )
	{
		if( q_UseAbleObj.Count < 1 )
		{
			SJGoPoolObj _pool_obj = m_go_BaseObj.GetComponent<SJGoPoolObj>();
			int add_count = (int)((float)_pool_obj.m_InstCount * _pool_obj.addInst_Ratio);

			if( add_count < 1 ) add_count = 5;

			for( int i = 0 ; i < add_count ; i++ )
			{
				Add_Inst();
			}

			//Debug.Log( "인스턴트 추가 : " + m_go_BaseObj.name + "   add_count : " + add_count );
		}

		GameObject obj = 	q_UseAbleObj.Dequeue();
		if( bParentNull ) obj.transform.parent = null;
		obj.SetActive(true);
		SJGoPoolObj pool_obj = obj.GetComponent<SJGoPoolObj>();
		pool_obj.m_bUse = true;
		return obj;
	}

	
	public	void 	ReturnInstDirect( GameObject obj )
	{
		SJGoPoolObj pool_obj = obj.GetComponent<SJGoPoolObj>();

		obj.SetActive(false);
		obj.transform.parent = transform;
		q_UseAbleObj.Enqueue( obj );
		pool_obj.m_bUse = false;
		pool_obj.EndInstSJ();
		obj.SendMessage("OnEndInstSJ", SendMessageOptions.DontRequireReceiver);
	}

	public	void 	ReturnAllInst()
	{
//		Debug.Log( "ReturnAllInst : " + gameObject.name );
		foreach( GameObject go in m_ltAllocObj )
		{
			SJGoPoolObj pool_obj = go.GetComponent<SJGoPoolObj>();
			if( pool_obj.m_bUse )
			{
				ReturnInstDirect(go);
				//Debug.Log( "ReturnAllInst : go :" + go.name );
			}
		}
	}

	public	void	GetInstAll_Use( List<GameObject> list )
	{
		foreach( GameObject go in m_ltAllocObj )
		{
			SJGoPoolObj pool_obj = go.GetComponent<SJGoPoolObj>();
			if( pool_obj.m_bUse )list.Add(go);
		}
	}
	
	public	void 	SendMsgAll( string func , object args = null , bool all = true )
	{
		foreach( GameObject go in m_ltAllocObj )
		{
			SJGoPoolObj pool_obj = go.GetComponent<SJGoPoolObj>();
			if( pool_obj.m_bUse || all )
			{
				SJ_Unity.SendMsg( go , func , args );
			}
		}
	}

	public	void 	ReAlign_Obj( HashSet<GameObject> hs )
	{
		q_UseAbleObj.Clear();
		foreach( GameObject go in m_ltAllocObj )
		{
			SJGoPoolObj pool_obj = go.GetComponent<SJGoPoolObj>();

			if( hs.Contains(go) )
			{
				pool_obj.m_bUse = true;
				go.SetActive(true);
			}else{
				pool_obj.m_bUse = false;
				go.SetActive(false);
				go.transform.parent = transform;
				q_UseAbleObj.Enqueue( go );
			}
		}
	}
}
