using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FpsDamage : MonoBehaviour
{
    [Header("UI")]
    public Image HPBar;
    public Text HPText;
    public int curHP = 0;
    public int initHP = 100;
    public string attackTag = "ATTACK_HITBOX";
    public bool isPlayerDie = false;
    public GameObject ScreenImage;
    
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void OnEnable()
    {
        curHP = initHP;
        GameManager.OnItemChange += UpdateSetUp;
        HPText.text = $"HP <color=#FFAAAA>{curHP.ToString()}</color>";
    }

    void UpdateSetUp()
    {
        initHP = (int)GameManager.G_Instance.gameData.HP;
        curHP += (int)GameManager.G_Instance.gameData.HP - curHP;
    }

    void Start()
    {
        //ScreenImage = GameObject.Find("Image_Screen").gameObject; //게임오브젝트가 체크 해제되어있으면 못찾음
        initHP = (int)GameManager.G_Instance.gameData.HP;
        curHP = initHP;
        ScreenImage = GameObject.Find("Canvas_UI").transform.GetChild(5).gameObject;
        HPBar.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(attackTag))
        {
            HP_Info();

            if (curHP <= 0)
                PlayerDie();
        }
    }
    private void HP_Info()
    {
        curHP -= 1;
        HPBar.fillAmount = (float)curHP / (float)initHP;

        if (HPBar.fillAmount <= 0.3f)
            HPBar.color = Color.red;

        else if (HPBar.fillAmount <= 0.5f)
            HPBar.color = Color.yellow;

        else if (HPBar.fillAmount <= 1f)
            HPBar.color = Color.green;

        HPText.text = $"HP <color=#FFAAAA>{curHP.ToString()}</color>";
    }

    public void PlayerDie()
    {
        /* ScreenImage.SetActive(true);    //해당 오브젝트 활성화
        isPlayerDie = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        //런타임에서 ENEMY라는 태그를 가진 오브젝트를 찾아서 enemies 배열에 저장

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SendMessage("PlayerDeath", SendMessageOptions.DontRequireReceiver);
            //다른 게임오브젝트에 있는 함수를 호출할 때 사용하는 함수
        } */

        OnPlayerDie();

        Invoke("MoveNextScene", 3.0f);
        //3초 후 MoveNextScene 함수 호출
    }
    void MoveNextScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    
}
