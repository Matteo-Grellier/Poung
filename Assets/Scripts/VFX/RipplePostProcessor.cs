using UnityEngine;

public class RipplePostProcessor : MonoBehaviour
{
    public Material RippleMaterial;
    public float MaxAmount = 50f;
    private GameObject Ball;

    [Range(0, 1)]
    public float Friction = .9f;

    private float Amount = 0f;

    private void Start()
    {
        Ball = GameObject.Find("Ball");
    }

    void Update()
    {
        if (GameObject.Find("Player1Goal").GetComponent<Goal>().hasColide || GameObject.Find("Player2Goal").GetComponent<Goal>().hasColide)
        {
            this.Amount = this.MaxAmount;
            Vector2 pos = new Vector2(Ball.transform.position.x,0);
            this.RippleMaterial.SetFloat("_CenterX", pos.x);
            this.RippleMaterial.SetFloat("_CenterY", pos.y);
            GameObject.Find("Player1Goal").GetComponent<Goal>().hasColide = false;
            GameObject.Find("Player2Goal").GetComponent<Goal>().hasColide = false;
        }

        this.RippleMaterial.SetFloat("_Amount", this.Amount);
        this.Amount *= this.Friction;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, this.RippleMaterial);
    }
}