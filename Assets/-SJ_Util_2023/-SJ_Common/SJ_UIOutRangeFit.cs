using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 바깥범위에 있는거 안쪽으로..
public class SJ_UIOutRangeFit : MonoBehaviour
{
    public  Camera      cam_world;
    public  Camera      cam_UI;

    public  GameObject  obj_World;

    RectTransform   rectTransform;
    CanvasScaler    canvasScaler;
    RectTransform   rt_cs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {

    }

    public  bool    RePos()
    {
        if( rectTransform == null ) rectTransform = GetComponent<RectTransform>();
        if( canvasScaler == null )
        {
            canvasScaler = GetComponentInParent<CanvasScaler>();
            if( canvasScaler != null )
            {
                rt_cs = canvasScaler.GetComponent<RectTransform>();
            }
        }
         
        if( obj_World == null || rectTransform == null || canvasScaler == null ) return false;

        SJ_Unity.WorldPos_ToScreenPos( cam_world , cam_UI , obj_World.transform.position , transform );

        float   check_ui_x_min = -rt_cs.rect.width  / 2;
        float   check_ui_x_max =  rt_cs.rect.width  / 2;
        float   check_ui_y_min = -rt_cs.rect.height / 2;
        float   check_ui_y_max =  rt_cs.rect.height / 2;

        float   ui_x_min = check_ui_x_min + rectTransform.rect.width / 2;
        float   ui_x_max = check_ui_x_max - rectTransform.rect.width / 2;
        float   ui_y_min = check_ui_y_min + rectTransform.rect.height / 2;
        float   ui_y_max = check_ui_y_max - rectTransform.rect.height / 2;

        Vector3 ui_pos = rectTransform.localPosition;

        //Debug.Log( "rt_cs.rect.width : " + rt_cs.rect.width );

        bool   b = false;
        // if( ui_pos.x < check_ui_x_min ){ui_pos.x = ui_x_min;b=true;}
        // if( ui_pos.x > check_ui_x_max ){ui_pos.x = ui_x_max;b=true;}
        // if( ui_pos.y < check_ui_y_min ){ui_pos.y = ui_y_min;b=true;}
        // if( ui_pos.y > check_ui_y_max ){ui_pos.y = ui_y_max;b=true;}

        if( ui_pos.x < ui_x_min ){ui_pos.x = ui_x_min;b=true;}
        if( ui_pos.x > ui_x_max ){ui_pos.x = ui_x_max;b=true;}
        if( ui_pos.y < ui_y_min ){ui_pos.y = ui_y_min;b=true;}
        if( ui_pos.y > ui_y_max ){ui_pos.y = ui_y_max;b=true;}

        rectTransform.localPosition = ui_pos;        

        return b;
    }
}
