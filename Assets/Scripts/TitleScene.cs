using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{

    //Load 패널 관련
    [SerializeField] private Text HpText;
    [SerializeField] private Text BulletText;
    [SerializeField] private Text GrenadeText;
    [SerializeField] private Text StageText;
    [SerializeField] private Text zombieKillsText;

    [SerializeField] private GameObject LoadButton;

    private StatusSaveData saveData1;

    private void Start()
    {
        saveData1 = JsonUtility.FromJson<StatusSaveData>(GameManager.Instance.saveLoadManager.LoadSaveData());
        //DescribeSaveDataTexts();
    }

    public void DescribeSaveDataTexts()    //Load 패널 텍스트 설정 함수
    {
        if (saveData1 != null)
        {
            HpText.text = $"HP : {saveData1.nowHp}";
            BulletText.text = $"Bullet : {saveData1.nowBullet_mag + saveData1.nowBullet_reserve}";
            GrenadeText.text = $"Grenade : {saveData1.nowGrenade}";
            StageText.text = $"Stage : {saveData1.nowStageIndex}";
            zombieKillsText.text = $"Zombie Kills : {saveData1.totalZombieKills}";
            LoadButton.SetActive(true);
        }
        else
        {
            HpText.text = "";
            BulletText.text = "";
            GrenadeText.text = "No Data";
            StageText.text = "";
            zombieKillsText.text = "";
            LoadButton.SetActive(false);
        }
    }

    public void StartGame()     //start 버튼 함수
    {
        GameManager.Instance.LoadNextScene();
    }

    public void LoadGame()      //Load 버튼 함수
    {
        GameManager.Instance.saveLoadManager.InsertSaveData(saveData1);
        GameManager.Instance.LoadCurrentScene();
    }

    public void QuitGame()      //게임종료 함수
    {
        Application.Quit();
    }
}
