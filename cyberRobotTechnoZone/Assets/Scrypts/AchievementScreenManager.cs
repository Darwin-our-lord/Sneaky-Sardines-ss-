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
    [SerializeField] private GameObject pauseUI;

    public static AchievementScreenManager instance;
    private Transform[] children;
    private RectTransform[] UIchildren;

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
    }

    public void HideAchievementScreen()
    {
        achievementScreen.SetActive(false);
        isShowingAchievementScreen = false;
        Time.timeScale = 1f;
    }

    private void ShowNewAchivement()
    {
        currentAbility.transform.Find("Locked").gameObject.SetActive(false);
        currentAbility.transform.Find("Unlocked").gameObject.SetActive(true);
    }
    private void RevealNewAbility()
    {
        currentAbility.GetComponent<Image>().color = Color.white;
        particleSystem.transform.position = currentAbility.transform.position;
        particleSystem.Emit(20);
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
                    GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                    animator.SetTrigger("NewAbility");
                    break;
            case "LeafAttack":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                    GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                    animator.SetTrigger("NewAbility");
                    break;
                }
            case "WallJump":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                    GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                    animator.SetTrigger("NewAbility");
                    break;
                }
            case "SpinSpin":
                {

                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                    GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
                    animator.SetTrigger("NewAbility");
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
