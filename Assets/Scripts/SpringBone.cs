using UnityEngine;

public class SpringBone : MonoBehaviour
{
    public Transform target;  // O alvo para onde o Spring Bone tenta voltar (posi��o "neutra")
    public float stiffness = 0.1f;  // Rigidez da mola
    public float damping = 0.9f;  // Fator de amortecimento
    private Vector3 velocity;  // Velocidade do movimento da mola

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calcule a dire��o e a dist�ncia entre o osso e o alvo
        Vector3 direction = target.position - transform.position;

        // Aplique uma for�a de mola para puxar o osso de volta para o alvo
        Vector3 springForce = direction * stiffness;

        // Atualize a velocidade aplicando amortecimento
        velocity = (velocity + springForce) * damping;

        // Aplique a velocidade ao osso
        transform.position += velocity * Time.deltaTime;
    }
}
