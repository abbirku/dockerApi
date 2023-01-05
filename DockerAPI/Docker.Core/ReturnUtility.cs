using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Docker.Core
{
    public class Result
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }

    public class ListResult : Result
    {
        public int Count { get; set; }
    }

    public class ListResult<T> : ListResult
    {
        public IEnumerable<T> Data { get; set; }
    }

    public class PageConfig
    {
        public PageConfig(int totalItems, int? CurrentPage, int pageSize = 20)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = CurrentPage != null ? (int)CurrentPage : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 12)
                {
                    startPage = endPage - 11;
                }
            }

            TotalItems = totalItems;
            this.CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public string Link { get; set; }
    }

    public class Pagination<T> : ListResult<T>
    {
        public PageConfig Pageconfig { get; set; }
    }
}
