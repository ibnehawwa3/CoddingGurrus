﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Interface
{
    public interface IBaseHandler
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string apiEndpoint);
        Task<TResponse> GetAsync<TResponse>(string apiEndpoint);
        Task<TResponse> DeleteAsync<TResponse>(string apiEndpoint);
        Task<TResponse> GetByIdAsync<TResponse>(string apiEndpoint, int id);
    }
}
