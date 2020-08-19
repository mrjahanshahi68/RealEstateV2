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
				var attachmentQuery = uow.Repository<Attachment>().Queryable().Where(e => e.ObjectType == Common.AppEnums.ObjectType.Blog);
				var blogQuery = uow.Repository<Blog>().Queryable().Where(e=>e.IsActive);
				var query = from blog in blogQuery
							join attach in attachmentQuery on blog.ID equals attach.ObjectId into attachGroup
							select new {
								HashKey = blog.HashKey,
								CategoryId = blog.CategoryId,
								Title = blog.Title,
								UrlTitle = blog.UrlTitle,
								MetaDescription = blog.MetaDescription,
								Summery = blog.Summery,
								SlugUrl = blog.SlugUrl,
								Discriminator = blog.Discriminator,
								ViewNumber = blog.ViewNumber,
								Body = blog.Body,
								Sort = blog.Sort,
								Image = blog.Image,
								IsDeleted = blog.IsDeleted,
								PublishDateTime = blog.PublishDateTime,
								IsActive = blog.IsActive,
								ID = blog.ID,
								attachments=attachGroup,
							};
				var result = query.OrderByDescending(e=>e.PublishDateTime).Select(blog => new BlogVM {
					HashKey = blog.HashKey,
					CategoryId = blog.CategoryId,
					Title = blog.Title,
					UrlTitle = blog.UrlTitle,
					MetaDescription = blog.MetaDescription,
					Summery = blog.Summery,
					SlugUrl = blog.SlugUrl,
					Discriminator = blog.Discriminator,
					ViewNumber = blog.ViewNumber,
					Body = blog.Body,
					Sort = blog.Sort,
					Image = blog.Image,
					IsDeleted = blog.IsDeleted,
					PublishDateTime = blog.PublishDateTime,
					IsActive = blog.IsActive,
					ID = blog.ID,
					//attachments = blog.attachments.Any() ? ,
				}).ToList();
				//var result = query.OrderByDescending(e => e.blog.PublishDateTime)
				//	.Take(4)
				//	.Select(c=>new BlogVM {
				//		ID = c.blog.ID,
				//		Title = c.blog.Title,
				//		Summery = c.blog.Summery,
				//		MetaDescription = c.blog.MetaDescription,
				//		PublishDateTime = c.blog.PublishDateTime,
				//		SlugUrl = c.blog.SlugUrl
				//	})		  
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