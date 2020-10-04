using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Main : MonoBehaviourPunCallbacks
{
    public Text status;
    public Text roomInfo;
    public Text title;
    public InputField nameInput;

    public PlayerManager playerManager;
    public string playerName;
    public GameObject player;

    public Vector3 spawnPositionRed;
    public Vector3 spawnPositionBlue;
    Vector3 spawnPosition;

    string team;
    bool stopPoint = false;

    void Start()
    {
        // connecting to PUN server

        PhotonNetwork.ConnectUsingSettings();
        status.text = "Connected";
    }

    void Update()
    {
        if (PhotonNetwork.CountOfRooms >= 1)
            roomInfo.text = "room already created please press JOIN";
        else
            roomInfo.text = "no current room made please press CREATE";
    }

    public void BlueTeam()
    {
        title.color = Color.blue;
        team = "BlueTeam";
    }

    public void RedTeam()
    {
        title.color = Color.red;
        team = "RedTeam";
    }

    public void CreateRoom()
    {
        // creating room TESTROOM in server
        // and checking if player already gave an input for name and team

        if (nameInput.text != "" && team == "BlueTeam" || team == "RedTeam")
            PhotonNetwork.CreateRoom("TESTROOM");
        else
            status.text = "enter name / select team";
    }

    public void Join()
    {
        // joining room TESTROOM in server
        // and checking if player already gave an input for name and team

        if (nameInput.text != "" && team == "BlueTeam" || team == "RedTeam")
            PhotonNetwork.JoinRoom("TESTROOM");
        else
            status.text = "enter name / select team";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // in case joining the room failed (because of a connection loss or the room is not yet created)
        // start creating a room instead, if it has already tried this it will stop

        base.OnJoinRoomFailed(returnCode, message);

        if (stopPoint)
        {
            stopPoint = false;
            status.text = "Both join and create failed";
            return;
        }
        else
            stopPoint = true;

        status.text = "Failed to join, creating room instead";

        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // in case creating the room failed (because of a connection loss or the room is already created)
        // start joining the room instead, if it has already tried this it will stop

        base.OnCreateRoomFailed(returnCode, message);

        if (stopPoint)
        {
            stopPoint = false;
            status.text = "Both join and create failed";
            return;
        }
        else
            stopPoint = true;

        status.text = "Failed to create room, joining instead";

        Join();
    }

    public override void OnJoinedRoom()
    {
        // check wether the player has input a name (just to be save)
        // after that checking what team the player is and setting variable(s) to the correct team
        // instantiating player on server and setting GameObject.name to playerName to filter out this computer's player
        // lastly disabeling this gameObject because it is no longer needed

        status.text = "room joined/created";

        if (playerName.Length > 0)
        {
            spawnPosition = team == "BlueTeam" ? spawnPositionBlue : spawnPositionRed;

            player.transform.name = nameInput.text;
            PhotonNetwork.Instantiate(playerName, spawnPosition, Quaternion.identity, 0);
            player.transform.name = playerName;

        }

        playerManager.playerTeam = team;
        playerManager.playerName = nameInput.text;
        this.gameObject.SetActive(false);
    }
}
