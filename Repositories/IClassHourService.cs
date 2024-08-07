﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IClassHourService
    {
        Task<ClassHourServiceResponseDto> GetAll(ClassHourDto classHourDto);

        Task<ClassHourServiceResponseDto> CreateClassAsync(ClassHourDto model, string userId);

        Task<ClassHourServiceResponseDto> GetByIdAsync(ClassHourDto classHourDto, string id);

        Task<ClassHourServiceResponseDto> DeleteClassAsync(string id);

        string GenerateClassId();

        string GetCurrentUserId();
    }
}
