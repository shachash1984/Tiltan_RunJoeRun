using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private Transform _target;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private CameraFilterPack_Broken_Screen _brokenEffect;
    private Vector3 _distenceFromTarget;
    private Vector3 _wantedPos;
    
    void Awake()
    {
        _distenceFromTarget = new Vector3(0f, 12f, -15f);
        StartCoroutine(Camera.main.GetComponent<CameraFollow>().ToggleBrokenScreen(false, true));
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        _wantedPos = _target.position + _distenceFromTarget;
        transform.position = Vector3.Lerp(transform.position, _wantedPos, Time.deltaTime*_cameraSpeed);

    }

    public IEnumerator ToggleBrokenScreen(bool on, bool immediate = false)
    {
        if (on)
        {
            _brokenEffect.Fade = 0;
            _brokenEffect.enabled = true;            
            while (_brokenEffect.Fade < 1)
            {
                _brokenEffect.Fade += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }            
        }
        else
        {
            if (immediate)
            {
                _brokenEffect.enabled = false;
            }
                
            else
            {
                while (_brokenEffect.Fade > 0)
                {
                    _brokenEffect.Fade -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                _brokenEffect.enabled = false;
            }
        }
            
    }
}
