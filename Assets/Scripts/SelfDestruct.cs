using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class SelfDestruct : MonoBehaviour
{
    public float selfDestructTime = 30f;

    private void Start()
    {
        StartCoroutine(SelfDestructCoroutine());
    }

    private IEnumerator SelfDestructCoroutine()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }
}