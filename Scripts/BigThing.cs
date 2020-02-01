using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BigThing : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public BrokenManager[] brokenParts;

    private bool networkIsBroken = false;
    public bool isBroken = false;

    public void SetBroken(bool isBroken)
    {
        this.isBroken = isBroken;
        foreach (BrokenManager manager in brokenParts)
        {
            manager.setBrokenState(isBroken);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 43:
            {
                SetBroken((bool) photonEvent.CustomData);
                break;
            }
        }
    }
    
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
