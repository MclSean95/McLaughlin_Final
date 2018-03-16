using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class DataController : MonoBehaviour {

    public RoundData[] allRoundData;
    public SocketIOComponent socket;
    bool dataSent = false;
    private RoundData dataFromServer;

	// Use this for initialization
	void Start () {
        socket = GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MenuScreen");
        socket.On("dataRecieved", OnRecieved);
        socket.On("gotData", GetCurrentRoundData);
	}
	
	public void GetCurrentRoundData (SocketIOEvent e) {
        dataFromServer = JsonUtility.FromJson<RoundData>(e.data.ToString());
	}

    private void SendQuestionData()
    {
        socket.Emit("sendData", new JSONObject(JsonUtility.ToJson(allRoundData[0])));       
    }

    public RoundData GetRoundDataFromServer()
    {
        return dataFromServer;
    }

    private void OnRecieved(SocketIOEvent e)
    {
        dataSent = true;
        socket.Emit("getData");
    }

    private void Update()
    {
        //This may look odd but it wouldn't work in the start function
        if (!dataSent)
            SendQuestionData();
    }
}
