using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


static	public class SJ_Menu_Misc
{

#if UNITY_EDITOR
	[MenuItem("SJMisc/Export Texture")]
	static void Export_Texture()
	{
		Texture2D texture = Selection.activeObject as Texture2D;
		if (texture == null)
		{
			EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok");
			return;
		}
   
		var bytes = texture.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + "/exported_texture.png", bytes);
	}


	static	string fileName_ScreenShot(int width, int height)
     {
        return string.Format("screen_{0}x{1}_{2}.png",
                              width, height,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
     }

	[MenuItem("SJMisc/Camera ScreenShot")]
	static void Camera_ScreenShot()
	{

		if (Selection.activeGameObject == null)
		{
			EditorUtility.DisplayDialog("Select Camera", "카메라 선택하셈~", "Ok");
			return;
		}
		Camera cam = Selection.activeGameObject.GetComponent<Camera>();
		if (cam == null)
		{
			EditorUtility.DisplayDialog("Select Camera", "카메라 선택하셈~", "Ok");
			return;
		}
		if (cam.targetTexture == null)
		{
			EditorUtility.DisplayDialog("Select Camera", "랜더 타겟 택스쳐 없다~", "Ok");
			return;
		}
		RenderTexture currentRT = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Texture2D imageOverview = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.ARGB32, false);
		imageOverview.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
		imageOverview.Apply();
		RenderTexture.active = currentRT;



		//Texture2D imageOverview = new Texture2D(Screen.width, Screen.height);
		//imageOverview.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		//imageOverview.Apply();


		// Encode texture into PNG
		byte[] bytes = imageOverview.EncodeToPNG();
		// save in memory
		string filename = fileName_ScreenShot(imageOverview.width, imageOverview.height);
		string path = Application.dataPath + "/" + filename;
		System.IO.File.WriteAllBytes(path, bytes);

		Debug.Log( "screen : " + path );
	}


	[MenuItem("SJMisc/CaptureScreenshot")]
	static public	void ScreenCapture_CaptureScreenshot()
	{
		string filename = fileName_ScreenShot(1, 1);
		string path = Application.dataPath + "/" + filename;
		ScreenCapture.CaptureScreenshot( path );
		Debug.Log( "screen : " + path );
	}

	//=============================================================================================
	// 컴포넌트 추가
	static	public	GameObject	SJ_Component_ADD(System.Type componentType) 
	{
		if( Selection.activeGameObject == null )
		{
			EditorUtility.DisplayDialog("SJ_Component", "객체 선택하셈~", "Ok");
			return null;
		}
		if( Selection.activeGameObject.GetComponent(componentType) == null )Selection.activeGameObject.AddComponent(componentType);

		return Selection.activeGameObject;
	}

	[MenuItem("SJMisc/SJ_Component/SJGoPoolObj")]
	static void SJ_Component_SJGoPoolObj(){SJ_Component_ADD( typeof(SJGoPoolObj) );}

	[MenuItem("SJMisc/SJ_Component/SJTagObj_Mono")]
	static void SJ_Component_SJTagObj_Mono()
	{
		SJ_Component_ADD( typeof(SJTagObj_Mono) );
	}


	[MenuItem("SJMisc/SJ_Component/SJTagSys_Mono")]
	static void SJ_Component_SJTagSys_Mono()
	{
		GameObject go =	SJ_Component_ADD( typeof(SJTagSys_Mono) );
		SJTagSys_Mono		tag = go.GetComponent<SJTagSys_Mono>();
		SJTrgPlayer_Mono	player = go.GetComponent<SJTrgPlayer_Mono>();
		if( tag == null || player == null ) return;
		player.sjtagsys_mono = tag;
		tag.sjtrgplayer_mono = player;
	}


	[MenuItem("SJMisc/SJ_Component/SJTexColor")]
	static void SJ_Component_SJTexColor(){SJ_Component_ADD( typeof(SJTexColor) );}

	[MenuItem("SJMisc/SJ_Component/SJ_ReturnDestroy")]
	static void SJ_Component_SJ_ReturnDestroy(){SJ_Component_ADD( typeof(SJ_ReturnDestroy) );}


	[MenuItem("SJMisc/SJ_Component/자식에 액션플레이어 객체 추가")]
	static void SJ_Component_ADDObj_ActPlayer()
	{
		if( Selection.activeGameObject == null ) return;
		
		GameObject inst_go = new GameObject( "ActPlayer" );
		inst_go.AddComponent<SJTrgActionPlayer_Mono>();
		inst_go.transform.parent = Selection.activeGameObject.transform;
	}

	//=============================================================================================

	[MenuItem("SJMisc/SJGoPoolObj_OBJ_ID")]
	static void SJGoPoolObj_OBJ_ID()
	{
		//if( Selection.activeGameObject == null ) return;
		//Object go =	PrefabUtility.GetPrefabParent(Selection.activeGameObject);
		//if( go == null ) return;
		//Debug.Log( "SJGoPoolObj_OBJ_ID : " + go.ToString() );
		//Selection.activeGameObject = go as GameObject ; 


		GameObject res_SJ_Edit_Misc = Resources.Load( "SJ_Edit_Misc" ) as GameObject;
		SJ_Edit_Misc sj_edit_misc =	res_SJ_Edit_Misc.GetComponent<SJ_Edit_Misc>();
		GameObject	prf_Select = sj_edit_misc.prf_OBJ;

		if( prf_Select == null )
		{
			EditorUtility.DisplayDialog("SJGoPoolObj_OBJ_ID", "SJ_Edit_Misc 프리펩 선택하셈~", "Ok");
			return;
		}

		GameObject[] ls =	GameObject.FindObjectsOfType<GameObject>();

		int id_count=0;
		foreach( GameObject go in ls )
		{
			//Debug.Log( go.name );
			GameObject prf_par =	PrefabUtility.GetCorrespondingObjectFromSource(go) as GameObject;
			if( prf_Select == prf_par )
			{
				SJGoPoolObj sjgopoolobj = go.GetComponent<SJGoPoolObj>();
				sjgopoolobj.UID = id_count;
				id_count++;
			}
		}

		Debug.Log( prf_Select.name + " : id_count : " + id_count );
	}

	[MenuItem("SJMisc/SJ_RandomObjBatch")]
	static void SJ_RandomObjBatch()
	{
		if( Selection.activeGameObject == null ) return;
		SJ_RandomObjBatch sj_rb =	Selection.activeGameObject.GetComponent<SJ_RandomObjBatch>();
		if( sj_rb == null ) return;
		sj_rb.Random_CreateBatch();
	}




	[MenuItem("SJMisc/SJ_RailObj/AddComponent")]
	static void SJ_RailObj_AddComponent()
	{
		// if( Selection.activeGameObject == null ) return;

		// if( Selection.activeGameObject.GetComponent<SJGoPoolObj>() == null )		Selection.activeGameObject.AddComponent<SJGoPoolObj>();
		// if( Selection.activeGameObject.GetComponent<iTweenPath>() == null )			Selection.activeGameObject.AddComponent<iTweenPath>();
		// if( Selection.activeGameObject.GetComponent<SJ_Itweens_Link>() == null )	Selection.activeGameObject.AddComponent<SJ_Itweens_Link>();

		// Debug.Log( Selection.activeGameObject.name + " SJ_RailObj_AddComponent " );
	}

	[MenuItem("SJMisc/SJ_RailObj/Create_ITweenRandObj")]
	static void SJ_RailObj_Create_ITweenRandObj()
	{
		// if( Selection.activeGameObject == null ) return;
		// SJ_RailObj sj =	Selection.activeGameObject.GetComponent<SJ_RailObj>();
		// if( sj == null ) return;

		// sj.Create_ITweenRandObj();

		// Debug.Log( sj.name + " Create_ITweenRandObj " );
	}

	[MenuItem("SJMisc/SJ_RailObj/FitHeight_Terrain_Itween")]
	static void SJ_RailObj_FitHeight_Terrain_Itween()
	{
		// if( Selection.activeGameObject == null ) return;
		// SJ_RailObj sj =	Selection.activeGameObject.GetComponent<SJ_RailObj>();
		// if( sj == null ) return;

		// sj.FitHeight_Terrain_Itween();

		// Debug.Log( sj.name + " FitHeight_Terrain_Itween " );
	}

	[MenuItem("SJMisc/SJTrgActionPlayer_Mono 정리")]
	static void SJTrgActionPlayer_Mono_Align()
	{
		if( Selection.activeGameObject == null ) return;
		SJTrgActionPlayer_Mono[] act_players = Selection.activeGameObject.GetComponentsInChildren<SJTrgActionPlayer_Mono>(true);
		if( act_players.Length < 1 ) return;

		foreach( SJTrgActionPlayer_Mono s in act_players )
			s.Align_ChildAction();
	}

	[MenuItem("SJMisc/NGUI/All_Change_NGUI_Font")]
	static void All_Change_NGUI_Font()
	{
		// if( Selection.activeGameObject == null ) return;
		// if( Selection.activeObject == null ) return;
		// UILabel[]	lb = Selection.activeGameObject.GetComponentsInChildren<UILabel>(true);
		// Font font = Selection.activeObject as Font;
		// Debug.Log( "폰트 선택 : " + font.name );
	}

	// 캔버스 처음자식 text 의 폰트로 설정
	[MenuItem("SJMisc/UI/All_Change_Font")]
	static void All_Change_Font()
	{
		if( Selection.activeGameObject == null )
		{
			Debug.Log( "All_Change_Font : Selection.activeGameObject == null" );
			return; 
		} 
		Text[]	lb = Selection.activeGameObject.GetComponentsInChildren<Text>(true);

		if(lb.Length < 1 ) return;

		Font font = lb[0].font;

		foreach( Text s in lb )
		{
			s.font = font;
		}
	}

	// [MenuItem("SJMisc/NGUI/All_Change_Button_Color")]
	// static void All_Change_Button_Color()
	// {
	// 	GameObject go_Editor_SJ_NGUI_Ref =	GameObject.Find("Editor_SJ_NGUI_Ref");
	// 	if( go_Editor_SJ_NGUI_Ref == null )
	// 	{
	// 		Debug.LogError("Error!!! All_Change_Button_Color : Editor_SJ_NGUI_Ref no!!!!");
	// 		return;
	// 	}

	// 	Editor_SJ_NGUI_Ref editor_SJ_NGUI_Ref = go_Editor_SJ_NGUI_Ref.GetComponent<Editor_SJ_NGUI_Ref>();

	// 	UIButton[]	bts = Selection.activeGameObject.GetComponentsInChildren<UIButton>(true);
	// 	foreach( UIButton s in bts )	
	// 	{
	// 		s.defaultColor = s.hover = s.pressed = editor_SJ_NGUI_Ref.col_ButtonDefault;
	// 	}
	// 	Debug.Log( "All_Change_Button_Color : " + bts.Length );
	// }
	[MenuItem("SJMisc/Delete_ComponentAll_Collider")]
	static	public	void 	Delete_ComponentAll_Collider()
	{
		if( Selection.activeGameObject == null ) return;

		Collider[] colls = Selection.activeGameObject.GetComponentsInChildren<Collider>();
		foreach( Collider s in colls )
		{
			GameObject.DestroyImmediate( s );
		}
	}

#endif

}
