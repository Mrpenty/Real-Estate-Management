namespace RealEstateManagement.Data.Entity
{
    public class UserPreferenceFavoriteProperties
    {
        public int UserPreferenceId { get; set; }
        public int PropertyId { get; set; }

        public UserPreference UserPreference { get; set; }
        public Property Property { get; set; }
    }
}
