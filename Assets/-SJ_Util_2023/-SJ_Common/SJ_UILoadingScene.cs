using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SJ_UILoadingScene : MonoBehaviour
{
    //public  List<string>    lt_path_image;
    public  List<Sprite>     lt_sprite;
    public  Image           image_Main;
    public  Scrollbar       scr_gage;
    public  Slider          slider_gage;
    public  Text            text_gage_per;
    public  float           fix_load_gage = 1.1f;
    public  float           fix_min;

    public  float           wait_time = 1.0f;

    public  string            cur_load_scene;
    static public   SJ_UILoadingScene   cur_sj_UILoadingScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static  public  void    LoadScene( string str , bool  load_start = true )
    {
        GameObject go_Canvas = GameObject.Find("Canvas");
        SJ_UILoadingScene sj_UILoadingScene = go_Canvas.GetComponentInChildren<SJ_UILoadingScene>(true);
        cur_sj_UILoadingScene = sj_UILoadingScene;
        sj_UILoadingScene.Init( str , load_start );   
    }


    public  void    Init(string str , bool  load_start)
    {
        cur_load_scene = str;
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        if( image_Main != null && lt_sprite.Count > 0 )
        {
            int idx = UnityEngine.Random.Range(0,lt_sprite.Count);
            //image_Main.sprite = Resources.Load<Sprite>( lt_path_image[idx] );
            image_Main.sprite = lt_sprite[idx];

        }
        Update_Gage(0);


        if( load_start )
            StartCoroutine( CO_LoadSceneSync(str) );
    }

    static public   void    Load_StartCoroutine()
    {
        cur_sj_UILoadingScene.StartCoroutine( cur_sj_UILoadingScene.CO_LoadSceneSync(cur_sj_UILoadingScene.cur_load_scene) );
    }   

    public  void    Update_Gage( float gage )
    {
        Debug.Log( "Update_Gage : " + gage );
        gage *= fix_load_gage; // 0.9 -> 1.0

        float f1 = 1.0f - fix_min;
        float f2 = gage * f1;
        gage = f2 + fix_min;

        if( scr_gage != null )      scr_gage.size = gage;
        if( slider_gage != null )   slider_gage.value = gage;

        if( text_gage_per != null )
        {
            gage *= 100.0f;
            string str = string.Format( "{0:F01}%" , gage );
            text_gage_per.text = str;
        }
    }

    IEnumerator CO_LoadSceneSync(string str)
    {
        yield return new WaitForSeconds(wait_time);
        AsyncOperation async = SceneManager.LoadSceneAsync( str );
        //async.allowSceneActivation = false;
        while( !async.isDone )
        {
            Update_Gage(async.progress);
            // if (async.progress >= 0.9f)
            // {
            //     yield return new WaitForSeconds(0.1f);
            //     async.allowSceneActivation = true;
            // }

            yield return null;
        }
    }
}




//         yield return null;

//         //Begin to load the Scene you specify
//         AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scene3");
//         //Don't let the Scene activate until you allow it to
//         asyncOperation.allowSceneActivation = false;
//         Debug.Log("Pro :" + asyncOperation.progress);
//         //When the load is still in progress, output the Text and progress bar
//         while (!asyncOperation.isDone)
//         {
//             //Output the current progress
//             m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

//             // Check if the load has finished
//             if (asyncOperation.progress >= 0.9f)
//             {
//                 //Change the Text to show the Scene is ready
//                 m_Text.text = "Press the space bar to continue";
//                 //Wait to you press the space key to activate the Scene
//                 if (Input.GetKeyDown(KeyCode.Space))
//                     //Activate the Scene
//                     asyncOperation.allowSceneActivation = true;
//             }

//             yield return null;
//         }
//     }
// }