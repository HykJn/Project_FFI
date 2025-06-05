using UnityEngine;
using System.Collections.Generic;

public class Chicken : Animal, ISellable
{
    public override int Grown { get; protected set; }
    public int Price
    {
        get
        {
            if (Grown < 3) return 50;
            else return 100 + 10 * Grown;
        }
    }

    [SerializeField] RuntimeAnimatorController[] controllers;
    [SerializeField] ItemID eggID;

    private float timer = 3.0f;
    private float tick = 0;

    private Vector3 destination;
    private Vector3 moveDir;

    private byte state = 0b00;

    protected override void Awake()
    {
        base.Awake();
        Grown = 0;
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnEndDay += Grow;
    }

    private void Start()
    {
        //OnEnable();
    }

    private void Update()
    {
        SetRandomBehaviour();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEndDay -= Grow;
    }

    public void Sell()
    {
        //TODO: Implement selling logic
    }

    protected override void Grow()
    {
        this.Grown++;
        if (Grown == 3)
        {
            anim.runtimeAnimatorController = controllers[1];
        }
        if (Grown > 3)
        {
            //Instantiate(eggPrefab, this.transform.position, Quaternion.identity);
            GameObject egg = ObjectManager.Instance.GetInstance(eggID, true);
            egg.transform.position = this.transform.position;
        }
    }

    void SetRandomBehaviour()
    {
        if (!GameManager.Instance.IsPlaying) return;
        if (state == 0b00)
        {
            tick += Time.deltaTime;
            if (tick >= timer)
            {
                if (Random.Range(0, 2) == 0)
                {
                    ToMove();
                }
                else
                {
                    ToPick();
                }
            }
        }
        else
        {
            switch (state)
            {
                case 0b10:
                    OnMove();
                    break;
                case 0b11:
                    OnPick();
                    break;
            }
        }
    }

    bool Movable(Vector2Int position)
    {
        if (!TileManager.Instance.HasLand(position)) return false;

        if (TileManager.Instance.TryGetPropObj(position, out PropObj prop))
        {
            return false;
        }
        return true;
    }

    void ToMove()
    {
        do
        {
            int randX = Random.Range(0, 2) == 0 ? -1 : 1;
            int randY = Random.Range(0, 2) == 0 ? -1 : 1;

            if (randX == -1) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;


            destination = this.transform.position + new Vector3(randX, randY);
        } while (!Movable(Vector2Int.RoundToInt(destination)));


        moveDir = destination - this.transform.position;

        anim.SetBool("Move", true);
        state = 0b10;
    }

    void OnMove()
    {
        if (Vector2.Distance(this.transform.position, destination) < 0.05f)
        {
            this.transform.position = destination;

            timer = Random.Range(3f, 5f);
            tick = 0;

            anim.SetBool("Move", false);
            state = 0b00;
        }
        else
        {
            this.transform.position += Time.deltaTime * 1f * moveDir.normalized;
        }
    }

    void ToPick()
    {
        timer = Random.Range(4f, 6f);
        tick = 0;

        anim.SetTrigger("Pick");
        state = 0b11;
    }

    void OnPick()
    {
        state = 0b00;
    }
}
