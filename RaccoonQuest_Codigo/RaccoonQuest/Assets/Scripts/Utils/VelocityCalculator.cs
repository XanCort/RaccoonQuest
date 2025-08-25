using UnityEngine;
//Clase auxiliar la usamos para calcular la velocidad de objetos, la utilizamos para transmitir la velocidad de plataformas a cajas y jugador
public class VelocityCalculator : MonoBehaviour
{
    private Vector3 _previousPosition;
    private Vector3 _velocity;

    void Start()
    {
        _previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        _velocity = (transform.position - _previousPosition) / Time.fixedDeltaTime;
        _previousPosition = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}