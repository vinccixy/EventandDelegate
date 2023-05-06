using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerEventAppl {
    // boiler 类
    internal class Boiler {
        private int temp;
        private int pressure;
        public Boiler(int t,int p) {
            this.temp = t;
            this.pressure = p;
        }
        public int GetTemp() {
            return temp;
        }
        public int GetPressure() {
            return pressure;
        }
    }
    /// <summary>
    /// 事件发布器
    /// </summary>
    class DelegateBoilerEvent {
        /// <summary>
        /// 声明一个委托
        /// </summary>
        /// <param name="status"></param>
        public delegate void BoilerLogHandler(string status);
        /// <summary>
        /// 基于上面的委托定义事件
        /// </summary>
        public event BoilerLogHandler BoilerEventLog;

        /// <summary>
        /// log处理方法
        /// </summary>
        public void LogProcess() {
            string remarks = "o.k";
            Boiler b = new Boiler(100, 12);
            int t = b.GetTemp();
            int p = b.GetPressure();
            if (t > 150 || t < 80 || p < 12 || p > 15) {
                remarks = "need Maintenace";
            }
            OnBoilerEventLog("Logging Info:\n");
            OnBoilerEventLog("Temparature " + t + "\nPressure: " + p);
            OnBoilerEventLog("\nMessage: " + remarks);
        }
        //方法-存放事件的触发
        protected void OnBoilerEventLog(string message) {
            if (BoilerEventLog != null) {
                BoilerEventLog(message);
            }
        }
    }
    /// <summary>
    /// 该类保留写入日志文件的条款
    /// </summary>
    class BoilerInfoLogger {
        FileStream fs;
        StreamWriter sw;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filename"></param>
        public BoilerInfoLogger(string filename) {
            fs = new FileStream(filename, FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(fs);
        }

        /// <summary>
        /// 写入log
        /// </summary>
        /// <param name="info"></param>
        public void Logger(string info) {
            sw.WriteLine(info);
        }

        /// <summary>
        /// 关闭log方法
        /// </summary>
        public void Close() {
            sw.Close();
            fs.Close();
        }
    }

    /// <summary>
    /// 事件订阅器
    /// </summary>
    public class RecordBoilerInfo {
        static void Logger(string info) {
            Console.WriteLine(info);
        }//end of logger
        static void Main(string[] args) {
            //实例化对象-记录到日志文件类
            BoilerInfoLogger filelog = new BoilerInfoLogger("e:\\boiler.txt");
            //实例化对象-事件发布器类
            DelegateBoilerEvent  boilerEvent = new DelegateBoilerEvent();
            //注册事件----委托执行某一方法
            //事件在生成时 会调用委托
            boilerEvent.BoilerEventLog += new DelegateBoilerEvent.BoilerLogHandler(Logger);
            boilerEvent.BoilerEventLog += new DelegateBoilerEvent.BoilerLogHandler(filelog.Logger);
            //调用方法
            boilerEvent.LogProcess();
            Console.ReadLine();
            filelog.Close();
        }//end of main

    }//end if RecordBolierInfo
}
