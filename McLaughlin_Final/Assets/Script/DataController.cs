using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class DataController : MonoBehaviour {

    public RoundData[] allRoundData;
    public SocketIOComponent socket;
    bool dataSent = false;

	// Use this for initialization
	void Start () {
        socket = GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MenuScreen");
        socket.On("dataRecieved", OnRecieved);
        socket.On("gotData", GetCurrentRoundData);
	}
	
	public void GetCurrentRoundData (SocketIOEvent e) {
        GetRoundData(e.data.ToString());
	}

    private void SendQuestionData()
    {
        for (int i = 0; i < allRoundData[0].questions.Length; i++)
        {
            socket.Emit("sendData", new JSONObject(DataFormatting(allRoundData[0], i)));
            Debug.Log(DataFormatting(allRoundData[0], i));
        }        
    }

    private string DataFormatting(RoundData data, int index)
    {
        return string.Format(@"{{""Name"":""{0}"", ""TimeLimit"":""{1}"", ""Points"":""{2}"", ""Question"":""{3}"", ""A1"":""{4}"", ""A2"":""{5}"", ""A3"":""{6}"", ""A4"":""{7}"", ""A1IsCorrect"":""{8}"", ""A2IsCorrect"":""{9}"",""A3IsCorrect"":""{10}"",""A4IsCorrect"":""{11}""}}", data.name, data.timeLimitInSeconds, data.timeLimitInSeconds, data.questions[index].questionText, data.questions[index].answers[0].answerText, data.questions[index].answers[1].answerText, data.questions[index].answers[2].answerText, data.questions[index].answers[3].answerText, data.questions[index].answers[0].isCorrect, data.questions[index].answers[1].isCorrect, data.questions[index].answers[2].isCorrect, data.questions[index].answers[3].isCorrect);
    }

    private RoundData GetRoundData(string data)
    {
        return JsonUtility.FromJson<RoundData>(data);
    }

    private void OnRecieved(SocketIOEvent e)
    {
        dataSent = true;
        //socket.Emit("getData");
    }

    private void Update()
    {
        if (!dataSent)
            SendQuestionData();
    }
}
