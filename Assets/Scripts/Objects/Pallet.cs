using UnityEngine;

public class Pallet : MonoBehaviour
{
    public void OnPalletTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    public void OnPalletTriggerExit(Collider other)
    {
        Debug.Log(other.name);
    }
}
