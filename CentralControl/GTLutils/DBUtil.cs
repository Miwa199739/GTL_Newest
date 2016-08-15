using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace CentralControl
{
    public class DBUtil
    {
        //private static String ConnectionString = "data source = LAB229\\SQLEXPRESS;initial catalog = gtltest; user id = gtltest;password = jiaoda";
        private static String ConnectionString = ConfigurationSettings.AppSettings["connectionstring"];
        //private static List<string> columnname;
        public static Dictionary<string, string> englishtochinese, chinesetoenglish;
        public static Dictionary<string, int> cancompare;
        private static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        public DBUtil()
        {
            translateinit();

        }

        protected void translateinit()
        {
            englishtochinese = new Dictionary<string, string>();
            chinesetoenglish = new Dictionary<string, string>();
            cancompare = new Dictionary<string, int>();
            string[] englishname ={ " * ",
            "CurrentTime","Device_Id","HAC_Inbarcode","HAC_Outbarcode",
            "HAC_State","HAC_Motor_Text_Speed","HAC_Motor_Elecspeed","HAC_Motor_Power",
            "HAC_Addr","HAC_Lumin","HAC_X","HAC_Y","HAC_Pwm",
            "HAC_OD","HAC_Barcode",
            "HAC_TH_Temperature1","HAC_TH_Temperature2","HAC_TH_Temperature3","HAC_TH_Humidity1","HAC_TH_Humidity2",
            "LPS_Source","LPS_Target","LPS_Quantity",
            "LPS_Include","LPS_Exclude",
            "LPS_Setting",
            "LPS_Includespeed","LPS_Excludespeed",
            "MB_Barcode","MB_Volume",
            "MB_current1","MB_current2","MB_current3","MB_current4","MB_state",
            "MMR_Serial","MMR_Temperature","MMR_PH","MMR_oxygen",
            "MSR_X","MSR_Y","MSR_OD",
            "MSR_Chemlight",
            "MSR_Lumin",
            "Onlinestate","Setupstring",
            "OP_current1","OP_current2","OP_current3","OP_current4","OP_state",
            "OP_Barcode","OP_Volume",
            "Operate","Direction",
            "HAC_BARCODE","HAC_ENGINE","HAC_LUMIN","HAC_OD","HAC_STATE","HAC_TMPMOD","LPS_LIQUID","LPS_PLACE", 
            "LPS_SETTING","LPS_SPEEDSETTING","MB_VOL","MB","MMR_INFO","MSR_OD","MSR_CHEMLIGHT","MSR_LUMIN",
            "ONLINEDEVICE","OP","OP_VOL","OPERATELOG"};
            int[] cancomparearr ={0,
            2,0,0,0,
            0,1,1,1,
            0,1,
            0,1,1,1,0,
            1,1,1,1,1,
            0,0,1,
            1,1,
            0,
            1,1,
            0,1,
            1,1,1,1,0,
            1,1,1,1,
            1,1,1,
            1,
            1,
            0,0,
            1,1,1,1,0,
            0,1,
            0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            string[] chinesename = { "所有属性",
            "数据插入时间","仪器标识","输入条码","输出条码",
            "设备状态","摇床转速","电机转速","电机功率",
            "选择匀光地址","光强",
            "od值测量模块状态","x轴坐标位置","y轴坐标位置","od值","条码",
            "温度计1参数","温度计2参数","温度计3参数","湿度计1参数","湿度计2参数",
            "液体移动源位置","液体移动目标位置","液体移动体积",
            "输入液体量","输出液体量",
            "设置字符串",
            "输入液体速度","输出液体速度",
            "条码 ","液体容量",
            "电流表1数值","电流表2数值","电流表3数值","电流表4数值","设备状态 ",
            "序列信息","温度","ph值","含氧量",
            "x坐标","y坐标","od值",
            "化学光检测值",
            "荧光检测值",
            "设备在线状态","状态字",
            "电流表1数值 ","电流表2数值 ","电流表3数值 ","电流表4数值","仪器工作状态",
            "条码  ","液体容量 ",
            "插入数据库的字符串","信息方向",
            "阵列式高通量培养装置条形码表","阵列式高通量培养装置引擎表","阵列式高通量培养装置照明表","阵列式高通量培养装置OD值表","阵列式高通量培养装置状态表",
            "阵列式高通量培养装置温湿度表","液体工作站操作表","液体工作站操作表2","液体工作站设置表","液体工作站速度设置表","全自动培养基封装仪容量表",
            "全自动培养基封装仪","非接触式多参数微反应检测仪信息表","多通道高速代谢分析OD值表","多通道高速代谢分析化学光检测表","多通道高速代谢分析荧光检测表",
            "仪器在线离线信息表","全自动培养基平皿表","全自动培养基平皿容量表","数据库操作日志表"};
            
            
            for (int i = 0; i < englishname.Length; i++)
            {
                englishtochinese[englishname[i]] = chinesename[i];
                chinesetoenglish[chinesename[i]] = englishname[i];
                cancompare[chinesename[i]] = cancomparearr[i];
                //Console.WriteLine(englishname[i] + " " + chinesename[i]);
            }

        }

        public static ArrayList getTableList()
        {
            ArrayList list = new ArrayList();
            SqlConnection conn = getConnection();
            try
            {
                conn.Open();
                DataTable tables = conn.GetSchema("Tables");
                foreach (DataRow row in tables.Rows)
                {
                    list.Add(row[2].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return list;
        }

        public static ArrayList getTableColumns(String tableName)
        {
            ArrayList list = new ArrayList();
            SqlConnection conn = getConnection();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select Name FROM SysColumns Where id=Object_Id('" + tableName + "')", conn);
                SqlDataReader objReader = cmd.ExecuteReader();
                while (objReader.Read())
                {
                    list.Add(objReader[0].ToString());

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return list;
        }

        public static List<List<string>> executeQueryCmd(string cmdStr)
        {
            SqlConnection conn = getConnection();
            List<List<string>> list = new List<List<string>>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                List<string> columnname = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columnname.Add(reader.GetName(i));
                }
                list.Add(columnname);
                while (reader.Read())
                {
                    List<string> col = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        col.Add(reader[i].ToString());
                    }
                    list.Add(col);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return list;
        }

        public static int executedNonQueryCmd(string cmdStr)
        {
            SqlConnection conn = getConnection();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return -1;
        }
    }
}
