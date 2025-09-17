using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Entity;

[SugarTable("traveloke_cache", "traveloke_cache")]
public class Traveloke_cache : EntityBaseId
{
    public string hotid { get; set; }
    public string status { get; set; }
    public string rid { get; set; }
    public string rname { get; set; }
    public string maxOccupancy { get; set; }
    public DateTime? createTime { get; set; }
}
