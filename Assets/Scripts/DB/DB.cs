using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class DB // : MonoBehaviour
{
    bool useXML = true;
    private static DB instance = null;
    public static DB getInstance()
    {
        if (instance == null)
            instance = new DB();
        return instance;
    }
    ~DB()
    {
        //if (conn != null)
        //    conn.CloseConnection();
    }
    /// <summary>
    /// SQLite数据库辅助
    /// </summary>
   // private SQLiteHelper conn = null;
    public XMLDB.XMLHelper conn = null;
    private DB()
    {
#if UNITY_WEBPLAYER
        string path = Config.webIp + Config.dbPath;//Config.dbPathLoc;// Application.persistentDataPath + "/XML/db.xml";
#else
        string path = Application.streamingAssetsPath + "/XML/db.xml";
        conn = new XMLDB.XMLHelper(path);
#endif

    }
    public void SetData(WWW www)
    {
        conn = new XMLDB.XMLHelper(www);
    }
    public void Close()
    {
        // conn.CloseConnection();
    }
    /// <summary>
    /// 取得病例列表 
    /// </summary>
    /// <returns></returns>
    public List<MedicalRecord> getMedicalRecords()
    {
        List<MedicalRecord> result = new List<MedicalRecord>();
        string sql = "select * from MedicalRecord";
        //SqliteDataReader reader = conn.ExecuteQuery(sql);
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        while (reader.Read())
        {
            MedicalRecord item = new MedicalRecord();
            item.ID = reader.GetInt(reader.GetOrdinal("ID"));
            item.AD = reader.GetString(reader.GetOrdinal("AD"));
            item.name = reader.GetString(reader.GetOrdinal("name"));
            item.sex = reader.GetString(reader.GetOrdinal("sex"));
            item.age = reader.GetInt(reader.GetOrdinal("age"));
            item.nativePlace = reader.GetString(reader.GetOrdinal("nativePlace"));
            item.ethnic = reader.GetString(reader.GetOrdinal("ethnic"));
            item.phone = reader.GetString(reader.GetOrdinal("phone"));
            item.departments = reader.GetString(reader.GetOrdinal("departments"));
            item.bed = reader.GetString(reader.GetOrdinal("bed"));
            item.mainSuit = reader.GetString(reader.GetOrdinal("mainSuit"));
            item.medicalHistory = reader.GetString(reader.GetOrdinal("medicalHistory"));
            item.diagnosis = reader.GetString(reader.GetOrdinal("diagnosis"));
            item.auxiliaryExamination = reader.GetString(reader.GetOrdinal("auxiliaryExamination"));
            item.contraindication = reader.GetString(reader.GetOrdinal("contraindication"));
            item.physicalExamination = reader.GetString(reader.GetOrdinal("physicalExamination"));
            item.doctorName = reader.GetString(reader.GetOrdinal("doctorName"));
            item.doctorAdvice = reader.GetString(reader.GetOrdinal("doctorAdvice"));
            result.Add(item);
        }
        return result;
    }

    /// <summary>
    /// 取得配置表信息
    /// </summary>
    /// <param name="type">分类,为空时，不指定</param>
    /// <param name="key">键</param>
    /// <returns>满足条件的第一个值</returns>
    public string getConfigValue(string key, string type = "")
    {
        string str = "";
        string sql = "";
        if (type == "")
        {
            sql = "select Value from Config where Key = '" + key + "'";//
        }
        else
            sql = "select Value from Config where Key = '" + key + "' and Class = '" + type + "'";//
        //SqliteDataReader reader = conn.ExecuteQuery(sql);
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        if (reader.Read())
        {
            str = reader.GetString(0);
        }
        return str;
    }
    /// <summary>
    /// 取得评分表
    /// </summary>
    /// <param name="sheetId"></param>
    /// <returns></returns>
    public Dictionary<string, float> getGradingRule(string sheetId)
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        string sql = "select Code,itemScore from GradingRule where SheetID = '" + sheetId + "'";
        //SqliteDataReader reader = conn.ExecuteQuery(sql);
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        while (reader.Read())
        {
            result[reader.GetString(0)] = reader.GetFloat(1);
        }
        return result;
    }
    /// <summary>
    /// 取得评分表内部编码
    /// </summary>
    /// <param name="sheetId"></param>
    /// <returns></returns>
    public Dictionary<string, string> getGradingRuleSub(string sheetId)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        string sql = "select internalCode,Code from GradingRule where SheetID = '" + sheetId + "'";
        //SqliteDataReader reader = conn.ExecuteQuery(sql);
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        while (reader.Read())
        {
            result[reader.GetString(0)] = reader.GetString(1);
        }
        return result;
    }
    public int GetAudioCount()
    {
        return 8 * 4 + 79;
    }
    /// <summary>
    /// 取得病例是否需要该物品
    /// </summary>
    /// <returns>为null 表示不需要</returns>
    public string getArticle(string name, int id)
    {
        string sql = "select parameter from Article where MedicalRecordID = '" + id.ToString() + "' and ArticleName = '" + name + "'";//
        //SqliteDataReader reader = conn.ExecuteQuery(sql);
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        if (reader.Read())
        {
            return reader.GetString(0);
        }
        return null;
    }
    /// <summary>
    /// 取得病例所需要的物品数
    /// </summary>
    /// <returns>数量</returns>
    public int getArticleCount(int id)
    {
        string sql = "select parameter from Article where MedicalRecordID = '" + id.ToString() + "'";//
        //SqliteDataReader reader = conn.ExecuteQuery(sql);
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        int i = 0;
        while (reader.Read())
        {
            i++;
        }
        return i > 6 ? i - 4 : 0;
    }
    /// <summary>
    /// Gets the recode sub info.
    /// </summary>
    /// <returns>The recode sub info.</returns>
    /// <param name="name">Name.</param>
    /// <param name="id">Identifier.</param>
    public string getRecodeSubInfo(string name, int id)
    {
        string sql = "select parameter from MedicalRecordSub where (MedicalRecordID = 0 or MedicalRecordID ='" + id.ToString() + "') and Key = '" + name + "' order by MedicalRecordID DESC";//
        XMLDB.XMLDataReader reader = conn.ExecuteQuery(sql);
        if (reader.Read())
        {
            return reader.GetString(0);
        }
        return null;
    }
}
