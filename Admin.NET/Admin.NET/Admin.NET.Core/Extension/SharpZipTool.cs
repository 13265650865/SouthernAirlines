// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Extension;
public sealed class SharpZipTool
{
    public static byte[] CompressGZip(byte[] rawData)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipOutputStream compressedzipStream = new GZipOutputStream(ms))
            {
                compressedzipStream.Write(rawData, 0, rawData.Length);
                compressedzipStream.Close();
                return ms.ToArray();
            }
        }
    }

    public static byte[] UnGZip(byte[] byteArray)
    {
        byte[] overarr = null;
        using (GZipInputStream gzi = new GZipInputStream(new MemoryStream(byteArray)))
        {
            using (MemoryStream re = new MemoryStream(50000))
            {
                int count;
                byte[] data = new byte[50000];
                while ((count = gzi.Read(data, 0, data.Length)) != 0)
                {
                    re.Write(data, 0, count);
                }
                overarr = re.ToArray();
            }
        }
        return overarr;
    }


    /// <summary>
    /// 压缩
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Compress(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        using (MemoryStream mZipStreamIn = new MemoryStream(Encoding.UTF8.GetBytes(text)))
        {
            using (MemoryStream mZipStreamOut = new MemoryStream())
            {
                BZip2.Compress(mZipStreamIn, mZipStreamOut, true, 5);
                return Convert.ToBase64String(mZipStreamOut.ToArray());
            }
        }

    }

    /// <summary>
    /// 解压缩
    /// </summary>
    /// <param name="zipText"></param>
    /// <returns></returns>
    public static string DeCompress(string zipText)
    {
        if (string.IsNullOrEmpty(zipText))
        {
            return string.Empty;
        }
        using (MemoryStream mZipStreamIn = new MemoryStream(Convert.FromBase64String(zipText)))
        {
            using (MemoryStream mZipStreamOut = new MemoryStream())
            {
                BZip2.Decompress(mZipStreamIn, mZipStreamOut, false);
                return Encoding.UTF8.GetString(mZipStreamOut.ToArray());
            }
        }
    }
}
