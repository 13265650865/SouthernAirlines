// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application;
public class WebScarperRes
{
    /// <summary>
    /// 
    /// </summary>
    public string _id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> startUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<SelectorsItem> selectors { get; set; }
}
public class SelectorsItem
{
    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> parentSelectors { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string selector { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool multiple { get; set; }
}



public class Data
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
}

public class Response
{
    /// <summary>
    /// 
    /// </summary>
    public string success { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Data data { get; set; }
}

public class JobDto
{

    /// <summary>
    /// 
    /// </summary>
    public int sitemap_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string driver { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int page_load_delay { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int request_interval { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int proxy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string custom_id { get; set; }

}

public class tss
{


    /// <summary>
    /// 单人房B 禁烟
    /// </summary>
    public string type { get; set; }
    /// <summary>
    /// 1张1.1米单人床
    /// </summary>
    public string bed { get; set; }
    /// <summary>
    /// 有窗
    /// </summary>
    public string wind { get; set; }
    /// <summary>
    /// 禁烟
    /// </summary>
    public string smoking { get; set; }
    /// <summary>
    /// 无早餐
    /// </summary>
    public string fas { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pri { get; set; }

    public string quxiao { get; set; }

    public string zaocan { get; set; }

}
