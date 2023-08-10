using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SJ_CineCam : MonoBehaviour 
{
	static	public	SJ_CineCam	g;

	public	CinemachineVirtualCamera	cam_Main;
	public	CinemachineVirtualCamera	cam_Recent;

	public	class _CineCam_Inf_Backup
	{
		public	NoiseSettings		noise;
		public	LensSettings		lens;
	}


	
}
