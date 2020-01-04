using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Allows private attribute but access in inspector for drag and drop, mod
    private int _lives = 3;

    // [SerializeField]
    private float _speed;
    [SerializeField]
    private float _defaultSpeed = 5f;
    [SerializeField]
    private float _boostSpeed = 8.5f;

    [SerializeField]
    private float _laserOffset = 1.05f;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nexFire = 0.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    private SpawnManager _spawnManager;
    // private float[] _hLimits = new float[2];
    // private Hashtable _limits = new Hashtable();

    void Start()
    {
        // setup initial position of the player
        transform.position = new Vector3(0, 0, 0);
        _speed = _defaultSpeed;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("the spawnManager is NULL !!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nexFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // clamp position values
        float x = transform.position.x;
        // tip : keep value between min and max value
        float y = Mathf.Clamp(transform.position.y, -5.0f, 0.0f);

        // x = MinAttribute
        if (x >= 11)
        {
            x = -11;
        }
        else if (x <= -11)
        {
            x = 11;
        }
        transform.position = new Vector3(x, y, 0);
    }

    void FireLaser()
    {
        _nexFire = Time.time + _fireRate;
        // Instantiate(_laserPrefab, transform.position + Vector3.up * _laserOffset, Quaternion.identity);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + Vector3.up * _laserOffset, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives--;
        Debug.Log("Player lives : " + _lives);
        if (_lives <= 0)
        {
            Destroy(this.gameObject);

            _spawnManager.OnPlayerDeath();
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StopCoroutine("TripleShotSpawnLife");
        StartCoroutine("TripleShotSpawnLife", 5.0f);
    }

    private IEnumerator TripleShotSpawnLife(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _speed = _boostSpeed;
        StopCoroutine("SpeedBoostSpawnLife");
        StartCoroutine("SpeedBoostSpawnLife", 5.0f);
    }

    private IEnumerator SpeedBoostSpawnLife(float duration)
    {
        yield return new WaitForSeconds(duration);
        _speed = _defaultSpeed;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        StartCoroutine(ShieldSpawnLife(5.0f));
    }

    private IEnumerator ShieldSpawnLife(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isShieldActive = false;
    }
}
