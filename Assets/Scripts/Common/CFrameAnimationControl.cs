using UnityEngine;
using System.Collections;

public class CFrameAnimationControl : MonoBehaviour {

    public Texture2D m_texture;//原序列图

    public Texture2D[] m_texturesss;//序列图集

    public int cellWidth = 375;//单张宽度

    public int cellHeight = 454;//单张高度

    public int row = 7;//行

    public int col = 6;//列

    public int cellCount = 41;//总序列图数

    //当前帧
    public static int m_iCurFram = 0;

    public static bool isShow = false;


    // Use this for initialization
    void Start()
    {
        m_iCurFram = 0;
        isShow = false;
        m_texturesss = new Texture2D[cellCount];
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                if (((row - 1 - j) * col + i) < cellCount)
                    DePackTexture(i, j);
                else
                    continue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
      

    }

    void OnGUI()
    {
        if (isShow == true)
        {
            DrawAnimation(m_texturesss, new Rect(0, 0, cellWidth, cellHeight));
        }
    }

    void DePackTexture(int i, int j)
    {
        int cur_x = i * cellWidth;
        int cur_y = j * cellHeight;

        Texture2D newTexture = new Texture2D(cellWidth, cellHeight);

        for (int m = cur_y; m < cur_y + cellHeight; ++m)
        {
            for (int n = cur_x; n < cur_x + cellWidth; ++n)
            {
                newTexture.SetPixel(n - cur_x, m - cur_y, m_texture.GetPixel(n, m));
            }
        }
        newTexture.Apply();
        m_texturesss[(row - 1 - j) * col + i] = newTexture;
    }

    void DrawAnimation(Texture[] tex, Rect rect)
    {

        if (m_iCurFram < cellCount && m_iCurFram>=0)
        {
            //绘制当前帧
            GUI.DrawTexture(rect, tex[m_iCurFram], ScaleMode.StretchToFill, true, 0);
        }
        else
        {
            if (m_iCurFram >= cellCount)
            {
                GUI.DrawTexture(rect, tex[cellCount-1], ScaleMode.StretchToFill, true, 0);
            }
            if(m_iCurFram < 0)
            {
                GUI.DrawTexture(rect, tex[0], ScaleMode.StretchToFill, true, 0);
            }
            
        }
    }
}
