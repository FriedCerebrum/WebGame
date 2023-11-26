using UnityEngine;
using TMPro;

public class Dev_EntityHp : MonoBehaviour // dev
{
    public Entity hpscript;
    public TextMeshProUGUI text;

    void Update()
    {
        if (hpscript != null & hpscript.hp > 0)
        {
            int hpValue = hpscript.hp;
            text.text = hpValue.ToString();
        }
        else
        {
            text.text = "";
        }
    }
}

