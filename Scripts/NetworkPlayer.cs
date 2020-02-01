using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{

    protected PlayerController Player;
    private Rigidbody2D rigidbody;
    protected Vector3 networkDiff;

    private void Awake()
    {
        Player = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody2D>();
        //destroy the controller if the player is not controlled by me
//        if (!photonView.IsMine && GetComponent<PlayerController>() != null)
//            Destroy(GetComponent<PlayerController>());
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rigidbody.position = networkDiff;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
//            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            networkDiff = (Vector3) stream.ReceiveNext();
            
//            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
//            networkDiff = (rigidbody.velocity * lag);
        }
    }
}

