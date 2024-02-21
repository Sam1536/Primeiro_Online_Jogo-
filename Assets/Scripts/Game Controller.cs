using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviourPunCallbacks
{

    public GameObject ConnectedScreen;
    public GameObject disconnectedScreen;


    //conecta no servidor
    public void ConnectBT()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    //entra no lobby
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    //conexão falhou
    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectedScreen.SetActive(true);
    }


    //apos o login no lobby
    public override void OnJoinedLobby()
    {
        ConnectedScreen.SetActive(true);
    }
}
