using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Token : MonoBehaviour {

	public int scoreValue { get; private set; }
    private Vector3 _initialPos;
    private Animator _animator;
    void Awake()
    {
        scoreValue = 100;
    }

    void OnEnable()
    {
        //StartCoroutine(Rotate());
        //transform.localPosition = _initialPos;
    }

    void Start()
    {
        _initialPos = transform.localPosition;
        //_animator = GetComponent<Animator>();
    }
    
    void OnDisable()
    {
        //StopCoroutine(Rotate());
        BackTOInitialPosition();
    }
   

    private void FadeOut()
    {
        transform.DOLocalMove(new Vector3(10, 20f, transform.localPosition.z), 1.5f);
    }

    private void DoScoreAnimation()
    {
        //show score animation after desolving
    }

    public void DestroyToken()
    {
        Player.S.AddScore(scoreValue);
        UIManager.S.SetScoreText();
        FadeOut();
    }

    private void BackTOInitialPosition()
    {
        transform.localPosition = _initialPos;
    }
}
