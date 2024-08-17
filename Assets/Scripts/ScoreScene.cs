using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScene : MonoBehaviour
{
    [SerializeField] private Animator myAC;
    [SerializeField] private Text heartScoreTxt;
    [SerializeField] private Text bulletScoreTxt;
    [SerializeField] private Text grenadeScoreTxt;
    [SerializeField] private Text killScoreTxt;
    [SerializeField] private Text totalScoreTxt;

    [SerializeField] private int heartScore_Per_One;
    [SerializeField] private int bulletScore_Per_One;
    [SerializeField] private int grenadeScore_Per_One;
    [SerializeField] private int killPoint_Per_One;
    private int totalScore = 0;

    
    void Start()
    {
        CalCulateScore();
        myAC.SetTrigger("Start");
    }

    private void StartSceneAnimation()  //클리어 씬 애니메이션 시작 함수
    {
        myAC.SetTrigger("Start");
    }

    private void CalCulateScore()   //점수 계산 함수
    {
        //체력 점수 계산
        int buffer = (int)GameManager.Instance.CurrentSaveData.nowHp * heartScore_Per_One;
        heartScoreTxt.text = $"{buffer} pt";
        totalScore += buffer;

        //총알 점수 계산
        buffer = (GameManager.Instance.CurrentSaveData.nowBullet_mag 
            + GameManager.Instance.CurrentSaveData.nowBullet_reserve) 
            * bulletScore_Per_One;
        bulletScoreTxt.text = $"{buffer} pt";
        totalScore += buffer;

        //수류탄 점수 계산
        buffer = GameManager.Instance.CurrentSaveData.nowGrenade * grenadeScore_Per_One;
        grenadeScoreTxt.text = $"{buffer} pt";
        totalScore += buffer;

        //좀비 킬 점수 계산
        buffer = GameManager.Instance.CurrentSaveData.totalZombieKills;
        killScoreTxt.text = $"{buffer} Kills";
        totalScore += buffer;

        //totalScore 텍스트 설정
        totalScoreTxt.text = totalScore.ToString();
    }

    public void GotoTitleScene()
    {
        GameManager.Instance.GoToTitleScene();
    }

}
