using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    // Every wall has the same player
    static private Transform player;

    public new Renderer renderer;

    private MaterialPropertyBlock propBlock;
    private CapsuleCollider playerCol;

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        playerCol = player.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (player == null) Debug.Log("No tengo player");
        else Debug.Log("Tengo player");

        propBlock.SetVector("_SpherePos", player.position);
        propBlock.SetFloat("_SphereRadius", playerCol.radius);
        renderer.SetPropertyBlock(propBlock);
    }

    public void GetPlayerTransform(Transform transform)
    {
        Debug.Log("otra vez hola");
        player = transform;
    }
}