using UnityEngine;

public class Cow : Animal
{
    #region ==========Fields==========
    public override int Grown { get; protected set; }

    [SerializeField] RuntimeAnimatorController[] controllers;
    [SerializeField] ItemID milkID;

    private float tick = 0f;
    private float timer = 5f;

    private Vector3 destination;
    private Vector3 moveDir;

    private byte state = 0b00;
    #endregion

    #region ==========Unity Events==========
    protected override void Awake()
    {
        base.Awake();
        Grown = 0;
        if (controllers.Length > 0)
        {
            anim.runtimeAnimatorController = controllers[0];
        }
        else
        {
            Debug.LogError("Cow animator controllers not set.");
        }
    }

    private void Update()
    {
        SetRandomBehaviour();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnEndDay += Grow;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEndDay -= Grow;
    }
    #endregion

    #region ==========Methods==========
    protected override void Grow()
    {
        this.Grown++;
        if (Grown == 3)
        {
            anim.runtimeAnimatorController = controllers[1];
        }
        if (Grown > 3 && Grown % 2 == 0)
        {
            GameObject milk = ObjectManager.Instance.GetInstance(milkID, true);
            milk.transform.position = this.transform.position;
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

            timer = Random.Range(5f, 7f);
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
        timer = Random.Range(5f, 7f);
        tick = 0;

        anim.SetTrigger("Pick");
        state = 0b11;
    }

    void OnPick()
    {
        state = 0b00;
    }
    #endregion
}
