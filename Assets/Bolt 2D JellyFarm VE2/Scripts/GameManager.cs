using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int jelly_point;
    public int gold;

    public GameObject JellyPrefab;

    public TextMeshProUGUI jelly_text;
    public TextMeshProUGUI gold_text;

    public RuntimeAnimatorController[] acs;
    public int[] Sell_List;
    public Sprite[] Sprite_List;
    public string[] Name_List;
    public int[] JellyPoint_List;
    public int[] JellyUnlock_List;
    public int[] SkillUnlock_List;

    public TextMeshProUGUI Jelly_ID_text;
    public Image Jelly_Sprite;
    public TextMeshProUGUI Jelly_Name;
    public TextMeshProUGUI Jelly_Price;

    int jelly_idx = 0;
    int multiplierup = 0;
    int limitup = 0;

    public bool isSell;

    public Image jelly_panel;
    Animator jelly_Animator;

    public Image upgrade_panel;
    Animator upgrade_Animator;

    public Animator addGold;
    public Animator addJellyPoint;
    public Animator minusGold;
    public Animator minusJellyPoint;

    public GameObject Lock;
    public Image Lock_group_jelly_Image;
    public TextMeshProUGUI Lock_group_jelly_text;

    bool[] jelly_unlock_list;

    public int jellyNum;
    public int jellyLimit;
    public TextMeshProUGUI jellyNumText;

    public Animator optionAnimator;
    public GameObject optionPanel;

    public List<GameObject> Jellies = new();

    public string[] newJellyData = new string[5];

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        isSell = false;
        jelly_Animator = jelly_panel.GetComponent<Animator>();
        upgrade_Animator = upgrade_panel.GetComponent<Animator>();

        gold = PlayerPrefs.GetInt("Gold");
        //gold = 1000;
        jelly_point = PlayerPrefs.GetInt("JellyPoint");

        // PlayerPrefs에 저장되어 있는 젤리 로드
        jellyNum = PlayerPrefs.GetInt("JellyNum");
        for (int i = 0; i < jellyNum; i++)
        {
            Debug.Log("Jelly loaded");
            newJellyData = PlayerPrefs.GetString($"{i}J").Split(",");
            GameObject tempJelly = Instantiate(JellyPrefab, new Vector2(0, 0), Quaternion.identity);
            Jellies.Add(tempJelly);
            Jelly jelly = Jellies[Jellies.Count - 1].GetComponent<Jelly>();
            jelly.jellyStat.ID = int.Parse(newJellyData[0]);
            jelly.jellyStat.idx = int.Parse(newJellyData[1]);
            jelly.jellyStat.level = int.Parse(newJellyData[2]);
            jelly.jellyStat.sprite = int.Parse(newJellyData[3]);
            jelly.jellyStat.exp = float.Parse(newJellyData[4]);
            Jellies[Jellies.Count - 1].name = Name_List[jelly.jellyStat.idx];
            jelly.GetComponent<SpriteRenderer>().sprite = Sprite_List[jelly.jellyStat.sprite];
        }

        if (Jellies.Count == 0)
        {
            Debug.Log("처음시작");
            PlayerPrefs.SetInt("Limit", 20);
            PlayerPrefs.SetInt("LimitUp", 0);
            PlayerPrefs.SetInt("Multiplier", 10);
            PlayerPrefs.SetInt("MultiplierUp", 0);
            JellyCreate();
        }
        else
        {
            jellyLimit = PlayerPrefs.GetInt("Limit");
            Jelly.multiplier = PlayerPrefs.GetInt("Multiplier");
            limitup = PlayerPrefs.GetInt("LimitUp");
            multiplierup = PlayerPrefs.GetInt("MultiplierUp");
        }
        Lock_group_jelly_text.text = $"{JellyPoint_List[0]}";
        jelly_unlock_list = new bool[Sprite_List.Length];

        for(int i = 0; i < jelly_unlock_list.Length; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                jelly_unlock_list[i] = true;
            }
        }
        Lock.SetActive(!jelly_unlock_list[jelly_idx]);
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.BGM_Play(BGMType.InGame);
    }

    private void FixedUpdate()
    {
        //jelly_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelly_text.text), PlayerPrefs.GetInt("JellyPoint", jelly_point), 1f));
        jelly_text.text = string.Format("{0:n0}", Mathf.Lerp(jelly_point, PlayerPrefs.GetInt("JellyPoint", jelly_point), 0.5f));

        gold_text.text = string.Format("{0:n0}", Mathf.Lerp(gold, PlayerPrefs.GetInt("Gold", gold), 0.5f));

        jellyNumText.text = $"{Jellies.Count} / {jellyLimit}";
    }

    //private void LateUpdate()
    //{
    //    // A 지점에서 B 지점 , 거리 비율 T

    //    jelly_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelly_text.text), PlayerPrefs.GetInt("JellyPoint", jelly_point) , 1f));

    //    gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), PlayerPrefs.GetInt("Gold", gold), 1f));

    //}


    public void CheckSell()
    {
        isSell = isSell == false;
    }

    public void GetGold(int id, int level)
    {
        int getGold = Sell_List[id] * level;
        gold += getGold;
        AddGold(getGold);
        PlayerPrefs.SetInt("Gold", gold);
        SoundManager.instance.SEPlay(SEType.Sell);
    }
    public void ChangeAnimatorController(Animator animator, int level)
    {
        animator.runtimeAnimatorController = acs[level - 1];
    }


    public bool isJellyPanelClicked;
    public void OnClickPanelEnter()
    {
        SetPage();
        SoundManager.instance.SEPlay(SEType.Button);
        if (isJellyPanelClicked)
        {
            jelly_Animator.SetTrigger("Hide");
            jelly_panel.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            jelly_Animator.SetTrigger("Show");
            jelly_panel.gameObject.GetComponent<Button>().interactable = true;
        }
            
        isJellyPanelClicked = !isJellyPanelClicked;
    }

    public bool isOptionPanelClicked;
    public void Option()
    {
        SoundManager.instance.SEPlay(SEType.Button);
        if (isOptionPanelClicked)
        {
            optionAnimator.SetTrigger("Hide");
        }
        else
        {
            optionAnimator.SetTrigger("Show");
        }

        isOptionPanelClicked = !isOptionPanelClicked;
    }

    public bool isUpgradePanelClicked;
    public void OnUpgradeClickPanelEnter()
    {
        SetUpgradePage();
        SoundManager.instance.SEPlay(SEType.Button);
        if (isUpgradePanelClicked)
        {
            upgrade_Animator.SetTrigger("Hide");
            upgrade_panel.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            upgrade_Animator.SetTrigger("Show");
            upgrade_panel.gameObject.GetComponent<Button>().interactable = true;
        }
        isUpgradePanelClicked = !isUpgradePanelClicked;
    }

    public void PageUp()
    {
        SoundManager.instance.SEPlay(SEType.Button);
        if (jelly_idx + 1 < Sprite_List.Length)
        {
            jelly_idx++;
            SetPage();
        }
    }

    public void PageDown()
    {
        SoundManager.instance.SEPlay(SEType.Button);
        if (jelly_idx - 1 >= 0)
        {
            jelly_idx--;
            SetPage();
        }
    }

    public void SetPage()
    {
        Lock.SetActive(!jelly_unlock_list[jelly_idx]);
        Jelly_ID_text.text = $"NO. {jelly_idx + 1}";

        if (Lock.activeSelf)
        {
            Lock_group_jelly_Image.sprite = Sprite_List[jelly_idx];
            Lock_group_jelly_text.text = string.Format("{0:N0}", JellyUnlock_List[jelly_idx]);
        }
        else
        {
            Jelly_Sprite.sprite = Sprite_List[jelly_idx];
            Jelly_Name.text = Name_List[jelly_idx];
            Jelly_Price.text = string.Format("{0:N0}", JellyPoint_List[jelly_idx]);
        }
    }
    public void SetUpgradePage()
    {
        limitlv.text = $"Lv. {limitup + 1}";
        multlv.text = $"Lv. {multiplierup + 1}";
        limitprice.text = string.Format("{0:N0}", SkillUnlock_List[limitup]);
        multprice.text = string.Format("{0:N0}", SkillUnlock_List[multiplierup]);

    }
    public void UnLock()
    {
        if (gold < JellyUnlock_List[jelly_idx])
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }
        jelly_unlock_list[jelly_idx] = true;
        SetPage();
        PlayerPrefs.SetInt(jelly_idx.ToString(), 1);
        int minusGold = JellyUnlock_List[jelly_idx];
        gold -= minusGold;
        MinusGold(minusGold);
        SoundManager.instance.SEPlay(SEType.Unlock);
        PlayerPrefs.SetInt("Gold", gold);
    }

    
    public void Buy()
    {
        if(jelly_point < JellyPoint_List[jelly_idx]) 
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }

        if (Jellies.Count < jellyLimit)
        {
            SoundManager.instance.SEPlay(SEType.Sell);
            int minusJellyPoint = JellyPoint_List[jelly_idx];
            jelly_point -= minusJellyPoint;
            MinusJellyPoint(minusJellyPoint);
            PlayerPrefs.SetInt("JellyPoint", jelly_point);
            JellyCreate();
        }
        else
            Debug.Log("젤리가 최대치입니다.");
    }

    public TextMeshProUGUI limitlv;
    public TextMeshProUGUI limitprice;
    //public TextMeshProUGUI limitex;
    public void Limitup()
    {
        if (gold < SkillUnlock_List[limitup])
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }
        else
        {
            if (limitup == 3)
            {
                limitlv.text = $"Lv. {limitup}";
                limitprice.text = "";
                Debug.Log("업그레이드 최대치입니다.");
            }
            else
            {
                SoundManager.instance.SEPlay(SEType.Sell);
                int minusGold = SkillUnlock_List[multiplierup];
                gold -= minusGold;
                MinusGold(minusGold);
                PlayerPrefs.SetInt("Gold", gold);
                limitup++;
                jellyLimit += 3;
                PlayerPrefs.SetInt("LimitUp", limitup);
                PlayerPrefs.SetInt("Limit", jellyLimit);
                limitlv.text = $"Lv. {limitup + 1}";
                limitprice.text = string.Format("{0:N0}", SkillUnlock_List[limitup]);
                //limitex.text = $"Jelly Limitation+";
            }
        }
        
    }

    public TextMeshProUGUI multlv;
    public TextMeshProUGUI multprice;
    //public TextMeshProUGUI multex;
    public void Multiplierup()
    {
        if (gold < SkillUnlock_List[multiplierup])
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }
        else
        {
            if (multiplierup == 3)
            {
                multlv.text = $"Lv. {multiplierup}";
                multprice.text = "";
                Debug.Log("업그레이드 최대치입니다.");
            }
            else
            {
                SoundManager.instance.SEPlay(SEType.Sell);
                int minusGold = SkillUnlock_List[multiplierup];
                gold -= minusGold;
                MinusGold(minusGold);
                PlayerPrefs.SetInt("Gold", gold);
                multiplierup++;
                Jelly.multiplier += 5;
                PlayerPrefs.SetInt("MultiplierUp", multiplierup);
                PlayerPrefs.SetInt("Multiplier", Jelly.multiplier);
                multlv.text = $"Lv. {multiplierup + 1}";
                multprice.text = string.Format("{0:N0}", SkillUnlock_List[multiplierup]);
                //multex.text = $"Jelly Multiplier+";
            }
        }
        
    }

    public void JellyCreate()
    {
        GameObject tempJelly = Instantiate(JellyPrefab, new Vector2(0, 0), Quaternion.identity);
        Jellies.Add(tempJelly);
        Jelly jelly = Jellies[Jellies.Count-1].GetComponent<Jelly>();
        Jellies[Jellies.Count-1].name = Name_List[jelly_idx];
        jelly.jellyStat.idx = jelly_idx;
        jelly.GetComponent<SpriteRenderer>().sprite = Sprite_List[jelly_idx];
        jellyNum++;
        PlayerPrefs.SetInt("JellyNum", jellyNum);
    }

    public void AddGold(int gold)
    {
        addGold.gameObject.GetComponent<TextMeshProUGUI>().text = gold.ToString();
        addGold.SetTrigger("Get");
    }

    public void AddJellyPoint(int jellypoint)
    {
        addJellyPoint.gameObject.GetComponent<TextMeshProUGUI>().text = jellypoint.ToString();
        addJellyPoint.SetTrigger("Get");
    }

    public void MinusGold(int gold)
    {
        minusGold.gameObject.GetComponent<TextMeshProUGUI>().text = gold.ToString();
        minusGold.SetTrigger("Minus");
    }

    public void MinusJellyPoint(int jellypoint)
    {
        minusJellyPoint.gameObject.GetComponent<TextMeshProUGUI>().text = jellypoint.ToString();
        minusJellyPoint.SetTrigger("Minus");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
        PlayerPrefs.DeleteAll();
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
