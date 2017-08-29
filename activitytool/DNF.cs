using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace activitytool
{
	public class smsg
	{
		public string msg { get; set; }
		public int ret { get; set; }
		public string time { get; set; }

	}
	public class DNFRoleInfo : smsg
	{
		public List<DNFRole> data { get; set; }

	}
	public class DNFRole
	{
		public string nick { get; set; }
		public string role_id { get; set; }
	}

	public class amsResponse : smsg
	{
		public int actid { get; set; }
		public data data { get; set; }
		public int rettype { get; set; }
	}
	public class data
	{
		public act act { get; set; }
		public string actname { get; set; }
		public string game { get; set; }
		public hook hook { get; set; }
		public join join { get; set; }
		public object op { get; set; }
		public rule rule { get; set; }

	}
	public class act
	{
		public int start_time { get; set; }
		public int end_time { get; set; }
		public string numlimit_num { get; set; }
		public int numlimit_per_num { get; set; }
		public string numlimit_step { get; set; }
		public long numlimit_totalnum { get; set; }
		public string op { get; set; }
		public int qqlimit_num { get; set; }
		public string qqlimit_step { get; set; }
		public int qqlimit_totalnum { get; set; }
		public string tlimit { get; set; }

	}

	public class hook
	{
		public recordGift recordGift { get; set; }
	}
	public class recordGift
	{
		public int actid { get; set; }
		public string info { get; set; }
		public string is_life_coupon { get; set; }
		public int level { get; set; }
		public string name { get; set; }
		public int qqvipCardId { get; set; }
		public int status { get; set; }
		public int time { get; set; }
		public int type { get; set; }

	}
	public class join
	{
		public int diamonds { get; set; }
		public string info { get; set; }
		public int level { get; set; }
		public int time { get; set; }

	}

	public class op
	{
		public string cdkavailtime { get; set; }
		public string cdkey { get; set; }
		public string mid { get; set; }
		public string type { get; set; }

	}
	public class rule
	{
		public string rolexval { get; set; }

	}
	public class DNFHelper
	{
		public class ServerSelect
		{
			public string t { get; set; }
			public string v { get; set; }
			public List<ServerSelect> opt_data_array { get; set; }
		}

		public static List<ServerSelect> GETServerSelect()
		{
			var Server = new List<ServerSelect>
				{
						new ServerSelect{t="广东", v="21", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "广东一区",v= "1",opt_data_array=null}
						,
							new ServerSelect{t= "广东二区",v= "15",opt_data_array=null}
						,
							new ServerSelect{t= "广东三区",v= "22",opt_data_array=null}
						,
							new ServerSelect{t= "广东四区",v= "45",opt_data_array=null}
						,
							new ServerSelect{t= "广东五区",v= "52",opt_data_array=null}
						,
							new ServerSelect{t= "广东六区",v= "65",opt_data_array=null}
						,
							new ServerSelect{t= "广东七区",v= "71",opt_data_array=null}
						,
							new ServerSelect{t= "广东八区",v= "81",opt_data_array=null}
						,
							new ServerSelect{t= "广东九区",v= "89",opt_data_array=null}
						,
							new ServerSelect{t= "广东十区",v= "98",opt_data_array=null}
						,
							new ServerSelect{t= "广东十一区",v= "105",opt_data_array=null}
						,
							new ServerSelect{t= "广东十二区",v= "126",opt_data_array=null}
						,
							new ServerSelect{t= "广东十三区",v= "134",opt_data_array=null}

					}}
					,
						new ServerSelect{t="福建", v="33", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "福建一区",v= "14",opt_data_array=null}
						,
							new ServerSelect{t= "福建二区",v= "44",opt_data_array=null}
						,
							new ServerSelect{t= "福建3/4区",v= "80",opt_data_array=null}

					}}
					,
						new ServerSelect{t="浙江", v="30", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "浙江一区",v= "11",opt_data_array=null}
						,
							new ServerSelect{t= "浙江二区",v= "21",opt_data_array=null}
						,
							new ServerSelect{t= "浙江三区",v= "55",opt_data_array=null}
						,
							new ServerSelect{t= "浙江4/5区",v= "84",opt_data_array=null}
						,
							new ServerSelect{t= "浙江六区",v= "116",opt_data_array=null}
						,
							new ServerSelect{t= "浙江七区",v= "129",opt_data_array=null}

					}}
					,
						new ServerSelect{t="北京", v="22", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "北京一区",v= "2",opt_data_array=null}
						,
							new ServerSelect{t= "北京2/4区",v= "35",opt_data_array=null}
						,
							new ServerSelect{t= "北京三区",v= "72",opt_data_array=null}

					}}
					,
						new ServerSelect{t="上海", v="23", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "上海一区",v= "3",opt_data_array=null}
						,
							new ServerSelect{t= "上海二区",v= "16",opt_data_array=null}
						,
							new ServerSelect{t= "上海三区",v= "36",opt_data_array=null}
						,
							new ServerSelect{t= "上海4/5区",v= "93",opt_data_array=null}

					}}
					,
						new ServerSelect{t="四川", v="24", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "四川一区",v= "4",opt_data_array=null}
						,
							new ServerSelect{t= "四川二区",v= "26",opt_data_array=null}
						,
							new ServerSelect{t= "四川三区",v= "56",opt_data_array=null}
						,
							new ServerSelect{t= "四川四区",v= "70",opt_data_array=null}
						,
							new ServerSelect{t= "四川五区",v= "82",opt_data_array=null}
						,
							new ServerSelect{t= "四川六区",v= "107",opt_data_array=null}

					}}
					,
						new ServerSelect{t="湖南", v="25", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "湖南一区",v= "5",opt_data_array=null}
						,
							new ServerSelect{t= "湖南二区",v= "25",opt_data_array=null}
						,
							new ServerSelect{t= "湖南三区",v= "50",opt_data_array=null}
						,
							new ServerSelect{t= "湖南四区",v= "66",opt_data_array=null}
						,
							new ServerSelect{t= "湖南五区",v= "74",opt_data_array=null}
						,
							new ServerSelect{t= "湖南六区",v= "85",opt_data_array=null}
						,
							new ServerSelect{t= "湖南七区",v= "117",opt_data_array=null}

					}}
					,
						new ServerSelect{t="山东", v="26", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "山东一区",v= "6",opt_data_array=null}
						,
							new ServerSelect{t= "山东2/7区",v= "37",opt_data_array=null}
						,
							new ServerSelect{t= "山东三区",v= "59",opt_data_array=null}
						,
							new ServerSelect{t= "山东四区",v= "75",opt_data_array=null}
						,
							new ServerSelect{t= "山东五区",v= "78",opt_data_array=null}
						,
							new ServerSelect{t= "山东六区",v= "106",opt_data_array=null}

					}}
					,
						new ServerSelect{t="江苏", v="27", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "江苏一区",v= "7",opt_data_array=null}
						,
							new ServerSelect{t= "江苏二区",v= "20",opt_data_array=null}
						,
							new ServerSelect{t= "江苏三区",v= "41",opt_data_array=null}
						,
							new ServerSelect{t= "江苏四区",v= "53",opt_data_array=null}
						,
							new ServerSelect{t= "江苏5/7区",v= "79",opt_data_array=null}
						,
							new ServerSelect{t= "江苏六区",v= "90",opt_data_array=null}
						,
							new ServerSelect{t= "江苏八区",v= "109",opt_data_array=null}

					}}
					,
						new ServerSelect{t="湖北", v="28", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "湖北一区",v= "9",opt_data_array=null}
						,
							new ServerSelect{t= "湖北二区",v= "24",opt_data_array=null}
						,
							new ServerSelect{t= "湖北三区",v= "48",opt_data_array=null}
						,
							new ServerSelect{t= "湖北四区",v= "68",opt_data_array=null}
						,
							new ServerSelect{t= "湖北五区",v= "76",opt_data_array=null}
						,
							new ServerSelect{t= "湖北六区",v= "94",opt_data_array=null}
						,
							new ServerSelect{t= "湖北七区",v= "115",opt_data_array=null}
						,
							new ServerSelect{t= "湖北八区",v= "127",opt_data_array=null}

					}}
					,
						new ServerSelect{t="华北", v="29", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "华北一区",v= "10",opt_data_array=null}
						,
							new ServerSelect{t= "华北二区",v= "19",opt_data_array=null}
						,
							new ServerSelect{t= "华北三区",v= "54",opt_data_array=null}
						,
							new ServerSelect{t= "华北四区",v= "87",opt_data_array=null}

					}}
					,
						new ServerSelect{t="西北", v="31", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "西北一区",v= "12",opt_data_array=null}
						,
							new ServerSelect{t= "西北2/3区",v= "46",opt_data_array=null}

					}}
					,
						new ServerSelect{t="东北", v="32", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "东北一区",v= "13",opt_data_array=null}
						,
							new ServerSelect{t= "东北二区",v= "18",opt_data_array=null}
						,
							new ServerSelect{t= "东北3/7区",v= "23",opt_data_array=null}
						,
							new ServerSelect{t= "东北4/5/6区",v= "83",opt_data_array=null}

					}}
					,
						new ServerSelect{t="西南", v="34", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "西南一区",v= "17",opt_data_array=null}
						,
							new ServerSelect{t= "西南二区",v= "49",opt_data_array=null}
						,
							new ServerSelect{t= "西南三区",v= "92",opt_data_array=null}

					}}
					,
						new ServerSelect{t="河南", v="35", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "河南一区",v= "27",opt_data_array=null}
						,
							new ServerSelect{t= "河南二区",v= "43",opt_data_array=null}
						,
							new ServerSelect{t= "河南三区",v= "57",opt_data_array=null}
						,
							new ServerSelect{t= "河南四区",v= "69",opt_data_array=null}
						,
							new ServerSelect{t= "河南五区",v= "77",opt_data_array=null}
						,
							new ServerSelect{t= "河南六区",v= "103",opt_data_array=null}
						,
							new ServerSelect{t= "河南七区",v= "135",opt_data_array=null}

					}}
					,
						new ServerSelect{t="广西", v="36", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "广西一区",v= "28",opt_data_array=null}
						,
							new ServerSelect{t= "广西2/4区",v= "64",opt_data_array=null}
						,
							new ServerSelect{t= "广西三区",v= "88",opt_data_array=null}
						,
							new ServerSelect{t= "广西五区",v= "133",opt_data_array=null}

					}}
					,
						new ServerSelect{t="江西", v="37", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "江西一区",v= "29",opt_data_array=null}
						,
							new ServerSelect{t= "江西二区",v= "62",opt_data_array=null}
						,
							new ServerSelect{t= "江西三区",v= "96",opt_data_array=null}

					}}
					,
						new ServerSelect{t="安徽", v="38", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "安徽一区",v= "30",opt_data_array=null}
						,
							new ServerSelect{t= "安徽二区",v= "58",opt_data_array=null}
						,
							new ServerSelect{t= "安徽三区",v= "104",opt_data_array=null}

					}}
					,
						new ServerSelect{t="辽宁", v="39", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "辽宁一区",v= "31",opt_data_array=null}
						,
							new ServerSelect{t= "辽宁二区",v= "47",opt_data_array=null}
						,
							new ServerSelect{t= "辽宁三区",v= "61",opt_data_array=null}

					}}
					,
						new ServerSelect{t="山西", v="40", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "山西一区",v= "32",opt_data_array=null}
						,
							new ServerSelect{t= "山西二区",v= "95",opt_data_array=null}

					}}
					,
						new ServerSelect{t="陕西", v="41", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "陕西一区",v= "33",opt_data_array=null}
						,
							new ServerSelect{t= "陕西2/3区",v= "63",opt_data_array=null}

					}}
					,
						new ServerSelect{t="广州", v="42", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "广州1/2区",v= "34",opt_data_array=null}

					}}
					,
						new ServerSelect{t="河北", v="43", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "河北一区",v= "38",opt_data_array=null}
						,
							new ServerSelect{t= "河北2/3区",v= "67",opt_data_array=null}
						,
							new ServerSelect{t= "河北四区",v= "118",opt_data_array=null}
						,
							new ServerSelect{t= "河北五区",v= "132",opt_data_array=null}

					}}
					,
						new ServerSelect{t="重庆", v="44", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "重庆一区",v= "39",opt_data_array=null}
						,
							new ServerSelect{t= "重庆二区",v= "73",opt_data_array=null}

					}}
					,
						new ServerSelect{t="黑龙江", v="45", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "黑龙江一区",v= "40",opt_data_array=null}
						,
							new ServerSelect{t= "黑龙江2/3区",v= "51",opt_data_array=null}

					}}
					,
						new ServerSelect{t="吉林", v="46", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "吉林1/2区",v= "42",opt_data_array=null}

					}}
					,
						new ServerSelect{t="云贵", v="277", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "云南一区",v= "120",opt_data_array=null}
						,
							new ServerSelect{t= "贵州一区",v= "122",opt_data_array=null}
						,
							new ServerSelect{t= "云贵一区",v= "124",opt_data_array=null}

					}}
					,
						new ServerSelect{t="天津", v="278", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "天津一区",v= "121",opt_data_array=null}

					}}
					,
						new ServerSelect{t="新疆", v="281", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "新疆一区",v= "123",opt_data_array=null}

					}}
					,
						new ServerSelect{t="内蒙", v="538", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "内蒙古一区",v= "125",opt_data_array=null}

					}}
					,
						new ServerSelect{t="体验服", v="7016761", opt_data_array=new List<ServerSelect>{

							new ServerSelect{t= "体验一区",v= "99",opt_data_array=null}
						,
							new ServerSelect{t= "体验二区",v= "199",opt_data_array=null}

					}}
				};


			return Server;
		}
		public Dictionary<string, Dictionary<string, string>> GetTask_list_ids()
		{
			var task_list_ids = new Dictionary<string, Dictionary<string, string>>
			{
				{"1",
					new Dictionary<string, string> 
					{
						{"280445","282029"},
						{"280648","282042"},
						{"280649","282043"},
						{"280499","282045"},
						{"280664","282046"},
						{"280666","282048"},
						{"280463","282099"},
						{"280660","282101"},
						{"280663","282104"},
						{"280505","282110"},
						{"280670","282111"}, 
						{"280671","282112"},
						{"280631","282115"},
						{"280706","282117"},
						{"280721","282118"},
						{"319571","319572"},
						{"319591","319605"},
						{"319617","319619"},
						{"319629","319646"},
						{"319630","319647"},
						{"319631","319648"},
						{"327341","327342"}

					}},
				{"2",
					new Dictionary<string, string> 
					{
						{"280445","282344"},
						{"280648","282346"},
						{"280649","282348"},
						{"280499","282360"},
						{"280664","282355"},
						{"280666","282356"},
						{"280463","282354"},
						{"280660","282357"},
						{"280663","282359"},
						{"280505","282361"},
						{"280670","282362"},
						{"280671","282363"},
						{"280631","282364"},
						//280638","282367"},
						{"280706","282369"},
						{"280721","282372"},
						//281054","282373"},//通关地下城四次
						{"319571","319578"},
						{"319591","319611"},
						{"319617","319621"},
						{"319629","319722"},
						{"319630","319724"},
						{"319631","319726"},
						{"327341","327343"}
					}
				}
			
			};
			return task_list_ids;

		}
	}

}
