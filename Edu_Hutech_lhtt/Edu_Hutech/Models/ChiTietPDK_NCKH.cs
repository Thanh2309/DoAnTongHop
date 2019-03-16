using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu_Hutech.Models
{
    public class ChiTietPDK_NCKH : DS_SV_NCKH_theoMaPDKResult
    {
        public string SP_DeTai { get; set; }

        public string tenGV_1 { get; set; }
        public string tenGV_2 { get; set; }

        public string mssv_sv1 { get; set; }
        public string hoten_sv1 { get; set; }
        public string malop_sv1 { get; set; }
        public string tenkhoa { get; set; }

        public bool sv2 { get; set; }
        public string mssv_sv2 { get; set; }
        public string hoten_sv2 { get; set; }
        public string malop_sv2 { get; set; }

        public bool sv3 { get; set; }
        public string mssv_sv3 { get; set; }
        public string hoten_sv3 { get; set; }
        public string malop_sv3 { get; set; }

        public bool sv4 { get; set; }
        public string mssv_sv4 { get; set; }
        public string hoten_sv4 { get; set; }
        public string malop_sv4 { get; set; }

        public bool sv5 { get; set; }
        public string mssv_sv5 { get; set; }
        public string hoten_sv5 { get; set; }
        public string malop_sv5 { get; set; }
    }
}