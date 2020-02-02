using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ShipManager : MonoBehaviour, IOnEventCallback
{
    public int hp = 100;
    public int maxHp = 100;

    public BigThing[] bigThings;
    
    public Hole[] holes;
    
    void Start()
    {
        
    }

    private int tick = 0, globalTick = 0;
    private int nextBrokeTimer = 500, endGameTicks = 2000;

    public ProgressBar ProgressBar;
    private void FixedUpdate()
    {

        ProgressBar.BarValue = hp;
        
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        if (globalTick++ >= endGameTicks)
        {
            RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.All};
            SendOptions sendOptions = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(45, 3, option, sendOptions);
        }
        else
        {
            if (hp <= 0)
            {
                RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.All};
                SendOptions sendOptions = new SendOptions {Reliability = true};
                PhotonNetwork.RaiseEvent(45, 4, option, sendOptions);
            }
        }
        
        if (tick++ >= nextBrokeTimer)
        {
            int brokeId = Random.Range(0, bigThings.Length);
            
            bigThings[brokeId].SetBroken(true);
            int damage = Random.Range(3, 10);
            hp -= damage;
            
            RaiseEventOptions option = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
            SendOptions sendOptions = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(42, brokeId, option, sendOptions);
            PhotonNetwork.RaiseEvent(44, hp, option, sendOptions);

            tick = 0;
            nextBrokeTimer = Random.Range(50, 2500);
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
            case 44:
            {
                this.hp = (int) photonEvent.CustomData;
                break;
            }
            case 45:
            {
                PhotonNetwork.LeaveLobby();
                SceneManager.LoadScene((int)photonEvent.CustomData);
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
