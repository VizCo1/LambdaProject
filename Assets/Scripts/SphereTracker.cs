using UnityEngine;

public class SphereTracker : MonoBehaviour
{
    public Transform sphere;
    public new Renderer renderer;

    private MaterialPropertyBlock propBlock;
    private CapsuleCollider sphereCol;

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        sphereCol = sphere.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        propBlock.SetVector("_SpherePos", sphere.position);
        propBlock.SetFloat("_SphereRadius", sphereCol.radius);
        renderer.SetPropertyBlock(propBlock);
    }
}