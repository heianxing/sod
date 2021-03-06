﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 数据参数序列化类
    /// </summary>
    public class DbParameterSerialize
    {
        public static string Serialize(IDataParameter[] paras)
        {
            List<MyDbParameter> list = new List<MyDbParameter>();
            foreach (IDataParameter para in paras)
            {
                IDbDataParameter para1 = (IDbDataParameter)para;
                MyDbParameter item = new MyDbParameter();
                item.Name = para1.ParameterName;
                item.Length = para1.Size;
                item.Value = para1.Value;
                item.ParaDbType = para1.DbType;
                list.Add(item);
            }

            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            XmlSerializer xs = new XmlSerializer(typeof(List<MyDbParameter>));
            xs.Serialize(xw, list);
            string strEntity = sb.ToString();
            return strEntity;
        }

        public static IDataParameter[] DeSerialize(string input,AdoHelper db)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<MyDbParameter>));
            var desObj = xs.Deserialize(new System.IO.StringReader(input));
            List<MyDbParameter> list = (List<MyDbParameter>)desObj;
            List<IDataParameter> result = new List<IDataParameter>();
            foreach (MyDbParameter item in list)
            {
                IDbDataParameter para = (IDbDataParameter)db.GetParameter(item.Name, item.ParaDbType);
                para.Value = item.Value;
                para.Size = item.Length;
                result.Add(para);
            }
            return result.ToArray();
        }
    }

    public class MyDbParameter
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public object Value { get; set; }
        public DbType ParaDbType { get; set; }
    }


}
