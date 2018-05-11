using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XMLDB {

    
    public class XMLHelper
    {
        XmlDocument doc;
        string docPath;
        List<XMLTable> dataPool = new List<XMLTable>();
        /// <summary>
        /// 通过路径加载xml
        /// </summary>
        /// <param name="path"></param>
        public XMLHelper(string path)
        {
            docPath = path;
            doc = new XmlDocument();
            doc.Load(path);
            LoadData();
        }
        public XMLHelper(WWW www)
        {
            doc = new XmlDocument();
            doc.LoadXml(www.text);
            LoadData();
        }
        void LoadData()
        {
            var nodes = doc.SelectNodes("tables");
            if (nodes == null)
            {
                Debug.Log("nodes is null");
                return;
            }
            foreach (XmlNode node in nodes[0].ChildNodes)
            {
                //循环表节点
                if (node.Attributes["name"] != null)
                {
                    //添加一张表
                    XMLTable table = new XMLTable();
                    table.tableName = node.Attributes["name"].Value.ToLower();//取表名
                    Debug.Log("add table :"+ table.tableName);
                    #region 处理表头数据
                    XmlNode node_t = node.SelectSingleNode("head");
                    if (node_t == null || node_t.ChildNodes.Count == 0)
                    {
                        Debug.Log(table.tableName + "表头错误！");
                        continue;//空表
                    }
                    table.colCount = node_t.ChildNodes.Count;
                    bool err = false;
                    //取得表头 添加表头信息
                    for (int i = 0; i < table.colCount; i++)
                    {
                        XMLTable.STHead headItem = new XMLTable.STHead();
                        headItem.index = i;
                        if (node_t.ChildNodes[i].Attributes["field"] != null || node_t.ChildNodes[i].Attributes["type"] != null || node_t.ChildNodes[i].Attributes["desc"] != null)
                        {
                            headItem.field = node_t.ChildNodes[i].Attributes["field"].Value.ToLower();
                            headItem.type = node_t.ChildNodes[i].Attributes["type"].Value.ToLower();
                            headItem.desc = node_t.ChildNodes[i].Attributes["desc"].Value;
                        }
                        else
                        {
                            Debug.LogWarning("<head> marker index = " + i + "format error");
                            err = true;
                            break;
                        }
                        table.head.Add(headItem);
                    }
                    if (err)
                        continue;
                    #endregion
                    #region 处理表记录
                    XmlNode node_i = node.SelectSingleNode("items");
                    if (node_i == null || node_i.ChildNodes.Count == 0)
                    {
                        Debug.Log(table.tableName + "表数据为空");
                        table.rowCount = 0;
                    }
                    else
                    {
                        table.rowCount = node_i.ChildNodes.Count;
                        int errNumber = 0;
                        for (int i = 0; i < table.rowCount; i++)
                        {
                            XmlNodeList values = node_i.ChildNodes[i].SelectNodes("value");
                            if (values.Count != table.colCount)
                            {
                                Debug.LogWarning("<items> 标签下第" + i + "个<row>标签格式不匹配！");
                                errNumber++;
                                continue;
                            }
                            string[] row = new string[table.colCount];
                            for (int ro = 0; ro < table.colCount; ro++)
                            {
                                XmlNode value = values[ro];
                                row[ro] = value.InnerText;
                            }
                            table.data.Add(row);
                        }
                        table.rowCount -= errNumber;
                    }
                    #endregion
                    Debug.Log("table :" + table.tableName + " count="+ table.rowCount);
                    dataPool.Add(table);
                }
            }
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="queryString">查询语句，支持sql标准查询语句,不区分大小写</param>
        /// <returns>查询结果集</returns>
        public XMLDataReader ExecuteQuery(string queryString)
        {
            XMLDataReader dataReader = new XMLDataReader();
            //List<string> headList = new List<string>();//查询字段
            //string tableName;//表名
            string[] queryStringt = queryString.Trim().Split('\'');
            queryString = "";
            for (int i = 0; i < queryStringt.Length; i++)
            {
                if(i%2 == 0)
                    queryString += queryStringt[i].ToLower();
                else
                    queryString += "'"+queryStringt[i] + "'";
            }
            //queryString = queryString.Trim().ToLower();
            queryString = System.Text.RegularExpressions.Regex.Replace(queryString, @"\b\s+\b", " ");
            if (SqlCheck(queryString))
            {
                //分解sql语句
                string tableStr = queryString.Substring(queryString.IndexOf("from") + 4, (queryString.IndexOf("where")>0? queryString.IndexOf("where"): queryString.Length) - queryString.IndexOf("from") - 4);
                string[] tableStrList = tableStr.Trim().Split(',');
                dataReader.table.tableName = tableStr.Trim();
                string headStr = queryString.Substring(6, queryString.IndexOf("from") - 6);
                string[] headStrList = headStr.Trim().Split(',');
                string whereStr = "";
                string orderStr = "";
                string[] orderStrList = null;
                if (queryString.IndexOf("where") > 0)
                {
                    int endIndex = queryString.Length -1;
                    if (queryString.IndexOf(";") >0 && queryString.IndexOf(";") < endIndex)
                    {
                        endIndex = queryString.IndexOf(";") - 1;
                    }
                    if (queryString.IndexOf("order by") > 0 && queryString.IndexOf("order by") < endIndex)
                    {
                        endIndex = queryString.IndexOf("order by") - 1;
                        orderStr = queryString.Substring(queryString.IndexOf("order by") + 8).Replace(';', ' ').Trim();
                        orderStrList = orderStr.Split(',');
                    }
                    whereStr = queryString.Substring(queryString.IndexOf("where") + 5, endIndex - queryString.IndexOf("where") - 5).Trim();
                    //whereStr = System.Text.RegularExpressions.Regex.Replace(whereStr, @"\b\s+\b", " ");
                }
                dataReader.table.colCount = 0;
                
                XMLTable tmpTable = new XMLTable();
                tmpTable.tableName = tableStr;
                //查询
                #region 全表连接
                foreach (string tablename in tableStrList)
                {
                    XMLTable subTable = dataPool.Find(name =>
                    {
                        if (name.tableName == tablename)
                            return true;
                        else
                            return false;
                    }).Clone();
                    if (tmpTable.colCount == 0)
                    {
                        tmpTable.colCount = subTable.colCount;
                        tmpTable.rowCount = subTable.rowCount;
                        foreach (var item in subTable.head)
                        {
                            XMLTable.STHead sthead = item.Clone();
                            sthead.field = subTable.tableName + "." + sthead.field;
                            tmpTable.head.Add(sthead);
                        }
                        foreach (var item in subTable.data)
                        {
                            tmpTable.data.Add(item);
                        }
                    }
                    else
                    {
                        XMLTable tempTableCp = tmpTable.Clone();
                        foreach (var item in subTable.head)
                        {
                            XMLTable.STHead sthead = item.Clone();
                            sthead.field = subTable.tableName + "." + sthead.field;
                            sthead.index = tempTableCp.colCount + sthead.index;
                            tmpTable.head.Add(sthead);
                        }
                        tmpTable.data.Clear();
                        for (int i = 0; i < tempTableCp.rowCount; i++)
                        {
                            for (int k = 0; k < subTable.rowCount; k++)
                            {
                                string[] newRow = new string[tempTableCp.colCount + subTable.colCount];
                                for (int t = 0; t < tempTableCp.colCount + subTable.colCount; t++)
                                {
                                    if (t < tempTableCp.colCount)
                                        newRow[t] = tempTableCp.data[i][t];
                                    else
                                        newRow[t] = subTable.data[k][t- tempTableCp.colCount];
                                }
                                tmpTable.data.Add(newRow);
                            }
                        }
                        tmpTable.colCount = tempTableCp.colCount + subTable.colCount;
                        tmpTable.rowCount = tmpTable.rowCount * subTable.rowCount;
                    }

                }
                #endregion
                //tmpTable有全表数据
                //条件处理
                if(whereStr != "")
                    tmpTable = TableWhere(whereStr, tmpTable);//获得条件筛选后的全字段表
                dataReader.table.rowCount = tmpTable.rowCount;
                //添加表头
                dataReader.table.head = MergeHeader(headStrList, tableStrList);
                dataReader.table.colCount = dataReader.table.head.Count;
                #region 筛选视图数据
                for (int i = 0; i < tmpTable.rowCount; i++)
                {
                    string[] row = new string[dataReader.table.colCount];
                    foreach (var item in dataReader.table.head)
                    {
                        int index = tmpTable.head.Find(name => {
                            if (name.field == item.field)
                                return true;
                            else
                                return false;
                        }).index;
                        row[item.index] = tmpTable.data[i][index];//提取数据
                    }
                    dataReader.table.data.Add(row);
                }
                #endregion
                //取得显示视图完成

                #region 检查排序
                if (orderStrList != null && orderStrList.Length >0)
                {
                    if(orderStrList.Length == 1 && orderStrList[0].Trim()== "")
                    {
                        //
                    }
                    else
                    {
                        //排序
                        //取得排序规则
                        bool asc = true;//顺序排序
                        //检查有没有排序标志
                        string [] lastItem = orderStrList[orderStrList.Length - 1].Trim().Split(' ');
                        if (lastItem.Length == 2)
                        {
                            if(lastItem[1].Trim() == "desc")
                                asc = false;//使用倒叙
                            orderStrList[orderStrList.Length - 1] = lastItem[0].Trim();
                        }
                        //规范化字段
                        for (int i = 0; i < orderStrList.Length; i++)
                        {
                            orderStrList[i] = GetFullField(orderStrList[i], tableStrList);
                        }
                        //启用冒泡排序
                        dataReader.table = BubbleSort(orderStrList, dataReader.table, asc);
                        //处理结束
                    }
                }
                #endregion
            }
            else if(queryString.IndexOf("update ") >= 0 && queryString.IndexOf("set") > 0)
            {
                //分解sql语句
                string tableStr = queryString.Substring(queryString.IndexOf("update") + 6, queryString.IndexOf("set")  - queryString.IndexOf("update") - 6);
                tableStr = tableStr.Trim();
                queryString = queryString.Replace(';', ' ').Trim();
                string headStr = queryString.Substring(queryString.IndexOf("set") + 3, (queryString.IndexOf("where") > 0 ? queryString.IndexOf("where") : queryString.Length) - queryString.IndexOf("set") - 3);
                string[] headStrList = headStr.Trim().Split(',');
                string whereStr = "";
                if (queryString.IndexOf("where") > 0)
                {
                    int endIndex = queryString.Length;
                    whereStr = queryString.Substring(queryString.IndexOf("where") + 5, endIndex - queryString.IndexOf("where") - 5).Trim();
                }
                XmlNode nodes = doc.DocumentElement;
                //var nodes = root.SelectNodes("tables");
                foreach (XmlNode node in nodes.ChildNodes)
                {
                    //循环表节点
                    if (node.Attributes["name"] != null && node.Attributes["name"].Value.ToLower() == tableStr)
                    {
                        //添加一张表
                        XMLTable table = dataPool.Find(name=> {
                            if (name.tableName == tableStr)
                                return true;
                            else
                                return false;
                        });
                        if (table == null)
                            break;
                        #region 处理表头数据
                        XMLTable tempTable = table.Clone();
                        tempTable = TableWhere(whereStr, StandardizedTable(tempTable));//条件过滤
                        XmlNode node_t = node.SelectSingleNode("items");
                        XmlNodeList list = node_t.SelectNodes("row");//.ChildNodes;//

                        for (int i = 0,ip = 0; i < list.Count && ip < tempTable.rowCount; i++)
                        {
                            
                            if (string.Compare(tempTable.data[ip][0] , table.data[i][0]) == 0)
                            {
                                //更新
                                foreach (string head in headStrList)
                                {
                                    string[] headList = head.Split('=');
                                    if (headList.Length < 2)
                                        continue;
                                    Debug.Log("headList[0] = " + headList[0].Trim());
                                    XMLTable.STHead sthead = table.head.Find(name => {
                                        if (name.field == headList[0].Trim())
                                            return true;
                                        else
                                            return false;
                                    });
                                    if (sthead == null)
                                        continue;
                                    
                                    list[i].SelectNodes("value")[sthead.index].InnerText = headList[1].Replace("\'","").Trim();
                                    table.data[i][sthead.index] = headList[1].Replace("\'", "").Trim();
                                    Debug.Log("list[i].SelectNodes(\"value\")[sthead.index].InnerText = " + list[i].SelectNodes("value")[sthead.index].InnerText);
                                }
                                ip++;
                            }
                        }
                        #endregion
                    }
                }
                doc.Save(docPath);
            }
            else
            {
                Debug.LogWarning("查询语句错误");
            }
            return dataReader;
        }
        /// <summary>
        /// 检查语句是否正确
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool SqlCheck(string sql)
        {
            if (sql.IndexOf("select") != 0 || sql.IndexOf("from") < 0)
            {
                return false;
            }
            string subStr = sql.Substring(sql.IndexOf("from") + 4, (sql.IndexOf("where") > 0 ? sql.IndexOf("where") : sql.Length) - sql.IndexOf("from") - 4);
            string[] strList = subStr.Trim().Split(',');
            if (strList.Length < 1)
            {
                return false;
            }
            else if (strList[0].Trim() == "")
            {
                return false;
            }
            else
            {
                foreach (string tab in strList)
                {
                    XMLTable table = dataPool.Find(name =>
                    {
                        if (name.tableName == tab.Trim())
                            return true;
                        else
                            return false;
                    });
                    if (table == null)
                    {
                        Debug.LogWarning(tab.Trim() + "表不存在");
                        return false;
                    }
                }
            }
            //检查字段
            string subStr2 = sql.Substring(6, sql.IndexOf("from") - 6);
            string[] strList2 = subStr2.Trim().Split(',');
            foreach (string field in strList2)
            {
                if (GetFieldFromTable(field, strList) == null)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 标准化sql
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        string StandardizedSql(string queryString)
        {
            //string tableStr = queryString.Substring(queryString.IndexOf("from") + 4, (queryString.IndexOf("where") > 0 ? queryString.IndexOf("where") : queryString.Length) - queryString.IndexOf("from") - 4);
            //string[] tableStrList = tableStr.Trim().Split(',');
            //string headStr = queryString.Substring(6, queryString.IndexOf("from") - 6);
            //string[] headStrList = headStr.Trim().Split(',');
            //string whereStr = "";
            //if (queryString.IndexOf("where") > 0)
            //{
            //    whereStr = queryString.Substring(queryString.IndexOf("where") + 5, (queryString.IndexOf(";") > 0 ? queryString.IndexOf(";") : queryString.Length) - queryString.IndexOf("where") - 5).Trim();
            //    whereStr = System.Text.RegularExpressions.Regex.Replace(whereStr, @"\b\s+\b", " ");
            //}
            //string[] whereStrList = whereStr.Trim().Split(' ');
            //foreach (var item in headStrList)
            //{
            //    XMLTable table = GetFieldFromTable(item, tableStrList);
            //    if(table != null)
            //    {

            //    }
            //}
            return queryString;
        }
        /// <summary>
        /// 字段是否在表中
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="tables">表名称列表</param>
        /// <returns>表对象</returns>
        XMLTable GetFieldFromTable(string field,string [] tables)
        {
            XMLTable findtable = null;
            string subTable = "";
            string subField = "";
            string[] fieldt = field.Trim().Split('.');
            if (tables.Length > 1)
            {
                subTable = fieldt[0].Trim();
                subField = fieldt[1].Trim();
            }
            else
                subField = fieldt[0].Trim();
            if (subTable == "")
            {
                findtable = dataPool.Find(name =>
                {
                    //检查名字存在 且字段有效
                    foreach (string item in tables)
                    {
                        if (name.tableName == item.Trim())
                        {
                            foreach (var itemt in name.head)
                            {
                                if(itemt.field == subField || subField =="*")
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                });
            }
            else
            {
                findtable = dataPool.Find(name =>
                {
                    //检查名字存在 且字段有效

                    if (name.tableName == subTable && name.head.Find(subname =>
                    {
                        if (subname.field == subField)
                            return true;
                        else
                            return false;
                    }) != null)
                        return true;
                    else
                        return false;

                });
            }
            if (findtable == null)
            {
                //Debug.LogWarning(field.Trim() + "字段不存在");
            }
            else
                findtable = findtable.Clone();
            return findtable;
        }
        /// <summary>
        /// 表数据过滤条件
        /// </summary>
        /// <param name="where"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        XMLTable TableWhere(string where, XMLTable table)
        {
            XMLTable relust = table.Clone();
            relust.data.Clear();
            bool add = true;
            #region 分割条件
            while (FindMark(where) >= 0)
            {
                XMLTable tmp = TableWhere(where.Substring(0, FindMark(where)), table);
                if (add)
                {
                    //并集
                    foreach (var item in tmp.data)
                    {
                        if (relust.data.IndexOf(item) < 0)
                        {
                            relust.data.Add(item);
                        }
                    }
                }
                else
                {
                    //交集
                    for (int i = relust.data.Count - 1; i >= 0; i--)
                    {
                        if (tmp.data.IndexOf(relust.data[i]) < 0)
                        {
                            relust.data.RemoveAt(i);
                        }
                    }
                }
                where = where.Substring(FindMark(where)).TrimStart();
                if (where.Substring(0, 2) == "or")
                {
                    add = true;
                    where = where.Substring(2).Trim();
                }
                else
                {
                    add = false;
                    where = where.Substring(3).Trim();
                }

            }
            #endregion
            where = where.Trim();
            //去括号
            if (where.Length > 2)
            {

                if (where[0] == '(' && where[where.Length - 1] == ')')
                {
                    #region 有括号处理
                    where = where.Substring(1, where.Length - 2).Trim();
                    while (FindMark(where) >= 0)
                    {
                        XMLTable tmp = TableWhere(where.Substring(0, FindMark(where)), table);
                        if (add)
                        {
                            //并集
                            foreach (var item in tmp.data)
                            {
                                if (relust.data.IndexOf(item) < 0)
                                {
                                    relust.data.Add(item);
                                }
                            }
                        }
                        else
                        {
                            //交集
                            for (int i = relust.data.Count - 1; i >= 0; i--)
                            {
                                if (tmp.data.IndexOf(relust.data[i]) < 0)
                                {
                                    relust.data.RemoveAt(i);
                                }
                            }
                        }
                        where = where.Substring(FindMark(where)).TrimStart();
                        if (where.Substring(0, 2) == "or")
                        {
                            add = true;
                            where = where.Substring(2).Trim();
                        }
                        else
                        {
                            add = false;
                            where = where.Substring(3).Trim();
                        }

                    }
                    #endregion
                }
                else //处理单条语句
                {
                    string[] list = new string[3] {"","",""};
                    if (where.IndexOf("=") > 0)
                    {
                        #region 等于条件处理
                        string[] listt = where.Split('=');
                        if(listt.Length == 2)
                        {
                            list[0] = listt[0].Trim();
                            list[1] = listt[1].Trim();
                            list[2] = "=";
                        }
                        #endregion
                    }
                    else if (where.IndexOf("<>") > 0)
                    {
                        #region 不等于条件处理
                        string[] listt = where.Split('<');
                        if (listt.Length == 2)
                        {
                            list[0] = listt[0].Trim();
                            list[1] = listt[1].Remove(0,1).Trim();
                            list[2] = "<>";
                        }
                        #endregion
                    }
                    else if (where.IndexOf(">") > 0)
                    {
                        #region 大于条件处理
                        string[] listt = where.Split('>');
                        if (listt.Length == 2)
                        {
                            list[0] = listt[0].Trim();
                            list[1] = listt[1].Trim();
                            list[2] = ">";
                        }
                        #endregion
                    }
                    else if (where.IndexOf("<") > 0)
                    {
                        #region 小于条件处理
                        string[] listt = where.Split('<');
                        if (listt.Length == 2)
                        {
                            list[0] = listt[0].Trim();
                            list[1] = listt[1].Trim();
                            list[2] = "<";
                        }
                        #endregion
                    }
                    if (list[0] != "" && list[1] != "" && list[2] != "")
                    {
                        string str1 = "";
                        string str2 = "";
                        XMLTable xmltable1 = GetFieldFromTable(list[0], table.tableName.Split(','));
                        XMLTable xmltable2 = GetFieldFromTable(list[1], table.tableName.Split(','));
                        if (xmltable1 == null)
                        {
                            str1 = list[0].Replace('\'', ' ').Trim();
                        }
                        else
                            str1 = xmltable1.tableName + '.' + list[0];
                        if (xmltable2 == null)
                        {
                            str2 = list[1].Replace('\'', ' ').Trim();
                        }
                        else
                            str2 = xmltable2.tableName + '.' + list[1];
                        #region 抽取记录
                        XMLTable tmpt = table.Clone();
                        tmpt.data.Clear();
                        foreach (var item in table.data)
                        {
                            string type = "varchar";
                            string _str1 = str1;
                            string _str2 = str2;
                            if (xmltable1 != null)
                            {
                                XMLTable.STHead head = table.head.Find(name =>
                                {
                                    //Debug.Log(name.field.Trim() + " == " + _str1.Trim() +" "+ (name.field.Trim() == _str1.Trim()));
                                    if (name.field.Trim() == _str1.Trim())
                                        return true;
                                    else
                                        return false;
                                });
                                int i = head.index;
                                type = head.type;
                                if(type.IndexOf('(') > 0)
                                {
                                    type = type.Substring(0, type.IndexOf('('));
                                }
                                _str1 = item[i];
                            }
                            if (xmltable2 != null)
                            {
                                XMLTable.STHead head = table.head.Find(name =>
                                {
                                    if (name.field == _str2)
                                        return true;
                                    else
                                        return false;
                                });
                                int i = head.index;
                                type = head.type;
                                if (type.IndexOf('(') > 0)
                                {
                                    type = type.Substring(0, type.IndexOf('('));
                                }
                                _str2 = item[i];
                            }
                            switch (list[2])
                            {
                                case "=":
                                    if (CompareEqual(_str1, _str2, type))
                                    {
                                        tmpt.data.Add(item);
                                    }
                                    break;
                                case "<>":
                                    if (!CompareEqual(_str1, _str2, type))
                                    {
                                        tmpt.data.Add(item);
                                    }
                                    break;
                                case ">":
                                    if (CompareGreater(_str1, _str2, type))
                                    {
                                        tmpt.data.Add(item);
                                    }
                                    break;
                                case "<":
                                    if (CompareGreater(_str2, _str1, type))
                                    {
                                        tmpt.data.Add(item);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            
                        }
                        #endregion
                        //添加到结果集
                        if (add)
                        {
                            //并集
                            foreach (var item in tmpt.data)
                            {
                                if (relust.data.IndexOf(item) < 0)
                                {
                                    relust.data.Add(item);
                                }
                            }
                        }
                        else
                        {
                            //交集
                            for (int i = relust.data.Count - 1; i >= 0; i--)
                            {
                                if (tmpt.data.IndexOf(relust.data[i]) < 0)
                                {
                                    relust.data.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //无条件
                relust.data = table.Clone().data;
            }
            relust.rowCount = relust.data.Count;
            return relust;
        }
        /// <summary>
        /// 标准化table
        /// </summary>
        XMLTable StandardizedTable(XMLTable table)
        {
            foreach (XMLTable.STHead item in table.head)
            {
                if(item.field.ToLower().IndexOf(table.tableName.ToLower())<0)
                {
                    item.field = table.tableName.Trim() + "." + item.field.Trim();
                }
            }
            return table;
        }
        int FindMark(string where_t)
        {
            int mark = 0;
            int startMark = 0;
            while (mark >= 0)
            {
                if (where_t.IndexOf(" and ", startMark) < 0)
                    mark = where_t.IndexOf(" or ", startMark);
                else if (where_t.IndexOf(" or ", startMark) < 0)
                    mark = where_t.IndexOf(" and ", startMark);
                else
                    mark = where_t.IndexOf(" and ", startMark) < where_t.IndexOf(" or ", startMark) ? where_t.IndexOf(" and ", startMark) : where_t.IndexOf(" or ", startMark);
                if (mark >= 0)
                {
                    int i1 = ComputeNewlineCount(where_t.Substring(0, mark + 1), "(");
                    int i2 = ComputeNewlineCount(where_t.Substring(0, mark + 1), ")");
                    if (i1 == i2)
                    {
                        break;
                    }
                    else
                    {
                        startMark = mark + 4;
                    }
                }
            }
            return mark;
        }
        /// <summary>
        /// 比较值str1=str2
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CompareEqual(string str1, string str2, string type)
        {
            bool bl = false;
            if (type.IndexOf('(') > 0)
            {
                type = type.Substring(0, type.IndexOf('('));
            }
            switch (type)
            {
                case "char":
                case "varchar":
                case "varchar2":
                    if (str1 == str2)
                    {
                        bl = true;
                    }
                    break;
                case "int":
                case "intarger":
                    try
                    {
                        if(int.Parse(str1) == int.Parse(str2))
                        {
                            bl = true;
                        }

                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    break;
                case "float":
                    try
                    {
                        if (float.Parse(str1) == float.Parse(str2))
                        {
                            bl = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    break;
                case "double":
                case "decimal":
                    try
                    {
                        if (double.Parse(str1) == double.Parse(str2))
                        {
                            bl = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    break;
                default:
                    break;
            }
            return bl;
        }
        /// <summary>
        /// 比较值str1>str2
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CompareGreater(string str1, string str2, string type)
        {
            bool bl = false;
            if (type.IndexOf('(') > 0)
            {
                type = type.Substring(0, type.IndexOf('('));
            }
            switch (type)
            {
                case "char":
                case "varchar":
                case "varchar2":
                    if (string.Compare( str1, str2) > 0)
                    {
                        bl = true;
                    }
                    break;
                case "int":
                case "intarger":
                    try
                    {
                        if (int.Parse(str1) > int.Parse(str2))
                        {
                            bl = true;
                        }

                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    break;
                case "float":
                    try
                    {
                        if (float.Parse(str1) > float.Parse(str2))
                        {
                            bl = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    break;
                case "double":
                case "decimal":
                    try
                    {
                        if (double.Parse(str1) > double.Parse(str2))
                        {
                            bl = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    break;
                default:
                    break;
            }
            return bl;
        }
        /// <summary>
        /// 根据查询字段 生成新表头
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="tableList"></param>
        /// <returns></returns>
        List<XMLTable.STHead> MergeHeader(string[] fieldList,string[] tableList)
        {
            List<XMLTable.STHead> headList = new List<XMLTable.STHead>();
            foreach (var itemone in fieldList)
            {
                string item = itemone.Trim();
                XMLTable tmp = GetFieldFromTable(item, tableList);
                if(tmp == null)
                {
                    Debug.LogWarning("查询字段不存在：" + item);
                    return null;
                }
                if(item.Split('.').Length == 2)
                {
                    #region 带表头的字段处理
                    if (item.Split('.')[1] == "*")
                    {
                        //全字段
                        foreach (var itemt in tmp.head)
                        {
                            itemt.index = headList.Count;
                            itemt.field = tmp.tableName + "." + itemt.field;
                            headList.Add(itemt);
                        }
                    }
                    else
                    {
                        //单字段
                        foreach (var itemt in tmp.head)
                        {
                            if (itemt.field == item.Split('.')[1])
                            {
                                itemt.index = headList.Count;
                                itemt.field = tmp.tableName + "." + itemt.field;
                                headList.Add(itemt);
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 不带表头的字段处理
                    if (item == "*")
                    {
                        //全表全字段
                        foreach (var tableitem in tableList)
                        {
                            XMLTable tabletmp = dataPool.Find(name => {
                                if (name.tableName == tableitem)
                                    return true;
                                else
                                    return false;
                            }).Clone();
                            if(tabletmp == null)
                            {
                                Debug.LogWarning("表不存在：" + tabletmp);
                                return null;
                            }
                            foreach (var itemt in tabletmp.head)
                            {
                                itemt.index = headList.Count;
                                itemt.field = tabletmp.tableName + "." + itemt.field;
                                headList.Add(itemt);
                            }
                        }
                        
                    }
                    else
                    {
                        //单字段
                        foreach (var itemt in tmp.head)
                        {
                            if (itemt.field == item)
                            {
                                itemt.index = headList.Count;
                                itemt.field = tmp.tableName + "." + itemt.field;
                                headList.Add(itemt);
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            return headList;
        }

        string GetFullField(string field, string[] tableList)
        {
            XMLTable tmp = GetFieldFromTable(field, tableList);
            if (tmp == null)
            {
                Debug.LogWarning("查询字段不存在：" + field);
                return null;
            }
            //head = tmp.head
            if (field.Split('.').Length == 2)
            {
                return field;
            }
            else
                return tmp.tableName + "." + field;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="table"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        XMLTable BubbleSort(string[] fieldList, XMLTable table,bool asc)
        {
            string[] tmp = new string[table.colCount];
            for (int i = 0; i < table.rowCount; i++)
                for (int j = i + 1;j< table.rowCount;j++)
                {
                    foreach (var item in fieldList)
                    {
                        XMLTable.STHead head = GetFieldHead(item, table);
                        if (CompareEqual(table.data[j][head.index], table.data[i][head.index], head.type))
                        {
                            //如果当前值相等，则比较下一个值
                            continue;
                        }
                        else if( CompareGreater(table.data[j][head.index], table.data[i][head.index], head.type))
                        {
                            //这条数据比上一条大
                            if(asc)
                            {
                                //顺序，不处理
                            }
                            else
                            {
                                //倒叙，需要交换位置
                                tmp = table.data[j];
                                table.data[j] = table.data[i];
                                table.data[i] = tmp;
                            }
                        }
                        else
                        {
                            //这条数据比上一条小
                            if (asc)
                            {
                                //顺序，需要交换位置
                                tmp = table.data[j];
                                table.data[j] = table.data[i];
                                table.data[i] = tmp;
                            }
                            else
                            {
                                //倒叙，不处理
                            }
                        }
                        break;
                    }
                }
            return table;
        }
        /// <summary>
        /// 取得表头
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        XMLTable.STHead GetFieldHead(string field, XMLTable table)
        {
            return table.head.Find(name =>
            {
                if (name.field == field)
                    return true;
                else
                    return false;
            });
        }
        /// <summary>
        /// 计算子串的数量
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substr">子串</param>
        /// <returns>数量</returns>
        private int ComputeNewlineCount(string str, string substr)
        {
            int cnt = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if ((i + substr.Length) < str.Length)
                {
                    string tmpstr = str.Substring(i, substr.Length);
                    if (tmpstr == substr) cnt++;
                }
            }
            return cnt;
        }
    }
    internal class XMLTable
    {
        public XMLTable()
        {
            head = new List<STHead>();
            data = new List<string[]>();
            rowCount = 0;
            colCount = 0;
            tableName = "";
        }
        public XMLTable Clone()
        {
            XMLTable tem = new XMLTable();
            tem.tableName = this.tableName;
            tem.rowCount = this.rowCount;
            tem.colCount = this.colCount;
            tem.head = new List<STHead>();
            foreach (var item in this.head)
            {
                tem.head.Add(item.Clone());
            }
            tem.data = new List<string[]>();
            foreach (var item in this.data)
            {
                tem.data.Add((string[])item.Clone());
            }
            return tem;
        }
        public string tableName;
        /// <summary>
        /// 行总数
        /// </summary>
        public int rowCount;
        /// <summary>
        /// 列总数
        /// </summary>
        public int colCount;
        public List<STHead> head;
        public List<string[]> data;
        public class STHead
        {
            public STHead()
            {
                index = -1;
            }
            public STHead Clone()
            {
                STHead tem = new STHead();
                tem.index = this.index;
                tem.field = this.field;
                tem.type = this.type;
                tem.desc = this.desc;
                return tem;
            }
            public int index;
            public string field;
            public string type;
            public string desc;
        }
    }
    /// <summary>
    /// 查询结果集
    /// </summary>
    public class XMLDataReader
    {
        internal XMLTable table;
        int readerIndex;
        internal XMLDataReader()
        {
            readerIndex = -1;
            table = new XMLTable();
        }
        /// <summary>
        /// 取得行总数
        /// </summary>
        /// <returns></returns>
        public int GetRowCount()
        {
            return table.rowCount;
        }
        /// <summary>
        /// 取得列总数
        /// </summary>
        /// <returns></returns>
        public int GetColCount()
        {
            return table.colCount;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns>true 有数据 false 无数据</returns>
        public bool Read()
        {
            readerIndex++;
            if (readerIndex < table.rowCount)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 取得当前读取器的索引对应值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetString(int index)
        {
            if (index < table.colCount && index >= 0)
                return table.data[readerIndex][index];
            else
            {
                Debug.LogWarning("索引超出范围");
                return null;
            }
        }
        public int GetInt(int index)
        {
            if (index < table.colCount && index >= 0 && table.head[index].type.Substring(0, 3).ToLower() == "int")
            {
                try
                {
                    return int.Parse(table.data[readerIndex][index]);
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("转换错误");
                    return -8899;
                }

            }
            else
            {
                Debug.LogWarning("索引超出范围");
                return -8899;
            }
        }
        public float GetFloat(int index)
        {
            if (index < table.colCount && index >= 0 && table.head[index].type.Substring(0, 5).ToLower() == "float")
            {
                try
                {
                    return float.Parse(table.data[readerIndex][index]);
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("转换错误");
                    return -8899;
                }

            }
            else
            {
                Debug.LogWarning("索引超出范围");
                return -8899;
            }
        }
        public double GetDouble(int index)
        {
            if (index < table.colCount && index >= 0 && table.head[index].type.Substring(0, 6).ToLower() == "double")
            {
                try
                {
                    return double.Parse(table.data[readerIndex][index]);
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("转换错误");
                    return -8899;
                }

            }
            else
            {
                Debug.LogWarning("索引超出范围");
                return -8899;
            }
        }
        /// <summary>
        /// 根据字段取得索引
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public int GetOrdinal(string field)
        {
            field = field.ToLower();
            string tableName = "";
            string fieldName = "";
            string[] fieldList = field.Trim().Split('.');
            if (fieldList.Length > 1)
            {
                tableName = fieldList[0];
                fieldName = fieldList[1];
            }
            else
                fieldName = field.Trim();
            XMLTable.STHead find = table.head.Find(name =>
            {
                string[] subFieldList = name.field.Trim().Split('.');
                if (subFieldList[1] == fieldName && tableName == "")
                {
                    return true;
                }
                else if (subFieldList[1] == fieldName && tableName == subFieldList[0])
                {
                    return true;
                }
                else
                    return false;
            });
            if (find != null)
                return find.index;
            else
                return -1;
        }
    }
}
