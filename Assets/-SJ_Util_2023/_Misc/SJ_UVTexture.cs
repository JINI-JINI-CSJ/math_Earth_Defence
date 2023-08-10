using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_UVTexture : MonoBehaviour 
{
	[System.Serializable]
	public	class _UVMat
	{
		public	Renderer	rd;
		public	float		time_u;
		public  float		time_v;

		public	void		Update()
		{
			if(	rd.material != null )
			{
				Vector2 v = new Vector2( time_u  ,time_v );
				rd.material.SetTextureOffset( "_MainTex" , v*Time.time );
			}
		}
	}

	public	List<_UVMat>	list_UVMat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach( _UVMat s in list_UVMat )
		{
			s.Update();
		}
	}
}
