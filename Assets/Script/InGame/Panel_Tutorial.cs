using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Tutorial : MonoBehaviour
{
    public List<GameObject> lt_page;

    public int  cur_page;

    public void     OpenPopup()
    {
        Time.timeScale = 0;
        cur_page = 0;
        lt_page[0].SetActive(true);
    }

    public void     OnClick_Next()
    {
        lt_page[cur_page].SetActive(false);
        cur_page++;
        if( cur_page >=  lt_page.Count )
        {
            // 게임 스타트
            Time.timeScale = 1;
            InGameMain.g.Play();
            SJ_UnityUIMng.ClosePopup();
            return;
        }
        lt_page[cur_page].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
