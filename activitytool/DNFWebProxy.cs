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
        public List<string> loadRole(string area)
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
                Value_Dictionary["{u2rolename}"] = System.Web.HttpUtility.UrlEncode(Value_Dictionary["{u1rolename}"]);

                return true;
            }
            catch
            {
                return false;
            }

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
                    AddText(Analysis(t,result));
                });

        }

        private string Analysis(node obj, string rl)
        {
            StringBuilder result = new StringBuilder();
            _MJson m = new _MJson(rl);
            try
            {
                if (obj.GetNode("model").toString() == "2")
                {
                    if (m.GetNode("modRet") == null) result.Append("\r\n" + DateTime.Now.ToString() + "领取失败:" + m.GetNode("sMsg").toString());
                    else result.Append("\r\n" + DateTime.Now.ToString() + ":" + m.GetNode("modRet").GetNode("sMsg").toString());

                }
                else
                {
                    if (m.GetNode("msg").toString() == "success")
                    {
                        if (m.GetNode("op").toString() == "online_cdk")
                            result.Append("\r\n======CDK=======\r\n" + m.GetNode("cdkey").toString() + "\r\n================");
                        result.Append("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】成功");

                    }
                    else
                        result.Append("\r\n" + DateTime.Now.ToString() + ",领取【" + m.GetNode("actname").toString() + "】失败，原因：" + m.GetNode("msg").toString());
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
                .Replace("{actid}", act.GetNode("actid").toString())
                .Replace("{flowid}", act.GetNode("flowid").toString());
            string postdata = act.GetNode("subDate").toString()
                .Replace("{g_tk}", Value_Dictionary["{gtk}"])
                .Replace("{area}", Value_Dictionary["{area}"])
                .Replace("{roleid}", Value_Dictionary["{roleid}"])
                .Replace("{ametk}", Value_Dictionary["{ametk}"])
                .Replace("{actid}", act.GetNode("actid").toString())
                .Replace("{flowid}", act.GetNode("flowid").toString());

            if (act.GetNode("model").toString() == "2")
            {
                string ext1 = act.GetNode("Ext1").toString();
                if (!sSDIDList.ContainsKey(ext1))
                {
                    string ams_actdesc = web.SendDataByGET(ext1, "", ref myCookieContainer, act.GetNode("Ext2").toString(), act.GetNode("Ext1").toString());
                    _MJson m = new _MJson(ams_actdesc);
                    sSDIDList[ext1] = m.GetNode("sSDID").toString();

                }
                gifurl = gifurl.Replace("{checkparam}", Value_Dictionary["{u2checkparam}"]).Replace("{md5str}", Value_Dictionary["{md5str}"]).Replace("{ametk}", Value_Dictionary["{ametk}"]).Replace("{sSDID}", sSDIDList[ext1]);
                postdata = postdata.Replace("{checkparam}", Value_Dictionary["{u2checkparam}"]).Replace("{md5str}", Value_Dictionary["{md5str}"]).Replace("{ametk}", Value_Dictionary["{ametk}"]).Replace("{sSDID}", sSDIDList[ext1]);
            }
            string au = "";
            if (act.GetNode("model").toString() == "3")
            {
                au = act.GetNode("Ext3").toString();
                gifurl = gifurl.Replace("{ametk}", Value_Dictionary["{ametk}"]);
                postdata = postdata.Replace("{ametk}", Value_Dictionary["{ametk}"]);

            }

            string result = "";
            if (act.GetNode("subMethod").toString().ToLower() == "post")
                result = web.SendDataByPost(gifurl, postdata, ref myCookieContainer, act.GetNode("Host").toString(), act.GetNode("Referer").toString(), au);
            else
                result = web.SendDataByGET(gifurl, postdata, ref myCookieContainer, act.GetNode("Host").toString(), act.GetNode("Referer").toString(), au);
            return result;
        }

    }
}
