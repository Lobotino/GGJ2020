using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public GameObject currentCamera;
    public GameObject mainCameraPrefab;
    
    void Start()
    {
        Vector3 pos = new Vector3(Random.Range(-5f, 5f), 1, -40);
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, pos, Quaternion.identity);
        player.name = "Player_" + PhotonNetwork.NickName;
        GameObject[] otherCamers = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject otherCamera in otherCamers)
        {
            if (otherCamera != currentCamera)
            {
                otherCamera.SetActive(false);
            }
        }
        
        currentCamera = Instantiate(mainCameraPrefab, transform.position, Quaternion.identity);
        player.GetComponent<PlayerController>().SetMainCamera(currentCamera);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    void OnUpdate()
    {
        if(currentCamera != null)
            currentCamera.transform.position = new Vector3();
    }
    
    public override void OnJoinedRoom()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject[] otherCamers = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject otherCamera in otherCamers)
        {
            if (otherCamera != currentCamera)
            {
                otherCamera.SetActive(false);
            }
        }
        Debug.Log("Player " + newPlayer.NickName + "entered room!");
    } 
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player " + otherPlayer.NickName + " left room!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect " + cause);
    }
}
