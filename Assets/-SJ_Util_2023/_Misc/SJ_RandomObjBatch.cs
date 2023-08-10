using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_RandomObjBatch : MonoBehaviour 
{
	public	Transform			tr_par;
	public	List<GameObject>	list_obj;
	public	Vector3				pos_bb;
	public	int					count;

	public	void		Random_CreateBatch()
	{
		SJ_Unity.Delete_Child(tr_par);
		SJ_Unity.Random_CreateBatch( tr_par , pos_bb , count , list_obj);
	}
}
