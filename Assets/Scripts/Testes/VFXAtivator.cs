using System.Collections;
using UnityEngine;

public class VFXAtivator : MonoBehaviour
{
    public ParticleSystem[] stoneEdgeParticle;
    public bool ActivateVFX;
    public GameObject stoneVFX;

    private void Start()
    {
        stoneEdgeParticle = GetComponentsInChildren<ParticleSystem>();

    }
    void Update()
    {

        if (ActivateVFX)
        {
            ActivateVFX = false;
            Instantiate(stoneVFX);
            //StartCoroutine(InstantParticles());
        }
    }

    /*private IEnumerator InstantParticles()
    {
        yield return new WaitForSeconds(1);
        GameObject instantiatedVFX = Instantiate(stoneVFX, transform.position, Quaternion.identity); 
        ParticleSystem[] particles = instantiatedVFX.GetComponentsInChildren<ParticleSystem>();

        foreach (var item in particles)
        {
            item.Play();
        }
    }*/


}

