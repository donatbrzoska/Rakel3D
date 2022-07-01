using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDrawer : MonoBehaviour
{
    private Camera Camera;

    private int CanvasColliderID;
    private float CanvasWidth; // world space
    private float CanvasHeight; // world space

    public int TextureResolution = 100; // texture space pixels per 1 world space // TODO GUI
    private OilPaintTexture Texture;
    private int TextureWidth; // texure space
    private int TextureHeight; // texture space

    private Rakel Rakel;
    private float RakelLength = 2.5f; // world space // TODO GUI
    private float RakelWidth = 1f; // world space // TODO GUI

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        CanvasColliderID = GameObject.Find("Canvas").GetComponent<MeshCollider>().GetInstanceID();

        // convert scale attribute to world space
        CanvasWidth = GameObject.Find("Canvas").GetComponent<Transform>().localScale.x * 10;
        CanvasHeight = GameObject.Find("Canvas").GetComponent<Transform>().localScale.y * 10;

        TextureWidth = (int)(TextureResolution * CanvasWidth);
        TextureHeight = (int)(TextureResolution * CanvasHeight);
        Texture = new OilPaintTexture(TextureWidth, TextureHeight);
        GetComponent<Renderer>().material.SetTexture("_MainTex", this.Texture.Texture);

        int rakelLength_textureSpace = (int)(TextureResolution * RakelLength);
        int rakelWidth_textureSpace = (int)(TextureResolution * RakelWidth);
        //Rakel = new Rakel(rakelLength_textureSpace, rakelWidth_textureSpace);
        Rakel = new OptimizedRakel(rakelLength_textureSpace, rakelWidth_textureSpace);
        //Rakel = new Rakel(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldSpaceHit = InputUtil.GetMouseHit(Camera, CanvasColliderID);
        if (!worldSpaceHit.Equals(Vector3.negativeInfinity))
        {
            Vector2Int preciseBrushPosition = ToCanvasTextureSpacePoint(worldSpaceHit);
            Rakel.UpdateColor(new Color(0.3f, 0, 0.7f));
            Rakel.UpdatePosition(preciseBrushPosition);
            Rakel.UpdateNormal(new Vector2(1, -1));
            Rakel.ApplyToCanvas(Texture);
        }
    }

    Vector2Int ToCanvasTextureSpacePoint(Vector3 worldSpacePoint)
    {
        // "move" canvas to positive coordinates only, so further calculations are simplified
        float absX = worldSpacePoint.x + CanvasWidth / 2; // TODO what about odd numbers?
        float absY = worldSpacePoint.z + CanvasHeight / 2; // TODO what about odd numbers?

        // for the space conversion, a ratio between the two sizes is needed
        float xMultiplier = TextureWidth / CanvasWidth; // Calculate: How much wider is the texture than the world space?
        float yMultiplier = TextureHeight / CanvasHeight;

        int textureX = Mathf.RoundToInt(absX * xMultiplier);
        int textureY = Mathf.RoundToInt(absY * yMultiplier);

        return new Vector2Int(textureX, textureY);
    }

    /*
     MouseInput: Canvas hitpoint in world space
     Canvas:     Convert world space hitpoint --> canvas texture space hitpoint
     Brush:      Do stuff with texture space hitpoint
    */
}
