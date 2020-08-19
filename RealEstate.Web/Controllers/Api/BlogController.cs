using RealEstate.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Common;
using System.Threading.Tasks;
using RealEstate.Web.Security.Filters;
using RealEstate.Web.Models.Common;
using static RealEstate.Common.AppConstants;
using QueryDesigner;
using System.Data.Entity;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Models;
using System.IO;
using RealEstate.Common;
using System.Web;
using RealEstate.DataAccess;
using static RealEstate.Common.AppEnums;
using System.Collections;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
	public class BlogController : BaseApiController<Blog, BlogVM>
	{
		private string BlogFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigurations.BlogImageFolder);
		protected override IBusinessRule<Blog> CreateRule() => new BlogBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetBlogs(FilterContainer filter)
		{
			try
			{
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var blogQuery = BusinessRule.Queryable();
				string[] names = Enum.GetNames(typeof(Categories));

				//var categories = names?.Select(e => new KeyValueVM { Key = ((int)Enum.Parse(typeof(Categories), e)), Value = e }).ToList().AsQueryable();

				var query = from blog in blogQuery
							//join cate in categories on blog.CategoryId equals cate.Key
							select new BlogVM
							{
								ID=blog.ID,
								Title=blog.Title,
								PublishDateTime=blog.PublishDateTime,
								ViewNumber=blog.ViewNumber,
								HashKey=blog.HashKey,
								CategoryId=blog.CategoryId,
								//CategoryName = cate.Value,
								IsDeleted =blog.IsDeleted,
								IsActive=blog.IsActive,
								InsertDateTime=blog.InsertDateTime,
							};

				var totalCount = await query.CountAsync();
				if (filter.OrderBy == null)
				{
					filter.OrderBy = new List<OrderFilter>
					{
						new OrderFilter
						{
							Field="InsertDateTime",
							Order = OrderFilterType.Desc
						}
					};
				}
				query = query.Request(filter);
				var result = await query.ToListAsync();

				return Success(new FilterQueryRsponse
				{
					TotalCount = totalCount,
					Records = result,
				});
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}


		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> SubmitBlog()
		{
			DbContextTransaction transaction = null;
			try
			{
				var fileDataRequestParameters = HttpContext.Current.Request.GetDataFileRequestParameters<BlogVM, FileDataModel>();
				var parameters = fileDataRequestParameters.ViewModel;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var errors = new List<string>();
				//if (!parameters.CategoryId.HasValue)
				//	errors.Add(string.Format(MessageTemplate.Required, "گروه پست"));
				if (string.IsNullOrWhiteSpace(parameters.Title))
					errors.Add(string.Format(MessageTemplate.Required, " عنوان "));
				if (string.IsNullOrWhiteSpace(parameters.Body))
					errors.Add(string.Format(MessageTemplate.Required, " متن "));
				if (string.IsNullOrWhiteSpace(parameters.Summery))
					errors.Add(string.Format(MessageTemplate.Required, " خلاصه  "));

				if (errors.Any())
					throw new ValidationModelException(errors);

				if(!parameters.CategoryId.HasValue)
					parameters.CategoryId = (int)Categories.Learning;

				using (var dbContext = new AppDataContext())
				{

					transaction = dbContext.Database.BeginTransaction();
					using (var uow = new UnitOfWork<AppDataContext>(dbContext))
					{
						var blogRepo = uow.Repository<Blog>();
						var attachmentRepo = uow.Repository<Attachment>();
						var imageFile = fileDataRequestParameters.Files.Where(e => e.FileData.ExtraInfoString == "blogImage").FirstOrDefault();
						var entity = Mapper.Map<Blog>(parameters);
						if (entity.ID > 0)
						{
							var dbEntity = blogRepo.Find(entity.ID);
							if (dbEntity == null)
								throw new ValidationModelException(MessageTemplate.RecordNotFound);
							entity.HashKey = dbEntity.HashKey;
							if (imageFile == null)
								entity.Image = dbEntity.Image;
							else
							{
								string imagePath = Path.Combine(BlogFolder, entity.HashKey, $"{entity.Image}");
								if (File.Exists(imagePath))
									File.Delete(imagePath);
								entity.Image = imageFile.HttpPostedFile?.FileName;
							}

							entity.InsertDateTime = dbEntity.InsertDateTime;
							entity.InsertUserId = dbEntity.InsertUserId;
							entity.UpdateDateTime = DateTime.Now;
							entity.UpdateUserId = CurrentUser.UserId;

							uow.Repository<Blog>().Update(entity);

							if (imageFile != null)
							{
								var attchments = uow.Repository<Attachment>().Queryable().Where(e => e.ObjectType == ObjectType.Blog && e.ObjectId == entity.ID).ToList();
								foreach (var item in attchments)
								{
									uow.Repository<Attachment>().Delete(item);
								}
							}
							
						}
						else
						{
							string hashkey = string.Empty;
							while (true)
							{
								hashkey = Guid.NewGuid().ToString();
								if (uow.Repository<Blog>().Queryable().Where(e => e.HashKey == hashkey).Any()) continue;
								break;
							}
							entity.HashKey = hashkey;
							if (imageFile != null)
								entity.Image = imageFile.HttpPostedFile?.FileName;

							entity.InsertDateTime = DateTime.Now;
							entity.InsertUserId = CurrentUser.UserId;
							entity.PublishDateTime = DateTime.Now;

							uow.Repository<Blog>().Insert(entity);
						}

						await uow.SaveChangeAsync();

						var currentDirectory = Path.Combine(BlogFolder, entity.HashKey);
						if (!Directory.Exists(currentDirectory))
							Directory.CreateDirectory(currentDirectory);

						var attachments = new List<Attachment>();
						foreach (var item in fileDataRequestParameters.Files)
						{
							var file = item.HttpPostedFile;


							if (item.FileData?.ExtraInfoString == "blogImage")
							{
								file.SaveAs($"{currentDirectory}/{file.FileName}");
							}
							attachments.Add(new Attachment
							{
								HashKey = Guid.NewGuid().ToString(),
								ContentType =file.ContentType,
								FileName=file.FileName,								
								FileSize=file.ContentLength / AppConstants.Kilobyte,
								FileUnit = FileUnit.KB,
								ObjectType =ObjectType.Blog,
								ObjectId=entity.ID,
								InsertUserId = CurrentUser.UserId,
								InsertDateTime = DateTime.Now,
							});
						}

						foreach (var item in attachments)
							attachmentRepo.Insert(item);
						await uow.SaveChangeAsync();

						transaction.Commit();
					}
				}


				return Success();
			}
			catch (Exception ex)
			{
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}
		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> GetBlog(BlogVM parameter)
		{
			try
			{
				BlogVM result = null;
				if (parameter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);
				if (parameter.ID == 0)
					throw new ValidationModelException(MessageTemplate.InvalidIdentity);

				var blog = await BusinessRule.Queryable().Where(e => e.ID == parameter.ID).FirstOrDefaultAsync();
				if (blog == null)
					throw new ValidationModelException(MessageTemplate.RecordNotFound);

				result = new BlogVM
				{
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
				};

				var attachments =await BusinessRule.UnitOfWork.Repository<Attachment>().Queryable().Where(e => e.ObjectType == ObjectType.Blog && e.ObjectId == parameter.ID).ToListAsync();
				result.Attachments = attachments?.Select(e => new AttachmentVM
				{
					ID = e.ID,
					FileName = e.FileName,
					HashKey = e.HashKey,
					ObjectId = e.ObjectId,
					FileSize = e.FileSize,
					ContentType = e.ContentType,
					Path = $"{AppConfigurations.BlogImageFolder}/{result.HashKey}/{result.Image}",
				}).ToList();

				//if (attachments != null && attachments.Any())
				//{
				//	var blogImage = attachments[0];
				//	result.ImagePath= $"{AppConfigurations.BlogImageFolder}/{result.HashKey}/Image_{blogImage.FileName}";
				//}
				return Success(result);
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion

		#region Validations
		protected override List<string> InsertValidation(BlogVM viewmodel)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(viewmodel.Title))
				errors.Add(string.Format(MessageTemplate.Required, " عنوان "));
			if (string.IsNullOrWhiteSpace(viewmodel.Body))
				errors.Add(string.Format(MessageTemplate.Required, " متن "));
			if (string.IsNullOrWhiteSpace(viewmodel.Summery))
				errors.Add(string.Format(MessageTemplate.Required, " خلاصه  "));
			//if (!viewmodel.CategoryId.HasValue)
			//	errors.Add(string.Format(MessageTemplate.Required, " دسته بندی "));

			var entityByName = BusinessRule.Queryable()
				.SingleOrDefault(c => c.Title == viewmodel.Title);
			if (entityByName != null)
				errors.Add(string.Format(MessageTemplate.Repetitious));

			viewmodel.PublishDateTime = DateTime.Now;
			viewmodel.CategoryId = 1;

			return errors;

		}

		protected override List<string> UpdateValidation(BlogVM viewmodel)
		{
			var errors = new List<string>();
			if (viewmodel.ID <= 0)
				errors.Add("کد شناسته نامعتبر است");
			var entity = BusinessRule.FindEntity(viewmodel.ID);
			if (entity == null) throw new Exception("رکورد مورد نظر یافت نشد");
			if (string.IsNullOrWhiteSpace(viewmodel.Title))
				errors.Add(string.Format(MessageTemplate.Required, " عنوان "));
			if (string.IsNullOrWhiteSpace(viewmodel.Body))
				errors.Add(string.Format(MessageTemplate.Required, " متن "));
			if (string.IsNullOrWhiteSpace(viewmodel.Summery))
				errors.Add(string.Format(MessageTemplate.Required, " خلاصه  "));
			if (viewmodel.CategoryId == 0)
				errors.Add(string.Format(MessageTemplate.Required, " دسته بندی "));

			var entityByName = BusinessRule.Queryable()
				.SingleOrDefault(c => c.ID != viewmodel.ID && c.Title == viewmodel.Title);
			if (entityByName != null)
				errors.Add(string.Format(MessageTemplate.Repetitious));

			return errors;
		}
		#endregion
	}
}
