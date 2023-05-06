// See https://aka.ms/new-console-template for more information
using BoilerEventAppl;
//Console.WriteLine("Hello, World!");
//用于热水锅炉系统故障排除的应用程序。当维修工程师检查锅炉时，锅炉的温度和压力会随着维修工程师的备注自动记录到日志文件中。
//实例化对象-记录到日志文件类

BoilerInfoLogger filelog = new BoilerInfoLogger("e:\\boiler.txt");
//实例化对象-事件发布器类
DelegateBoilerEvent boilerEvent = new DelegateBoilerEvent();
//注册事件----委托执行某一方法
//事件在生成时 会调用委托
boilerEvent.BoilerEventLog += new DelegateBoilerEvent.BoilerLogHandler(Logger);
boilerEvent.BoilerEventLog += new DelegateBoilerEvent.BoilerLogHandler(filelog.Logger);
//调用方法
boilerEvent.LogProcess();
Console.ReadLine();
filelog.Close();
static void Logger(string info) {
    Console.WriteLine(info);
}