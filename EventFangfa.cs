//1.公用的
//声明事件参数类 class xxxEventArgs{}
//声明委托 ：delegate void xxxEventHandler(obj，args)-一般带两个参数
//一个是object,一个特色的类-class xxxEventArgs{}
//2.在一个类中
//定义事件：public event 类型 名称
//发生事件：事件名（参数）
//3.在别的类中
//定义一个方法：void 方法名（obj,args）
//注册事件： xxx.事件 +=new 委托（方法名）
///实例-----下载时发送的事件
using system;
#region  公共的
//声明委托
public delegate void DownloadStartHandler(object sender, DownloadStartEventArgs e);  //声明委托
public delegate void DownloadEndHandler(object sender, DownloadEndEventArgs e);  
public delegate void DownloadingHandler(object sender, DownloadingEventArgs e);  

///开始下载类
public class DownloadStartEentArgs{
    //url属性
    public string Url{get{return _url;}set{_byteCount=value;}}
    private string url;
    //构造函数
    public DownloadStartEentArgs(string url){this._url = url;}
}
///<summary>结束下载类</summary>
public class DownloadEndEentArgs
{
    //URL属性
    public string Url{get{return _url;set{_url = value;}}}
    private string _url;
    //ByteCount属性
    public long ByteCount{get{return _byteCount;}set{ _byteCount=value;}}
    private long _byteCount;
    //构造函数
    public DownloadEndEentArgs(string url,long size){
        this._url = url;this._byteCount = size;
    }
}
///正在下载类
public class DownloadingEventArgs{
    //url属性
    public string Url{get{return _ulr;}set{_ulr=value}}
    private string _ulr;
    //百分比属性
    public double Percent{get{return _percent;}set{_percent = value;}}
    private double _percent;
    //构造函数
    public DownloadingEventArgs(string url,doub percent){
            this._url = url; this._percent = percent;
    }
}
#endregion
//发布器（publisher） 类-定义事件和发生事件
public class Crawler{
    //声明事件-public event 类型 名称（public event 委托类型 名称）
    public event DownloadStartHandler DownloadStart;  // 声明事件
    public event DownloadEndHandler DownloadEnd;  // 声明事件
    public event DownloadingHandler Downloading;  // 声明事件
    //Name 属性
    public string Name{get{return name;}set{name=value;}}
    public string name;

    private string site;
    //构造函数
    public Crawler(string name,string site){
        this.name=name;
        this.site = site;
    }
    //方法-有发生事件内容
    public void Craw(){
        while (true)
        {
            string url = GetNextUrl();
            if(url==null)break;
            long size = GetSizeOfUrl(url);//网址大小
            //下载开始的事件发生
            if(DownloadStart!=null){
                DownloadStart(this,new DownloadStartEentArgs(url))//事件名（参数-构造函数）
            }
            for(long i =0;i<size+1024;i+=1024){
                //下载数据
                System.Threading.Thread.Sleep( 100 );
                double percent = (int)(i*100.0/size);
                if( percent>100) percent=100;
                if( Downloading != null ) //下载数据的事件发生
                {
                    Downloading( this, new DownloadingEventArgs(url, percent));
                }
            }
            //下载结束
            if(DownloadEnd!=null){
                DownloadEnd(this,new DownloadEndEventArgs(url,size))
            }
        }
    }
     //方法-获取位置
     private string GetNextUrl()
    {
        int a = rnd.Next(10);
        if( a == 0 ) return null;
        return site + "/Page" + a + ".htm";
    }
    //方法-获取网址大小
    private long GetSizeOfUrl( string url)
    {
        return rnd.Next(3000 * url.Length);
    }
    //随机数
    private Random rnd = new Random();
}
//订阅器（subscriber）- 是一个接受事件并提供事件处理程序的对象
public class Test{
    static void Main(){
        Crawler crawler = new Crawler("Crawer101", "https://www.pku.edu.cn");//调用Crawler构造函数
        //注册事件 --xxx.事件 +=new 委托（方法名）
        crawler.DownloadStart += new DownloadStartHandler( ShowStart ); //注册事件
        crawler.DownloadEnd += new DownloadEndHandler( ShowEnd );
        crawler.Downloading += new DownloadingHandler( ShowPercent );
        crawler.Craw();
    }

    static void ShowStart(object sender, DownloadStartEventArgs e){
        Console.WriteLine( (sender as Crawler).Name + "开始下载" + e.Url );
    }

    static void ShowEnd(object sender, DownloadEndEventArgs e){
        Console.WriteLine( "\n\r下载" + e.Url + "结束,其下载" + e.ByteCount + "字节" );
    }

    static void ShowPercent(object sender, DownloadingEventArgs e){
        Console.Write( "\r下载" + e.Url + "......." + e.Percent + "%" );
    }
}
