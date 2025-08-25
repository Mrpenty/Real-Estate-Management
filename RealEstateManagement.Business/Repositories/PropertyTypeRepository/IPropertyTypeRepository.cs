using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.PropertyTypeRepository
{
    public interface IPropertyTypeRepository: IRepositoryAsync<PropertyType>
    {

    }
}
