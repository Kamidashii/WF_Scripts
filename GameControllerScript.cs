using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GameControllerScript : MonoBehaviour
{

    public GameObject player;
    public GameObject boss;

    public FridgeScript fridge;

    public GameObject death_Screen;

    public Transform respawnPosition;
    public float respawnTime;

    public Slider enemySlider;

    public Text dialogText;
    public Text hintText;
    public Text scoreText;


    public GameObject fire;
    public Transform fireSpawn;

    public bool respawn = false;
    public GameObject buttons;

    public bool key = false;
    public float maxHealth;
    public float maxBossHealth;
    public int maxGems;

    public float playerHealth;
    public float bossHealth;

    public int gems = 0;

    public string gameOverText;
    public string gameOverTips;

    public float cameraMinSize;
    public float cameraNormalSize;
    public float cameraDegree;

    public List<GameObject> activeObjects;
    public GameObject[] bullets;

    public Inventory playerInventory;


    [SerializeField]
    Btn_ExitScript ExitActions;

    #region

    [SerializeField] string bossHealthSaveName;
    [SerializeField] string playerHealthSaveName;
    [SerializeField] string playerPosXSaveName;
    [SerializeField] string playerPosYSaveName;
    [SerializeField] string playerKeySaveName;
    [SerializeField] string currentGemsSaveName;
    [SerializeField] string fireNumberSaveName;
    [SerializeField] string bulletSaveName;
    [SerializeField] string bulletsCountSaveName;
    [SerializeField] string stopSaveName;
    [SerializeField] string countItemsSaveName;
    [SerializeField] string itemSaveName;
    [SerializeField] string FridgeSaveName;
    [SerializeField] string FridgeItemSaveName;

    #endregion

    public int SaveGame()
    {
        SaveActiveObjects();

        SaveBullets();

        SavePlayer();
        SaveBoss();
        
        SaveObjBin(fridge.data, FridgeSaveName);

        SaveInventory();

        SavePause();
        return 0;
    }



    void SaveActiveObjects()
    {
        foreach (var obj in activeObjects)
        {
            if (obj != null)
            {
                var script = obj.GetComponent<ActiveObject>();
                if (script != null)
                {
                    SaveObjBin(obj.GetComponent<ActiveObject>().data, obj.name);
                }
            }
        }
    }

    void SaveBullets()
    {
        bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int j = 0; j < bullets.Length; ++j)
        {
            PlayerPrefs.SetFloat(bulletSaveName + j, bullets[j].GetComponent<Transform>().position.x);
        }
        PlayerPrefs.SetInt(bulletsCountSaveName, bullets.Length);
    }

    void SavePlayerCoords()
    {
        PlayerPrefs.SetFloat(playerPosXSaveName, player.GetComponent<Transform>().position.x);
        PlayerPrefs.SetFloat(playerPosYSaveName, player.GetComponent<Transform>().position.y);
    }

    void SaveHealth()
    {
        PlayerPrefs.SetFloat(playerHealthSaveName, player.GetComponent<playerController>().health);
    }

    void SaveGems()
    {
        PlayerPrefs.SetInt(currentGemsSaveName, gems);
    }

    void SaveKey()
    {
        PlayerPrefs.SetString(playerKeySaveName, (player.GetComponent<playerController>().key).ToString());
    }
    void SavePlayer()
    {
        SavePlayerCoords();
        SaveHealth();
        SaveKey();
        SaveGems();
    }

    void SaveBoss()
    {
        PlayerPrefs.SetFloat(bossHealthSaveName, boss.GetComponent<BossScript>().health);
        PlayerPrefs.SetInt(fireNumberSaveName, (boss.GetComponent<BossScript>().fireNum));
    }



    private int CountItems()
    {
        int count = 0;
        for (int i = 0; i < playerInventory.capacity; ++i)
        {
            if (playerInventory.content[i].transform.childCount > 0)
            {
                count++;
            }
        }
        return count;
    }

    private void SaveItems()
    {
        for (int i = 0; i < playerInventory.capacity; ++i)
        {
            if (playerInventory.content[i].transform.childCount > 0)
            {
                PlayerPrefs.SetInt(itemSaveName + i, playerInventory.content[i].GetComponentInChildren<Item>().id);
            }
        }
    }

    void SaveInventory()
    {
        PlayerPrefs.SetInt(countItemsSaveName, CountItems());
        SaveItems();
    }

    void SavePause()
    {
        PlayerPrefs.SetInt(stopSaveName, (buttons.GetComponent<Btn_ExitScript>().stop) ? 1 : 0);
    }

    void SaveObjBin(object obj,string name)
    {
        Debug.Log("name: " + name);
        string ext = ".bs";
        string path = Application.dataPath + "/" + name + ext;
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream fs = new FileStream(path, FileMode.CreateNew,FileAccess.Write);
        try
        {
            bf.Serialize(fs, obj);
            fs.Close();
        }
        catch(Exception exc)
        {
            AppendLog(exc);
            fs.Close();
        }
    }

    void AppendLog(Exception exc)
    {
        using (FileStream excFS = new FileStream(Application.dataPath + "/log.txt", FileMode.Append, FileAccess.Write))
        {
            using (StreamWriter writer = new StreamWriter(excFS))
            {

                string mess = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " : " + exc.Message + " \n";
                writer.WriteLine(mess);
            }
        }
    }

    private void SetDeathTexts()
    {
        dialogText.GetComponent<Text>().text = gameOverText;
        hintText.GetComponent<Text>().text = gameOverTips;
    }

    public void ResetTexts()
    {
        dialogText.GetComponent<Text>().text = "";
        hintText.GetComponent<Text>().text = "";
    }

    void CameraToPlayer()
    {
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,
           Camera.main.transform.position.z);
    }

    public void LoadSave()
    {
        if (PlayerPrefs.HasKey(playerHealthSaveName))
        {
            LoadActiveObjects();

            LoadBoss();

            LoadPlayer();

            CameraToPlayer();

            LoadBullets();
            
            fridge.data = LoadObjBin(ref fridge.data, FridgeSaveName);

            LoadPause();

            LoadItems();

            ClearPickedItemsFromMap();

        }
    }

    void LoadActiveObjects()
    {
        foreach (var obj in activeObjects)
        {
            var script = obj.GetComponent<ActiveObject>();
            if (script != null)
            {
                obj.GetComponent<ActiveObject>().data= LoadObjBin(ref obj.GetComponent<ActiveObject>().data, obj.name);
            }
        }
    }

    void LoadBoss()
    {
        boss.GetComponent<BossScript>().health = PlayerPrefs.GetFloat(bossHealthSaveName);
        boss.GetComponent<BossScript>().fireNum = PlayerPrefs.GetInt(fireNumberSaveName);
    }

    void LoadPlayerHealth()
    {
        player.GetComponent<playerController>().health = PlayerPrefs.GetFloat(playerHealthSaveName);

        player.GetComponent<playerController>().healthSlider.value = player.GetComponent<playerController>().health;
    }

    void LoadPlayerCoords()
    {
        player.GetComponent<Transform>().position = new Vector3(PlayerPrefs.GetFloat(playerPosXSaveName), PlayerPrefs.GetFloat(playerPosYSaveName), 0f);
    }

    void LoadKey()
    {
        player.GetComponent<playerController>().key = bool.Parse(PlayerPrefs.GetString(playerKeySaveName));
    }

    void LoadGems()
    {
        gems = PlayerPrefs.GetInt(currentGemsSaveName);

        scoreText.GetComponent<Text>().text = "";
        scoreText.GetComponent<ScoreChangeScript>().ChangeScore(gems);
    }

    void LoadPlayer()
    {
        LoadPlayerHealth();
        LoadPlayerCoords();
        LoadKey();
        LoadGems();
    }

    void LoadBullets()
    {
        int count = PlayerPrefs.GetInt(bulletsCountSaveName);
        for (int i = 0; i < count; ++i)
        {
            Vector3 spawn = new Vector3(PlayerPrefs.GetFloat(bulletSaveName + i),
                fireSpawn.position.y, fireSpawn.position.z);
            Instantiate(fire, spawn, fireSpawn.rotation);
        }
    }

    void LoadPause()
    {
        bool stop = (PlayerPrefs.GetInt(stopSaveName) == 1) ? true : false;

        if (stop)
        {
            buttons.GetComponent<Btn_ExitScript>().stop = stop;
            hintText.text = buttons.GetComponent<Btn_ExitScript>().stopHint;
            Time.timeScale = 0f;
        }
    }


    T LoadObjBin<T>(ref T obj, string name)
    {
        string ext = ".bs";
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.dataPath + "/" + name + ext))
        {
            using (FileStream fs = new FileStream(Application.dataPath + "/" + name + ext, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    obj = (T)bf.Deserialize(fs);
                    fs.Close();
                }
                catch (Exception exc)
                {
                    AppendLog(exc);
                    fs.Close();
                }
            }
        }
        return obj;
    }
    
    void LoadItems()
    {
        int count = PlayerPrefs.GetInt(countItemsSaveName);
        for (int i = 0; i < count; ++i)
        {
            foreach (Item item in Resources.LoadAll<Item>("SuperTest"))
            {
                if (item.id == PlayerPrefs.GetInt(itemSaveName + i))
                {
                    playerInventory.Add_Item(item.gameObject);
                }
            }
        }
    }

    void ClearPickedItemsFromMap()
    {
        for (int i = 0; i < playerInventory.capacity; ++i)
        {
            if (playerInventory.content[i].transform.childCount > 0)
            {
                foreach (GameObject obj in activeObjects)
                {
                    var item = obj.GetComponent<ActiveObject>().item;
                    if (item != null)
                    {
                        if (item.GetComponent<Item>().id == (playerInventory.content[i].GetComponentInChildren<Item>().id))
                        {
                            Destroy(obj);
                            break;
                        }
                    }
                }
            }
        }
    }

    void PlayerSetFullHealth()
    {
        player.GetComponent<playerController>().health = maxHealth;
        player.GetComponent<playerController>().healthSlider.value = maxHealth;
    }

    void BossSetFullHealth()
    {
        boss.GetComponent<BossScript>().health = maxBossHealth;
        boss.GetComponent<BossScript>().healthSlider.value = maxBossHealth;
    }
    void Start()
    {
        if (PlayerPrefs.HasKey(playerHealthSaveName))
        {
            LoadSave();
        }
        else
        {
            PlayerSetFullHealth();
            BossSetFullHealth();

            ChangeScore();
        }
        enemySlider.gameObject.SetActive(false);
    }

    public void ChangeScore()
    {
        scoreText.GetComponent<ScoreChangeScript>().ChangeScore(gems);
    }

    void DisablePlayer()
    {
        player.GetComponent<playerController>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void EnablePlayer()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
        player.GetComponent<playerController>().enabled = true;
        player.GetComponent<playerController>().dying = false;
    }
    public IEnumerator DyingTime()
    {
        SetDeathTexts();

        DisablePlayer();

        Camera.main.GetComponent<CameraMoveScript>().dying = true;
        death_Screen.SetActive(true);
        while (Camera.main.GetComponent<Camera>().orthographicSize > cameraMinSize)
        {
            Camera.main.GetComponent<Camera>().orthographicSize -= cameraDegree;
            Camera.main.GetComponent<Transform>().position = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, Camera.main.GetComponent<Transform>().position.z);

        }
        yield return new WaitForSeconds(respawnTime);
        ResetTexts();
        yield return null;
    }

    public IEnumerator Respawn()
    {
        StartCoroutine(DyingTime());
        yield return new WaitForSeconds(respawnTime);
        player.GetComponent<playerController>().health = maxHealth;
        Camera.main.GetComponent<CameraMoveScript>().dying = false;
        player.GetComponent<Transform>().position = respawnPosition.position;
        Camera.main.GetComponent<Camera>().orthographicSize = cameraNormalSize;

        death_Screen.SetActive(false);

        EnablePlayer();

        boss.GetComponent<BossScript>().health = boss.GetComponent<BossScript>().maxHealth;
        respawn = false;
    }

    void BossDefeated()
    {
        boss.SetActive(false);
        enemySlider.gameObject.SetActive(false);
        player.GetComponent<playerController>().damageLeft.SetActive(false);
        player.GetComponent<playerController>().damageRight.SetActive(false);
    }

    void PlayerCombatEnable()
    {
        player.GetComponent<playerController>().damageLeft.SetActive(true);
        player.GetComponent<playerController>().damageRight.SetActive(true);
    }

    void PlayerCombatDisable()
    {
        player.GetComponent<playerController>().damageLeft.SetActive(false);
        player.GetComponent<playerController>().damageRight.SetActive(false);
    }
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z)) Don't forget to delete this later!
        //{
        //    ChaosMode();
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitActions.MenuLoader());
        }

        if (Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.P))
        {
            ExitActions.Stop();
        }

        if (boss.GetComponent<BossScript>().health <= 0)
        {
            BossDefeated();
        }
        else
        {
            if (boss.GetComponent<BossScript>().seen)
            {
                PlayerCombatEnable();
            }
            else
            {
                PlayerCombatDisable();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !respawn)
        {
            StartCoroutine(Respawn());
            respawn = true;
        }
    }

    void ChaosMode()
    {
        foreach (GameObject obj in activeObjects)
        {
            var collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = false;
                obj.AddComponent<Rigidbody2D>();
                obj.GetComponent<Rigidbody2D>().gravityScale = 0.03f;
                Vector2 randCoords = UnityEngine.Random.Range(400, 500) * UnityEngine.Random.insideUnitCircle;
                obj.GetComponent<Rigidbody2D>().AddForce(randCoords);
                obj.GetComponent<Rigidbody2D>().rotation = UnityEngine.Random.Range(0, 360);
            }
        }
    }
}
