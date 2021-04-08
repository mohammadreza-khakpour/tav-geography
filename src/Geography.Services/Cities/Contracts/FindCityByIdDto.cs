namespace Geography.Services.Cities.Contracts
{
    public class FindCityByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
    }
}
