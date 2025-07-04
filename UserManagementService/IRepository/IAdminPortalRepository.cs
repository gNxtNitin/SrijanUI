﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.DTOs.ResponseModels;
using UserManagementService.Models;

namespace UserManagementService.IRepository
{
    public interface IAdminPortalRepository
    {
        Task<List<CityRequestModel>> GetAllCity();
        Task<List<ZoneRequestModel>> GetAllZone();
        Task<APIResponse> CreateUpdateDeleteZone(ZoneRequestModel zrm);
        Task<APIResponse> CreateUpdateDeleteCity(CityRequestModel crm);
        Task<string> GetData(string flag, string? cid2);
        Task<string> GetDashboarddata(string userId);
        Task<List<SchoolRequestModel>> GetAllSchool(string empid);
        Task<List<EmployeeRequestModel>> GetAllEmployees();
        Task<APIResponse> CreateUpdateDeleteEmployee(EmployeeRequestModel erm);
        Task<bool> CreateUpdateDeleteSchool(SchoolRequestModel srm);
        Task<APIResponse> AssignSchoolIncharge(SchoolRequestModel srm);
        Task<string> GetSchoolIncharge();
    }
}
