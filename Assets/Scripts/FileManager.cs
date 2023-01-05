using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileManager : MonoBehaviour
{
    public static FileManager Inst = null;
    BinaryFormatter myBf = null;
    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
    }
    
    public void SaveFile<T>(string filePath, T data)
    {
        using (FileStream fs = File.Create(filePath))
        {
            myBf.Serialize(fs, data);
            fs.Close();
        }
    }
    public T LoadFile<T>(string filePath)
    {
        T data = default;
        if(File.Exists(filePath))
        {
            using (FileStream fs = File.Open(filePath, FileMode.Open))
            {
                data = (T)myBf.Deserialize(fs);
                fs.Close();
            }
        }
        else
        {
            Debug.Log($"{filePath}에 파일이 존재하지 않습니다.");
        }
        return data;
    }
}
