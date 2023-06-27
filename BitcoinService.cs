using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Caching.Memory;

namespace BitocCore1
{
    public class BitcoinService : IPairsProvider
    {
        private static readonly string strConn = Ut.GetConnetString();
        IMemoryCache cache;

        public BitcoinService(IMemoryCache memoryCache)
        {
            cache = memoryCache;
        }

        uint IPairsProvider.Count()
        {
            cache.TryGetValue(0, out uint count);

            if (count == 0)
            {
                try
                {
                    using (IDbConnection db = new SqlConnection(strConn))
                    {
                        count = db.Query<uint>("SELECT COUNT(*) FROM BitItem B1, BitItem B2 WHERE B1.Id > B2.Id;").FirstOrDefault();
                        cache.Set(0, count, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                        Console.WriteLine($"Count = {count} ");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return count;
        }

        IEnumerable<string> IPairsProvider.GetPairs(int page)
        {
            int OFFSET = (page - 1) * 20;
            cache.TryGetValue(page, out IEnumerable<string> list);

            if (list == null)
            {
                using (IDbConnection db = new SqlConnection(strConn))
                {
                    list = db.Query<string>("SELECT B1.symbol + '-' + B2.symbol  FROM BitItem B1, BitItem B2 WHERE B1.Id > B2.Id ORDER By B1.Id  OFFSET @OFFSET ROWS FETCH NEXT 20 ROWS ONLY; ",  new { OFFSET }).ToList();

                    cache.Set(page, list, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                    DateTime dateTime = DateTime.Now;
                    Console.WriteLine($"List.Count = {list.Count()}  Time = {dateTime.ToString("g")} ");
                }
            }
            return list;
        }
    }
}

//       "SqlConnString": "Data Source=DESKTOP-OV65M43\\MSSQL2019;Initial Catalog=BitocDB;User Id=sa;Password=sa"

