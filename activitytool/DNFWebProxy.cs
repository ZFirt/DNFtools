using MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace activitytool
{
    public class DNFWebProxy : QQWebProxy
    {
        public List<node> actnodeList = null;
        public List<activitytool.DNFHelper.ServerSelect> svlist { get { return svlist_value; } }
        List<activitytool.DNFHelper.ServerSelect> svlist_value = null;
        List<string> roleInfo = null;
        Dictionary<string, string> sSDIDList = new Dictionary<string, string>();
        Listnode tasklist = null;
        public DNFWebProxy(CookieContainer cookie)
            : base(cookie)
        {
            svlist_value = DNFHelper.GETServerSelect();
        }
        public DNFWebProxy(string cookie)
            : base(cookie)
        {
            svlist_value = DNFHelper.GETServerSelect();
        }
        public bool SetActList(Listnode tmp)
        {
            try
            {
                actnodeList = loadListnode(tmp);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private List<node> loadListnode(Listnode tmp)
        {
            List<node> tmplist = new List<node>();
            if (tmp == null)
                return tmplist;
            foreach (var v in tmp.val)
            {
                tmplist.Add(v);
                if (v.GetNode("atcExt").toString() != "null")
                {
                    tmplist.AddRange(loadListnode(v.GetNode("atcExt").toListnode()));
                }
            }
            return tmplist;
        }
        public List<string> loadRole(string area, string areaname)
        {
            string query_role_result = SendDataByGET("http://comm.aci.game.qq.com/main?game=dnf&area=" + area + "&sCloudApiName=ams.gameattr.role&iAmsActivityId=http%3A%2F%2Fdnf.qq.com%2Fgift.shtml&sServiceDepartment=group_3"
                , "", ref myCookieContainer, "comm.aci.game.qq.com", "http://dnf.qq.com/gift.shtml");
            string roledata = Regex.Match(query_role_result, "(?<=data:').*?(?=')").Value;
            roleInfo = roledata.Split('|').ToList().FindAll(t => t.IndexOf(" ") > -1).Distinct().ToList();

            Value_Dictionary["{md5str}"] = Regex.Match(query_role_result, "(?<=md5str:').*?(?=')").Value;
            Value_Dictionary["{checkparam}"] = Regex.Match(query_role_result, "(?<=checkparam:').*?(?=')").Value;
            Value_Dictionary["{u1checkparam}"] = System.Web.HttpUtility.UrlEncode(Value_Dictionary["{checkparam}"]);
            Value_Dictionary["{u2checkparam}"] = System.Web.HttpUtility.UrlEncode(Value_Dictionary["{u1checkparam}"]);
            Value_Dictionary["{area}"] = area;
            Value_Dictionary["{areaname}"] = areaname;
            Value_Dictionary["{u1areaname}"] = System.Web.HttpUtility.UrlEncode(Value_Dictionary["{areaname}"]).ToUpper();
            Value_Dictionary["{u2areaname}"] = System.Web.HttpUtility.UrlEncode(Value_Dictionary["{u1areaname}"]).ToUpper();
            Value_Dictionary["{roleid}"] = "";
            return roleInfo.Select(t => System.Web.HttpUtility.UrlDecode(t.Split(' ')[1])).ToList();

        }
        public bool SetRoleId(int index)
        {
            try
            {
                string[] tmprole = roleInfo[index].Split(' ');
                Value_Dictionary["{roleid}"] = tmprole[0];
                Value_Dictionary["{rolename}"] = System.Web.HttpUtility.UrlDecode(tmprole[1]);
                Value_Dictionary["{u1rolename}"] = tmprole[1];
                Value_Dictionary["{u2rolename}"] = System.Web.HttpUtility.UrlEncode(Value_Dictionary["{u1rolename}"]).ToUpper();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool ValueVerify()
        {
            return Value_Dictionary.Keys.Contains("{roleid}") && Value_Dictionary["{roleid}"] != "";

        }
        public void SubmitAct(List<int> indexlist, Action<string> AddText)
        {
            string result = "";
            foreach (var tmp in indexlist)
            {
                result = GetGift(actnodeList[tmp]);
                AddText(Analysis(actnodeList[tmp], result));
            }

        }
        public void OneSubmitAct(Action<string> AddText)
        {
            string result = "";
            List<node> tmplist = actnodeList.FindAll(t => t.GetNode("autoSub").toString() == "1");
            tmplist.ForEach(
                t =>
                {
                    result = GetGift(t);
                    AddText(Analysis(t, result));
                });

        }

        private string Analysis(node obj, string rl)
        {
            StringBuilder result = new StringBuilder();
            _MJson m = new _MJson(rl);
            try
            {

                switch (obj.GetNode("model").toString())
                {
                    case "1":
                        {
                            if (m.GetNode("msg").toString() == "success")
                            {
                                if (m.GetNode("op").toString() == "online_cdk")
                                    result.Append("\r\n======CDK=======\r\n" + m.GetNode("cdkey").toString() + "\r\n================");
                                result.Append("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】成功");
                            }
                            else
                            {
                                result.Append("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】失败，原因：" + m.GetNode("msg").toString());
                            }
                        }
                        break;
                    case "2":
                        {
                            if (m.GetNode("modRet") == null)
                            { 
                                result.Append("\r\n" + DateTime.Now.ToString() + "领取失败:" + m.GetNode("sMsg").toString());
                            }
                            else 
                            {
                                result.Append("\r\n" + DateTime.Now.ToString() + ":" + m.GetNode("modRet").GetNode("sMsg").toString());
                            }
                        }
                        break;
                    case "3":
                        {
                            if (m.GetNode("modRet") == null)
                            { 
                                result.Append("\r\n" + DateTime.Now.ToString() + "领取失败:" + m.GetNode("sMsg").toString());
                            }
                            else 
                            {
                                result.Append("\r\n" + DateTime.Now.ToString() + ":" + m.GetNode("modRet").GetNode("sMsg").toString());
                            }
                        }
                        break;
                    case "4":
                        {
                            if (m.GetNode("code").toString() != "0")
                            {
                                result.Append("\r\n" + DateTime.Now.ToString() + "领取失败:" + m.GetNode("msg").toString());
                            }
                            else
                            {
                                result.Append("\r\n" + DateTime.Now.ToString() + ": 成功,领取到" + m.GetNode("gift_name").toString());
                            }
                        }
                        break;
                }


                if (obj.GetNode("model").toString() == "2")
                {
                    

                }
                else
                {

                }
            }
            catch
            {
                result.Append("\r\n" + DateTime.Now.ToString() + ",出现未知错误！！！");
            }

            return result.ToString();
        }
        private string GetGift(node act)
        {
            string gifurl = act.GetNode("subURL").toString()
                .Replace("{g_tk}", Value_Dictionary["{gtk}"])
                .Replace("{area}", Value_Dictionary["{area}"])
                .Replace("{roleid}", Value_Dictionary["{roleid}"])
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{skey}", Value_Dictionary["{skey}"])
                .Replace("{actid}", act.GetNode("actid").toString())
                .Replace("{flowid}", act.GetNode("flowid").toString());
            string postdata = act.GetNode("subDate").toString()
                .Replace("{g_tk}", Value_Dictionary["{gtk}"])
                .Replace("{area}", Value_Dictionary["{area}"])
                .Replace("{roleid}", Value_Dictionary["{roleid}"])
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{skey}", Value_Dictionary["{skey}"])
                .Replace("{actid}", act.GetNode("actid").toString())
                .Replace("{flowid}", act.GetNode("flowid").toString());
            string au = "";
            string Host = act.GetNode("Host").toString();
            string Referer = act.GetNode("Referer").toString();
            string addcookies = "";
            switch (act.GetNode("model").toString())
            {
                case "1": { } break;
                case "2":
                    {
                        string ext1 = act.GetNode("Ext1").toString();
                        if (!sSDIDList.ContainsKey(ext1))
                        {
                            string ams_actdesc = SendDataByGET(ext1, "", ref myCookieContainer, act.GetNode("Ext2").toString(), act.GetNode("Ext1").toString());
                            _MJson m = new _MJson(ams_actdesc);
                            sSDIDList[ext1] = m.GetNode("sSDID").toString();

                        }
                        gifurl = gifurl.Replace("{checkparam}", Value_Dictionary["{u2checkparam}"]).Replace("{md5str}", Value_Dictionary["{md5str}"]).Replace("{ametk}", Value_Dictionary["{ametk}"]).Replace("{sSDID}", sSDIDList[ext1]);
                        postdata = postdata.Replace("{checkparam}", Value_Dictionary["{u2checkparam}"]).Replace("{md5str}", Value_Dictionary["{md5str}"]).Replace("{ametk}", Value_Dictionary["{ametk}"]).Replace("{sSDID}", sSDIDList[ext1]);

                    } break;
                case "3":
                    {
                        au = act.GetNode("Ext3").toString();
                    } break;
                case "4":
                    {

                        //addcookies =  System.Web.HttpUtility.UrlDecode(act.GetNode("Ext3").toString())
                        //            .Replace("{g_tk}", Value_Dictionary["{gtk}"])
                        //            .Replace("{area}", Value_Dictionary["{area}"])
                        //            .Replace("{roleid}", Value_Dictionary["{roleid}"])
                        //            .Replace("{ametk}", Value_Dictionary["{ametk}"])
                        //            .Replace("{checkparam}", Value_Dictionary["{checkparam}"])
                        //            .Replace("{u1rolename}", Value_Dictionary["{u1rolename}"])
                        //            .Replace("{u1areaname}", Value_Dictionary["{u1areaname}"])
                        //            .Replace("{skey}", Value_Dictionary["{skey}"])
                        //            .Replace("{actid}", act.GetNode("actid").toString())
                        //            .Replace("{flowid}", act.GetNode("flowid").toString());
                        gifurl = gifurl.Replace("{u1rolename}", Value_Dictionary["{u1rolename}"])
                                    .Replace("{QQ}", QQ)
                                    .Replace("{u1areaname}", Value_Dictionary["{u1areaname}"]);
                        ;
                    } break;
            }

            string result = "";
            if (act.GetNode("subMethod").toString().ToLower() == "post")
                result = SendDataByPost(gifurl, postdata, ref myCookieContainer, Host, Referer, au, addcookies);
            else
                result = SendDataByGET(gifurl, postdata, ref myCookieContainer, Host, Referer, au, addcookies);
            return result;
        }

        public string CDKexchange(string CDK, string Code)
        {
            try
            {
                string result = SendDataByPost(Properties.Resources.DNFCDKURL,
                    Properties.Resources.DNFCDKPost
                    .Replace("{CDK}", CDK)
                    .Replace("{Code}", Code)
                    .Replace("{QQ}", QQ)
                    .Replace("{g_tk}", Value_Dictionary["{gtk}"])
                    .Replace("{area}", Value_Dictionary["{area}"])
                    .Replace("{roleid}", Value_Dictionary["{roleid}"])
                    .Replace("{md5str}", Value_Dictionary["{md5str}"])
                    .Replace("{u2rolename}", Value_Dictionary["{u2rolename}"])
                    .Replace("{ametk}", Value_Dictionary["{ametk}"])
                    .Replace("{u2checkparam}", Value_Dictionary["{u2checkparam}"])
                    , ref myCookieContainer);
                _MJson m = new _MJson(result);
                node w = m.GetNode("modRet");
                w.Trim();
                return w.GetNode("sMsg").toString();
            }
            catch
            {
                return "未知错误！！！";

            }
        }
        public System.Drawing.Bitmap GetCodeBitmap()
        {
            return DowloadCheckImg("http://captcha.qq.com/getimage?aid=21000104", myCookieContainer);
        }

        //心悦专区================

        public void ryzcsSDID()
        {
            string ams_actdesc = SendDataByGET("http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js", "", ref myCookieContainer, "apps.game.qq.com", "http://xinyue.qq.com/act/pc/a20160623dnfryzc/index.shtml");
            _MJson m = new _MJson(ams_actdesc);
            sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"] = m.GetNode("sSDID").toString();
        }
        public string AmsSubmit(string url, string postdata)
        {
            return SendDataByPost(url, postdata, ref myCookieContainer, "apps.game.qq.com", "http://apps.game.qq.com/ams/postMessage.html");
        }
        public string XinyueGetint()
        {
            string iActivityId = "54842";
            string iFlowId = "280421";
            string posturl = Properties.Resources.ameURL
                .Replace("{sSDID}", sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"])
                .Replace("{actid}", iActivityId)
                ;
            string postdata = Properties.Resources.ryzcPostdata
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", iActivityId)
                .Replace("{flowid}", iFlowId);
            string rseult = AmsSubmit(posturl,postdata);
            return Regex.Match(rseult, "(?<=\"sOutValue2\":\").*?(?=\")").Value;
        }

        public string XinyueGetRYint()
        {
            string iActivityId = "54842";
            string iFlowId = "281012";
            string posturl = Properties.Resources.ameURL
                .Replace("{sSDID}", sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"])
                .Replace("{actid}", iActivityId)
                ;
            string postdata = Properties.Resources.ryzcPostdata
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", iActivityId)
                .Replace("{flowid}", iFlowId);
            string rseult = AmsSubmit(posturl, postdata);
            return Regex.Match(rseult, "(?<=\"sOutValue1\":\").*?(?=\")").Value;
        }
        public string XinyueGetRYtask()
        {
            string iActivityId = "54842";
            string iFlowId = "280926";
            string posturl = Properties.Resources.ameURL
                .Replace("{sSDID}", sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"])
                .Replace("{actid}", iActivityId)
                ;
            string postdata = Properties.Resources.ryzcPostdata
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", iActivityId)
                .Replace("{flowid}", iFlowId);
            string rseult = AmsSubmit(posturl, postdata);
            string data = Regex.Match(rseult, "(?<=\"data\":\").*?(?=\"})").Value.Replace("\\\"", "\"").Replace("\\\\", "\\");
            _MJson m = new _MJson(data);
            Listnode tasklist = m.toListnode();
            return data;
        }

        public Dictionary<string, int> XinyueGetRYProp()
        {
            string iActivityId = "54842";
            string iFlowId = "280741";
            string posturl = Properties.Resources.ameURL
                .Replace("{sSDID}", sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"])
                .Replace("{actid}", iActivityId)
                ;
            string postdata = Properties.Resources.ryzcPostdata
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", iActivityId)
                .Replace("{flowid}", iFlowId);
            string rseult = AmsSubmit(posturl, postdata);
            _MJson m = new _MJson(rseult);
            Dictionary<string, int> card = new Dictionary<string, int>();
            card["two_score"] = m.GetNode("sOutValue1").toInt() - m.GetNode("sOutValue5").toInt();
            card["free_do"] = m.GetNode("sOutValue3").toInt() - m.GetNode("sOutValue4").toInt();
            card["rd_do"] = m.GetNode("sOutValue6").toInt() - m.GetNode("sOutValue7").toInt();
            return card;
        }

        public Dictionary<string, string> XinyueGetRYBinding()
        {
            string iActivityId = "54842";
            string iFlowId = "280301";
            string posturl = Properties.Resources.ameURL
                .Replace("{sSDID}", sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"])
                .Replace("{actid}", iActivityId)
                ;
            string postdata = Properties.Resources.ryzcPostdata
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", iActivityId)
                .Replace("{flowid}", iFlowId);
            string rseult = AmsSubmit(posturl, postdata);
            _MJson m = new _MJson(rseult);
            Dictionary<string, string> binding = new Dictionary<string, string>();
            binding["roleId"] = m.GetNode("FroleId").toString();
            binding["roleName"] =System.Web.HttpUtility.UrlDecode(m.GetNode("FroleName").toString());
            binding["areaName"] = m.GetNode("FareaName").toString();

            return binding;
        }
        public string XinyueRYBinding()
        {
            string iActivityId = "54842";
            string iFlowId = "280302";
            string posturl = Properties.Resources.ameURL
                .Replace("{sSDID}", sSDIDList["http://apps.game.qq.com/comm-htdocs/js/ams/v0.2R02/act/49210/act.desc.js"])
                .Replace("{actid}", iActivityId)
                ;
            string postdata = Properties.Resources.ryzcbindPostdata
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", iActivityId)
                .Replace("{flowid}", iFlowId)
                .Replace("{area}", Value_Dictionary["{area}"])
                .Replace("{roleid}", Value_Dictionary["{roleid}"])
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{checkparam}", Value_Dictionary["{u2checkparam}"])
                .Replace("{u1areaname}", Value_Dictionary["{u1areaname}"])
                .Replace("{u1rolename}", Value_Dictionary["{u1rolename}"])
                .Replace("{md5str}", Value_Dictionary["{md5str}"]);
            string rseult = AmsSubmit(posturl, postdata);
            return Regex.Match(rseult, "(?<=\"ret\":\").*?(?=\")").Value;
        }


    }
}
