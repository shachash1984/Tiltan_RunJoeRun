using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    static public MainMenuManager S;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _shareButton;
    [SerializeField] private GameObject _mainTextPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _coin;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Text _percentText;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private Text _highDistanceText;
    [SerializeField] private Text _coinsText;
    [SerializeField] private Text _shopCoinsText;
    [SerializeField] private Toggle _SFXToggle;
    [SerializeField] private Toggle _MusicToggle;
    [SerializeField] private Toggle _TutorialToggle;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private Text _resultText;
    [SerializeField] private Button _resultCloseButton;
    [SerializeField] private string _thankYouMessage;
    [SerializeField] private string _errorMessage;


    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        _loadingBar.GetComponent<CanvasGroup>().alpha = 0;        
        _optionsPanel.GetComponent<CanvasGroup>().alpha = 0f;
        _shopPanel.GetComponent<CanvasGroup>().alpha = 0f;
        _creditsPanel.GetComponent<CanvasGroup>().alpha = 0f;
        BackToMainMenu();        
        ToggleResultPanel(false);
    }

    
    void Start()
    {
        _soundManager.PlayBGMusic();
        SetOptionsPrefs();
        SetHighScoreText(DataManager.S.GetHighScore());
        SetHighDistanceText(DataManager.S.GetHighDistance());
        SetCoinsText(DataManager.S.GetCoinsAmt());
    }

    public void StartGame()
    {
        _loadingBar.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation a = SceneManager.LoadSceneAsync(1);
        while (!a.isDone)
        {
            //Debug.Log(a.progress);
            _loadingBar.DOValue(a.progress, 0.02f);
            SetPercents(a.progress);
            yield return new WaitForEndOfFrame();
        }        
    }

    private void SetPercents(float newValue)
    {
        _percentText.text = string.Format("{0}%", newValue*100);
    }

    public void ToggleOptionsPanel()
    {
        StartCoroutine(FadeOptionsPanel(!_optionsPanel.activeSelf));
    }

    private IEnumerator FadeOptionsPanel(bool show)
    {
        CanvasGroup cg = _optionsPanel.GetComponent<CanvasGroup>();
        if (show)
        {
            _optionsPanel.SetActive(true);
            cg.DOFade(1, 0.75f);
        }
        else
        {
           cg.DOFade(0, 0.75f);
            yield return new WaitUntil(() => !DOTween.IsTweening(cg));
            _optionsPanel.SetActive(false);
        }
            
    }

    public void ToggleShopPanel()
    {
        StartCoroutine(FadeShopPanel(!_shopPanel.activeSelf));
    }

    private IEnumerator FadeShopPanel(bool show)
    {
        CanvasGroup cg = _shopPanel.GetComponent<CanvasGroup>();
        if (show)
        {
            _shopPanel.SetActive(true);
            cg.DOFade(1, 0.75f);
            ToggleCoin(true);
        }
        else
        {
            cg.DOFade(0, 0.75f);
            ToggleCoin(false);
            yield return new WaitUntil(() => !DOTween.IsTweening(cg));
            _shopPanel.SetActive(false);
        }

    }

    public void ToggleCreditsPanel()
    {
        StartCoroutine(FadeCreditsPanel(!_creditsPanel.activeSelf));
    }

    private IEnumerator FadeCreditsPanel(bool show)
    {
        CanvasGroup cg = _creditsPanel.GetComponent<CanvasGroup>();
        if (show)
        {
            _creditsPanel.SetActive(true);
            cg.DOFade(1, 0.75f);
        }
        else
        {
            cg.DOFade(0, 0.75f);
            yield return new WaitUntil(() => !DOTween.IsTweening(cg));
            _creditsPanel.SetActive(false);
        }

    }

    private IEnumerator PopResultPanel(bool show)
    {
        
        CanvasGroup cg = _resultPanel.GetComponent<CanvasGroup>();
        if (show)
        {            
            _resultPanel.SetActive(true);
            cg.DOFade(1, 0.75f);            
        }
        else
        {
            cg.DOFade(0, 0.75f);
            yield return new WaitUntil(() => !DOTween.IsTweening(cg));
            _resultPanel.SetActive(false);
        }

    }

    public void BackToMainMenu()
    {
        if (_shopPanel.activeSelf)
            StartCoroutine(FadeShopPanel(false));
        if (_optionsPanel.activeSelf)
            StartCoroutine(FadeOptionsPanel(false));
        if (_creditsPanel.activeSelf)
            StartCoroutine(FadeCreditsPanel(false));
    }

    public void ToggleCoin(bool show)
    {
        _coin.SetActive(show);
    }

    private void SetHighScoreText(int newHighScore)
    {
        _highScoreText.text = string.Format("High Score: {0}", newHighScore);
    }

    private void SetHighDistanceText(int newHighDistance)
    {
        _highDistanceText.text = string.Format("High Distance: {0}", newHighDistance);
    }

    public void SetCoinsText(int newCoinsAmt)
    {
        _coinsText.text = string.Format("Coins: {0}", newCoinsAmt);
        _shopCoinsText.text = _coinsText.text;
    }

    public void ToggleSFX()
    {
        if(_SFXToggle.isOn)       
            DataManager.S.SetSFX(true);        
        else
            DataManager.S.SetSFX(false);
    }

    public void ToggleMusic()
    {
        if (_MusicToggle.isOn)
        {
            DataManager.S.SetMusic(true);
            _soundManager.PlayBGMusic();
        }
        else
        {
            DataManager.S.SetMusic(false);
            _soundManager.StopBGMusic();
        }            
    }

    public void ToggleTutorial()
    {
        if (_TutorialToggle.isOn)
            DataManager.S.SetTutorial(true);
        else
            DataManager.S.SetTutorial(false);
    }

    public void SetOptionsPrefs()
    {
        _MusicToggle.isOn = DataManager.S.GetMusic() == 1;
        _SFXToggle.isOn = DataManager.S.GetSFX() == 1;
        _TutorialToggle.isOn = DataManager.S.GetTutorial() == 1;
    }

    public void RewardPlayer(int reward)
    {
        DataManager.S.SetCoins(DataManager.S.GetCoinsAmt() + reward);
    }

    public void ToggleResultPanel(bool show)
    {
        StartCoroutine(PopResultPanel(show));      
    }

    public bool GetPurchaseResult(bool isComplete)
    {
        return isComplete;
    }

    public void SetResultData(bool isComplete)
    {
        if (isComplete)
        {
            DataManager.S.SetCoins(DataManager.S.GetCoinsAmt() + 10000);
            SetCoinsText(DataManager.S.GetCoinsAmt());
            _resultText.text = _thankYouMessage;
            
        }
        else
        {
            SetCoinsText(DataManager.S.GetCoinsAmt());
            _resultText.text = _errorMessage;
        }
    }
}
