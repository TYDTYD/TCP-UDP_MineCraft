using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject[] gameObjects;
    public GameObject Cube;
    public int seq = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroy object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id,string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    // int로 이넘 타입을 받아서 각각의 타입을 받아넣기
    public void SpawnCube(Vector3 _position, int type)
    {
        //gameObjects[seq++].transform.Translate(_position);
        Instantiate(gameObjects[type], _position, Quaternion.identity);
    }
}