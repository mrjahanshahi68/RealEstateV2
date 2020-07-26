using RealEstate.Web.Infrastrcuture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using RealEstate.Common.Entities;
//using System.Security.Authentication;
using RealEstate.Domain;
using static RealEstate.Common.AppEnums;
using AutoMapper;
using System.Data.Entity;
using System.Linq;
using RealEstate.Common.Exceptions;
using static RealEstate.Common.AppConstants;
using RealEstate.Web.Cache;
using RealEstate.Web.Security;

namespace RealEstate.Web.Controllers
{
    public abstract class BaseApiController<TEntity,TEntityVM> : ApiController 
        where TEntity:class,IEntity,new()
        where TEntityVM:class
    {
        #region Properties
        private Mapper mapper { get; set; }
        protected IBusinessRule<TEntity> BusinessRule { get; set; }
		public bool IsAuthenticated => SecurityManager.IsAuthenticated(Token);
		public string Token => SecurityManager.GetToken(Request);
        public AppUserInfo CurrentUser => IsAuthenticated ? CacheManager.GetValue(Token) as AppUserInfo : throw new AuthenticationException("Not Athenticate");
        #endregion

        protected abstract IBusinessRule<TEntity> CreateRule();

        public BaseApiController()
        {
            BusinessRule = CreateRule();
            mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<TEntityVM, TEntity>()));
        }

        #region Response
        protected virtual HttpResponseMessage CreateResponse(ResultCode resultCode, object data=null, List<string> messages=null)
		{
			return Request.CreateResponse(HttpStatusCode.OK, new ApiResult {
                ResultCode=resultCode,
                Data=data,
                Messages=messages,
            });
		}
        protected virtual HttpResponseMessage CreateResponse(ResultCode resultCode, object data, string message)=> CreateResponse(resultCode, data, new List<string> { message });
        protected virtual HttpResponseMessage CreateResponse(ResultCode resultCode,string message)=> CreateResponse(resultCode, null, new List<string> { message });
        protected virtual HttpResponseMessage Success(object data=null)=> CreateResponse(ResultCode.Success, data);
        protected virtual  Task<HttpResponseMessage> HandleExceptionAsync(Exception ex)=> throw ex;
		#endregion

		#region Validation
		protected virtual List<string> InsertValidation(TEntityVM viewmodel)
		{
			return null;
		}
		protected virtual List<string> UpdateValidation(TEntityVM viewmodel)
		{
			return null;
		}
		#endregion

		#region CRUD Operation
		public async Task<HttpResponseMessage> Post()
		{
			try
			{
				var requestContent = await Request.Content.ReadAsStringAsync();
				var parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntityVM>(requestContent);

				var errors = InsertValidation(parameters);
				if (errors != null && errors.Count > 0)
					throw new ValidationModelException(errors);

				var entity = MapEntityVmToEntity(parameters);

				if (entity is ILoggableEntity)
				{
					var loggableEntity = entity as ILoggableEntity;
					loggableEntity.InsertDateTime = DateTime.Now;
					loggableEntity.InsertUserId = IsAuthenticated ? CurrentUser.UserId : -1;
				}

				BusinessRule.InsertEntity(entity);

				return Success(entity);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		public async Task<HttpResponseMessage> Put()
		{
			try
			{
				var requestContent = await Request.Content.ReadAsStringAsync();
				var parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntityVM>(requestContent);
				var errors = UpdateValidation(parameters);
				if (errors?.Any() ?? false)
					throw new ValidationModelException(errors);

				var entity = MapEntityVmToEntity(parameters);

				if (entity is ILoggableEntity)
				{
					var dbEntity = BusinessRule.FindEntity(entity.ID) as ILoggableEntity;
					var loggableEntity = entity as ILoggableEntity;
					loggableEntity.InsertDateTime = dbEntity.InsertDateTime;
					loggableEntity.InsertUserId = dbEntity.InsertUserId;
					loggableEntity.UpdateDateTime = DateTime.Now;
					loggableEntity.UpdateUserId = IsAuthenticated ? CurrentUser.UserId : -1;
				}

				BusinessRule.UpdateEntity(entity);

				return Success(entity);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		public async Task<HttpResponseMessage> Delete(int id)
		{
			try
			{
				var entity = BusinessRule.FindEntity(id);
				if (entity == null) throw new Exception(string.Format(MessageTemplate.RecordNotFound));
				//var entity = new TEntity();
				//entity.ID = id;
				if (entity is ILogicalDeletable && entity is ILoggableEntity)
				{
					var dbEntity = entity as ILoggableEntity;
					var loggableEntity = entity as ILoggableEntity;
					loggableEntity.InsertDateTime = dbEntity.InsertDateTime;
					loggableEntity.InsertUserId = dbEntity.InsertUserId;
					loggableEntity.UpdateDateTime = DateTime.Now;
					loggableEntity.UpdateUserId = IsAuthenticated ? CurrentUser.UserId : -1;
				}
				BusinessRule.DeleteEntity(entity);
				return Success();
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		public async Task<HttpResponseMessage> Get(int? id)
		{
			try
			{
				if (id.HasValue)
				{
					var entity = BusinessRule.FindEntity(id.Value);
					var result = MapEntityToEntityVm(entity);
					return Success(result);
				}
				else
				{
					var entities = await BusinessRule.Queryable().ToListAsync();
					var result = entities?.Select(e => MapEntityToEntityVm(e));
					return Success(result);
				}
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion

		#region Translate
		protected virtual TEntity MapEntityVmToEntity(TEntityVM vm)
		{
			return mapper.Map<TEntity>(vm);
		}
		protected virtual TEntityVM MapEntityToEntityVm(TEntity entity)
		{
			return mapper.Map<TEntityVM>(entity);
		}
		#endregion

	}
}
