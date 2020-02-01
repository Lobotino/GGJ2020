using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipManager : MonoBehaviour, IOnEventCallback
{
    public int hp = 100;
    public int maxHp = 100;

    public BigThing[] bigThings;
    
    void Start()
    {
        
    }

    private int tick = 0;
    private int nextBrokeTimer = 1000;
    
    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        
        if (tick++ >= nextBrokeTimer)
        {
            int brokeId = Random.Range(0, bigThings.Length);
            
            bigThings[brokeId].SetBroken(true);
            
            RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
            SendOptions sendOptions = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(42, brokeId, option, sendOptions);

            tick = 0;
            nextBrokeTimer = Random.Range(250, 6000);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 42:
            {
                bigThings[(int)photonEvent.CustomData].SetBroken(true);
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
