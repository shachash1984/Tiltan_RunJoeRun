using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

    [SerializeField] private string ADID = "1567950";

    void Awake()
    {
        Advertisement.Initialize(ADID);
    }

    public void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show(options);
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            DataManager.S.SetCoins(DataManager.S.GetCoinsAmt() + 200);
            MainMenuManager.S.SetCoinsText(DataManager.S.GetCoinsAmt());
            UIManager.S.SetCoinsText();

        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }
}
