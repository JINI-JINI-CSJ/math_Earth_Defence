using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SJ_UIAlphaHitValue : MonoBehaviour
{
    public Image image;

    public  float   alpha = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        image.alphaHitTestMinimumThreshold = alpha;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
