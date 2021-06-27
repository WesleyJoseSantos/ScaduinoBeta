using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCom.DataBase
{
    public static class Data
    {
        public static TagsDataBase TagsDataBase { get; set; }

        static Data()
        {
            TagsDataBase = new TagsDataBase();
        }
    }
}
