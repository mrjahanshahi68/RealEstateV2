using QueryDesigner;
using RealEstate.Common.Entities.Common;
using RealEstate.Common.Exceptions;
using RealEstate.DataAccess;
using RealEstate.Web.Models;
using RealEstate.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RealEstate.Common.AppConstants;

namespace RealEstate.Web.Controllers.View
{
    public class BlogUserController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            var vm = new List<BlogVM>();
            int totalCount = 0;
            using (var uow = new AppUnitOfWork())
            {
                 totalCount = uow.Repository<Blog>().Queryable().Count();
                 vm = uow.Repository<Blog>().Queryable()
                    .Where(c=>c.IsActive && !c.IsDeleted)
                      .Select(c => new BlogVM
                      {
                          ID = c.ID,
                          Title =c.Title,
                          Summery = c.Summery,
                          MetaDescription = c.MetaDescription,
                          PublishDateTime = c.PublishDateTime,
                          SlugUrl = c.SlugUrl
                      }).OrderByDescending(e=>e.PublishDateTime).Take(4)
                      .ToList();
            }

            return View(new FilterQueryRsponse { 
            Records=vm,
            TotalCount= totalCount,
            });
        }

        [HttpPost]
        public ActionResult GetBlogs(FilterContainer filter)
        {

            if (filter == null)
                throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

            var uow = new AppUnitOfWork();
           var  query = uow.Repository<Blog>().Queryable();
            var totalCount = query.Count();
            if (filter.OrderBy == null)
            {
                filter.OrderBy = new List<OrderFilter>
                    {
                        new OrderFilter
                        {
                            Field="PublishDateTime",
                            Order = OrderFilterType.Desc
            }
                    };
            }
            query = query.Request(filter);
            
            var result = query.ToList();
            return Json(new FilterQueryRsponse
            {
                TotalCount = totalCount,
                Records = result,
            });
        }
    
        public ActionResult BlogDetails(int a)
        {
            var vm = new BlogVM();

            using (var uow = new AppUnitOfWork())
            {
              vm=uow.Repository<Blog>().Queryable()
                    .Where(c=>c.ID == a)
                    .Select(c=>new BlogVM
                    {
                        ID = c.ID,
                        Title = c.Title,
                        Summery = c.Summery,
                        MetaDescription = c.MetaDescription,
                        PublishDateTime = c.PublishDateTime,
                        SlugUrl = c.SlugUrl,
                        Body = c.Body
                    })
                    .Single();
            }

            return View(vm);
        }
    }
}