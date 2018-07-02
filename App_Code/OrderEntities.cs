using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Order
/// </summary>
public class OrderEntities : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public OrderEntities()
        : base("DefaultConnection")
    {
    }
}

public class Order
{
    // Primary key 
    public int OrderID { get; set; }
    public string NameOnCheck { get; set; }
    public string EMailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string AddressOnCheck { get; set; }
    public string CityOnCheck { get; set; }
    public string StateOnCheck { get; set; }
    public string ZipCodeOnCheck { get; set; }
    public string BankName { get; set; }
    public string AccountingNumber { get; set; }
    public string RoutingNumber { get; set; }
    public int OnesOrdered { get; set; }
    public int FivesOrdered { get; set; }
    public int TwentyFivesOrdered { get; set; }
    public int HundredsOrdered { get; set; }
    public int TwoHundredFiftiesOrdered { get; set; }
    public int TotalCoinsOrdered { get; set; }
    public double TotalPrice { get; set; }
    public string ExportFileName { get; set; }
    public string GPResult { get; set; }
    public string CheckNumber { get; set; }
    public string CheckID { get; set; }
    public string VerificationResultCode { get; set; }
    public string VerificationResultDesc { get; set; }
    public string FileName { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Affiliate { get; set; }
}