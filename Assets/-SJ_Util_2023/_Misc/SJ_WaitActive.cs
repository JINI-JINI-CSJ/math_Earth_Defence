using UnityEngine;
using System.Collections;

public class SJ_WaitActive : MonoBehaviour
{
	public	float	wait_time = 1.0f;

	public	_SJ_GO_FUNC	end_func;

	void	OnEnable()
	{
		StartCoroutine( "CO_Func" );
	}

	IEnumerator CO_Func()
	{
		yield return new WaitForSeconds(wait_time);
		gameObject.SetActive(false);

		end_func.Func(gameObject);
	}
}
