using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject achievementScreen;
    public bool isShowingAchievementScreen = false;
    [SerializeField] private List<GameObject> abilities = new List<GameObject>();
    private GameObject currentAbility;
    private Animator animator;

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private ParticleSystem particleSystemTwo;

    [SerializeField] private GameObject pauseUI;

    public static AchievementScreenManager instance;
    private Transform[] children;
    private RectTransform[] UIchildren;

    private chechPoint checkPointScript;

    private bool isShowing = false;
    private void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
 
    void Start()
    {
        animator = GetComponent<Animator>();
        particleSystem.Stop();
        checkPointScript = FindAnyObjectByType<chechPoint>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !pauseUI.activeSelf)
        {
            if (isShowingAchievementScreen)
            {
                HideAchievementScreen();
                
            }
        }
    }

    public void ShowAchievementScreen()
    {
        achievementScreen.SetActive(true);
        isShowingAchievementScreen = true;
        Time.timeScale = 0f;
        isShowing = true;
    }

    public void HideAchievementScreen()
    {
        achievementScreen.SetActive(false);
        particleSystem.Stop();
        isShowingAchievementScreen = false;
        Time.timeScale = 1f;
        isShowing = false;
    }

    private void ShowNewAchivement()
    {
        currentAbility.transform.Find("Unlocked").gameObject.SetActive(true);
    }
    private void RevealNewAbility()
    {
        particleSystem.transform.position = currentAbility.transform.position;
        if (isShowing == true)
        {
            particleSystem.Emit(45);
        }
    }
    private void PlayParticlesTwo()
    {
        particleSystemTwo.transform.position = currentAbility.transform.position;
    
        if (isShowing == true)
        {
            particleSystemTwo.Emit(10);
        }
    }
    public void UnlockNewAbility(string abilityName)
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        switch (abilityName)
        {
            case "AcornShield":
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));

                    if (!checkPointScript.HasAcornShild)
                    {
                        currentAbility.transform.Find("Unlocked").gameObject.SetActive(false);
                        GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                        animator.SetTrigger("NewAbility");
                    }

                    break;
            case "LeafAttack":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));

                    if (!checkPointScript.HasVineWhip)
                    {
                        currentAbility.transform.Find("Unlocked").gameObject.SetActive(false);
                        GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                        animator.SetTrigger("NewAbility");
                    }
                    break;
                   
                }
            case "WallJump":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));

                    if (!checkPointScript.HasWalljump)
                    {
                        currentAbility.transform.Find("Unlocked").gameObject.SetActive(false);
                        GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                        animator.SetTrigger("NewAbility");
                    }
                    break;
                }
            case "SpinSpin":
                {

                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));

                    if (!checkPointScript.HasSpispin)
                    {
                        currentAbility.transform.Find("Unlocked").gameObject.SetActive(false);
                        GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                        animator.SetTrigger("NewAbility");
                    }
                    break;
                }
            default:
                break;
        } 
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        pauseUI = GameObject.FindWithTag("PauseUI");

        UIchildren = pauseUI.GetComponentsInChildren<RectTransform>(true);
        for (int i = 0; i < UIchildren.Length; i++)
        {
            if (UIchildren[i].name == "PauseMenu")
            {
                pauseUI = UIchildren[i].gameObject;
            }
        }

        animator = GetComponent<Animator>();

        abilities.Clear();
        children = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].CompareTag("AbilityScreen"))
            {
                abilities.Add(children[i].gameObject);
            }
        }
    }

}
