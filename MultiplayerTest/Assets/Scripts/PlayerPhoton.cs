using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class PlayerPhoton : MonoBehaviour
{
    public Text nameText;

    PhotonView photonView;

    void Start()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();
        photonView.RPC("SetName", RpcTarget.AllBuffered, this.gameObject.name);
    }

    [PunRPC]
    void SetName(string name)
    {
        string[] names = name.Split('(');
        nameText.text = names[0];
    }
}
