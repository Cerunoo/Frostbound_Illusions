using UnityEngine;
using System.Collections;

public class DreamsEntrance : MonoBehaviour
{
    [SerializeField] private int indexDreamsScene;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (anim.GetBool("Open") == true)
            {
                anim.SetTrigger("Takeover");
            }
        }
    }

    public void ActiveEntrance()
    {
        anim.SetBool("Open", true);
    }

    public void LoadDreams()
    {
        StartCoroutine(Wait());
        if (PlayerController.Instance != null) PlayerController.Instance.disableMove = true;
        if (Inventory.Instance != null) Inventory.Instance.SetStateWork(false);
    }

    private IEnumerator Wait() // Искусственное ожидание загрузки сцены, временно
    {
        yield return new WaitForSecondsRealtime(0.85f);
        AsyncLoading.LoadScene(indexDreamsScene);
    }
}