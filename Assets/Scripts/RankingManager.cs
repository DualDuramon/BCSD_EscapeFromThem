using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Palmmedia.ReportGenerator.Core.Common;
using System;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private int maxRankingCount = 10;
    //��ŷ ������ ��ġ ����
    private string RANK_DATA_DIRECTORY;
    private string RANK_FILENAME = "RankFile.txt";

    //��ŷ������ ���� ����
    private List<RankData> rankingDataList = null;
    [SerializeField] private List<GameObject> rankingSet;
    
    
    private void Awake()
    {
        RANK_DATA_DIRECTORY = Application.dataPath + "/RankData/";
        if (!Directory.Exists(RANK_DATA_DIRECTORY)) {
            Directory.CreateDirectory(RANK_DATA_DIRECTORY);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            RoadRankingData();
            InitRankingList();
        }
    }

    private void RoadRankingData()  //��ŷ ������ �ε�
    {
        if(!File.Exists(RANK_DATA_DIRECTORY + RANK_FILENAME))
        {
            Debug.Log("RankFile.txt does not exist.");
            rankingDataList = new List<RankData>(maxRankingCount);
            
            for(int i = 0; i< maxRankingCount; i++)
            {
                rankingDataList.Add(new RankData());
            }
            MakeRankingFile_Json();
            return;
        }
        else
        {
            string json = File.ReadAllText(RANK_DATA_DIRECTORY + RANK_FILENAME);
            RankDataList myList = JsonUtility.FromJson<RankDataList>(json);
            rankingDataList = new List<RankData>(myList.dataList);
        }
    }

    private void MakeRankingFile_Json()
    {
        RankDataList myList = new RankDataList();
        myList.dataList = rankingDataList.ToArray();

        string json = JsonUtility.ToJson(myList);
        File.WriteAllText(RANK_DATA_DIRECTORY + RANK_FILENAME, json);
    }

    private void InitRankingList()  //��ŷ ���ھ�� �ʱ�ȭ
    {
        for (int i = 0; i < rankingDataList.Count; i++)
        {
            rankingSet[i].transform.GetChild(1).GetComponent<Text>().text = rankingDataList[i].name;
            rankingSet[i].transform.GetChild(3).GetComponent<Text>().text = rankingDataList[i].score.ToString();
        }

        //���ӸŴ����� ������ ���
        GameManager.Instance.minScore = rankingDataList[rankingDataList.Count - 1].score;
    }

    public void RankingFileUpdate(InputField nameInputField, int totalScore)  //��ŷ ���� ������Ʈ �Լ�
    {
        string userName = nameInputField.text;

        if (userName == null || userName == "")
        {
            userName = "Anonymous";
            Debug.Log("UserName is empty. Set to \"Anonymous\"");
        }

        if (!File.Exists(RANK_DATA_DIRECTORY + RANK_FILENAME))
        {
            Debug.Log("RankFile.txt does Not Exist. Can't Regist new Ranking.");
            return;
        }

        //��ŷ ������ �ε�
        string myJson = File.ReadAllText(RANK_DATA_DIRECTORY + RANK_FILENAME);
        RankDataList myList = JsonUtility.FromJson<RankDataList>(myJson);
        rankingDataList = new List<RankData>(myList.dataList);

        //10�� ��ŷ ������ ����
        rankingDataList[rankingDataList.Count - 1].name = nameInputField.text;
        rankingDataList[rankingDataList.Count - 1].score = totalScore;

        rankingDataList.Sort();
        MakeRankingFile_Json();

        GameManager.Instance.GoToTitleScene();
    }
}
