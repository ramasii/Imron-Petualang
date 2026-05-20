using UnityEngine;

public class Transition : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyTransition()
    {
        Destroy(gameObject);
    }

    public void StartTransition()
    {
        GetComponent<Animator>().SetTrigger("StartTransition");
    }
    
    public void EndTransition()
    {
        GetComponent<Animator>().SetTrigger("EndTransition");
    }
}
