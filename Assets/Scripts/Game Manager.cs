using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public Text spawnTimer;
    public Text pingText;
    public GameObject respawnUI;

    public float totalRespawnTimer;

    private float respawnTime;
    private bool startRespawn;

    //player do atual cliente
    [HideInInspector]public GameObject localPlayer;

    public bool isAlive = true;

    public static GameManager instance;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        SpawnPlayer();

        respawnTime = totalRespawnTimer;
    }


    private void Update()
    {
        if (startRespawn)
        {
            StartRespawn();
        }

        pingText.text = "Ping: " + PhotonNetwork.GetPing().ToString();
    }


    

    public void SpawnPlayer()
    {
        
        float randomPos = 0f; // Random.Range(-2f, 1f);
        PhotonNetwork.Instantiate(player.name, new Vector2(player.transform.position.x + randomPos, player.transform.position.y), Quaternion.identity);
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    #region Respawn Functions
    public void PlayerRespawnPos()
    {
        float randomPos = Random.Range(-5, 5);
        localPlayer.transform.localPosition = new Vector2(randomPos, -1);
    }

    public void EnableRespawn()
    {
        respawnTime = totalRespawnTimer;

        startRespawn = true;
        respawnUI.SetActive(true);
    }

    void StartRespawn()
    {
        isAlive = false;
        respawnTime -= Time.deltaTime;
        spawnTimer.text = "Respawn IN: " + respawnTime.ToString("F0");

        if(respawnTime  <= 0)
        {
            respawnUI.SetActive(false);
            localPlayer.GetComponent<PhotonView>().RPC("Revive",RpcTarget.AllBuffered);
            PlayerRespawnPos();
            isAlive = true;
            startRespawn = false;
        }
    }

    #endregion

}
