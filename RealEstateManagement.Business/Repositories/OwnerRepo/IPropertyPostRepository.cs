using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public interface IPropertyPostRepository
    {
        Task<int> CreatePropertyPostAsync(Property property, PropertyPost post, List<int> amenityIds);
    }

}
