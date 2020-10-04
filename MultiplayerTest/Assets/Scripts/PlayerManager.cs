using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public Text pingText;

    [Header("Information")]
    public string playerName;
    public string playerTeam;

    GameObject[] players;
    bool playerFound = false;

    void Start()
    {
        InvokeRepeating("UpdatePlayerList", 0f, 1f);
    }

    public void UpdatePlayerList()
    {
        // showing ping

        pingText.text = "Ping: " + PhotonNetwork.GetPing();
        players = GameObject.FindGameObjectsWithTag("Player");

        if (!playerFound)
        {
            if (players.Length == 0)
                return;

            foreach (GameObject player in players)
            {
                if (player.name.Contains(playerName))
                {
                    ActivatePlayer(player);

                    playerFound = true;
                }
            }
        }
    }
    
    void ActivatePlayer(GameObject player)
    {
        Camera camera = player.transform.GetComponentInChildren<Camera>();
        RigidbodyFirstPersonController playerScript = player.transform.GetComponent<RigidbodyFirstPersonController>();
        PlayerPhoton playerPhoton = player.transform.GetComponent<PlayerPhoton>();
        Rigidbody playerRigidbody = player.transform.GetComponent<Rigidbody>();

        camera.enabled = true;
        playerScript.enabled = true;
        playerPhoton.enabled = true;

        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
