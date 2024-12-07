using UnityEngine;
using System.Collections;

public class ScaleController : MonoBehaviour
{
    public Transform targetObject;  // O objeto cujo tamanho será modificado
    public Vector3 initialScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 finalScale = new Vector3(1f, 1f, 1f);
    public float growDuration = 0.1f;
    public float sustainDuration = 0.8f;
    public float shrinkDuration = 0.1f;

    void Start()
    {
        StartCoroutine(AnimateScale());
    }

    private IEnumerator AnimateScale()
    {
        // Define a escala inicial
        targetObject.localScale = initialScale;

        // Aumenta para a escala final em growDuration
        yield return StartCoroutine(ChangeScale(targetObject, initialScale, finalScale, growDuration));

        // Sustenta a escala final por sustainDuration
        yield return new WaitForSeconds(sustainDuration);

        // Diminui para a escala inicial em shrinkDuration
        yield return StartCoroutine(ChangeScale(targetObject, finalScale, initialScale, shrinkDuration));
    }

    private IEnumerator ChangeScale(Transform obj, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            obj.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.localScale = endScale;
    }
}
