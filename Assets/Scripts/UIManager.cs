using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    static public UIManager S;
    public Text countdownText;
    [SerializeField] private GameObject __countdownPanel;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _distanceText;
    [SerializeField] private Text _coinsText;
    [SerializeField] private RawImage _redImage;
    [SerializeField] private GameObject _hitPausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private Text _resultText;    
    [SerializeField] private Button _resultCloseButton;
    [SerializeField] private GameObject _continuePanel;
    [SerializeField] private string _thankYouMessage;
    [SerializeField] private string _errorMessage;
    [SerializeField] private Text _tutorialMessage;
    [SerializeField] private Toggle _tutorialToggle;
    [SerializeField] private string _welcomeMessage;
    [SerializeField] private string _useArrowsMessage;
    [SerializeField] private string _useJumpMessage;
    [SerializeField] [TextArea(2,2)] private string _becarefulMessage;

    private int currentDistance;
    private int prevDistance;
    public bool isTutorialSequencePlaying;
    

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        ToggleContinuePanel(false);
        ToggleResultPanel(false);
        __countdownPanel.SetActive(false);
        ClearTutorialMessage();
        
    }    
   
    void Start()
    {
        SetCoinsText();
        SetScoreText();
        GetTutorialStatus();
        SetDistanceText(true);
    }

    public void SetScoreText()
    {
        _scoreText.text = string.Format("Score: {0}", Player.S.Score);
    }

    public void SetDistanceText(bool push = false)
    {
        if(push)
            _distanceText.text = string.Format("Distance: {0}", 0);
        currentDistance = (int)Player.S.Distance;
        if (prevDistance != currentDistance)
        {
            _distanceText.text = string.Format("Distance: {0}", currentDistance);
            prevDistance = currentDistance;
        }

    }

    public void SetCoinsText()
    {
        _coinsText.text = string.Format("Coins: {0}", DataManager.S.GetCoinsAmt());
    }

    public void RedFlash()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetEase<Sequence>(Ease.InBounce);
        seq.Append(_redImage.DOFade(0.2f, 0.035f));
        seq.Append(_redImage.DOFade(0f, 0.035f));
    }

    public void Continue()
    {
        ToggleHitPausePanel(false);
        GameManager.S.Pause(false);
    }

    public void ToggleHitPausePanel(bool show)
    {
        _hitPausePanel.SetActive(show);
    }

    public void ToggleGameOverPanel(bool show)
    {
        gameOverPanel.SetActive(show);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
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
            SetCoinsText();
            _resultText.text = _thankYouMessage;

        }
        else
        {
            SetCoinsText();
            _resultText.text = _errorMessage;
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

    public void ToggleContinuePanel(bool show)
    {
        StartCoroutine(PopContinuePanel(show));
    }

    private IEnumerator PopContinuePanel(bool show)
    {
        CanvasGroup cg = _continuePanel.GetComponent<CanvasGroup>();
        if (show)
        {
            _continuePanel.SetActive(true);
            cg.DOFade(1, 0.75f);
        }
        else
        {
            cg.DOFade(0, 0.75f);
            yield return new WaitUntil(() => !DOTween.IsTweening(cg));
            _continuePanel.SetActive(false);
        }
    }

   public IEnumerator Countdown()
    {
        Player.S.SetIsAlive(false);
        ToggleGameOverPanel(false);
        Sequence seq = DOTween.Sequence();
        __countdownPanel.SetActive(true);
        seq.Append(countdownText.DOText("3", 0.05f));
        seq.Append(countdownText.DOFade(1, 0.25f));       
        seq.Play<Sequence>();
        yield return new WaitUntil(() => !DOTween.IsTweening(countdownText));
        yield return new WaitForSeconds(1f);
        countdownText.DOFade(0, 0.05f);
        seq = DOTween.Sequence();
        seq.Append(countdownText.DOText("2", 0.05f));
        seq.Append(countdownText.DOFade(1, 0.25f));
        seq.Play<Sequence>();
        yield return new WaitUntil(() => !DOTween.IsTweening(countdownText));
        yield return new WaitForSeconds(1f);
        countdownText.DOFade(0, 0.05f);
        seq = DOTween.Sequence();
        seq.Append(countdownText.DOText("1", 0.05f));
        seq.Append(countdownText.DOFade(1, 0.25f));
        seq.Play<Sequence>();
        yield return new WaitUntil(() => !DOTween.IsTweening(countdownText));
        yield return new WaitForSeconds(1f);
        Player.S.SetIsAlive(true);
        countdownText.DOFade(0, 0.05f);
        __countdownPanel.SetActive(false);
        if (GameManager.S.playTutorial && !GameManager.S.isContinue)
        {
            Debug.Log("UIManager.PlayTutorialSequence()");
            StartCoroutine(PlayTutorialSequence());
        }
        GameManager.S.Pause(false);
        Player.S.UpdateLives(false, 1);
        Player.S.SetIsAlive(true);
        GameManager.S.SetGameOver(false);
        StartCoroutine(Player.S.SwitchToInvincible());
    }

    public IEnumerator PlayTutorialSequence()
    {
        isTutorialSequencePlaying = true;
        yield return new WaitForSeconds(2f);
        _tutorialMessage.DOText(_welcomeMessage, 1f);
        yield return new WaitForSeconds(3f);
        _tutorialMessage.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ClearTutorialMessage();
        yield return new WaitForSeconds(0.5f);
        _tutorialMessage.DOFade(1f, 0.05f);
        _tutorialMessage.DOText(_useArrowsMessage, 3f);
        yield return new WaitForSeconds(5f);
        _tutorialMessage.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ClearTutorialMessage();
        yield return new WaitForSeconds(0.5f);
        _tutorialMessage.DOFade(1f, 0.05f);
        _tutorialMessage.DOText(_useJumpMessage, 3f);
        yield return new WaitForSeconds(5);
        _tutorialMessage.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ClearTutorialMessage();
        yield return new WaitForSeconds(0.5f);
        _tutorialMessage.DOFade(1f, 0.05f);
        _tutorialMessage.DOText(_becarefulMessage, 1f);
        yield return new WaitForSeconds(4);
        _tutorialMessage.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ClearTutorialMessage();
        isTutorialSequencePlaying = false;
    }

    public void ClearTutorialMessage()
    {
        _tutorialMessage.text = "";
    }

    public void ToggleTutorial()
    {
        if (_tutorialToggle.isOn)
            DataManager.S.SetTutorial(true);
        else
            DataManager.S.SetTutorial(false);
    }

    public void GetTutorialStatus()
    {
        _tutorialToggle.isOn = DataManager.S.GetTutorial() == 1;
    }
}
