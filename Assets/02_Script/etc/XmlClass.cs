using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;
using System.Xml;
using System.IO;

public class XmlClass {
    
    XmlDocument xmlFile;

    string fileName = "/xmlUserDATA.xml";

    public string FILENAME
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + fileName;
#else
            return Application.persistentDataPath + fileName;
#endif
        }
    }

    public void _loadxml()
    {
        TextAsset textAsset = Resources.Load(fileName) as TextAsset;
        xmlFile.LoadXml(textAsset.text);
    }

    public XmlClass()
    {
        xmlFile = new XmlDocument();
    }

    public void XmlSave(Dictionary<string, UserInfo> dic)
    {
        xmlFile = new XmlDocument();
                
        xmlFile.AppendChild(xmlFile.CreateXmlDeclaration("1.0", "utf-8", "yes"));
        
        XmlNode rootNode = xmlFile.CreateNode(XmlNodeType.Element, "Root", string.Empty);
        xmlFile.AppendChild(rootNode);

        foreach (KeyValuePair<string, UserInfo> Users in dic)
        {
            XmlNode character = xmlFile.CreateNode(XmlNodeType.Element, "Character", string.Empty);
            rootNode.AppendChild(character);

            XmlNode id = xmlFile.CreateElement("ID");
            id.InnerText = Users.Value.ID;
            character.AppendChild(id);

            XmlNode pw = xmlFile.CreateElement("PW");
            pw.InnerText = Users.Value.PW;
            character.AppendChild(pw);
            
            XmlNode money = xmlFile.CreateElement("MONEY");
            money.InnerText = Users.Value.MONEY.ToString();
            character.AppendChild(money);

            XmlNode debt = xmlFile.CreateElement("DEBT");
            debt.InnerText = Users.Value.DEBT.ToString();
            character.AppendChild(debt);

            XmlNode date = xmlFile.CreateElement("DATE");
            date.InnerText = Users.Value.DATE.ToString();
            character.AppendChild(date);

            XmlNode whatday = xmlFile.CreateElement("WHATDAY");
            whatday.InnerText = Users.Value.WHATDAY.ToString();
            character.AppendChild(whatday);

            XmlNode probity = xmlFile.CreateElement("PROBITY");
            probity.InnerText = Users.Value.PROBITY.ToString();
            character.AppendChild(probity);

            XmlNode coinbox = xmlFile.CreateElement("COINBOX"); 
            character.AppendChild(coinbox);

            for (int i = 0; i < Users.Value.COIN.Length; i++)
            {
                string n = "COIN" + (i+1);
                XmlNode coin = xmlFile.CreateElement(n);
                coin.InnerText = Users.Value.COIN[i].ToString();
                coinbox.AppendChild(coin);
            }

            XmlNode coinprice = xmlFile.CreateElement("COINPRICE");
            character.AppendChild(coinprice);

            for (int i = 0; i < Users.Value.COINPRICE.Length; i++)
            {
                string n = "COINP" + (i + 1);
                XmlNode coinp = xmlFile.CreateElement(n);
                coinprice.AppendChild(coinp);
                for (int j = 0; j < Users.Value.COINPRICE[i].Count; j++)
                {
                    string n1 = "RECORD_COINP" + (j + 1);
                    XmlNode Record_coinp = xmlFile.CreateElement(n1);
                    Record_coinp.InnerText = Users.Value.COINPRICE[i][j].ToString();
                    coinp.AppendChild(Record_coinp);
                }
            }

            XmlNode itembox = xmlFile.CreateElement("ITEMBOX");
            character.AppendChild(itembox);

            for (int i = 0; i < Users.Value.ITEM.Length; i++)
            {
                string n = "ITEM" + (i + 1);
                XmlNode item = xmlFile.CreateElement(n);
                item.InnerText = Users.Value.ITEM[i].ToString();
                itembox.AppendChild(item);
            }

            XmlNode coininfo = xmlFile.CreateElement("COININFO");
            character.AppendChild(coininfo);

            XmlNode coinday = xmlFile.CreateElement("COINDAY");
            coinday.InnerText = Users.Value.COINDAY.ToString();
            coininfo.AppendChild(coinday);

            XmlNode cd_num = xmlFile.CreateElement("CD_NUM");
            cd_num.InnerText = Users.Value.CD_NUM.ToString();
            coininfo.AppendChild(cd_num);

            XmlNode cd_count = xmlFile.CreateElement("CD_COUNT");
            cd_count.InnerText = Users.Value.CD_COUNT.ToString();
            coininfo.AppendChild(cd_count);

            XmlNode bossinfo = xmlFile.CreateElement("BOSSINFO");
            character.AppendChild(bossinfo);

            XmlNode bossday = xmlFile.CreateElement("BOSSDAY");
            bossday.InnerText = Users.Value.BOSSDAY.ToString();
            bossinfo.AppendChild(bossday);

            XmlNode bd_num = xmlFile.CreateElement("BD_NUM");
            bd_num.InnerText = Users.Value.BD_NUM.ToString();
            bossinfo.AppendChild(bd_num);

            XmlNode soundVolum = xmlFile.CreateElement("SOUNDVOLUM");
            soundVolum.InnerText = Users.Value.SOUNDVOLUM.ToString();
            character.AppendChild(soundVolum);
        }

#if UNITY_EDITOR
        xmlFile.Save(Application.dataPath + fileName);
#else
        xmlFile.Save(Application.persistentDataPath + fileName);
#endif
    }

    public Dictionary<string, UserInfo> XmlLoad()
    {     
        try
        {
            xmlFile = new XmlDocument();
#if UNITY_EDITOR
            xmlFile.Load(Application.dataPath + fileName);
#else
            xmlFile.Load(Application.persistentDataPath + fileName);
#endif
            Dictionary<string, UserInfo> outputData = new Dictionary<string, UserInfo>();

            XmlNodeList list = xmlFile.SelectNodes("/Root/Character");

            foreach (XmlNode node in list)
            {
                UserInfo user = new UserInfo();

                user.ID = node["ID"].InnerText;
                user.PW = node["PW"].InnerText;
                user.MONEY = int.Parse(node["MONEY"].InnerText);
                user.DEBT = int.Parse(node["DEBT"].InnerText);
                user.DATE = float.Parse(node["DATE"].InnerText);
                user.WHATDAY = int.Parse(node["WHATDAY"].InnerText);
                user.PROBITY = float.Parse(node["PROBITY"].InnerText);

                XmlNode coinboxChild = node["COINBOX"].FirstChild;
                int i = 0;
                while (node["COINBOX"].ChildNodes.Count > i)
                {
                    user.COIN[i] = int.Parse(coinboxChild.InnerText);
                    coinboxChild = coinboxChild.NextSibling;
                    i++;
                }

                XmlNode coinPriceChild = node["COINPRICE"].FirstChild;
                i = 0;
                while (node["COINPRICE"].ChildNodes.Count > i) 
                {
                    XmlNode RecordcoinPriceChild = coinPriceChild.FirstChild;
                    int j = 0;
                    while (coinPriceChild.ChildNodes.Count > j)
                    {
                        user.COINPRICE[i][j] = float.Parse(RecordcoinPriceChild.InnerText);
                        RecordcoinPriceChild = RecordcoinPriceChild.NextSibling;
                        j++;
                    }
                    coinPriceChild = coinPriceChild.NextSibling;
                    i++;
                }

                XmlNode itemChild = node["ITEMBOX"].FirstChild;
                i = 0;
                while (node["ITEMBOX"].ChildNodes.Count > i)
                {
                    user.ITEM[i] = int.Parse(itemChild.InnerText);
                    itemChild = itemChild.NextSibling;
                    i++;
                }
                
                XmlNode cd_info = node["COININFO"].FirstChild;

                user.COINDAY = int.Parse(cd_info.InnerText);
                cd_info = cd_info.NextSibling;
                user.CD_NUM = int.Parse(cd_info.InnerText);
                cd_info = cd_info.NextSibling;
                user.CD_COUNT = int.Parse(cd_info.InnerText);

                XmlNode bd_info = node["BOSSINFO"].FirstChild;
                user.BOSSDAY = int.Parse(bd_info.InnerText);
                bd_info = bd_info.NextSibling;
                user.BD_NUM = int.Parse(bd_info.InnerText);

                user.SOUNDVOLUM = float.Parse(node["SOUNDVOLUM"].InnerText);

                outputData.Add(user.ID, user);
            }
            return outputData;
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("xml 로드 에러");
            return null;
        }
    }
}
