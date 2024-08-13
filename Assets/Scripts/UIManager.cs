using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject bonusPanel;
    public GameObject BonusPanel { get => bonusPanel; }
    [SerializeField] private GameObject gameOverRetryButton;
    [SerializeField] private GameObject pauseMenuPanel;

    [SerializeField] private Text servedGrenadeText;
    [SerializeField] private Text nowBulletText;
    [SerializeField] private Text servedBulletText;
    [SerializeField] private Text HpText;
    [SerializeField] private Text StageIndex;
    //[SerializeField] private Text zombieKillsText;
    
    private void Awake()
    {
        
    }

    public void ShowBonusPanel(bool flag)   //보너스메뉴 활성화 함수
    {
        bonusPanel.SetActive(flag);
    }

    public void ShowGameOverRetryButton(bool flag)  //게임오버 재시작 버튼 활성화 함수
    {
        gameOverRetryButton.SetActive(flag);
    }

    public void ShowPauseMenuPanel(bool flag)   //일시정지 메뉴 활성화 함수
    {
        pauseMenuPanel.SetActive(flag);
        if (flag)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
            GameManager.Instance.isPause = false;
        }
    }

    public void UpdateBulletData(int nowBullet, int servedBullet)   //총알 데이터 업데이트 함수
    {
        nowBulletText.text = nowBullet.ToString();
        servedBulletText.text = servedBullet.ToString();
    }

    public void UpdateGrenadeData(int amount)   //수류탄 데이터 업데이트 함수
    {
        servedGrenadeText.text = amount.ToString();
    }

    public void UpdateHpData(float amount)      //체력 데이터 업데이트 함수
    {
        HpText.text = Mathf.CeilToInt(amount).ToString();
    }

    public void ResetAllTexts(ref StatusSaveData saveData) //스테이지 내 모든 텍스트들 초기화 함수
    {
        servedGrenadeText.text = saveData.nowGrenade.ToString();
        nowBulletText.text = saveData.nowBullet_mag.ToString();
        servedBulletText.text = saveData.nowBullet_reserve.ToString();
        HpText.text = Mathf.CeilToInt(saveData.nowHp).ToString();
        StageIndex.text = $"Stage {GameManager.Instance.nowStageIndex}";
        //zombieKillsText.text = GameManager.Instance.zombieKills.ToString();
    }

    public void StageBonust(string type)    //스테이지 보너스 함수 -> 게임매니저 호출
    {
        GameManager.Instance.StageBonus(type);
    }

    public void QuitGame()  //게임종료 함수
    {
        Application.Quit();
    }
}