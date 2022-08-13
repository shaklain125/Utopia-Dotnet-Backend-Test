using UtopiaBackendChallenge.Models;
using UtopiaBackendChallenge.Utils;

namespace UtopiaBackendChallenge.Lib
{
    public class CustomerDTO
    {
        public CustomerDTO(Customer? customer = null)
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
            if (customer != null) Obj.setProps(this, customer);
        }

        public int CustomerId { get; set; }
        public string? Title { get; set; }
        public string? Suffix { get; set; }
        public string? CompanyName { get; set; }
        public string? SalesPerson { get; set; }
        public string? EmailAddress { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}