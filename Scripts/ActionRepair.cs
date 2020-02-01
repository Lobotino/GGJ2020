using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ActionRepair : MonoBehaviour, Actionable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAction()
    {
        gameObject.GetComponent<BigThing>().SetBroken(false);
        RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
        SendOptions sendOptions = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(43, false, option, sendOptions);
    }
}
