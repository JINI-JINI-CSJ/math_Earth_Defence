﻿using UnityEngine;
using System.Collections;

public class SJ_ReturnDestroy : MonoBehaviour
{
	public	float	timer = 1;
	//public	bool	startFunc;
	public	bool	start_Enable=true;

	public	GameObject	go_return;

	public	int 		debug_state;

	public	void 	Stop()
	{
		StopAllCoroutines();
		enabled = false;
	}

	private void OnEnable()
	{
		if( start_Enable )
		{
			debug_state = 1;
			StartCoroutine( "CO_End" );
		}
	}

	public	void	Return_Start( float _timer = -1 )
	{
		if( _timer > 0 ) timer = _timer;
		enabled = true;

		if( timer > 0 )
			StartCoroutine( "CO_End" );
		else
			Return_Obj();
	}

	IEnumerator CO_End()
	{
		debug_state = 2;
		yield return new WaitForSeconds(timer);
		Return_Obj();
		debug_state = 3;
	}


	public	void	Return_Obj()
	{
		debug_state = 4;
		if( go_return != null )	SJPool.ReturnInst_Or_Destroy(go_return);
		else					SJPool.ReturnInst_Or_Destroy(gameObject);
	}

}
