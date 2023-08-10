using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class SJ_DragCamPos : MonoBehaviour
{
    public  Camera      cam_move;

    public  CinemachineVirtualCamera    cam_cine;

    public  float       move_fix = 1.0f;

    //public  Transform   tr_base;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(BaseEventData data)
    {
        if( enabled == false ) return;

        if( cam_move == null && cam_cine == null ) return;

        float orthographicSize = 1;
        if( cam_move != null ) orthographicSize = cam_move.orthographicSize;
        if( cam_cine != null ) orthographicSize = cam_cine.m_Lens.OrthographicSize;

        PointerEventData pointer_data = (PointerEventData)data;
        Vector2 move_delta = pointer_data.delta * orthographicSize * move_fix * -1.0f;

//        Debug.Log( pointer_data.delta + " , " + orthographicSize );

        if( cam_move != null )
            cam_move.transform.localPosition += new Vector3(move_delta.x , move_delta.y , 0);

        if( cam_cine != null )
            cam_cine.transform.localPosition += new Vector3(move_delta.x , move_delta.y , 0);
    }
}
