using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.VFX;

public class FileSelector : MonoBehaviour
{
    public string filePath;
    public Texture2D texColor;
    Texture2D texPosScale;
    uint resolution = 4096;
    uint particlecount = 0;
    public TMPro.TMP_InputField pathinput;
    public UnityEngine.UI.Button button;
    public float sphereSize = 0.1f;
    public Material oMat;
    bool toUpdate = false;
    int maxx = 0;
    int maxy = 0;
    int maxz = 0;
    public VisualEffect vfx;
    List<GameObject> spherePool = new List<GameObject>();
    void Start()
    {
        pathinput.text = filePath;
        vfx = GetComponent<VisualEffect>();
        vfx.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        button.onClick.AddListener(delegate { ButtonClick();});
    }
    void ButtonClick()
    {
        filePath = pathinput.text;
        if (filePath.Length > 5)
        {
            LoadFile(filePath);
        }
    }
    public void updatesize()
    {
            vfx.Reinit();
            vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particlecount);
            vfx.SetFloat(Shader.PropertyToID("spheresize"), sphereSize);
            vfx.SetTexture(Shader.PropertyToID("TexColor"), texColor);
            vfx.SetTexture(Shader.PropertyToID("TexPosScale"), texPosScale);
            vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
    }
        
    void LoadFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        StringBuilder sb = new StringBuilder();
        while (!reader.EndOfStream)
        {
            sb.AppendLine(reader.ReadLine());
        }
        reader.Close();
        string fileContents = sb.ToString();
        Debug.Log("file read");
        string[] lines = fileContents.Split("\n");
        Color[] colors = new Color[lines.Length];
        Vector3[] positions = new Vector3[lines.Length];
        int count = 0;
        foreach(string l in lines)
        {
            if (!l.Contains(" "))
            {
                continue;
            }
            string[] parts = l.Split(" ");
            if (parts.Length < 6)
            {
                continue;
            }
            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;
            maxx = Mathf.Max((int)maxx, (int)x);
            maxy = Mathf.Max((int)maxy, (int)y);
            maxz = Mathf.Max((int)maxz, (int)z);
            float tempx = x;
            float tempy = y;
            float tempz = z;
            if (float.TryParse(parts[0], out tempx))
            {
                x = tempx;
            }
            if (float.TryParse(parts[1], out tempy))
            {
                y = tempy;
            }
            if (float.TryParse(parts[2], out tempz))
            {
                z = tempz;
            }
            int r = 0;
            int g = 0;
            int b = 0;
            int.TryParse(parts[3], out r);
            int.TryParse(parts[4], out g);
            int.TryParse(parts[5], out b);
            colors[count] = new Color(r/255f, g/255f, b/255f);
            positions[count] = new Vector3(y, x, z);
            count++;
        }
        SetParticles(colors, positions);
       

    }
    private void Update()
    {
        if (toUpdate)
        {
            toUpdate = false;
            vfx.Reinit();
            vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particlecount);
            vfx.SetFloat(Shader.PropertyToID("spheresize"), sphereSize);
            vfx.SetTexture(Shader.PropertyToID("TexColor"), texColor);
            vfx.SetTexture(Shader.PropertyToID("TexPosScale"), texPosScale);
            vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        }
    }
    public void SetParticles(Color[] colors, Vector3[] positions)
    {
        texColor = new Texture2D(positions.Length > (int)resolution ? (int) resolution : positions.Length, Mathf.Clamp(positions.Length / (int) resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        texPosScale = new Texture2D(positions.Length > (int)resolution ? (int) resolution : positions.Length, Mathf.Clamp(positions.Length / (int) resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        int texWidth = texColor.width;
        int texHeight = texColor.height;
        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidth; x++)
            {
                int index = x + y * texWidth;
                texColor.SetPixel(x, y, colors[index]);
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, sphereSize);
                texPosScale.SetPixel(x, y, data);
            }
        }
        texColor.Apply();
        texPosScale.Apply();
        particlecount = (uint)positions.Length;
        toUpdate = true;
    }
}