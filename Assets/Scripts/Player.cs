using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

    static public Player S;
    [HideInInspector] public float axisDirection;
    public int Score { get; private set; }
    public int Coins { get; private set; }
    public float Distance { get; private set; }
    public int Lives { get; private set; }
    public bool isInvincible = false;
    public SoundManager soundManager;
    [SerializeField] private float _speed;
    [SerializeField] private float _animSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private int _shields;
    [SerializeField] private bool _isAlive;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private bool _isInAir;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isTouchingWall;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private GameObject _halo;
    float prevZPos;
    float currenZPos;    
    

    void Awake()
    {
        if (S == null)
            S = this;
        else
            Destroy(gameObject);
        _animator.SetBool("isAlive", true);
        UpdateLives(false, 1);
        soundManager = FindObjectOfType<SoundManager>();
        _halo.SetActive(false);
    }    

    void FixedUpdate()
    {
        //Debug.Log("isAlive: " + _isAlive + " gameOver: " + GameManager.S.gameOver + " isPaused: " + GameManager.S.isPaused);
        if (_isAlive && !GameManager.S.gameOver && !GameManager.S.isPaused)
        {

            MoveSideways(Input.GetAxisRaw("Horizontal"));
            if (isInvincible)
                LimitPosition();
            MoveForward();            
        }      
        
    }

    void Update()
    {
        if (_isAlive && !GameManager.S.gameOver && !GameManager.S.isPaused)
        {
            currenZPos = transform.position.z;
            AnimatePlayer();
            if (Input.GetButtonDown("Jump") && !_isInAir && !_isJumping)
                StartCoroutine(Jump());            
            prevZPos = currenZPos;
        }        
    }

    void OnTriggerEnter(Collider col)
    {
        Token t = col.gameObject.GetComponent<Token>();
        if (t != null)
        {            
            AddScore(t.scoreValue);
            AddCoins();
            t.DestroyToken();
            
            soundManager.PlayCoinCollectSound();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("collided with: " + col.gameObject.name);
        if (col.transform.parent.GetComponent<Trap>() && !isInvincible)
        {            
            if (!_animator.GetBool("collided"))
            {
                _animator.SetBool("collided", true);
                StartCoroutine(OnPlayerHit());
                GameManager.S.Pause(true);                
                          
            }
                
            //_isAlive = false;
            //Debug.Log("<color=yellow>player died</color>");
        }
        //if touching the ground - then isInAir = true
        else if (col.gameObject.GetComponent<Ground>())
        {
            //Debug.Log("Collided with ground, time: " + Time.time);
            StartCoroutine(PlayParticleEffect());
            _isInAir = false;
        }            
        
            
    }

    void OnCollisionExit(Collision col)
    {
        //if not touching the ground anymore - then isInAir = true
        if (_isJumping && col.gameObject.GetComponent<Ground>())
        {
            //Debug.Log("Left the ground, time: " + Time.time);
            _isInAir = true;
        }
            
    }

    /// <summary>
    /// move the player forward
    /// </summary>
    void MoveForward()
    {        
        transform.Translate(transform.forward * Time.deltaTime * _speed);
        AddDistance();
        UIManager.S.SetDistanceText();
    }

    /// <summary>
    /// moves the player from side to side
    /// </summary>
    /// <param name="direction"> positive number will move the player right, while negative number will move the player left</param>
    public void MoveSideways(float direction)
    {
        transform.Translate(new Vector3(direction, 0, 0)* Time.deltaTime * _turnSpeed);
    }

    public void StartJump()
    {
        if (!_isInAir && !_isJumping)
            StartCoroutine(Jump());
    }

    /// <summary>
    /// if the player is not in air he can jump
    /// </summary>
    IEnumerator Jump()
    {
        if (_isAlive)
        {
            _isJumping = true;
            soundManager.PlayJumpSound();
            Vector3 jumpDistance = (Vector3.up) * _jumpForce;
            StartCoroutine(PlayParticleEffect());
            while (_isJumping)
            {
                transform.position += (jumpDistance * Time.deltaTime);
                if (transform.position.y >= _maxJumpHeight)
                {
                    _isJumping = false;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        else
            yield return null;
        
    }

    public void AddScore(int scoreToAdd)
    {
        this.Score += scoreToAdd;
        if (this.Score > DataManager.S.GetHighScore())
            DataManager.S.SetHighScore(Score);
    }

    private void AddDistance()
    {
        Distance = transform.position.z + 48f;
        if (this.Distance > DataManager.S.GetHighDistance())
            DataManager.S.SetHighDistance((int)Distance);
    }

    public void AddCoins()
    {
        Coins = DataManager.S.GetCoinsAmt();
        Coins += 1;
        DataManager.S.SetCoins(Coins);
        UIManager.S.SetCoinsText();
    }

    public void UpdateLives(bool lostLife = true, int livesToAdd = 0)
    {
        if(Lives > 0)
        {
            if (lostLife)            
                Lives -= 1;            
        }        
        else
        {
            if (!lostLife)
            {
                Lives += livesToAdd;
                SetIsAlive(true);
            }                
        }

        if (Lives <= 0)
        {
            SetIsAlive(false);
            GameManager.S.SetGameOver(true);
        }
            
    }

    void DestroyPlayer()
    {
        if(_shields > 0)
        {
            _shields--;
            //player shield loss animation
        }
        Destroy(gameObject);
    }

    void AnimatePlayer()
    {
        if (!GameManager.S.gameOver)
        {
            if (!_animator.GetBool("collided") && _isAlive)
            {
                _animSpeed = _speed + 50;
                //Debug.Log("ZPos diff: " + (currenZPos - prevZPos));// + transform.rotation.eulerAngles);
                
                _animator.SetFloat("speed", _animSpeed*Time.deltaTime);
                _animator.SetFloat("rotation", axisDirection);
                _animator.SetFloat("verticalPosition", transform.position.y);
                _animator.SetBool("isJumping", _isJumping);
            }
            else
            {
                _animator.SetBool("isAlive", false);
            }
                
        }

        
    }    

    public void SetIsAlive(bool alive)
    {
        _isAlive = alive;
        _animator.SetBool("isAlive", _isAlive);
    }

    IEnumerator OnPlayerHit()
    {
        UIManager.S.RedFlash();
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("collided", false);        
        //StartCoroutine(Camera.main.GetComponent<CameraFollow>().ToggleBrokenScreen(true));
        UpdateLives();
        yield return new WaitForSeconds(1f);
        _rigidBody.isKinematic = true;
        _collider.enabled = false;
        transform.DOMoveZ(transform.position.z + 5f, 1f);
        
        if (!GameManager.S.gameOver)
            UIManager.S.ToggleHitPausePanel(true);
        yield return new WaitUntil(() => !DOTween.IsTweening(transform));
        _collider.enabled = true;
        _rigidBody.isKinematic = false;
        _animator.Play("Idle");
        _animator.SetBool("isWaitingForInput", true);
        
        
        
        while (GameManager.S.isPaused)
        {
            yield return new WaitForEndOfFrame();
        }
        _animator.SetBool("isWaitingForInput", false);        
    }

    IEnumerator PlayParticleEffect()
    {       
        _particles.Play();
        //Debug.Log("PLaying particles");
        yield return new WaitForSeconds(1f);
        _particles.Stop();
        _particles.DORewind();
    }

    public void IncreaseSpeed()
    {        
        _speed += 1f;
    }

    public void ResetSpeed(int newSpeed = 10)
    {
        _speed = newSpeed;
    }

    public void Init()
    {
        ResetSpeed();
    }

    public IEnumerator SwitchToInvincible()
    {
        
        isInvincible = true;
        _collider.isTrigger = true;
        _rigidBody.isKinematic = true;
        _halo.SetActive(true);
        yield return new WaitForSeconds(3f);        
        
        
        _collider.isTrigger = false;
        _rigidBody.isKinematic = false;
        _halo.SetActive(false);
        isInvincible = false;

    }

    public void LimitPosition()
    {
        if (transform.position.x > 6)
            transform.position = new Vector3(6, transform.position.y, transform.position.z);
        else if (transform.position.x < -6)
            transform.position = new Vector3(-6, transform.position.y, transform.position.z);
    }
}
