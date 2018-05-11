using UnityEngine;
using System.Collections;

public class TextureAnimation : MonoBehaviour {
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

    Vector2 uvOffset = Vector2.zero;

    //Renderer renderer;
    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (gameObject.GetComponent<Renderer>().enabled)
        {
            Material[] materials = gameObject.GetComponent<Renderer>().materials;
            Texture texture = materials[materialIndex].mainTexture;// GetTexture(textureName);
            string name = texture.name;
            Texture texture2 = materials[materialIndex].GetTexture(textureName);
            //System.IntPtr ptr=texture2.GetNativeTexturePtr();

            gameObject.GetComponent<Renderer>().materials[materialIndex].mainTextureOffset = uvOffset;//.SetTextureOffset(textureName, uvOffset);
        }
    }
	// Use this for initialization
	void Start () {
        //renderer = gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
