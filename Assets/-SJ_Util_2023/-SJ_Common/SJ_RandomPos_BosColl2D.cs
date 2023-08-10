using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJ_RandomPos_BosColl2D : MonoBehaviour
{
    public  BoxCollider2D   boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  Vector2 RandomPos()
    {
        if( boxCollider2D == null )
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        if( boxCollider2D == null )
        {
            Debug.LogError( "SJ_RandomPos_BosColl2D : no BoxCollider2D : " + gameObject.name );
            return Vector2.zero;
        }

        float sx = boxCollider2D.size.x;
        float sy = boxCollider2D.size.y;

        float rx = UnityEngine.Random.Range( -sx * 0.5f, sx * 0.5f );
        float ry = UnityEngine.Random.Range( -sy * 0.5f, sy * 0.5f  );

        Vector3 v = new Vector3( rx , ry , 0 );
        v = transform.TransformPoint( v );
        return new Vector2( v.x , v.y );
    }
}
