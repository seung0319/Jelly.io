using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int jelly_point;
    public int gold;

    public TextMeshProUGUI jelly_text;
    public TextMeshProUGUI gold_text;

    public AnimatorController[] acs;
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

    public GameObject Lock;
    public Image Lock_group_jelly_Image;
    public TextMeshProUGUI Lock_group_jelly_text;

    bool[] jelly_unlock_list;

    public int jellyNum = 0;
    public int jellyLimit = 5;
    string[] jellyData;
    string[] newJellyData;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if(jellyNum == 0)
        {
            GameObject jelly_object = Instantiate(JellyPrefab, new Vector2(0, 0), Quaternion.identity);
            Jelly jelly = jelly_object.GetComponent<Jelly>();

            jelly_object.name = Name_List[jellyNum];
            jelly.jellyStat.ID = jellyNum;
            jelly.GetComponent<SpriteRenderer>().sprite = Sprite_List[jellyNum];
            jellyNum++;
        }
        isSell = false;
        jelly_Animator = jelly_panel.GetComponent<Animator>();
        upgrade_Animator = upgrade_panel.GetComponent<Animator>();
        gold = PlayerPrefs.GetInt("Gold");
        //gold = 1000;
        jelly_point = PlayerPrefs.GetInt("JellyPoint");
        PlayerPrefs.SetInt("0", 1);
        
        //jellyNum = PlayerPrefs.GetInt("JellyNum");
        //for(int i = 0; i < jellyNum; i++)
        //{
        //    newJellyData = PlayerPrefs.GetString($"{jellyNum}j").Split(",");
        //    GameObject jelly_object = Instantiate(JellyPrefab, new Vector2(i, 0), Quaternion.identity);
        //    Jelly jelly = jelly_object.GetComponent<Jelly>();
        //    jelly.jellyStat.ID = int.Parse(newJellyData[0]);
        //    jelly.jellyStat.level = int.Parse(newJellyData[1]);
        //    jelly.jellyStat.sprite = int.Parse(newJellyData[2]);
        //    jelly.jellyStat.exp = float.Parse(newJellyData[3]);

        //    jelly_object.name = Name_List[jelly.jellyStat.sprite];
        //    jelly.GetComponent<SpriteRenderer>().sprite = Sprite_List[jelly.jellyStat.sprite];
        //}


        //gold_text.text = $"{Sell_List[0]}";
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
        
    }

    // Update is called once per frame
    void Update()
    {
        jelly_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelly_text.text), PlayerPrefs.GetInt("JellyPoint", jelly_point), 1f));

        gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), PlayerPrefs.GetInt("Gold", gold), 1f));
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
        gold += Sell_List[id] * level;
        
        PlayerPrefs.SetInt("Gold", gold);
    }
    public void ChangeAnimatorController(Animator animator, int level)
    {
        animator.runtimeAnimatorController = acs[level - 1];
    }


    public bool isJellyPanelClicked;
    public void OnClickPanelEnter()
    {
        SetPage();
        if (isJellyPanelClicked)
            jelly_Animator.SetTrigger("Hide");
        else
            jelly_Animator.SetTrigger("Show");
        isJellyPanelClicked = !isJellyPanelClicked;
    }

    public bool isUpgradePanelClicked;
    public void OnUpgradeClickPanelEnter()
    {
        SetUpgradePage();
        if (isUpgradePanelClicked)
            upgrade_Animator.SetTrigger("Hide");
        else
            upgrade_Animator.SetTrigger("Show");
        isUpgradePanelClicked = !isUpgradePanelClicked;
    }

    public void PageUp()
    {
        if(jelly_idx + 1 < Sprite_List.Length)
        {
            jelly_idx++;
            SetPage();
        }
    }

    public void PageDown()
    {
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
        limitprice.text = $"Lv. {SkillUnlock_List[limitup]}";
        multprice.text = $"Lv. {SkillUnlock_List[multiplierup]}";

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
        gold -= JellyUnlock_List[jelly_idx];
        PlayerPrefs.SetInt("Gold", gold);
    }

    public GameObject JellyPrefab;
    public void Buy()
    {
        if(jelly_point < JellyPoint_List[jelly_idx]) 
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }

        if (jellyNum < jellyLimit)
        {
            jelly_point -= JellyPoint_List[jelly_idx];
            PlayerPrefs.SetInt("JellyPoint", jelly_point);
            GameObject jelly_object = Instantiate(JellyPrefab, new Vector2(0, 0), Quaternion.identity);
            Jelly jelly = jelly_object.GetComponent<Jelly>();

            jelly_object.name = Name_List[jelly_idx];
            jelly.jellyStat.ID = jelly_idx;
            jelly.GetComponent<SpriteRenderer>().sprite = Sprite_List[jelly_idx];

            jellyNum++;
            //jelly.jellyStat.ID = jellyNum;
            //jelly.GetComponent<SpriteRenderer>().sprite = Sprite_List[jelly_idx];
            //jellyData[jellyNum] = $"{jelly.jellyStat.ID},{jelly.jellyStat.level},{jelly.jellyStat.sprite},{jelly.jellyStat.exp}";
            //PlayerPrefs.SetString($"{jellyNum}j", $"{jellyData[jellyNum]}");
            //jellyNum++;
            //PlayerPrefs.SetInt("JellyNum", jellyNum);
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
        if(limitup == 2)
        {
            Debug.Log("업그레이드 최대치입니다.");
            return;
        }
        gold -= SkillUnlock_List[limitup];
        limitup++;
        jellyLimit++;
        limitlv.text = $"Lv. {limitup + 1}";
        limitprice.text = $"Lv. {SkillUnlock_List[limitup]}";
        //limitex.text = $"Jelly Limitation+";
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
        if (multiplierup == 2)
        {
            Debug.Log("업그레이드 최대치입니다.");
            return;
        }
        gold -= SkillUnlock_List[multiplierup+1];
        multiplierup++;
        Jelly.multiplier += 5;
        multlv.text = $"Lv. {multiplierup + 1}";
        multprice.text = $"Lv. {SkillUnlock_List[multiplierup]}";
        //multex.text = $"Jelly Multiplier+";
    }
}
