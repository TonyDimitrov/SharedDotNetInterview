﻿namespace DotNetInterview.Web.ViewModels.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DotNetInterview.Common;

    public abstract class PaginationVM
    {
        public int PaginationLength { get; set; }

        public int PreviousPage { get; set; }

        public int CurrentPage { get; set; }

        public string PrevtDisable { get; set; }

        public string NextDisable { get; set; }

        public virtual IEnumerable<T> SetPagination<T>(IEnumerable<T> collection, int page)
        {
            var paginationSets = (int)Math.Ceiling((double)collection.Count() / GlobalConstants.ResultsPerPage);

            for (int i = GlobalConstants.PaginationLength; true; i += GlobalConstants.PaginationLength)
            {
                if (page <= i)
                {
                    if (paginationSets > i)
                    {
                        this.PreviousPage = i - GlobalConstants.PaginationLength;
                        this.PaginationLength = GlobalConstants.PaginationLength;
                        this.NextDisable = string.Empty;
                    }
                    else
                    {
                        this.PreviousPage = i - GlobalConstants.PaginationLength;
                        this.PaginationLength = paginationSets - this.PreviousPage;
                        this.NextDisable = GlobalConstants.DisableLink;
                    }

                    break;
                }
                else if (paginationSets < i)
                {
                    this.PreviousPage = i - GlobalConstants.PaginationLength;
                    this.PaginationLength = paginationSets - this.PreviousPage;
                    this.NextDisable = GlobalConstants.DisableLink;

                    break;
                }
            }

            this.CurrentPage = page;

            if (this.PreviousPage == 0)
            {
                this.PrevtDisable = GlobalConstants.DisableLink;
            }
            else
            {
                this.PrevtDisable = string.Empty;
            }

            var skipPages = (page * GlobalConstants.ResultsPerPage) - GlobalConstants.ResultsPerPage;

            return collection
                 .Skip(skipPages)
                 .Take(GlobalConstants.ResultsPerPage)
                 .ToList();
        }
    }
}
