using UnityEngine;

public class EndlessBackground : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] Transform playerTransform;
    [SerializeField] float zPosition = 10f; // How far back the background stays

    Material mat;
    Vector2 offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Follow X and Y, but stay at a fixed Z distance
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, zPosition);

        // Scroll the texture
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}