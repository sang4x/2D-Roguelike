using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float LeveStartDelay = 2f;
    private List<Enermy> enermies;
    private bool enermyMoving;
    public float turndelay = 0f;
    public static GameManager instance = null;
    private BoardManager boardscript;
    private int level = 1;
    public int FoodPoint = 100;
    public bool playerturn = true;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetUp;
    void Awake()
    {
        enermies = new List<Enermy>();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardscript = GetComponent<BoardManager>();
        InitGame();
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }
    void InitGame()
    {
        Invoke("HideLevelImage", LeveStartDelay);
        doingSetUp = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        enermies.Clear();
        boardscript.setupscene(level);
    }
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetUp = false;
    }
    public void GameOver()
    {
        levelText.text = "After " + level + " Day You Starve";
        levelImage.SetActive(true);
        enabled = false;
    }
    IEnumerator MoveEnermy()
    {
        enermyMoving = true;
        yield return new WaitForSeconds(turndelay);
        if (enermies.Count == 0)
        {
            yield return new WaitForSeconds(turndelay);
        }
        for (int  i = 0;  i < enermies.Count;  i++)
        {
            enermies[i].moveenermy();
            yield return new WaitForSeconds(enermies[i].moveTime);

        }

        playerturn = true;
        enermyMoving = false;

    }
    private void Update()
    {
        if (playerturn || enermyMoving || doingSetUp)
        {
            return;
        }
        StartCoroutine(MoveEnermy());
    }
    public void AddEnermyToList(Enermy script)
    {
        enermies.Add(script);
    }
}
