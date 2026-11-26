using UnityEngine;
using System.IO;

public class SaveManager
{
    private readonly string savePath;
    private readonly string backPath;
    private readonly string tempPath;
    public SaveData Data { get; private set; }

    // コンストラクタでパスの生成
    public SaveManager()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        backPath = savePath + ".bak";
        tempPath = savePath + ".tmp";
        Load();

    }

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true); // trueで整形

        // まず一時ファイルへ「全部」書き切る
        File.WriteAllText(tempPath, json);

        if(File.Exists(savePath))
        {
            File.Replace(tempPath, savePath, backPath);
        }
        else
        {
            File.Move(tempPath, savePath);
        }
    }

    public void Load()
    {
        Data = TryLoad(savePath);

        if(Data == null)
        {
            Debug.LogWarning("メインのセーブが読めません。バックアップから復旧します");
            Data = TryLoad(backPath);
        }

        if (Data == null)
        {
            Debug.LogWarning("バックアップも読めません。新規データで開始します");
            Data = new SaveData();  // 新規作成
            Data.stage.isStageUnlocked[0] = true;
        }
    }

    private SaveData TryLoad(string path)
    {
        try
        {
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.Exception e)
        {
            // 壊れたJSONはここで例外になる。落とさずnullを返して次の段へ
            Debug.LogWarning("セーブの読み込みに失敗: " + path + "\n" + e.Message);
            return null;
        }
    }

    public void ResetSaveData()
    {
        Data = new SaveData();
        Data.stage.isStageUnlocked[0] = true;
        Save();
    }
}
