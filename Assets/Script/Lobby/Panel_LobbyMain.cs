using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_LobbyMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ME_CSV.Load(this , "OnCSVLoad");
    }

    public  void    OnCSVLoad()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
