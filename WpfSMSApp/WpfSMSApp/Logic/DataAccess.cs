using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSMSApp.Model;

namespace WpfSMSApp.Logic
{
    public class DataAccess
    {
        //select * from user와 동일
        public static List<User> GetUsers()
        {
            List<User> users;

            using(var ctx = new SMSEntities())
            {
                users = ctx.User.ToList();
            }

            return users;
        }

        /// <summary>
        /// 입력, 수정 동시에...
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static int SetUser(User user)
        {
            using(var ctx = new SMSEntities())
            {
                ctx.User.AddOrUpdate(user);
                return ctx.SaveChanges();
            }
        }

        //select * from store와 동일
        internal static List<Store> GetStores()
        {
            List<Store> stores;

            using (var ctx = new SMSEntities())
            {
                stores = ctx.Store.ToList();
            }

            return stores;
        }

        /// <summary>
        /// 입력, 수정 동시에...
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        internal static int SetStore(Store store)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Store.AddOrUpdate(store);
                return ctx.SaveChanges();
            }
        }

        internal static List<Stock> GetStocks()
        {
            List<Stock> stocks;

            using (var ctx = new SMSEntities())
            {
                stocks = ctx.Stock.ToList();
            }

            return stocks;
        }
        
        /// <summary>
         /// 입력, 수정 동시에...
         /// </summary>
         /// <param name="store"></param>
         /// <returns></returns>
        internal static int SetStock(Stock stock)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Stock.AddOrUpdate(stock);
                return ctx.SaveChanges();
            }
        }
    }
}
