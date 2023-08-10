using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_TimerFunc : MonoBehaviour
{
	_SJ_GO_FUNC sfunc = null;

	public	float	start_time;
	float	time_cur;


	// Use this for initialization
	void Start () {
		this.enabled = false;
	}
	
	public	void	Time_Start( float _start_time = -1 )
	{
		if( _start_time > 0 ) start_time = _start_time;
		this.enabled = true;
		time_cur = start_time;
	}

	// Update is called once per frame
	void Update ()
	{
		time_cur -= Time.deltaTime;

		if( time_cur <= 0 )
		{
			time_cur = 0;
			SJ_Unity.SendMsg(sfunc.go , sfunc.func , time_cur );
			this.enabled = false;
			return;
		}
		SJ_Unity.SendMsg(sfunc.go , sfunc.func , time_cur );
	}
}
