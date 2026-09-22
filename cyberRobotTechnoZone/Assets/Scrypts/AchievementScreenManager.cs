using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        animator = GetComponent<Animator>();
        particleSystem.Stop();
    }

    // Update is called once per frame
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
        switch (abilityName)
        {
            case "AcornShield":
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                animator.SetTrigger("NewAbility");
                    break;
            case "LeafAttack":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                    animator.SetTrigger("NewAbility");
                    break;
                }
            case "WallJump":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                    animator.SetTrigger("NewAbility");
                    break;
                }
            case "SpinSpin":
                {
                    currentAbility = abilities.Find((gameObject => gameObject.name == abilityName));
                    animator.SetTrigger("NewAbility");
                    break;
                }
            default:
                break;
        } 
    }

  

}
