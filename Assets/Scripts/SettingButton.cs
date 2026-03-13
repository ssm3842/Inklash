using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField]GameObject settingCavnasPrefab;

    public void ShowSettingCanvas()
    {
        if(SettingManger.Inst == null)
        {
            Instantiate(settingCavnasPrefab);
        }
        SettingManger.Inst.OpenSetting();
        SettingManger.Inst.ShowPanel("Graphic");
    }
}
