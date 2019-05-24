using System;

namespace lig.Data
{

        public class PageOption
        {


            public PageOption()
            {
                Total = 0;
                Page = 1;
                PageSize = 15;
                Sort = new Sort();
            }

            /// <summary>
            /// 当前页
            /// </summary>
            public int Page { get; set; }


            /// <summary>
            /// 每页数据量
            /// </summary>
            public int PageSize { get; set; }


            /// <summary>
            /// 总条数
            /// </summary>
            public int Total { get; set; }

            /// <summary>
            /// 排序字段
            /// </summary>
            public Sort Sort { get; set; }
        }


        public class Sort
        {
            public string Key { get; set; }

            public string Value { get; set; }

        }

   
}



