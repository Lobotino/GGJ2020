using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;
    public float speed = 1f;
    public float jumpSpeed = 1f;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    public bool Jump = false;
    public float RunX = 1f;

    private PhotonTransformView transformView; //Залезть сюда!

    public GameObject mainCamera;
    
    public void SetMainCamera(GameObject camera)
    {
        this.mainCamera = camera;
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<BoxCollider2D>();
       
        
        GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject otherPlayer in otherPlayers) 
        {
            Physics2D.IgnoreCollision(_collider2D, otherPlayer.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject otherPlayer in otherPlayers) 
        {
            Physics2D.IgnoreCollision(_collider2D, otherPlayer.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    private float _horizontalMove;
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        
        
        _horizontalMove = Input.GetAxisRaw("Horizontal");

        var position = transform.position;
        mainCamera.transform.position = new Vector3(position.x, position.y, -2);

        if (_horizontalMove > 0)
            _spriteRenderer.flipX = true;
        else {
            if (_horizontalMove < 0)
                _spriteRenderer.flipX = false;
        }

        if(_animator != null)
            _animator.SetFloat("Speed", Mathf.Abs(_horizontalMove * speed));
        
        if (Input.GetKey(KeyCode.A)) {
            _rigidbody.velocity = new Vector2(RunX* -speed, _rigidbody.velocity.y);
        }
        if (Input.GetKey(KeyCode.D)) {
            _rigidbody.velocity = new Vector2(RunX*speed, _rigidbody.velocity.y);
        }
        if (Input.GetKey(KeyCode.W) && Math.Abs(Math.Abs(_rigidbody.velocity.y)) < 0.01)
        {
            Jump = true;
        }

        if (_rigidbody != null && Jump && Math.Abs(Math.Abs(_rigidbody.velocity.y)) < 0.01)
        {
            Jump = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        }
    }
}
