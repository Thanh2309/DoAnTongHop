using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu_Hutech.Models
{
    public class ThongKe_DK
    {
        private long sl_sv5t_khoa;
        private long sl_sv5t_truong;
        private long sl_sv5t_thanh_tw;
        private long sl_detai_nckh;
        private long sl_dk_lhtt;
        private long tong_dk_sv5t;

        public long Sl_sv5t_khoa
        {
            get
            {
                return sl_sv5t_khoa;
            }

            set
            {
                sl_sv5t_khoa = value;
            }
        }

        public long Sl_sv5t_truong
        {
            get
            {
                return sl_sv5t_truong;
            }

            set
            {
                sl_sv5t_truong = value;
            }
        }

        public long Sl_sv5t_thanh_tw
        {
            get
            {
                return sl_sv5t_thanh_tw;
            }

            set
            {
                sl_sv5t_thanh_tw = value;
            }
        }

        public long Sl_detai_nckh
        {
            get
            {
                return sl_detai_nckh;
            }

            set
            {
                sl_detai_nckh = value;
            }
        }

        public long Sl_dk_lhtt
        {
            get
            {
                return sl_dk_lhtt;
            }

            set
            {
                sl_dk_lhtt = value;
            }
        }

        public long Tong_dk_sv5t
        {
            get
            {
                return tong_dk_sv5t;
            }

            set
            {
                tong_dk_sv5t = value;
            }
        }
    }
}