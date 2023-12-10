using UnityEngine;
using Photon.Pun; 

public class DeadScript2 : MonoBehaviourPun
{
    public PlayerMovement pm;
    public PlayerCombat pc;
    public Entity entity;
    public GameObject targetObject;
    public Rigidbody2D rb;

    public void Update()
    {
        if (photonView.IsMine && entity.hp <= 0 && gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (pm != null)
            {
                pm.enabled = false;
            }
            if (pc != null)
            {
                pc.enabled = false;
            }
        }
    }
}
