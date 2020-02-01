using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DragonBones;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public AudioSource walkAudio;
    
    private PhotonView photonView;
    public float speed = 1f;
    public float jumpSpeed = 1f;
    private Rigidbody2D _rigidbody;
    public UnityArmatureComponent armature;
    private Collider2D _collider2D;
    public bool Jump = false;
    public float RunX = 1f;
    public bool flipX = false;
    public float cameraYParalax = 0.5f;
    public Actionable actionThing;
    
    private PhotonTransformView transformView; 

    public GameObject mainCamera;
    
    public void SetMainCamera(GameObject camera)
    {
        this.mainCamera = camera;
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CapsuleCollider2D>();
       
        
        GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject otherPlayer in otherPlayers) 
        {
            Physics2D.IgnoreCollision(_collider2D, otherPlayer.gameObject.GetComponent<CapsuleCollider2D>());
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject otherPlayer in otherPlayers) 
        {
            Physics2D.IgnoreCollision(_collider2D, otherPlayer.gameObject.GetComponent<CapsuleCollider2D>());
        }
    }

    private float _horizontalMove;
    public bool isRun = false;
    void FixedUpdate()
    {
        if (Math.Abs(_rigidbody.velocity.x) > 0.01) //сука звук
        {
            walkAudio.Play();
        }
        else
        {
            walkAudio.Pause();
        }



        if (!photonView.IsMine)
        {
            armature.armature.flipX = flipX;
            if (isRun)
            {
                if (armature.armature.animation.lastAnimationName != "Run")
                    armature.armature.animation.GotoAndPlayByTime("Run");
            }
            else
            {
                if (armature.armature.animation.lastAnimationName != "Stand")
                    armature.armature.animation.GotoAndPlayByTime("Stand");
            }
        }
        else
        {

            _horizontalMove = Input.GetAxisRaw("Horizontal");
            var position = transform.position;
            mainCamera.transform.position = new Vector3(position.x, position.y+cameraYParalax, -50);


            isRun = Mathf.Abs(_horizontalMove) > 0.002f;
            if (isRun)
            {
                if (armature.armature.animation.lastAnimationName != "Run")
                    armature.armature.animation.GotoAndPlayByTime("Run");
            }
            else
            {
                if (armature.armature.animation.lastAnimationName != "Stand")
                    armature.armature.animation.GotoAndPlayByTime("Stand");
            }


            if (_horizontalMove > 0)
            {
                armature.armature.flipX = false;
                flipX = false;
            }
            else
            {
                if (_horizontalMove < 0)
                {
                    armature.armature.flipX = true;
                    flipX = true;
                }
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                _rigidbody.velocity = new Vector2(RunX * -speed, _rigidbody.velocity.y);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _rigidbody.velocity = new Vector2(RunX * speed, _rigidbody.velocity.y);
            }

            if (Input.GetKey(KeyCode.W) && Math.Abs(Math.Abs(_rigidbody.velocity.y)) < 0.01)
            {
                Jump = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                actionThing?.OnAction();
                armature.armature.animation.GotoAndPlayByProgress("Use", 100, 1);
            }

            if (_rigidbody != null && Jump && Math.Abs(Math.Abs(_rigidbody.velocity.y)) < 0.01)
            {
                Jump = false;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(flipX);
            stream.SendNext(isRun);
        }
        else
        {
            flipX = (bool) stream.ReceiveNext();
            isRun = (bool) stream.ReceiveNext();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Actionable"))
        {
            actionThing = other.gameObject.GetComponent<Actionable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Actionable"))
        {
            if (actionThing == other.gameObject.GetComponent<Actionable>())
                actionThing = null;
        }
    }
}
