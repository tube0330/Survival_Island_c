using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowSetting : MonoBehaviour
{
    public new Light light;
    public Dropdown dropdown;

    public void Shadow(int value)
    {
        if (dropdown != null && light != null)
        {
            switch (value)
            {
                case 0:
                    light.shadows = LightShadows.None;
                    break;
                case 1:
                    light.shadows = LightShadows.Soft;
                    break;
                case 2:
                    light.shadows = LightShadows.Hard;
                    break;
            }
        }

        else Debug.LogError("Directional Light와 Dropdown 중 하나가 없음");
    }
}
