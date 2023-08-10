using UnityEngine;
using System.Collections;

public class SJ_DymMaterial : MonoBehaviour
{
	public	Material			mat_src;

	[HideInInspector]
	public	Material			mat_new_Inst;

	public	MeshRenderer[]		meshRd_Change;

	public	bool				init_Awake;


	void Awkae()
	{
		if( init_Awake )
		{
			Init();
		}
	}

	public	void	Init()
	{
		if( mat_src == null ) return;
		if( mat_new_Inst != null  ) return;
		mat_new_Inst = new Material(mat_src);
		foreach( MeshRenderer s in meshRd_Change )
		{
			s.materials = new Material[] {mat_new_Inst};
		}
	}
	

}
