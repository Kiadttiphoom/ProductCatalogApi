namespace ProductCatalogApi.Models.Entities;

public class Product
{
    public string BrandID { get; set; } = "";
    public string ModID { get; set; } = "";
    public string ModName { get; set; } = "";
    public int YearBuild { get; set; } = 0;
    public int PTID { get; set; } = 0;
    public double PriceWS { get; set; } = 0.00;
    public double PriceWS2 { get; set; } = 0.00;
    public string CC { get; set; } = "";
    public string Status { get; set; } = "";
}