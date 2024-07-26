using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.UI;

//싱글톤 기법
//게임매니저는 게임 전체를 컨트롤 해야 하므로 접근이 쉬워야 한다.
//static 변수를 만든 후 이 변수가 대표해서 게임매니저에 접근하게 한다.
//무분별한 객체 생성을 막고 하나만 생성되게 하는 기법.

//Enemy가 생성되는 로직, 게임 전체를 관리하는 클래스

// 1. Enemy Prefabs     2. 스폰위치    3. 스폰간격    4. 몇마리 스폰할지

public class GameManager : MonoBehaviour
{
    public static GameManager G_Instance;
    public CanvasGroup canvasGroup;
    public Text killText;
    private float timePreV;
    public static int killCount = 0;
    public static bool isOpened = false;
    //public GameObject[] EnemyPrefabs;
    //public int maxCount = 10;
    //string enemyTag = "ENEMY";

    [Header("Datamanager")]
    [SerializeField] DataManager dataManager;
    public GameData gameData;

    //인벤토리 아이템이 변경되었을 때 발생 시킬 이벤트 정의
    public delegate void ItemChangedDelegate();
    public static event ItemChangedDelegate OnItemChange;
    [SerializeField] private GameObject slotList;
    public GameObject[] itemObjects;

    void Awake()
    {
        if (G_Instance == null)
            G_Instance = this;

        else if (G_Instance != this)
            Destroy(gameObject);

        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

        killText = GameObject.Find("Canvas_UI").transform.GetChild(4).GetComponent<Text>();

        DontDestroyOnLoad(gameObject);
        LoadGameData();
    }

    void Start()
    {
        canvasGroup = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
    }

    void LoadGameData()
    {
        //killCnt = PlayerPrefs.GetInt("KILLCOUNT", 0);

        #region 하드디스크에 저장된 데이터 넘어오는중
        GameData data = dataManager.Load();
        gameData.HP = data.HP;
        gameData.damage = data.damage;
        gameData.killcnt = data.killcnt;
        gameData.equipItem = data.equipItem;
        gameData.speed = data.speed;
        #endregion

        if (gameData.equipItem.Count > 0)
            InventorySetUp();

        killText.text = $"<color=#ff0000>KILL</color> " + gameData.killcnt.ToString("0000");
    }

    void InventorySetUp()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();

        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            for (int j = 1; j < slots.Length; j++)  //j=1 -> slotlist(부모) 빼고 하려고
            {
                if (slots[j].childCount > 0) continue;

                int itemIdx = (int)gameData.equipItem[i].itemtype;
                itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[j].transform);
                itemObjects[itemIdx].GetComponent<itemInfo>().C_item = gameData.equipItem[i];

                break;
            }
        }
    }

    void SaveGameData()
    {
        dataManager.Save(gameData);
    }

    public void AddItem(Item item)
    {
        if (gameData.equipItem.Contains(item)) return;

        gameData.equipItem.Add(item);

        switch (item.itemtype)
        {
            case Item.ITEMTYPE.HP:
                if (item.itemcal == Item.ITEMCAL.VALUE)
                    gameData.HP += item.value;

                else
                    gameData.HP += gameData.HP * item.value;
                break;

            case Item.ITEMTYPE.SPEED:
                if (item.itemcal == Item.ITEMCAL.VALUE)
                    gameData.speed += item.value;

                else
                    gameData.speed += gameData.speed * item.value;
                break;

            case Item.ITEMTYPE.DAMAGE:
                if (item.itemcal == Item.ITEMCAL.VALUE)
                    gameData.damage += item.value;

                else
                    gameData.damage += gameData.damage * item.value;
                break;

            case Item.ITEMTYPE.GRENADE:

                break;
        }

        OnItemChange();
    }

    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        switch (item.itemtype)
        {
            case Item.ITEMTYPE.HP:
                if (item.itemcal == Item.ITEMCAL.VALUE)
                    gameData.HP -= item.value;

                else
                    gameData.HP = gameData.HP / (1.0f + item.value);
                break;

            case Item.ITEMTYPE.SPEED:
                if (item.itemcal == Item.ITEMCAL.VALUE)
                    gameData.speed -= item.value;

                else
                    gameData.speed = gameData.speed / (1.0f + item.value);
                break;

            case Item.ITEMTYPE.DAMAGE:
                if (item.itemcal == Item.ITEMCAL.VALUE)
                    gameData.damage -= item.value;  //더하는 방식

                else
                    gameData.damage = gameData.damage / (1.0f + item.value);
                break;

            case Item.ITEMTYPE.GRENADE:

                break;
        }

        OnItemChange();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GamePause();

        if (Input.GetKeyDown(KeyCode.I))
            InventoryOnOff();
    }

    public bool isPaused = false;

    public void GamePause()
    {

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();

        if (!isOpened)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0.0f : 1.0f;

            foreach (var script in scripts)
                script.enabled = !isPaused;
        }


        else if (isOpened)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void InventoryOnOff()
    {
        if (!isPaused)
        {
            isOpened = !isOpened;
            InventoryUpdate();
        }
    }

    public void InventoryUpdate()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();

        foreach (var script in scripts)
            script.enabled = !isOpened;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        canvasGroup.alpha = isOpened ? 1.0f : 0.0f;
        canvasGroup.interactable = isOpened ? true : false;
        canvasGroup.blocksRaycasts = isOpened ? true : false;
    }

    public void KillScore(int score)
    {
        gameData.killcnt++;
        killText.text = $"<color=#ff0000>KILL</color> " + gameData.killcnt.ToString("0000");
    }

    void OnApplicationQuit()
    {
        SaveGameData();
    }

}