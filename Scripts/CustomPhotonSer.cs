using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Photon.Pun;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CustomPhotonSer : MonoBehaviour, IPunObservable
{

    private PhotonView m_PhotonView;
    private Vector3 m_Direction;

    public float time = 1f;
    
    private float m_Distance;
    private Vector3 m_NetworkPosition;
    private Vector3 m_StoredPosition;

    bool m_firstTake = false;
    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        m_StoredPosition = transform.position;
        m_NetworkPosition = Vector3.zero;
    }

    void OnEnable()
    {
        m_firstTake = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.m_PhotonView.IsMine)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(this.m_NetworkPosition.x, transform.position.y),
                this.m_Distance * (1.0f / PhotonNetwork.SerializationRate));

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, m_NetworkPosition.y, time));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            this.m_Direction = transform.position - this.m_StoredPosition;
            this.m_StoredPosition = transform.position;

            stream.SendNext(transform.position);
            stream.SendNext(this.m_Direction);
        }
        else
        {
            this.m_NetworkPosition = (Vector3) stream.ReceiveNext();
            this.m_Direction = (Vector3) stream.ReceiveNext();

            if (m_firstTake)
            {
                transform.position = this.m_NetworkPosition;
                this.m_Distance = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition += this.m_Direction * lag;
                this.m_Distance = Vector3.Distance(transform.position, this.m_NetworkPosition);
            }


            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }
}
