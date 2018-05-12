
using System.Collections.Generic;

namespace CFramework
{
    /// <summary>
    /// 评分表结构
    /// </summary>
    public class _CLASS_SCORESHEET
    {
        public string id = "";

        public List<_CLASS_SCORESHEET> childItem = new List<_CLASS_SCORESHEET>();

        public string code = "";

        public string score = "";

        public bool childmutx = true;

        public string codedesc = "";

        public string scoreitemid = "";

        public string variablename = "";

        public string variabledesc = "";

        public string classname = "";

        public string classdesc = "";

        public bool isDefault;
    }
    /// <summary>
	/// 相机快捷键数据
	/// </summary>
	public class _CLASS_CameraShortCut
    {
        /// <summary>
        /// id
        /// </summary>
        public string id = "";

        /// <summary>
        /// 按钮名字
        /// </summary>
        public string shortcutName = "";

        /// <summary>
        /// 按钮位置
        /// </summary>
        public string[] position = new string[]
        {
            "10000",
            "10000",
            "10000"
        };

        /// <summary>
        /// 按钮欧拉角
        /// </summary>
        public string[] euler = new string[]
        {
            "0",
            "0",
            "0"
        };
    }
    public enum AssetBundleStyleEnum
    {
        None,
        Model,
        Audios,
        UI
    }
    public class _CLASS_ASSETBUNDLE
    {
        public string id = "";

        public string assetpackagename = "";

        public string packagedesc = "";

        public string packagepath = "";

        public bool isallneed = true;

        public AssetBundleStyleEnum style;
    }
    /// 静态模型数据
	/// </summary>
	public class _CLASS_StaticModelInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string id = "";

        /// <summary>
        /// 模型路径
        /// </summary>
        public string modelPath = "";

        /// <summary>
        /// 模型位置
        /// </summary>
        public string[] modelPos = new string[]
        {
            "",
            "",
            ""
        };

        /// <summary>
        /// 模型欧拉角
        /// </summary>
        public string[] modelEuler = new string[]
        {
            "",
            "",
            ""
        };

        /// <summary>
        /// 场景模型默认状态
        /// </summary>
        public string defaultStatus = "";

        /// <summary>
        /// 模型描述
        /// </summary>
        public string modelDesc = "";

        /// <summary>
        /// 资源包名
        /// </summary>
        public string assetbundlename = "";
    }

    /// <summary>
	/// 病例
	/// </summary>
	public class _CLASS_MedicalRecord
    {
        /// <summary>
        /// 病例ID
        /// </summary>
        public string id;

        /// <summary>
        /// 病例名字
        /// </summary>
        public string medicalname;

        /// <summary>
        /// 医嘱内容
        /// </summary>
        public string doctoradvice;

        /// <summary>
        /// 病例类别
        /// </summary>
        public string problemid;

        /// <summary>
        /// 姓名
        /// </summary>
        public string name;

        /// <summary>
        /// 性别
        /// </summary>
        public string sex;

        /// <summary>
        /// 年龄
        /// </summary>
        public string age;

        /// <summary>
        /// 籍贯
        /// </summary>
        public string nativeplace;

        /// <summary>
        /// 民族
        /// </summary>
        public string ethnic;

        /// <summary>
        /// 电话
        /// </summary>
        public string phone;

        /// <summary>
        /// 科别
        /// </summary>
        public string departments;

        /// <summary>
        /// 住院号
        /// </summary>
        public string ad;

        /// <summary>
        /// 床号
        /// </summary>
        public string bednum;

        /// <summary>
        /// 主诉
        /// </summary>
        public string mainsuit;

        /// <summary>
        /// 现病史
        /// </summary>
        public string medicalhistory;

        /// <summary>
        /// 考虑诊断
        /// </summary>
        public string diagnosis;

        /// <summary>
        /// 辅助检查
        /// </summary>
        public string auxiliary;

        /// <summary>
        /// 体格检查
        /// </summary>
        public string physical;

        /// <summary>
        /// 禁忌症
        /// </summary>
        public string contraindication;

        /// <summary>
        /// 医师名字
        /// </summary>
        public string doctorname;

        /// <summary>
        /// 产品ID
        /// </summary>
        public string productid;

        public string Diagnosis
        {
            get
            {
                return this.diagnosis;
            }
            set
            {
                this.diagnosis = value;
            }
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public struct UserInfo
    {
        public string accountName; //用户名
        public string chineseName; //姓名
        public string sex;// n v
        public bool isLogin;
        public UserStatus status;
    }
    public enum UserStatus
    {
        isNULL,
        /// <summary>
        /// 用户
        /// </summary>
        user,
        /// <summary>
        /// 服务人员
        /// </summary>
        server,
        /// <summary>
        /// 游客
        /// </summary>
        visitor
    }

    /// <summary>
	/// 目录导航属性
	/// </summary>
	public class _CLASS_CatalogProperty
    {
        /// <summary>
        /// id
        /// </summary>
        public string id = "";

        /// <summary>
        /// 顺序
        /// </summary>
        public int order;

        /// <summary>
        /// 目录描述
        /// </summary>
        public string catalogName = "";

        /// <summary>
        /// 点击导航条执行的步骤类
        /// </summary>
        public string catalogClickStepName = "";

        /// <summary>
        /// 归属于当前目前的所有步骤类名
        /// </summary>
        public string[] catalogChildStepName = new string[0];
    }
}