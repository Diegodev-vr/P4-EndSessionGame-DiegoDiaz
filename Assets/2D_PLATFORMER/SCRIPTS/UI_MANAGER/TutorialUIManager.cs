using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private CanvasGroup panel1, panel2, panel3;
    [SerializeField] private SpriteRenderer panel4;
    [SerializeField] private CanvasGroup panel5, panel6, panel7, panel8, panel9, panel10;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject firstEnemy;

    [Header("Player")]
    [SerializeField] private Player_1_Shooter shooter;
    [SerializeField] private Player_1_Movement movement;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController shooterController;

    public float speed = 3f;

    private Input_Actions_Platformer input;
    private InputAction move, jump, fire;

    private int step = 1;
    private bool fading;


    void Awake()
    {
        input = new Input_Actions_Platformer();

        move = input.Player_1.Move;
        jump = input.Player_1.Jump;
        fire = input.Player_1.Fire;
    }

    void OnEnable()
    {
        input.Player_1.Enable();
    }

    void OnDisable()
    {
        input.Player_1.Disable();
    }

    void OnDestroy()
    {
        input.Dispose();
    }

    void Start()
    {
        panel1.alpha = 1;
        panel2.alpha = 0;
        panel3.alpha = 0;

        Color c = panel4.color; c.a = 0; panel4.color = c;

        panel5.alpha = 0;
        panel6.alpha = 0;
        panel7.alpha = 0;
        panel8.alpha = 0;
        panel9.alpha = 0;
        panel10.alpha = 0;

        shooter.enabled = false;
    }

    void Update()
    {
        // INPUT
        if (step == 1 && move.ReadValue<Vector2>().x != 0 && !fading) { step = 2; fading = true; }
        else if (step == 2 && jump.triggered && !fading) { step = 3; fading = true; }
        else if (step == 3 && fire.triggered && !fading) { step = 4; fading = true; movement.enabled = false; movement.transform.localScale = Vector3.one; }
        else if (step == 5 && fire.triggered && !fading) { step = 6; fading = true; }
        else if (step == 6 && fire.triggered && !fading) { step = 7; fading = true; firstEnemy.SetActive(true); }
        else if (step == 7 && fire.triggered && !fading) { step = 8; fading = true; playerAnimator.runtimeAnimatorController = shooterController; }
        else if (step == 8 && fire.triggered && !fading) { step = 9; fading = true; shooter.enabled = true; }
        else if (step == 9 && fire.triggered && !fading) { step = 10; fading = true; movement.enabled = true; startPanel.SetActive(false);  GameManager.Instance.StartGame(); GameManager.Instance.StartTimer();}

        // FADE PANELS
        if (fading)
        {
            if (step == 2)
            {
                panel1.alpha = Mathf.MoveTowards(panel1.alpha, 0, Time.deltaTime * speed);
                panel2.alpha = Mathf.MoveTowards(panel2.alpha, 1, Time.deltaTime * speed);
                if (panel1.alpha == 0) fading = false;
            }
            if (step == 3)
            {
                panel2.alpha = Mathf.MoveTowards(panel2.alpha, 0, Time.deltaTime * speed);
                panel3.alpha = Mathf.MoveTowards(panel3.alpha, 1, Time.deltaTime * speed);
                if (panel2.alpha == 0) fading = false;
            }
            if (step == 4)
            {
                panel3.alpha = Mathf.MoveTowards(panel3.alpha, 0, Time.deltaTime * speed);
                if (panel3.alpha == 0) fading = false;
            }
            if (step == 5)
            {
                panel5.alpha = Mathf.MoveTowards(panel5.alpha, 1, Time.deltaTime * speed);
                if (panel5.alpha == 1) fading = false;
            }
            if (step == 6)
            {
                panel5.alpha = Mathf.MoveTowards(panel5.alpha, 0, Time.deltaTime * speed);
                panel6.alpha = Mathf.MoveTowards(panel6.alpha, 1, Time.deltaTime * speed);
                if (panel5.alpha == 0) fading = false;
            }
            if (step == 7)
            {
                panel6.alpha = Mathf.MoveTowards(panel6.alpha, 0, Time.deltaTime * speed);
                panel7.alpha = Mathf.MoveTowards(panel7.alpha, 1, Time.deltaTime * speed);
                if (panel6.alpha == 0) fading = false;
            }
            if (step == 8)
            {
                panel7.alpha = Mathf.MoveTowards(panel7.alpha, 0, Time.deltaTime * speed);
                panel8.alpha = Mathf.MoveTowards(panel8.alpha, 1, Time.deltaTime * speed);
                if (panel7.alpha == 0) fading = false;
            }
            if (step == 9)
            {
                panel8.alpha = Mathf.MoveTowards(panel8.alpha, 0, Time.deltaTime * speed);
                panel9.alpha = Mathf.MoveTowards(panel9.alpha, 1, Time.deltaTime * speed);
                if (panel8.alpha == 0) fading = false;
            }
            if (step == 10)
            {
                panel9.alpha = Mathf.MoveTowards(panel9.alpha, 0, Time.deltaTime * speed);
                panel10.alpha = Mathf.MoveTowards(panel10.alpha, 1, Time.deltaTime * speed);
                if (panel9.alpha == 0) fading = false;
            }
        }

        // FADE SPRITE IN automatically on step 4, then move to step 5
        if (step == 4)
        {
            Color c = panel4.color;
            c.a = Mathf.MoveTowards(c.a, 1, Time.deltaTime * speed);
            panel4.color = c;
            if (panel4.color.a == 1) { step = 5; fading = true; }
        }
    }
}