using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JellyStat
{
    public int ID;
    public int idx;
    public int level;
    public int sprite;
    public float exp;
    public float max_exp;
    public float req_exp;
}
public class Jelly : MonoBehaviour
{
    public JellyStat jellyStat;

    public int delay;
    public int time;
    public static int multiplier = 10;


    float speed_X;
    float speed_Y;
    bool isWalk;
    bool isDelay;

    GameManager gamemanager;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public string jellydata;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        isDelay = false;
        isWalk = false;
        
        if(jellyStat.level >= 1)
            gamemanager.ChangeAnimatorController(animator, jellyStat.level);

        StartCoroutine("JellyAction");
       
    }

    // Update is called once per frame
    void Update()
    {
        if(jellyStat.exp < jellyStat.max_exp)
        {
            jellyStat.exp += Time.deltaTime;
        }
        if(jellyStat.exp > jellyStat.req_exp * jellyStat.level && jellyStat.level < 3)
        {
            //jellyStat.max_exp = jellyStat.req_exp * jellyStat.level;
            gamemanager.ChangeAnimatorController(animator, ++jellyStat.level);
            SoundManager.instance.SEPlay(SEType.Grow);
        }

        if (isWalk)
        {
            Move();
        }

        jellyStat.ID = gamemanager.Jellies.IndexOf(gameObject);
        jellydata = $"{jellyStat.ID},{jellyStat.idx},{jellyStat.level},{jellyStat.sprite},{jellyStat.exp}";
        PlayerPrefs.SetString($"{jellyStat.ID}J", $"{jellydata}");
    }

    IEnumerator JellyAction()
    {
        while (true)
        {
            speed_X = UnityEngine.Random.Range(-0.8f, 0.8f) * Time.deltaTime;
            speed_Y = UnityEngine.Random.Range(-0.8f, 0.8f) * Time.deltaTime;
            //speed_X = 0.8f * Time.deltaTime;
            //speed_Y = 0.8f * Time.deltaTime;
            isDelay = true;
            yield return new WaitForSeconds(delay);
            isWalk = true;
            // 애니메이터 파라미터 바꾸는 코드
            animator.SetBool("isWalk", true);
            int getJellyPoint = (jellyStat.idx + 1) * jellyStat.level * 10;
            gamemanager.jelly_point += getJellyPoint;
            gamemanager.AddJellyPoint(getJellyPoint);
            PlayerPrefs.SetInt("JellyPoint", gamemanager.jelly_point);
            time = UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(time);
            isWalk = false;
            animator.SetBool("isWalk", false);
            isDelay = false;
        }
    }

    private void Move()
    {
        if(speed_X != 0)
        {
            spriteRenderer.flipX = speed_X < 0;
        }
        
        transform.Translate(speed_X, speed_Y, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ObjectCollider")
        {
            speed_X *= -1;
        }

        if (collision.gameObject.tag == "ObjectColliderV")
        {
            speed_Y *= -1;
        }
    }

    private void OnMouseDown()
    {
        isWalk = false;
        animator.SetBool("isWalk", false);
        animator.SetTrigger("doTouch");
        if (jellyStat.exp < jellyStat.max_exp)
        {
            ++jellyStat.exp;
        }
        SoundManager.instance.SEPlay(SEType.Touch);
        int getJellyPoint = (jellyStat.idx + 1) * jellyStat.level * multiplier;
        gamemanager.jelly_point += getJellyPoint;
        gamemanager.AddJellyPoint(getJellyPoint);
        PlayerPrefs.SetInt("JellyPoint", gamemanager.jelly_point);
    }
    float pick_time;

    private void OnMouseDrag()
    {
        pick_time += Time.deltaTime;

        if (pick_time < 0.1f)
            return;

        isWalk = false;
        animator.SetBool("isWalk", false);
        animator.SetTrigger("doTouch");

        Vector3 mouse_position = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_position.x, mouse_position.y, 10));
        transform.position = point;
    }

    private void OnMouseUp()
    {
        pick_time = 0.0f;

        if (gamemanager.isSell)
        {
            if (gamemanager.Jellies.Count - 1 == 0)
            {
                Debug.Log("젤리가 0일순 없엉");
                return;
            }
            gamemanager.Jellies.Remove(gameObject);
            gamemanager.GetGold(jellyStat.idx, jellyStat.level);
            gamemanager.jellyNum--;
            PlayerPrefs.SetInt("JellyNum", gamemanager.jellyNum);
            Destroy(gameObject);
        }
        else
        {
            float pos_X = transform.position.x;
            float pos_Y = transform.position.y;
            if (pos_X > 6f)
            {
                pos_X = 5.7f;
            }
            if (pos_X < -6f)
            {
                pos_X = -5.7f;
            }
            if (pos_Y > 1.2f)
            {
                pos_Y = 1.0f;
            }
            if (pos_Y < -2.5f)
            {
                pos_Y = -2.2f;
            }
            SoundManager.instance.SEPlay(SEType.Drop);
            transform.position = new Vector3(pos_X, pos_Y, 0);
        }
    }
}
