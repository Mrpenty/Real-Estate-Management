using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Data;
using RealEstateManagement.Business.Repositories.Repository;
using Microsoft.EntityFrameworkCore;

public class SliderRepository : RepositoryAsync<Slider>, ISliderRepository
{
    public SliderRepository(RentalDbContext context) : base(context) { }
}