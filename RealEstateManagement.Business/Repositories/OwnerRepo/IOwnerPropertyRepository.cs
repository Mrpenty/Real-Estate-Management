﻿using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public interface IOwnerPropertyRepository : IRepositoryAsync<Property>
    {
        IQueryable<Property> GetByLandlordQueryable(int landlordId);
        Task<Property> GetByIdAsync(int id, int landlordId);
        Task DeleteAsync(Property property);
    }
}
