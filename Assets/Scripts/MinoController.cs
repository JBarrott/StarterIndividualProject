using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MinoController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 2;
    public int maxKey = 1;
    
    int currentHealth;
    public int health { get { return currentHealth; }}
    int currentKey;
    public int key { get { return currentKey; }}

    public Text healthText;
    public Text GameOverText;
    public Text nameText;
    public Text timeText;
    
    public static int level = 1;
    private int win;

    public ParticleSystem hitPrefab;

    public AudioClip hitSound;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth - 1;
        currentKey = 0;

        audioSource = GetComponent<AudioSource>();

        healthText.text = "";
        GameOverText.text = "";
        nameText.text = "";

        if (level == 2)
        {
            timerIsRunning = true;
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                GameOverText.text = "You Lose! Press R to Restart.";
                nameText.text = "Game created by Jordan Barrott";
                speed = 0.0f;
            }
        }

        if (win == 1)
        {
            GameOverText.text = "You Win! Press R to Restart.";
            nameText.text = "Game created by Jordan Barrott";
            speed = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (level == 1)
            {
                SceneManager.LoadScene("MainScene");
                level = level + 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GameOverText == true)
            {
                SceneManager.LoadScene("HomeScene");
                level = 1;
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthText.text = "Protection: " + currentHealth + "/" + maxHealth;

        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            ParticleSystem hitEffect = Instantiate(hitPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
        }
        
        Debug.Log(currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            GameOverText.text = "You Lose! Press R to Restart.";
            nameText.text = "Game created by Jordan Barrott";
            speed = 0.0f;
        }
    }

    public void ChangeKey(int amount)
    {
        currentKey += amount;

        currentKey = Mathf.Clamp(currentKey + amount, 0, maxKey);
        Debug.Log(currentKey + "/" + maxKey);

        if (currentKey == 1)
        {
            win = win + 1;
        }
        
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
