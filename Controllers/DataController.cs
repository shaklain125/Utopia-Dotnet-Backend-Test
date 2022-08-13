using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtopiaBackendChallenge.Data;
using UtopiaBackendChallenge.Lib;
using UtopiaBackendChallenge.Utils;

namespace UtopiaBackendChallenge.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly utopiaContext ctx;

        public DataController(utopiaContext context)
        {
            ctx = context;
        }

        [HttpGet("/SearchCustomers")]
        [Produces("application/json", Type = typeof(List<CustomerDTO>))]
        public async Task<IActionResult> SearchCustomers(string query)
        {
            query = query.ToLower();

            var customers = from c in ctx.Customers
                            select c;

            if (!string.IsNullOrEmpty(query))
            {
                customers = customers.Where((c) =>
                                            (c.CompanyName ?? "").Contains(query) ||
                                            (c.SalesPerson ?? "").Contains(query));
            }

            var list = await customers.ToListAsync();

            var listDto = list.Select((c) => new CustomerDTO(c));

            return Content(Obj.serialize(listDto));
        }

        [HttpGet("/TopSalesPeople")]
        [Produces("application/json", Type = typeof(List<TopSalesPeopleDTO>))]
        public async Task<IActionResult> TopSalesPeople()
        {
            IQueryable<object> salesPersonQuery = from c in ctx.Customers
                                                  join h in ctx.SalesOrderHeaders on c.CustomerId equals h.CustomerId
                                                  join d in ctx.SalesOrderDetails on h.SalesOrderId equals d.SalesOrderId
                                                  join p in ctx.Products on d.ProductId equals p.ProductId
                                                  select new
                                                  {
                                                      Id = c.CustomerId,
                                                      SalesPerson = c.SalesPerson,
                                                      TotalDue = h.TotalDue,
                                                      OrderQty = d.OrderQty,
                                                      ProductId = p.ProductId,
                                                  };

            List<object> list = await salesPersonQuery.ToListAsync();

            Dictionary<string, TopSalesPeopleDTO> dict = new Dictionary<string, TopSalesPeopleDTO>();

            foreach (object obj in list)
            {
                var item = Obj.getProps(obj);
                string SalesPerson = (string)item["SalesPerson"]!;
                decimal TotalDue = (decimal)item["TotalDue"]!;
                int OrderQty = Convert.ToInt32(item["OrderQty"]!);
                int ProductId = (int)item["ProductId"]!;
                if (!dict.ContainsKey(SalesPerson))
                {
                    TopSalesPeopleDTO dto = new TopSalesPeopleDTO();
                    dto.SalePerson = SalesPerson;
                    dict.Add(SalesPerson, dto);
                }
                TopSalesPeopleDTO topSalesPeopleDTO = dict[SalesPerson];
                topSalesPeopleDTO.TotalEarned += TotalDue;
                topSalesPeopleDTO.TotalItems += OrderQty;
                topSalesPeopleDTO.addProduct(ProductId);
            }

            List<TopSalesPeopleDTO> topSalesPeopleDTOs = dict.Values.ToList();

            foreach (var dto in topSalesPeopleDTOs) dto.sortProducts();

            topSalesPeopleDTOs.Sort((a, b) => b.TotalEarned.CompareTo(a.TotalEarned));

            return Content(Obj.serialize(topSalesPeopleDTOs, true));
        }

        [HttpGet("/RickAndMortyChars")]
        [Produces("application/json")]
        public async Task<RMResults<RMCharacter>?> RickAndMortyChars(string query)
        {
            query = HttpUtility.UrlDecode(query);
            HttpClient client = new HttpClient();
            var message = await client.GetAsync($"https://rickandmortyapi.com/api/character/?{query}");
            return await message.Content.ReadFromJsonAsync<RMResults<RMCharacter>>();
        }
    }
}
