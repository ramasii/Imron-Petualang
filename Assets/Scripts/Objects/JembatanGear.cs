using UnityEngine;

[RequireComponent(typeof(Animator))]
public class JembatanGear : MonoBehaviour
{
    public int numKey = 0;
    public int keyCount = 0;
    public int requiredKeyCount = 1;
    public GameObject gearObject;
    [SerializeField] private Animator anim;
    private Gameflow gameflow;

    void Awake()
    {
        gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow != null)
        {
            gameflow.onStateChange += OnStateChange;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gearObject.SetActive(false);
    }

    public void AddKey()
    {
        keyCount++;
        if (keyCount >= requiredKeyCount)
        {
            Debug.Log("JembatanGear activated!");
            gearObject.SetActive(true);
            anim.SetTrigger("Activate");
        }
    }

    public void Reset()
    {
        Debug.Log("Resetting JembatanGear");
        keyCount = 0;
        gearObject.SetActive(false);
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JembatanGear Active"))
        {
            anim.SetTrigger("Reset");
        }
    }

    public void OnStateChange(FlowState newState)
    {
        if (newState == FlowState.ArrangeRoute)
        {
            Reset();
        }
    }
}
