using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Hole : MonoBehaviour, IOnEventCallback
{
    public bool isVisible = false;

    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public void OnEvent(EventData photonEvent)
    {
        throw new System.NotImplementedException();
    }
    public void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
}
