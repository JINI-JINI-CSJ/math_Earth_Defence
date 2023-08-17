using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPopup_Config : MonoBehaviour
{
    public Toggle   tg_sound_on;
    public Toggle   tg_sound_off;
    public Toggle   tg_Vibrate_on;
    public Toggle   tg_Vibrate_off;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPopup()
    {
        if( ME_Account.user_Save.config_sound )
        {
            tg_sound_on.SetIsOnWithoutNotify(true);
        }else{
            tg_sound_off.SetIsOnWithoutNotify(true);
        }

        if( ME_Account.user_Save.config_Vibrate )
        {
            tg_Vibrate_on.SetIsOnWithoutNotify(true);
        }else{
            tg_Vibrate_off.SetIsOnWithoutNotify(true);
        }
    }

    public  void    OnClick_Sound( bool b )
    {
        ME_Account.user_Save.config_sound = b;
    }

    public  void    OnClick_Vibrate( bool b )
    {
        ME_Account.user_Save.config_Vibrate = b;
    }


    public void OnClick_TEST()
    {
        ME_Global.Save_PlayerData();
    }

}
