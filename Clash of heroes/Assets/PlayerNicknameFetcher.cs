using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerNicknameFetcher : MonoBehaviour
{
    public Text nicknameText;

    private string remoteNickname;

    RoundManager roundManager;
    void Start()
    {
        remoteNickname = roundManager.GetRemotePlayerNickname();
        nicknameText.text = remoteNickname;

    }



}
