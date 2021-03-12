namespace azure_cosmos_db_example {

    public class UserData {
       public User yanhe = new User {
            Id = "1",
            UserId = "yanhe",
            LastName = "He",
            FirstName = "Yan",
            Email = "yanhe@contoso.com",
            OrderHistory = new OrderHistory[] {
            new OrderHistory {
            OrderId = "1000",
            DateShipped = "08/17/2018",
            Total = "52.49"
            }
            },
            ShippingPreference = new ShippingPreference[] {
            new ShippingPreference {
            Priority = 1,
            AddressLine1 = "90 W 8th St",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            Country = "USA"
            }
            },
        };

     public   User nelapin = new User {
            Id = "2",
            UserId = "nelapin",
            LastName = "Pindakova",
            FirstName = "Nela",
            Email = "nelapin@contoso.com",
            Dividend = "8.50",
            OrderHistory = new OrderHistory[] {
            new OrderHistory {
            OrderId = "1001",
            DateShipped = "08/17/2018",
            Total = "105.89"
            }
            },
            ShippingPreference = new ShippingPreference[] {
            new ShippingPreference {
            Priority = 1,
            AddressLine1 = "505 NW 5th St",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            Country = "USA"
            },
            new ShippingPreference {
            Priority = 2,
            AddressLine1 = "505 NW 5th St",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            Country = "USA"
            }
            },
            Coupons = new CouponsUsed[] {
            new CouponsUsed {
            CouponCode = "Fall2018"
            }
            }
        };
    }
}