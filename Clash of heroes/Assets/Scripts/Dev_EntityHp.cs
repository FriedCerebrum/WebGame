using UnityEngine;
using TMPro;

public class Dev_EntityHp : MonoBehaviour
{
    public Entity hpscript;
    public TextMeshProUGUI text;

    void Start()
    {
    }

    void Update()
    {
        if (hpscript != null)
        {
            int hpValue = hpscript.hp;
            text.text = hpValue.ToString();
        }
    }
}

