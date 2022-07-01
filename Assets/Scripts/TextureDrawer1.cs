using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TextureDrawer : MonoBehaviour
{
    // TODO compress to one attribute, so that these don't have to be matched with the canvas size -> which would then implicitly define the following two values
    public int TextureResolution_x = 1024; // canvas texture space
    public int TextureResolution_y = 512; // canvas texture space

    private Texture2D Texture;
    private Camera Camera;
    private int CanvasColliderID;
    private float CanvasSize_x; // world space
    private float CanvasSize_y; // world space
    //public int speed = 128;

    private _Brush _Brush;
    private Vector2 PreviousPreciseBrushPosition;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        CanvasColliderID = GameObject.Find("Canvas").GetComponent<MeshCollider>().GetInstanceID();

        CanvasSize_x = GameObject.Find("Canvas").GetComponent<Transform>().localScale.x * 10;//20;
        CanvasSize_y = GameObject.Find("Canvas").GetComponent<Transform>().localScale.y * 10;//10;

        Texture = new Texture2D(TextureResolution_x, TextureResolution_y, TextureFormat.RGBA32, false);
        Texture.filterMode = FilterMode.Point;
        GetComponent<Renderer>().material.SetTexture("_MainTex", this.Texture);
        //GetComponent<Renderer>().material.mainTexture = this.Texture;
        //GetComponent<Renderer>().material.

        _Brush = new _Brush(20);
    }

    // Update is called once per frame
    void Update()
    {
        //      for (int i=0; i<speed; i++)
        //{
        //	var x = Random.Range(0, width);
        //	var y = Random.Range(0, height);
        //	Texture.SetPixel(x, y, Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f));
        //}

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.colliderInstanceID == CanvasColliderID)
            {
                //Debug.Log("hit canvas at " + hit.point);
                //Debug.Log("hit canvas at texture " + ToCanvasTextureSpacePoint(hit.point));
                Vector2 preciseBrushPosition = ToCanvasTextureSpacePoint(hit.point);
                _Brush.UpdatePosition(preciseBrushPosition);

                Vector2 preciseStrokeDirection = PreviousPreciseBrushPosition - preciseBrushPosition;
                Debug.Log("previous direction: " + PreviousPreciseBrushPosition);
                Debug.Log("precise stroke direction: " + preciseStrokeDirection);
                if (preciseStrokeDirection != PreviousPreciseBrushPosition)
                {
                    _Brush.UpdateDirection(preciseStrokeDirection);
                    _Brush.UpdateDirection(Vector2.one);
                    PreviousPreciseBrushPosition = preciseBrushPosition;
                }

                _Brush.UpdateTexture(Texture);

            }
        }

    }

    Vector2 ToCanvasTextureSpacePoint(Vector3 worldSpacePoint)
    {
        // "move" canvas to positive coordinates only, so further calculations are simplified
        float absX = worldSpacePoint.x + CanvasSize_x / 2; // TODO what about odd numbers?
        float absY = worldSpacePoint.z + CanvasSize_y / 2; // TODO what about odd numbers?

        // for the space conversion, a ratio between the two sizes is needed
        float xMultiplier = TextureResolution_x / CanvasSize_x; // Calculate: How much wider is the texture than the world space?
        float yMultiplier = TextureResolution_y / CanvasSize_y;

        float textureX = absX * xMultiplier;
        float textureY = absY * yMultiplier;

        // now invert, because texture 0,0 is in the upper right corner whereas screen 0,0 is in the lower left
        float invertedTextureX = textureX - TextureResolution_x;
        float invertedTextureY = textureY - TextureResolution_y;

        return new Vector2(System.Math.Abs(invertedTextureX), System.Math.Abs(invertedTextureY));
    }
}
