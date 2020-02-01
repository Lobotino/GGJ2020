using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text logText;
    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1, 100);

        Log("Player's name is set to " + PhotonNetwork.NickName);
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "2";
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Log("Connected to Master");
    }

    public void CreateRoom()
    {
        Log("Trying to creating room");
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions {MaxPlayers = 10});
        Log("Created room"); 
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        Log("Joined to the room");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("FirstShip");
        Log("On joined room");
    }

    public void Log(string log)
    {
        Debug.Log(log);
        logText.text += "\n\n" + log;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log(returnCode + " MESSAGE: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Log(returnCode + " MESSAGE: " + message);
    }
}
