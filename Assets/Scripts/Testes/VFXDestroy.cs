using System.Collections;
using UnityEngine;

public class VFXDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(DestroyVFX(1));
    }
    private IEnumerator DestroyVFX(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        

    }
}
