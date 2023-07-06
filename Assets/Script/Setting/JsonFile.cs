using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Unity.VisualScripting;
using System.Text;

public static class JsonFile
{
    public static string SettingJsonPath = "/Data/Setting.json";
    public static string ProgressJsonPath = "/Data/Progress.json";
    public static string ReadJsonStrData(string FilePath)
    {
        //string类型的数据常量
        string StringData;
        //获取到路径
        string fileUrl = Application.dataPath + FilePath;
        //读取文件

        using (StreamReader sr = File.OpenText(fileUrl))
        {
            //数据保存
            StringData = sr.ReadToEnd();
            sr.Close();
        }
        //返回数据
        return StringData;
    }
    public static void WriteJsonStrDate(string FilePath, string JsonStr)
    {
        //获取到路径
        string fileUrl = Application.dataPath + FilePath;
        using (StreamWriter sw = new(fileUrl, false, Encoding.Default))
        {
            //数据保存
            sw.Write(JsonStr);
            sw.Flush();
            sw.Close();
        }
    }
    
    public static Dictionary<string, int> ReadAndConvertJsonToStr(string filepath)
    {
        try
        {
            string JsonStr = ReadJsonStrData(filepath);
            Dictionary<string, int> Dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(JsonStr);
            return Dict;
        }
        catch
        {
            Debug.LogError("ERROR: Failed to read JSON, file missing or content incorrect. Please check if the " + "/Asserts/" + filepath + " file is missing or has incorrect content.");
            FlowControl.Exit();
            return new Dictionary<string, int> { };
        }
    }
    public static Dictionary<string, int> ReadSetting()
    {
        return ReadAndConvertJsonToStr(SettingJsonPath);
    }
    public static Dictionary<string, int> ReadProgress()
    {
        return ReadAndConvertJsonToStr(ProgressJsonPath);
    }
    public static void WriteSetting(Setting ST)
    {
        // Debug Test Use
        Setting PS = new()
        {
            ResolutionX = 1920,
            ResolutionY = 1080,
            FPS = 60,
            IsFullScreen = 0,
            ExperimentType = 0,
            SubjectNum = 20,
            RoundsPerSubject = 3
        };
        string JsonStr = JsonConvert.SerializeObject(ST, Newtonsoft.Json.Formatting.Indented);
        WriteJsonStrDate(SettingJsonPath, JsonStr);
    }
    public static Dictionary<string, int> WriteProgress(int TotalPresentedRounds, int SubjectIndex, int RoundIndex)
    {
        // Debug Test Use
        Progress PG = new();
        PG.TotalPresentedRounds = TotalPresentedRounds;
        PG.CurrentSubjectIndex = SubjectIndex;
        PG.CurrentRoundIndex = RoundIndex;
        string JsonStr = JsonConvert.SerializeObject(PG, Newtonsoft.Json.Formatting.Indented);
        WriteJsonStrDate(ProgressJsonPath, JsonStr);
        Dictionary<string, int> Dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(JsonStr);
        return Dict;
    }
    
    public static void SetDefaultSetting()
    {
        Setting ST = new()
        {
            ResolutionX = 1920,
            ResolutionY = 1080,
            FPS = 60,
            IsFullScreen = 0,
            ExperimentType = 0,
            SubjectNum = 20,
            RoundsPerSubject = 3,
            IndependentVarNum = 2,
            DependentVarNum = 4
        };
        WriteSetting(ST);

    }
    public static void SetDefaultProgress()
    {
        _ = WriteProgress(0, 0, 0);
    }
    public class Setting
    {
        public int ResolutionX;
        public int ResolutionY;
        public int IsFullScreen;
        public int FPS;
        public int ExperimentType;
        public int SubjectNum;
        public int RoundsPerSubject;
        public int IndependentVarNum;
        public int DependentVarNum;
    }
    public class Progress
    {
        public int TotalPresentedRounds;
        public int CurrentSubjectIndex;
        public int CurrentRoundIndex;
    }
}
