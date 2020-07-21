﻿using System;
using System.Collections.Generic;

namespace msgraph_angular_dotnetcore.Models
{
    public class ApplicationResponseModel
    {
        public ApplicationResponseModel(IEnumerable<ApplicationModel> list, long total)
        {
            List = list ?? throw new ArgumentNullException(nameof(list));
            Total = total;
        }

        public long Total { get; }

        public IEnumerable<ApplicationModel> List { get; }

        public IEnumerator<ApplicationModel> GetEnumerator()
        {
            return List.GetEnumerator();
        }
    }
}