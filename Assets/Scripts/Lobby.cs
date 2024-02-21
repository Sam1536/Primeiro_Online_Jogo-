using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Lobby : MonoBehaviourPunCallbacks
{
    public InputField createRoom;
    public InputField joinRoom;
    
    public InputField nickName;

    public GameObject nickOBJ;


    private void Start()
    {
        if(PhotonNetwork.NickName != "")
        {
            nickOBJ.SetActive(false);
        }
    }

    public void GetNickName()
    {
        PhotonNetwork.NickName = nickName.text;
        nickOBJ.SetActive(false);
    }

    // metodo chamado ao clicar do botão screenRoom
    public void CreateRoomBT()
    {
        PhotonNetwork.CreateRoom(createRoom.text, new RoomOptions { MaxPlayers = 20 }, null); 
      
    }

    // metodo chamado ao clicar do botão JoinRoom
    public void JoinRoomBT()
    {
        PhotonNetwork.JoinRoom(joinRoom.text, null);
    }


    //metodo chamado apos entra na sala com sucesso 
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined Success!");
        PhotonNetwork.LoadLevel(1);
    }


    //chamado se houver erro ao entra na sala
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Room Failed! " + returnCode + " Message " + message);
    }
}
