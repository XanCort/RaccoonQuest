using UnityEngine;

public class CartelController : MonoBehaviour
{
    [SerializeField] private Sprite spriteCartel;

    public Sprite GetSprite()
    {
        return spriteCartel;
    }
}
