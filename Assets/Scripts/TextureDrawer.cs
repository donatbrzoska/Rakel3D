using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDrawer : MonoBehaviour
{
    // TODO compress to one attribute, so that these don't have to be matched with the canvas size -> which would then implicitly define the following two values
    public int TextureResolution_x = 1024; // canvas texture space
    public int TextureResolution_y = 512; // canvas texture space

    private OilPaintTexture Texture;
    private Camera Camera;
    private int CanvasColliderID;
    private float CanvasSize_x; // world space
    private float CanvasSize_y; // world space

    private Rakel Rakel;
    private Vector2 PreviousPreciseBrushPosition;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        CanvasColliderID = GameObject.Find("Canvas").GetComponent<MeshCollider>().GetInstanceID();

        CanvasSize_x = GameObject.Find("Canvas").GetComponent<Transform>().localScale.x * 10;//20;
        CanvasSize_y = GameObject.Find("Canvas").GetComponent<Transform>().localScale.y * 10;//10;

        Texture = new OilPaintTexture(TextureResolution_x, TextureResolution_y);
        GetComponent<Renderer>().material.SetTexture("_MainTex", this.Texture.Texture);

        // TODO choose useful values
        Rakel = new Rakel(111, 33);
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
            Rakel.UpdateDirection(new Vector2(1, -1));
            Rakel.ApplyToCanvas(Texture);
        }
    }

    Vector2Int ToCanvasTextureSpacePoint(Vector3 worldSpacePoint)
    {
        // "move" canvas to positive coordinates only, so further calculations are simplified
        float absX = worldSpacePoint.x + CanvasSize_x / 2; // TODO what about odd numbers?
        float absY = worldSpacePoint.z + CanvasSize_y / 2; // TODO what about odd numbers?

        // for the space conversion, a ratio between the two sizes is needed
        float xMultiplier = TextureResolution_x / CanvasSize_x; // Calculate: How much wider is the texture than the world space?
        float yMultiplier = TextureResolution_y / CanvasSize_y;

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
