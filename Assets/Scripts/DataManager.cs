using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    static public DataManager S;
    private int isSFXON;
    private int isMusicON;    
    private int HighScore;
    private int HighDistance;
    private int Coins;
    private int isTutorialOn;
    private int _counter;

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        DontDestroyOnLoad(gameObject);
        SetCoins(PlayerPrefs.GetInt("Coins"));
        Debug.Log(GetSFX());
    }

    public void SetTutorial(bool on)
    {
        if (on)
        {
            isTutorialOn = 1;
            PlayerPrefs.SetInt("Tutorial", 1);

        }
        else
        {
            isTutorialOn = 0;
            PlayerPrefs.SetInt("Tutorial", 0);
        }
    }
    public int GetTutorial()
    {
        if (PlayerPrefs.HasKey("Tutorial"))
            return PlayerPrefs.GetInt("Tutorial");
        else
        {
            PlayerPrefs.SetInt("Tutorial", 1);
            return PlayerPrefs.GetInt("Tutorial");
        }
    }
    public void SetSFX(bool on)
    {
        if (on)
        {
            isSFXON = 1;
            PlayerPrefs.SetInt("SFX", 1);

        }
        else
        {
            isSFXON = 0;
            PlayerPrefs.SetInt("SFX", 0);
        }        
    }
    public int GetSFX()
    {
        if (PlayerPrefs.HasKey("SFX"))
            return PlayerPrefs.GetInt("SFX");
        else
        {
            PlayerPrefs.SetInt("SFX", 1);
            return PlayerPrefs.GetInt("SFX");
        }
    }
    public void SetMusic(bool on)
    {
        if (on)
        {
            isMusicON = 1;
            PlayerPrefs.SetInt("Music", 1);

        }
        else
        {
            isMusicON = 0;
            PlayerPrefs.SetInt("Music", 0);
        }
    }
    public int GetMusic()
    {
        if (PlayerPrefs.HasKey("Music"))
            return PlayerPrefs.GetInt("Music");
        else
        {
            PlayerPrefs.SetInt("Music", 1);
            return PlayerPrefs.GetInt("Music");
        }
    }
    public void SetHighScore(int newHighScore)
    {
        //Debug.Log("SetHighScore");
        HighScore = newHighScore;
        PlayerPrefs.SetInt("HighScore", HighScore);
    }
    public int GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            return PlayerPrefs.GetInt("HighScore");
        else
            return 0;
    }
    public int GetHighDistance()
    {
        if (PlayerPrefs.HasKey("HighDistance"))
            return PlayerPrefs.GetInt("HighDistance");
        else
            return 0;
    }
    public int GetCoinsAmt()
    {
        if (PlayerPrefs.HasKey("Coins"))
            return PlayerPrefs.GetInt("Coins");
        else
            PlayerPrefs.SetInt("Coins", Coins);
        return PlayerPrefs.GetInt("Coins");
    }
    public void SetHighDistance(int newHighDistance)
    {
        HighDistance = newHighDistance;
        PlayerPrefs.SetInt("HighDistance", HighDistance);
    }
    public void SetCoins(int newCoinsAmt)
    {
        Coins = newCoinsAmt;
        PlayerPrefs.SetInt("Coins", Coins);
    }


}
